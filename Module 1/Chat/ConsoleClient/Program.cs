using System;
using System.Threading;
using System.Threading.Tasks;
using Client;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var tokenSource = new CancellationTokenSource();
            var botsTask = new Task(() => GenerateBots(tokenSource.Token));
            botsTask.Start();
            
            Console.ReadLine();

            tokenSource.Cancel();

            botsTask.Wait();
            Console.ReadLine();
        }

        private static void GenerateBots(CancellationToken token)
        {
            ChatBot bot = null;

            try
            {
                while (true)
                {
                    if (!token.IsCancellationRequested)
                    {
                        bot = new ChatBot(GenerateBotName(), "127.0.0.1", 
                            8888, 10, "./BotPhrases.txt");
                        Console.WriteLine($"\t\t\tBot {bot.Name} started!");

                        bot.Connect();
                        bot.StartChatRoutine(token);

                        Console.WriteLine($"\t\t\tBye-Bye, {bot.Name}");
                    }
                    else
                    {
                        token.ThrowIfCancellationRequested();
                    }
                }
            }            
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                bot?.Disconnect();
            }
        }

        private static string GenerateBotName()
        {
            var random = new Random((int)DateTime.Now.Ticks);
            string[] names =
            {
                "Jack",
                "Alice",
                "Leo",
                "Emily",
                "Max",
                "Elizabeth",
                "Adam",
                "Amber",
                "Connor",
                "Victoria",
            };

            return names[random.Next(0, names.Length)];
        }
    }
}
