﻿using DotnetSpider.Core.Processor;
using System;
using System.Collections.Generic;
using System.Text;
using DotnetSpider.Core;
using DotnetSpider.Core.Selector;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Core.Scheduler;
using DotnetSpider.Core.Downloader;

namespace SpiderWork
{
    public class BaseUsage
    {
        #region Custmize processor and pipeline 完全自定义页面解析和数据管道

        public static void CustmizeProcessorAndPipeline()
        {
            // Config encoding, header, cookie, proxy etc... 定义采集的 Site 对象, 设置 Header、Cookie、代理等
            var site = new Site { EncodingName = "UTF-8", RemoveOutboundLinks = true };
            for (int i = 1; i < 5; ++i)
            {
                // Add start/feed urls. 添加初始采集链接
                site.AddStartUrl($"http://list.youku.com/category/show/c_96_s_1_d_1_p_{i}.html");
            }
            Spider spider = Spider.Create(site,
                // use memoery queue scheduler. 使用内存调度
                new QueueDuplicateRemovedScheduler(),
                // use custmize processor for youku 为优酷自定义的 Processor
                new YoukuPageProcessor())
                // use custmize pipeline for youku 为优酷自定义的 Pipeline
                .AddPipeline(new YoukuPipeline());
            spider.Downloader = new HttpClientDownloader();
            spider.ThreadNum = 1;
            spider.EmptySleepTime = 3000;

            // Start crawler 启动爬虫
            spider.Run();
        }

        public class YoukuPipeline : BasePipeline
        {
            private static long count = 0;

            public override void Process(params ResultItems[] resultItems)
            {
                foreach (var resultItem in resultItems)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (YoukuVideo entry in resultItem.Results["VideoResult"])
                    {
                        count++;
                        builder.Append($" [YoukuVideo {count}] {entry.Name}");
                    }
                    Console.WriteLine(builder);
                }
                // Other actions like save data to DB. 可以自由实现插入数据库或保存到文件
            }
        }

        public class YoukuPageProcessor : BasePageProcessor
        {
            protected override void Handle(Page page)
            {
                // 利用 Selectable 查询并构造自己想要的数据对象
                var totalVideoElements = page.Selectable.SelectList(Selectors.XPath("//div[@class='yk-pack pack-film']")).Nodes();
                List<YoukuVideo> results = new List<YoukuVideo>();
                foreach (var videoElement in totalVideoElements)
                {
                    var video = new YoukuVideo();
                    video.Name = videoElement.Select(Selectors.XPath(".//img[@class='quic']/@alt")).GetValue();
                    results.Add(video);
                }

                // Save data object by key. 以自定义KEY存入page对象中供Pipeline调用
                page.AddResultItem("VideoResult", results);

                // Add target requests to scheduler. 解析需要采集的URL
                foreach (var url in page.Selectable.SelectList(Selectors.XPath("//ul[@class='yk-pages']")).Links().Nodes())
                {
                    page.AddTargetRequest(new Request(url.GetValue(), null));
                }
            }
        }

        public class YoukuVideo
        {
            public string Name { get; set; }
        }

        #endregion
    }
}
