using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyScullion.Utils
{
    public static class Log
    {
        private static List<LogEvent> logEvents = new List<LogEvent>();

        public static void Trace(string content) => System.Diagnostics.Debug.WriteLine(content);
        

        public static void Start(string eventName)
        {
            var logEvent = logEvents.FirstOrDefault(x => x.EventName == eventName);

            if(logEvent != null)
            {
                Clear(eventName);
            }

            logEvent = new LogEvent() { EventName = eventName };
            logEvent.TagTime.Add(new Tuple<string, long>("Start", System.Environment.TickCount));
            logEvents.Add(logEvent);
        }

        public static void Track(string eventName, string tag)
        {            
            if(logEvents.Any(x => x.EventName == eventName))
            {
                Start(eventName);
            }

            var logEvent = logEvents.FirstOrDefault(x => x.EventName == eventName);

            logEvent.TagTime.Add(new Tuple<string, long>(tag, System.Environment.TickCount));
        }

        public static void Stop(string eventName)
        {
            var logEvent = logEvents.FirstOrDefault(x => x.EventName == eventName);

            if(logEvent != null)
            {
                Clear(eventName);
            }

            logEvent.TagTime.Add(new Tuple<string, long>("Stop", System.Environment.TickCount));
            logEvent.GetTime();
        }

        public static void Clear(string eventName)
        {
            var logEvent = logEvents.FirstOrDefault(x => x.EventName == eventName);

            if(logEvent != null)
            {
                logEvents.Remove(logEvent);
            }

            logEvent.GetTime();
        }

        public static List<Tuple<LogEvent, string>> Clear()
        {
            var events = logEvents.Select(x => new Tuple<LogEvent, string>(x, x.GetTime()));
            logEvents.Clear();
            return events.ToList();
        }
    }

    public class LogEvent
    {
        public string EventName { get; set; }

        public List<Tuple<string, long>> TagTime { get; set; } = new List<Tuple<string, long>>();

        public string GetTime()
        {
            var actualTick = System.Environment.TickCount;

            var builder = new StringBuilder();

            foreach (var tuple in TagTime)
            {
                builder.AppendLine($"{tuple}: {actualTick - tuple.Item2}");
            }

            return builder.ToString();
        }
    }
}
