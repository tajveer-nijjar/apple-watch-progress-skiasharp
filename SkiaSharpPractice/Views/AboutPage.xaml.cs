using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.IO;
using System.Reflection;
using TouchTracking;

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
        private bool showFill = true;
        private float x = 10, y = 40;

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
            //Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () =>
            //{
            //    canvasView.InvalidateSurface();
            //    return true;
            //});

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
                //var canvas = new SKCanvas(_savedBitmap);
                //canvas.DrawLine(0, 0, 10, 60, _whiteStrokePaint);

                showFill = !showFill;
                canvasView.InvalidateSurface();
            }
            catch (Exception e2)
            {
                var x = e2;
            }
        }

        private void OnTouchEffectAction(object sender, TouchTracking.TouchActionEventArgs args)
        {
            try
            {
                showFill = !showFill;

                //x = args.Location.X;
                //y = args.Location.X;

                var point = ConvertToPixel(args.Location);
                x = point.X;
                y = point.Y;

                canvasView.InvalidateSurface();
            }
            catch (Exception e)
            {
                var x = 10;
            }
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;

            int width = e.Info.Width;
            int height = e.Info.Height;

            canvas.Clear(SKColors.CornflowerBlue);


            // Green big ground.
            //canvas.Translate(width / 2, height / 2); // Moving the x, y axis to the middle of the screen.
            var scaleFactor = width / 210f;
            //canvas.Scale(scaleFactor);
            // To accommodate the cat
            //canvas.Scale(Math.Min(width / 210f, height / 520f));
            DrawGround(canvas);

            //canvas.DrawLine(0, 0,50.8182f, 150.9091f, _whiteStrokePaint);
            //canvas.DrawLine(259.6364f, 179.6364f, 0, 0, _whiteStrokePaint);

            canvas.DrawLine(width / 2, height / 2, x, y, _whiteStrokePaint);
            //canvas.DrawLine(0, 0, x, y, _whiteStrokePaint);
        }

        private void DrawGround(SKCanvas canvas)
        {
            canvas.DrawCircle(new SKPoint(0, 0), 100, _greenGroundPaint);

            // Blue circle.
            //canvas.Scale(1.5f);
            canvas.DrawCircle(new SKPoint(0, 0), 20, _blueCenterColor);

            //White pitch.
            _pitchRectangle = SKRect.Create(-5, -8, 10, 16);
            canvas.DrawRect(_pitchRectangle, _pitchColor);
        }

        SKPoint ConvertToPixel(TouchTrackingPoint pt)
        {
            var point = new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                               (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
            Console.WriteLine($"PT   : X={pt.X}, Y={pt.Y} \n");
            Console.WriteLine($"Point: {point}\n");

            return point;
        }
    }
}