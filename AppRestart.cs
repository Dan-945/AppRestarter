using System;
using System.Diagnostics;
using System.Timers;

namespace AppRestarter
{
    public class Restarter
    {
        public DateTime _runTime = new DateTime();
        private readonly Timer _timer;
        public string _monitoredProcess = "notepad++";
        public string _processExePath = @"C:\Program Files (x86)\Notepad++\Notepad++.exe";
        public Restarter()
        {
            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            //TODO: monitor selected service / kill and restart after given time, rinse repeat
            var startTime = GetProcessStartTime(_monitoredProcess);
            _runTime = startTime.AddSeconds(8);
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

        //TODO: set max runtime from int in hours
        //TODO: read target service name from config?
        //TODO: read target total time from config?

        public DateTime GetProcessStartTime(string processName)
        {
            Console.WriteLine(processName);
            Process[] p = Process.GetProcessesByName(processName);
            if (p.Length <= 0)
            {
                Console.WriteLine("Process not found");
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
            Process.Start(processExePath);
        }
    }
}