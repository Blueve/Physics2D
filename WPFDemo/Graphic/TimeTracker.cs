using System;

namespace WPFDemo
{
    public class TimeTracker
    {
        public double TimerInterval { get; set; } = -1;

        public DateTime ElapsedTime { get; private set; }

        public double DeltaSeconds { get; private set; }

        public event EventHandler TimerFired;

        public TimeTracker()
        {
            ElapsedTime = DateTime.Now;
        }

        public double Update()
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan diffTime = currentTime - ElapsedTime;

            DeltaSeconds = diffTime.TotalSeconds;
            if (TimerInterval > 0.0)
            {
                if (currentTime != ElapsedTime)
                {
                    TimerFired(this, null);
                }
            }
            ElapsedTime = currentTime;

            return DeltaSeconds;
        }
    }
}