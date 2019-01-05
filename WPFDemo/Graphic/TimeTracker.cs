namespace WPFDemo.Graphic
{
    using System;
    using System.Diagnostics;

    public class TimeTracker
    {
        public double TimerInterval { get; set; } = -1;

        public DateTime ElapsedTime { get; private set; }

        public double DeltaSeconds { get; private set; }

        public event EventHandler TimerFired;

        public TimeTracker()
        {
            this.ElapsedTime = DateTime.Now;
        }

        public double Update()
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan diffTime = currentTime - this.ElapsedTime;

            this.DeltaSeconds = diffTime.TotalSeconds;
            if (this.TimerInterval > 0)
            {
                if (currentTime != this.ElapsedTime)
                {
                    Debug.Assert(this.TimerFired != null, "TimerFired != null");
                    this.TimerFired(this, null);
                }
            }

            this.ElapsedTime = currentTime;

            return this.DeltaSeconds;
        }
    }
}