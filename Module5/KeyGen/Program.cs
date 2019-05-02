using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace KeyGen
{
    class Program
    {
        static async Task Main(string[] args)
        {
            WriteStuff(GenerateKey());
            BeepStuff();
            Train();
            Console.ReadLine();
        }

        private static string GenerateKey()
        {
            var netInterface = NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault();
            if (netInterface == null) {
                return string.Empty;
            }
            var bytes1 = netInterface.GetPhysicalAddress().GetAddressBytes();
            var bytes2 = BitConverter.GetBytes(DateTime.Now.Date.ToBinary());
            var key = bytes1.Select((item, index) => (item ^ bytes2[index]) * 10);
            return string.Join("-", key.Select(item => item.ToString()));
        }

        private static void WriteStuff(string key)
        {
            Console.WriteLine("                     You've just been hacked                    ");
            Console.WriteLine("          We proudly present you our unique crack!        ");
            Console.WriteLine("                     Here's your key:");
            Console.WriteLine($"                 {key}");
        }

        private static void Train()
        {
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 30; j++)
                {
                    Console.Clear();
                    
                    Console.Write("       . . . . o o o o o o");
                    for (var s = 0; s < j / 2; s++)
                    {
                        Console.Write(" o");
                    }
                    Console.WriteLine();

                    var margin = "".PadLeft(j);
                    Console.WriteLine(margin + "                _____      o");
                    Console.WriteLine(margin + "       ____====  ]OO|_n_n__][.");
                    Console.WriteLine(margin + "      [________]_|__|________)< ");
                    Console.WriteLine(margin + "       oo    oo  'oo OOOO-| oo\\_");
                    Console.WriteLine("   +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+");

                    Thread.Sleep(200);
                }
            }
        }

        private static void BeepStuff()
        {
            Console.Beep(440, 500);
            Console.Beep(440, 500);
            Console.Beep(440, 500);
            Console.Beep(349, 350);
            Console.Beep(523, 150);
            Console.Beep(440, 500);
            Console.Beep(349, 350);
            Console.Beep(523, 150);
            Console.Beep(440, 1000);
            Console.Beep(659, 500);
            Console.Beep(659, 500);
            Console.Beep(659, 500);
            Console.Beep(698, 350);
            Console.Beep(523, 150);
            Console.Beep(415, 500);
            Console.Beep(349, 350);
            Console.Beep(523, 150);
            Console.Beep(440, 1000);
            Console.Beep(880, 500);
            Console.Beep(440, 350);
            Console.Beep(440, 150);
            Console.Beep(880, 500);
            Console.Beep(830, 250);
            Console.Beep(784, 250);
            Console.Beep(740, 125);
            Console.Beep(698, 125);
            Console.Beep(740, 250);
            Console.Beep(455, 250);
            Console.Beep(622, 500);
            Console.Beep(587, 250);
            Console.Beep(554, 250);
            Console.Beep(523, 125);
            Console.Beep(466, 125);
            Console.Beep(523, 250);
            Console.Beep(349, 125);
            Console.Beep(415, 500);
            Console.Beep(349, 375);
            Console.Beep(440, 125);
            Console.Beep(523, 500);
            Console.Beep(440, 375);
            Console.Beep(523, 125);
            Console.Beep(659, 1000);
            Console.Beep(880, 500);
            Console.Beep(440, 350);
            Console.Beep(440, 150);
            Console.Beep(880, 500);
            Console.Beep(830, 250);
            Console.Beep(784, 250);
            Console.Beep(740, 125);
            Console.Beep(698, 125);
            Console.Beep(740, 250);
            Console.Beep(455, 250);
            Console.Beep(622, 500);
            Console.Beep(587, 250);
            Console.Beep(554, 250);
            Console.Beep(523, 125);
            Console.Beep(466, 125);
            Console.Beep(523, 250);
            Console.Beep(349, 250);
            Console.Beep(415, 500);
            Console.Beep(349, 375);
            Console.Beep(523, 125);
            Console.Beep(440, 500);
            Console.Beep(349, 375);
            Console.Beep(261, 125);
            Console.Beep(440, 1000);
        }
    }
}
