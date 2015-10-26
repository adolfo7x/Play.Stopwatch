using System;
using System.Reactive.Linq;
using Android.App;
using Android.OS;
using Android.Widget;
using Play.Stopwatch.Android.Views;
using Play.Stopwatch.Core;

namespace Play.Stopwatch.Android
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

            Observable
                .FromEventPattern<EventArgs>(_startButton, "Click")
                .Subscribe(evt => _stopwatch.Start());

            Observable
                .FromEventPattern<EventArgs>(_stopButton, "Click")
                .Subscribe(evt => _stopwatch.Stop());

            Observable
                .FromEventPattern<EventArgs>(_resetButton, "Click")
                .Subscribe(evt => _stopwatch.Reset());

            _stopwatch.TimeChanged.Subscribe(OnTimerChange);
            _stopwatch.StatusChanged.Subscribe(OnStatusChange);
        }

        private void OnStatusChange(StopwatchStatus status)
        {
            _stopButton.Enabled = (status == StopwatchStatus.Started);
            _startButton.Enabled = (status == StopwatchStatus.Stopped);
        }

        private void OnTimerChange(TimeSpan elapsedTime)
        {
            _stopwatchView.Refresh(elapsedTime.Minutes, elapsedTime.Seconds);

            _digitalTimer.Text = $"{Math.Floor(elapsedTime.TotalMinutes).ToString("00")}:" +
                                 $"{elapsedTime.Seconds.ToString("00")}." +
                                 $"{elapsedTime.Milliseconds.ToString("000")}";
        }
    }
}

