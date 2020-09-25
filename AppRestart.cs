using System;
using System.Configuration;
using System.Diagnostics;
using System.Timers;

namespace AppRestarter
{
    public class Restarter
    {
        public DateTime _runTime = new DateTime();
        private readonly Timer _timer;
        public string _monitoredProcess = ConfigurationManager.AppSettings.Get("Process to monitor");
        public string _processExePath = @"" + ConfigurationManager.AppSettings.Get("Path to .exe file to restart:");
        public int _runtimeSeconds = Int32.Parse(ConfigurationManager.AppSettings.Get("RuntimeSeconds"));
        public Restarter()
        {
            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            //TODO: monitor selected service / kill and restart after given time, rinse repeat
            var startTime = GetProcessStartTime(_monitoredProcess);
            _runTime = startTime.AddSeconds(_runtimeSeconds);
            Console.WriteLine($"compare to value: {startTime.CompareTo(_runTime)}");
            if (DateTime.Now.CompareTo(_runTime) > 0)
            {
                killProcess(_monitoredProcess);
                restartProcess(_processExePath);
            }
            else
            {
                Console.WriteLine($"App start time: {startTime }");
                Console.WriteLine($"Time Now: {DateTime.Now }");
                Console.WriteLine($"Shutdown time: {_runTime }");
            }
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public DateTime GetProcessStartTime(string processName)
        {
            Console.WriteLine(processName);
            Process[] p = Process.GetProcessesByName(processName);
            if (p.Length <= 0)
            {
                Console.WriteLine("Process not running");
                return DateTime.Now;
            }
            //throw new Exception("Process not found");
            else
            {
                return p[0].StartTime;
            }
        }

        public void killProcess(string processName)
        {
            Process[] p = Process.GetProcessesByName(_monitoredProcess);

            if (p.Length <= 0) throw new Exception("Process not found");
            p[0].Kill();
        }
        public void restartProcess(string processExePath)
        {
            try
            {
                Process.Start(processExePath);
            }
            catch
            {
                Console.WriteLine("no exe file found");
            }

        }
    }
}