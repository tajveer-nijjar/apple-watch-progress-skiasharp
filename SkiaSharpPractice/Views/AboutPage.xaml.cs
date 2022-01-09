using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.IO;
using System.Reflection;

namespace SkiaSharpPractice.Views
{
    public partial class AboutPage : ContentPage
    {
        private SKPaint _greenGroundPaint, _blueCenterColor, _pitchColor;
        private SKRect _pitchRectangle;
        private SKMatrix _matrix;
        private SKPaint _whiteStrokePaint; //For hours, minutes and seconds hand.
        private SKPaint _whiteFillColor; //Four minute and hour dots.

        public AboutPage()
        {
            InitializeComponent();

            _greenGroundPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Green
            };

            _blueCenterColor = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Blue
            };

            _pitchColor = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.White
            };

            _whiteStrokePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.White,
                StrokeWidth = 2,
                StrokeCap = SKStrokeCap.Round,
                IsAntialias = true
            };

            _matrix = SKMatrix.CreateIdentity();

            //Refreshing the screen 60 times a second, which is a typical refresh rate for video display.
            Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () =>
            {
                canvasView.InvalidateSurface();
                return true;
            });

            _whiteFillColor = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.White
            };
        }

        private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;

            canvas.Clear(SKColors.CornflowerBlue);

            int width = e.Info.Width;
            int height = e.Info.Height;

            // Green big ground.
            canvas.Translate(width / 2, height / 2); // Moving the x, y axis to the middle of the screen.
            canvas.Scale(width / 210f);
            canvas.DrawCircle(new SKPoint(0, 0), 100, _greenGroundPaint);

            // Blue circle.
            canvas.Scale(1.5f);
            canvas.DrawCircle(new SKPoint(0, 0), 20, _blueCenterColor);

            //White pitch.
            _pitchRectangle = SKRect.Create(-5, -8, 10, 16);
            canvas.DrawRect(_pitchRectangle, _pitchColor);

            // Hour and minute marks
            // Dots are drawn only once, hence no need for saving and restoring the canvas.
            for(int angle = 0; angle < 360; angle += 6)
            {
                var radius = angle % 30 == 0 ? 2 : 1;
                canvas.DrawCircle(0, -60, radius, _whiteFillColor);
                canvas.RotateDegrees(6);
            }


            // Hour hand
            // 30 degrees per hour + 1 degrees every 2 minutes.
            canvas.Save();
            canvas.RotateDegrees(30 * DateTime.Now.Hour + DateTime.Now.Minute / 2f);
            _whiteStrokePaint.StrokeWidth = 10;
            canvas.DrawLine(0, 0, 0, -40, _whiteStrokePaint);
            canvas.Restore();

            // Minute hand.
            canvas.Save();
            canvas.RotateDegrees(6 * DateTime.Now.Minute + DateTime.Now.Second / 10f);
            _whiteStrokePaint.StrokeWidth = 7;
            canvas.DrawLine(0, 0, 0, -50, _whiteStrokePaint);
            canvas.Restore();

            // Second hand
            canvas.Save();
            float seconds = DateTime.Now.Second + DateTime.Now.Millisecond / 1000f;
            canvas.RotateDegrees(6 * seconds);
            _whiteStrokePaint.StrokeWidth = 2;
            canvas.DrawLine(0, 10, 0, -60, _whiteStrokePaint);
            canvas.Restore();

        }
    }
}