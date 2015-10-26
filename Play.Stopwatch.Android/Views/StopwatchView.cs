using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;

namespace Play.Stopwatch.Android.Views
{
    [Register("play.stopwatch.views.StopwatchView")] 
    public sealed class StopwatchView : View
    {
        private int _seconds;
        private int _minutes;

        private Path _minuteHand;
        private Path _secondHand;

        private static readonly DashPathEffect MinuteDashEffect = new DashPathEffect(new[] {0.1f, 3*(float) Math.PI - 0.1f}, 10);
        private static readonly DashPathEffect FifthMinuteDashEffect = new DashPathEffect(new[] {0.1f, 15*(float) Math.PI - 0.1f}, 0);

        public StopwatchView(Context context) : base(context)
        {
            Initialize();
        }

        public StopwatchView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        public StopwatchView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Initialize();
        }

        public void Initialize()
        {
            _minuteHand = new Path();
            _minuteHand.MoveTo(0, 10);
            _minuteHand.LineTo(0, -55);
            _minuteHand.Close();

            _secondHand = new Path();
            _secondHand.MoveTo(0, -80);
            _secondHand.LineTo(0, 15);
            _secondHand.Close();
        }

        public void Refresh(int minutes, int seconds)
        {
            if (seconds != _seconds)
                Invalidate();

            _seconds = seconds;
            _minutes = minutes;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            
            var paint = new Paint
            {
                Color = new Color(45, 45, 45)
            };

            canvas.DrawPaint(paint);

            DrawStopwatch(canvas);
            DrawSecondsHand(canvas);
            DrawMinutesHand(canvas);
        }

        public void DrawStopwatch(Canvas canvas)
        {
            canvas.Save();
            canvas.Translate(Width / 2F, Height / 2F);
            
            var tickMarks = new Path();
            tickMarks.AddCircle(0, 0, 90, Path.Direction.Cw);

            var scale = Math.Min(Width, Height) / 2F / 120;
            canvas.Scale(scale, scale);

            var paint = new Paint
            {
                StrokeCap = Paint.Cap.Square,
                Color = new Color(240, 240, 240)
            };

            paint.SetStyle(Paint.Style.Stroke);
            paint.StrokeWidth = 3;
            paint.SetPathEffect(MinuteDashEffect);
            canvas.DrawPath(tickMarks, paint);

            paint.Color = new Color(240, 240, 240);
            paint.StrokeWidth = 4;
            paint.SetPathEffect(FifthMinuteDashEffect);
            canvas.DrawPath(tickMarks, paint);
        }

        public void DrawSecondsHand(Canvas canvas)
        {
            canvas.Save();
            canvas.Rotate(_seconds * 6);
            
            var paint = new Paint
            {
                Color = Color.Red,
                StrokeWidth = 2
            };
            paint.SetStyle(Paint.Style.Stroke);

            canvas.DrawPath(_secondHand, paint);
            canvas.Restore();
        }

        public void DrawMinutesHand(Canvas canvas)
        {
            canvas.Save();
            canvas.Rotate(_minutes * 6);
            
            var paint = new Paint
            {
                Color = new Color(206, 206, 206),
                StrokeWidth = 4
            };
            paint.SetStyle(Paint.Style.Stroke);

            canvas.DrawPath(_minuteHand, paint);
            canvas.Restore();
        }
    }
}
