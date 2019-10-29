using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Threading.Tasks;

namespace Messages
{
    public class SQS
    {
        private const string cQUEUE_URL = @"https://sqs.eu-west-2.amazonaws.com/566026652069/SQS_NumberPlates.fifo";

        private AmazonSQSConfig _config;
        private IAmazonSQS _sqs; 
        
        public async Task SendMessageAsync()
        {
            _config = new AmazonSQSConfig();
            _sqs = new AmazonSQSClient(_config);

            //Sending a message
            Console.WriteLine("Sending a message to MyQueue.\n");
            var sendMessageRequest = new SendMessageRequest();
            sendMessageRequest.QueueUrl = cQUEUE_URL; //URL from initial queue creation
            sendMessageRequest.MessageBody = "This is my message text.";
            await _sqs.SendMessageAsync(sendMessageRequest);
        }
    }
}
