using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using Topshelf;

namespace HandlerTopshelfService
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            var outDir = Path.Combine(currentDir, "out");

            var logConfig = new LoggingConfiguration();
            var target = new FileTarget
            {
                Name = "Default",
                FileName = Path.Combine(currentDir, "log.txt"),
                Layout = "${date} ${message} ${onexception:inner=${exception:format=toString}}"
            };

            logConfig.AddTarget(target);
            logConfig.AddRuleForAllLevels(target);

            var logFactory = new LogFactory(logConfig);

            HostFactory.Run(
                hostConf =>
                {
                    hostConf.Service<HandlerService>(
                        s =>
                        {
                            s.ConstructUsing(() => new HandlerService(outDir));
                            s.WhenStarted(serv => serv.Start());
                            s.WhenStopped(serv => serv.Stop());
                        }).UseNLog(logFactory);
                    hostConf.SetServiceName("HandlerService");
                    hostConf.SetDisplayName("Handler Service");
                    hostConf.StartManually();
                    hostConf.RunAsLocalService();
                }
            );
        }
    }
}
