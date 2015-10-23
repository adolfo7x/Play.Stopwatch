using System;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Android.OS;
using Play.Stopwatch.Views;

namespace Play.Stopwatch
{
    [Activity(Label = "Play.Stopwatch", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        TextView _digitalTimer;
        StopwatchView _stopwatchView;

        Button _startButton;
        Button _stopButton;
        Button _resetButton;

        private readonly System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            _digitalTimer = FindViewById<TextView>(Resource.Id.DigitalTimer);
            _stopwatchView = FindViewById<StopwatchView>(Resource.Id.Stopwatch);

            _startButton = FindViewById<Button>(Resource.Id.StartButton);
            _stopButton = FindViewById<Button>(Resource.Id.StopButton);
            _resetButton = FindViewById<Button>(Resource.Id.ResetButton);

            _startButton.Click += Start;
            _stopButton.Click += Stop;
            _resetButton.Click += Reset;

            _stopButton.Enabled = false;
        }

        private void Start(object o, EventArgs args)
        {
            _stopwatch.Start();
            _stopButton.Enabled = true;
            _startButton.Enabled = false;

            Timer();
        }

        private void Stop(object o, EventArgs args)
        {
            _stopwatch.Stop();
            _stopButton.Enabled = false;
            _startButton.Enabled = true;
        }

        private void Reset(object o, EventArgs args)
        {
            _stopwatch.Reset();
            _stopButton.Enabled = false;
            _startButton.Enabled = true;

            RefreshTimer();
        }

        async void Timer()
        {
            while (true)
            {
                await Task.Delay(50);

                RefreshTimer();

                if (!_stopwatch.IsRunning) break;
            }
        }

        private void RefreshTimer()
        {
            var span = TimeSpan.FromMilliseconds(_stopwatch.ElapsedMilliseconds);

            _stopwatchView.Refresh((int)Math.Floor(span.TotalMinutes), span.Seconds);

            _digitalTimer.Text = $"{Math.Floor(span.TotalMinutes).ToString("00")}:" +
                                 $"{span.Seconds.ToString("00")}." +
                                 $"{span.Milliseconds.ToString("000")}";
        }
    }
}

