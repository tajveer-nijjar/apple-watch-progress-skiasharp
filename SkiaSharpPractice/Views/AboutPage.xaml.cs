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
        private SKBitmap _savedBitmap;

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

        private void OnCanvasViewTapped(object sender, EventArgs e)
        {
            try
            {

                var canvas = new SKCanvas(_savedBitmap);

                canvas.Clear();
                canvas.DrawLine(0, 0, 50.8182f, 150.9091f, _whiteStrokePaint);
                (sender as SKCanvasView).InvalidateSurface();
            }
            catch (Exception e2)
            {
                var x = e2;
            }
        }

        private void OnTouchEffectAction(object sender, TouchTracking.TouchActionEventArgs args)
        {
            Console.WriteLine(args.Type);

            try
            {
                //canvasView.InvalidateSurface();
                var canvas = new SKCanvas(_savedBitmap);

                canvas.Clear();
                canvas?.DrawLine(0, 0, args.Location.X, args.Location.Y, _whiteStrokePaint);

                canvasView.InvalidateSurface();
                UpdateBitmap();
            }
            catch (Exception e)
            {
                var x = 10;
            }
        }

        void UpdateBitmap()
        {
            using (SKCanvas saveBitmapCanvas = new SKCanvas(_savedBitmap))
            {
                saveBitmapCanvas.Clear();

                saveBitmapCanvas.DrawLine(0, 0, 10, 60, _whiteStrokePaint);

                //foreach (SKPath path in completedPaths)
                //{
                //    saveBitmapCanvas.DrawPath(path, paint);
                //}

                //foreach (SKPath path in inProgressPaths.Values)
                //{
                //    saveBitmapCanvas.DrawPath(path, paint);
                //}
            }

            canvasView.InvalidateSurface();
        }


        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;


            // Create bitmap the size of the display surface
            if (_savedBitmap == null)
            {
                _savedBitmap = new SKBitmap(info.Width, info.Height);
            }
            // Or create new bitmap for a new size of display surface
            else if (_savedBitmap.Width < info.Width || _savedBitmap.Height < info.Height)
            {
                SKBitmap newBitmap = new SKBitmap(Math.Max(_savedBitmap.Width, info.Width),
                                                  Math.Max(_savedBitmap.Height, info.Height));

                using (SKCanvas newCanvas = new SKCanvas(newBitmap))
                {
                    newCanvas.Clear();
                    newCanvas.DrawBitmap(_savedBitmap, 0, 0);
                }

                _savedBitmap = newBitmap;
            }

            // Render the bitmap
            canvas.DrawBitmap(_savedBitmap, 0, 0);

            canvas.Clear(SKColors.CornflowerBlue);

            int width = e.Info.Width;
            int height = e.Info.Height;

            // Green big ground.
            canvas.Translate(width / 2, height / 2); // Moving the x, y axis to the middle of the screen.
            //canvas.Scale(width / 210f);
            // To accommodate the cat
            canvas.Scale(Math.Min(width / 210f, height / 520f));
            canvas.DrawCircle(new SKPoint(0, 0), 100, _greenGroundPaint);

            // Blue circle.
            canvas.Scale(1.5f);
            canvas.DrawCircle(new SKPoint(0, 0), 20, _blueCenterColor);

            // Head of the cat.
            canvas.DrawCircle(0, -100, 50, _greenGroundPaint);

            //White pitch.
            _pitchRectangle = SKRect.Create(-5, -8, 10, 16);
            canvas.DrawRect(_pitchRectangle, _pitchColor);

            // Hour and minute marks
            // Dots are drawn only once, hence no need for saving and restoring the canvas.
            for (int angle = 0; angle < 360; angle += 6)
            {
                var radius = angle % 30 == 0 ? 2 : 1;
                canvas.DrawCircle(0, -60, radius, _whiteFillColor);
                canvas.RotateDegrees(6);
            }

            //canvas.DrawLine(0, 0,50.8182f, 150.9091f, _whiteStrokePaint);
            //canvas.DrawLine(259.6364f, 179.6364f, 0, 0, _whiteStrokePaint);


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
            canvas.DrawLine(0, 0, 10, 60, _whiteStrokePaint);
            canvas.Restore();
        }
    }
}