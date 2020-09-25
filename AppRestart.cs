using System;
using System.Diagnostics;
using System.Timers;

namespace AppRestarter
{
    public class Restarter
    {
        public Timer _runTime = new Timer(8000*60*60);
        private readonly Timer _timer;

        public Restarter()
        {
            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed += timerElapsed;
        }

        private void timerElapsed(object sender, ElapsedEventArgs e)
        {
            //TODO: monitor selected service / kill and restart after given time, rinse repeat
            var startTime = GetProcessStartTime("notepad++");
            Console.WriteLine(startTime);
            if (startTime >= _runTime)
            {
                killProcess("notepad++");
            }
            else
            {
                Console.WriteLine($"Process runtime left: {DateTime.Now-(startTime.AddSeconds(_runTime)) }");
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
            if (p.Length <= 0) throw new Exception("Process not found");
            return p[0].StartTime;
        }

        public void killProcess(string processName)
        {
            Process[] p = Process.GetProcessesByName("notepad++");
            if (p.Length <= 0) throw new Exception("Process not found");
            p[0].Kill();
        }

        public void restartProcess(string processName)
        {
        }
    }
}