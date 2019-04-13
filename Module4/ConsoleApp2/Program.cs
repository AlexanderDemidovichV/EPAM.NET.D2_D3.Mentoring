using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
//using System.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ConsoleApp2
{
    class Program
    {
        private static IQueueClient queueClient;
        static async Task Main(string[] args)
        {
            var serviceBusConnectionString =
                "Endpoint=sb://dzemidovich-dev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=41JPYUS11Yx8b71bXdyLpoGsfsWRvNrIrBjkYYeTRS4=";
            var queueName = "images";  
                
            if (File.Exists("C:\\Users\\Aliaksandr_Dzemidovi\\Desktop\\new.jpg"))
                File.Delete("C:\\Users\\Aliaksandr_Dzemidovi\\Desktop\\new.jpg");

            var t = ImageToBase64("C:\\Users\\Aliaksandr_Dzemidovi\\Desktop\\36135994_2002047976524612_3088425035963039744_n.jpg");

            Console.WriteLine("---------------------");
            Console.WriteLine(t);
            Console.WriteLine("---------------------");

            queueClient = new QueueClient(serviceBusConnectionString, queueName, ReceiveMode.PeekLock);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press any key to exit after receiving all the messages.");
            Console.WriteLine("======================================================");

            // Register QueueClient's MessageHandler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages();

            // Send Messages
            await SendMessagesAsync(t);

           

            await queueClient.CloseAsync();

            Console.ReadKey();
            
            Console.ReadLine();
        }

        static async Task SendMessagesAsync(string messageBody)
        {
            try
            {
                var guid = new Guid();
                // Create a new message to send to the queue
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                message.UserProperties.Add("id", guid.ToString());

                // Write the body of the message to the console
                Console.WriteLine($"Sending message: {messageBody}");

                // Send the message to the queue
                await queueClient.SendAsync(message);
                
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }

        private static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of Concurrent calls to the callback `ProcessMessagesAsync`, set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false
            };

            // Register the function that will process messages
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        // Use this Handler to look at the exceptions received on the MessagePump
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            M1(Encoding.UTF8.GetString(message.Body), "C:\\Users\\Aliaksandr_Dzemidovi\\Desktop\\new.jpg");
            // Complete the message so that it is not received again.
            // This can be done only if the queueClient is created in ReceiveMode.PeekLock mode (which is default).
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);

            // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
            // If queueClient has already been Closed, you may chose to not call CompleteAsync() or AbandonAsync() etc. calls 
            // to avoid unnecessary exceptions.
        }

        public static string ImageToBase64(string path)
        {
            var data = File.ReadAllBytes(path);
            return Convert.ToBase64String(data);
        }

        public static byte[] Base64ToByteArray(string data)
        {
            return Convert.FromBase64String(data);
        }

        //public static Image ByteArrayToImage(byte[] data)
        //{
        //    var ms = new MemoryStream(data);
        //    return Image.FromStream(ms);
        //}

        public static void M1(string encoded, string outputPath)
        {
            using (var image = Image.Load(Base64ToByteArray(encoded)))
            {
                MutateImage(image);
                image.Save(outputPath); 
            }
        }

        public static void M1(byte[] data, string outputPath)
        {
            using (var image = Image.Load(data))
            {
                MutateImage(image);
                image.Save(outputPath);
            }
        }

        public static Image<Rgba32> MutateImage(Image<Rgba32> image)
        {
            double width, height;
            if (image.Width > image.Height)
            {
                width = 200;
                height = image.Height * (width / image.Width);
            }
            else
            {
                height = 200;
                width = image.Width * (height / image.Height);
            }

            image.Mutate(x => x
                .Resize((int)width, (int)height)
                .Grayscale());
            return image;
        }

        public static void SaveImage(Image<Rgba32> image, string path)
        {
            image.Save(path);
        }
    }
}
