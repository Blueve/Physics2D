using System;

namespace WPFDemo
{
    public class TimeTracker
    {
        private DateTime lastTime;
        private double deltaTime;
        private double timerInterval = -1;

        public double TimerInterval
        {
            get
            {
                return timerInterval;
            }
            set
            {
                timerInterval = value;
            }
        }

        public DateTime ElapsedTime
        {
            get
            {
                return lastTime;
            }
        }

        public double DeltaSeconds
        {
            get
            {
                return deltaTime;
            }
        }

        public event EventHandler TimerFired;

        public TimeTracker()
        {
            lastTime = DateTime.Now;
        }

        public double Update()
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan diffTime = currentTime - lastTime;

            deltaTime = diffTime.TotalSeconds;
            if (timerInterval > 0.0)
            {
                if (currentTime != lastTime)
                {
                    TimerFired(this, null);
                }
            }
            lastTime = currentTime;

            return deltaTime;
        }
    }
}