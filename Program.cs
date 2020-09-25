using System;
using Topshelf;

namespace AppRestarter
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
                {
                    x.Service<Restarter>(s =>
                        {
                            s.ConstructUsing(Restarter => new Restarter());
                            s.WhenStarted(Restarter => Restarter.Start());
                            s.WhenStopped(Restarter => Restarter.Stop());

                        });
                    x.RunAsLocalSystem();

                    x.SetServiceName("OneCoAppRestarter");
                    x.SetDisplayName("App restart service");
                    x.SetDescription("This is a service to restart another service after a given time.");

            });
            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;

        }
    }
}
