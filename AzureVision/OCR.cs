using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NumberPlates.Vision
{
    public class OCR
    {
        private ComputerVisionClient _client;

        private static string _subscriptionKey = Environment.GetEnvironmentVariable("COMPUTER_VISION_SUBSCRIPTION_KEY");

        private static string _endpoint = Environment.GetEnvironmentVariable("COMPUTER_VISION_ENDPOINT");

        private static string _numberPlate = @"(^[A-Z]{4} [A-Z]{3}$)|(^[A-Z]{2}[0-9]{2} [A-Z]{3}$)|(^[A-Z][0-9]{1,3} [A-Z]{3}$)|(^[A-Z]{3} [0-9]{1,3}[A-Z]$)|(^[0-9]{1,4} [A-Z]{1,2}$)|(^[0-9]{1,3} [A-Z]{1,3}$)|(^[A-Z]{1,2} [0-9]{1,4}$)|(^[A-Z]{1,3} [0-9]{1,3}$)";

        private static Regex _reNumberPlate = new Regex(_numberPlate);

        // the OCR method endpoint
        private static string ocrBase = _endpoint + "vision/v2.1/ocr";

        public OCR()
        {
            _client = Authenticate(_endpoint, _subscriptionKey);
        }

        /*
        * AUTHENTICATE
        * Creates a Computer Vision client used by each example.
        */
        private ComputerVisionClient Authenticate(string endpoint, string key)
        {
            var client =
                new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
                { Endpoint = endpoint };
            return client;
        }

        /*
        *	EXTRACT TEXT - URL IMAGE
        */
        public async Task ExtractTextFromURL(string url)
        {
            const int numberOfCharsInOperationId = 36;

            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("EXTRACT TEXT - URL");
            Console.WriteLine();

            Console.WriteLine($"Extracting text from url {url}...");
            Console.WriteLine();

            var headers = await _client.BatchReadFileWithHttpMessagesAsync(url);
            string operationLocation = headers.Headers.OperationLocation;

            // Retrieve the URI where the recognized text will be stored from the Operation-Location header. 
            // We only need the ID and not the full URL

            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text 
            // Delay is between iterations and tries a maximum of 10 times.
            int i = 0;
            int maxRetries = 10;
            ReadOperationResult results;

            do
            {
                results = await _client.GetReadOperationResultAsync(operationId);
                Console.WriteLine("Server status: {0}, waiting {1} seconds...", results.Status, i);
                await Task.Delay(1000);
            }
            while ((results.Status == TextOperationStatusCodes.Running ||
                    results.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries);

            // Display the found text.
            Console.WriteLine();
            var recognitionResults = results.RecognitionResults;
            foreach (TextRecognitionResult result in recognitionResults)
            {
                foreach (var line in result.Lines)
                {
                    if (_reNumberPlate.IsMatch(line.Text))
                    {
                        Console.WriteLine(line.Text);
                    }
                }
            }
            Console.WriteLine();
        }

        /*
         *	EXTRACT TEXT - LOCALFILE IMAGE
         */
        public async Task ExtractText(string localImage)
        {
            const int numberOfCharsInOperationId = 36;

            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("EXTRACT TEXT - LOCAL IMAGE");
            Console.WriteLine();

            Console.WriteLine($"Extracting text from local image {Path.GetFileName(localImage)}...");
            Console.WriteLine();
            using (Stream imageStream = File.OpenRead(localImage))
            {
                // Read the text from the local image
                BatchReadFileInStreamHeaders localFileTextHeaders = await _client.BatchReadFileInStreamAsync(imageStream);
                // Get the operation location (operation ID)

                // After the request, get the operation location (operation ID)
                string operationLocation = localFileTextHeaders.OperationLocation;

                // Retrieve the URI where the recognized text will be stored from the Operation-Location header. 
                // We only need the ID and not the full URL

                string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

                // Extract the text 
                // Delay is between iterations and tries a maximum of 10 times.
                int i = 0;
                int maxRetries = 10;
                ReadOperationResult results;
               
                do
                {
                    results = await _client.GetReadOperationResultAsync(operationId);
                    Console.WriteLine("Server status: {0}, waiting {1} seconds...", results.Status, i);
                    await Task.Delay(1000);
                }
                while ((results.Status == TextOperationStatusCodes.Running ||
                        results.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries);

                // Display the found text.
                Console.WriteLine();
                var recognitionResults = results.RecognitionResults;
                foreach (TextRecognitionResult result in recognitionResults)
                {
                    foreach (var line in result.Lines)
                    {
                        Console.WriteLine($"Checking... {line.Text}");
                        if (_reNumberPlate.IsMatch(line.Text))
                        {
                            Console.WriteLine(line.Text);
                        }
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
