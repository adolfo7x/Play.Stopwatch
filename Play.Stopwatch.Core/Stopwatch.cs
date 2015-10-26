using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Play.Stopwatch.Core
{
    public class Stopwatch
    {
        public IObservable<TimeSpan> TimeChanged => _timeChanged.AsObservable();
        private readonly Subject<TimeSpan> _timeChanged = new Subject<TimeSpan>();

        public IObservable<StopwatchStatus> StatusChanged => _statusChanged.AsObservable();
        private readonly Subject<StopwatchStatus> _statusChanged = new Subject<StopwatchStatus>();
        
        private readonly System.Diagnostics.Stopwatch _watch = new System.Diagnostics.Stopwatch();
        
        public void Start()
        {
            _watch.Start();
            _statusChanged.OnNext(StopwatchStatus.Started);

            StartTimer();
        }

        public void Stop()
        {
            _watch.Stop();
            _statusChanged.OnNext(StopwatchStatus.Stopped);
        }

        public void Reset()
        {
            _watch.Reset();
            _statusChanged.OnNext(StopwatchStatus.Stopped);

            _timeChanged.OnNext(TimeSpan.Zero);   
        }

        async void StartTimer()
        {
            while (true)
            {
                await Task.Delay(50);

                if (!_watch.IsRunning)
                    break;

                _timeChanged.OnNext(TimeSpan.FromMilliseconds(_watch.ElapsedMilliseconds));
            }
        }
    }
}

