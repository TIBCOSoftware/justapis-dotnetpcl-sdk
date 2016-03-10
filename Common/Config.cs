using System;
using Autofac;

namespace Common
{
    public class Config
    {

        public static void Setup() {
            var builder = new ContainerBuilder();
            builder.RegisterType<Logger> ().As<APGW.ILogger> ();
            APGW.Config.RebuildContainer (builder.Build());  
        }

        class Logger : APGW.ILogger
        {
            public Logger ()
            {
            }

            public void Log(string message) {
                Console.WriteLine (message);
            }

            public void Log(string message, Exception e) {
                Log (message + " : " + e.Message);
            }

        }
    }
}
    