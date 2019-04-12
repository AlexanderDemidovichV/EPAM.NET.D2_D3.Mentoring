using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace Generator
{
    class Program
    {
        private static IQueueClient queueClient;

        private const string ServiceBusConnectionString =
            "Endpoint=sb://dzemidovich-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=41JPYUS11Yx8b71bXdyLpoGsfsWRvNrIrBjkYYeTRS4=";
        private const string QueueName = "images";

        static async Task Main()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            while (true)
            {
                var image = ImageToBase64("C:\\Users\\Aliaksandr_Dzemidovi\\Desktop\\36135994_2002047976524612_3088425035963039744_n.jpg");

                await SendMessagesAsync(image);
                await Task.Delay(TimeSpan.FromSeconds(20));
            }
        }

        private static string ImageToBase64(string path)
        {
            var data = File.ReadAllBytes(path);
            return Convert.ToBase64String(data);
        }

        private static async Task SendMessagesAsync(string messageBody)
        {
            try
            {
                var guid = Guid.NewGuid();
                // Create a new message to send to the queue
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                message.UserProperties.Add("guid", guid.ToString());

                // Write the body of the message to the console
                Console.WriteLine($"Sending message: {guid}");

                // Send the message to the queue
                await queueClient.SendAsync(message);

            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}
