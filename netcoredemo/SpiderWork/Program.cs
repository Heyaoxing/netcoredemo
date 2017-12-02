using System;
using System.Text;

namespace SpiderWork
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            try
            {
                Console.WriteLine("开始");
                BaiduSearchSpider jdSkuSampleSpider = new BaiduSearchSpider();
                jdSkuSampleSpider.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.Read();
        }
    }
}
