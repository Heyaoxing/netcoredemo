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
                BaseUsage.CustmizeProcessorAndPipeline();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.Read();
        }
    }
}
