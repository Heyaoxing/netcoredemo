using DotnetSpider.Core.Selector;
using DotnetSpider.Extension;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Model.Attribute;
using DotnetSpider.Extension.Model.Formatter;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpiderWork
{
    public class ZhilianSpider : EntitySpider
    {
        public ZhilianSpider() : base("ZhilianSpider", new DotnetSpider.Core.Site()
        {
            EncodingName = "UTF-8",
            RemoveOutboundLinks = true
        })
        {}
        protected override void MyInit(params string[] arguments)
        {
            Identity = "zhilian_qiantai";
            ThreadNum = 10;//个线程
            AddStartUrl("https://sou.zhaopin.com/jobs/searchresult.ashx?jl=深圳&kw=前台&p=1", "https://sou.zhaopin.com/jobs/searchresult.ashx?jl=深圳&kw=行政&p=1", "https://sou.zhaopin.com/jobs/searchresult.ashx?jl=深圳&kw=人事&p=1");
            AddEntityType<ZhilianEntity>();
        }

        [EntityTable("test", "zhilian_docker", EntityTable.Today)]
        [EntitySelector(Expression = ".//table[@class='newlist']", Type = SelectorType.XPath)]
		[TargetUrlsSelector(XPaths = new[] { "//div[@class='pagesDown']" }, Patterns = new[] { @"&p=[0-9]", @"kw=前台", @"kw=行政", @"kw=人事" })]
        class ZhilianEntity : SpiderEntity
        {
            /// <summary>
            /// 职位
            /// </summary>
            [PropertyDefine(Expression = ".//td[@class='zwmc']//a")]
            [ReplaceFormatter(NewValue = "", OldValue = "<b>")]
            [ReplaceFormatter(NewValue = "", OldValue = "</b>")]
            public string Position { set; get; }

            /// <summary>
            /// 公司名
            /// </summary>
            [PropertyDefine(Expression = ".//td[@class='gsmc']//a[1]")]
            public string Company { set; get; }

            /// <summary>
            /// 最小金额
            /// </summary>
            [PropertyDefine(Expression = ".//td[@class='zwyx']")]
            [SplitFormatter(Splitors = new string[] { "-" }, ElementAt = 0)]
            public string MinSalary { get; set; }
            /// <summary>
            /// 最大金额
            /// </summary>
            [PropertyDefine(Expression = ".//td[@class='zwyx']")]
            [SplitFormatter(Splitors = new string[] { "-" }, ElementAt = 1)]
            public string MaxSalary { set; get; }

            /// <summary>
            /// 地点
            /// </summary>
            [PropertyDefine(Expression = ".//td[@class='gzdd']")]
            public string Address { set; get; }

            /// <summary>
            /// 反馈率
            /// </summary>
            [PropertyDefine(Expression = ".//td[@class='fk_lv']//span")]
            public string Feedback { set; get; }

            /// <summary>
            /// 链接
            /// </summary>
            [PropertyDefine(Expression = ".//td[@class='zwmc']//a/@href")]
            public string Url { set; get; }
        }
    }
}
