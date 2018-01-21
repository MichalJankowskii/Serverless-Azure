using System;
using System.Configuration;

public static void Run(TimerInfo myTimer, TraceWriter log)
{
    string timeFormat = ConfigurationManager.AppSettings["TimeFormat"];
    log.Info($"C# Timer trigger function executed at: {DateTime.Now.ToString(timeFormat)}");
}
