using System;
using APGW;
using System.Collections.Generic;
using Autofac;

namespace TEST_CONSOLE
{
    class MainClass
    {

        public static void SetupDI() {
            var builder = new ContainerBuilder();
            builder.RegisterType<Logger> ().As<ILogger> ();
            Config.RebuildContainer (builder.Build());  
        }

        public static void Main (string[] args)
        {
            SetupDI ();
        }
    }

    class Logger : APGW.ILogger
    {
        public Logger ()
        {
        }

        public void Log(string message) {
            Console.WriteLine ("test: " + message);
        }

        public void Log(string message, Exception e) {
            Log (message + " : " + e.Message);
        }

    }
}
