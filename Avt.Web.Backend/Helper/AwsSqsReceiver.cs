using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Avt.Web.Backend.Helper
{
    public class AwsSqsReceiver
    {
        public static async Task OnReceive(Func<string, Task> func, bool deleteAfterRead = true)
        {
            try
            {
                var sqs = new AmazonSQSClient();
                var receiveMessageRequest = new ReceiveMessageRequest
                {
                    QueueUrl = "https://sqs.eu-west-1.amazonaws.com/166778461577/ClientHubNotifierQueue"
                };
                var receiveMessageResponse = await sqs.ReceiveMessageAsync(receiveMessageRequest);
                if (receiveMessageResponse.Messages != null && receiveMessageResponse.Messages.Any())
                {
                    int counter = -1;
                    foreach (var message in receiveMessageResponse.Messages)
                    {
                        counter++;
                        if (!string.IsNullOrEmpty(message.Body))
                        {
                            await func.Invoke(message.Body);
                        }

                        if (deleteAfterRead)
                        {
                            var messageRecieptHandle = receiveMessageResponse.Messages[counter].ReceiptHandle;

                            var deleteRequest = new DeleteMessageRequest
                            {
                                QueueUrl = "https://sqs.eu-west-1.amazonaws.com/166778461577/ClientHubNotifierQueue",
                                ReceiptHandle = messageRecieptHandle
                            };
                            await sqs.DeleteMessageAsync(deleteRequest);
                        }
                    }
                }
            }
            catch (AmazonSQSException ex)
            {
                // do nothing, you may want to log, but let it give it another try in the next run
            }
        }
    }
}
