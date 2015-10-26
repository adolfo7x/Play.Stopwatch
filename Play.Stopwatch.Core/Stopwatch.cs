using System;
using System.Threading.Tasks;

namespace Play.Stopwatch.Core
{
    public class Stopwatch
    {
        public event EventHandler<TimeSpan> TimeChangedEvent;

        private bool _isRunning;
        private DateTime _startTime;
        private TimeSpan _elapsedTime;

        public TimeSpan ElapsedTime
        {
            get
            {
                return _elapsedTime;
            }

            private set
            {
                _elapsedTime = value;

                TimeChangedEvent?.Invoke(this,  _elapsedTime);
            }
        }

        public void Start()
        {
            _isRunning = true;
            _startTime = DateTime.Now;

            StartTimer();
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Reset()
        {
            _isRunning = false;
            ElapsedTime = TimeSpan.Zero;
        }

        async void StartTimer()
        {
            while (true)
            {
                await Task.Delay(50);

                if (!_isRunning)
                    break;

                UpdateTimespan();
            }
        }

        public void UpdateTimespan()
        {
            var now = DateTime.Now;

            ElapsedTime += (now - _startTime);
            _startTime = now;
        }
    }
}

