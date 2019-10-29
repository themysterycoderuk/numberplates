using NumberPlates.Vision;
using NumberPlates.Images;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Messages;

namespace CSHttpClientSample
{
    static class Program
    {
        private const string cIMAGE_FILE = @"C:\temp\numberplates\4.jpg";

        static void Main(string[] args)
        {
            //var contents = getFileAsByteArray(cIMAGE_FILE);

            //var img = new ImageStore();
            //var uri = img.StoreImage(contents).Result;

            //Console.WriteLine("Saved as:");
            //Console.WriteLine(uri);

            //var messager = new Messages.Queue();
            //messager.SendPhotoMessage(uri).Wait();
            //messager.RetrievePhotoMessage().Wait();

            var sqs = new SQS();
            sqs.SendMessageAsync().Wait();

            //var ocr = new OCR();
            //ocr.ExtractTextFromURL(uri).Wait();
            //ocr.ExtractText(cIMAGE_FILE).Wait();




            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Computer Vision quickstart is complete.");
            Console.WriteLine();
            Console.WriteLine("Press enter to exit...");
            Console.WriteLine();
            Console.ReadLine();
        }

        private static byte[] getFileAsByteArray(string file)
        {
            return File.ReadAllBytes(file);
        }
    }
}