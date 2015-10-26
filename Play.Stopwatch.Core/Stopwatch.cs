using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Play.Stopwatch.Core
{
    public class Stopwatch
    {
        private readonly Subject<TimeSpan> _timeChanged = new Subject<TimeSpan>();

        private readonly System.Diagnostics.Stopwatch _watch = new System.Diagnostics.Stopwatch();

        public IObservable<TimeSpan> TimeChanged => _timeChanged.AsObservable();
        
        public void Start()
        {
            _watch.Start();

            StartTimer();
        }

        public void Stop()
        {
            _watch.Stop();
        }

        public void Reset()
        {
            _watch.Reset();
         
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

