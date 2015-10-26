using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Play.Stopwatch.Views;
using Play.Stopwatch.Core;

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

        private readonly Core.Stopwatch _stopwatch = new Core.Stopwatch();

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

            _stopwatch.TimeChangedEvent += RefreshTimer;
        }

        private void Start(object o, EventArgs args)
        {
            _stopwatch.Start();
            _stopButton.Enabled = true;
            _startButton.Enabled = false;
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
        }

        private void RefreshTimer(object stopwatch, TimeSpan elapsedTime)
        {
            _stopwatchView.Refresh((int)Math.Floor(elapsedTime.TotalMinutes), elapsedTime.Seconds);

            _digitalTimer.Text = $"{Math.Floor(elapsedTime.TotalMinutes).ToString("00")}:" +
                                 $"{elapsedTime.TotalSeconds.ToString("00.000")}";
        }
    }
}

