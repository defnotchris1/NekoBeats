using System;
using System.Windows;
using System.Windows.Media;

namespace NekoBeats
{
    public class GradientBarsTheme : ITheme
    {
        public string Name => "🌈 Gradient Bars - FIXED";
        private Random random = new Random();

        public void Render(DrawingContext dc, float[] frequencies, double[] fft)
        {
            double screenWidth = 1920;
            double screenHeight = 1080;
            int barCount = 32;
            double barWidth = screenWidth / barCount;
            double maxHeight = screenHeight * 0.8;

            for (int i = 0; i < barCount; i++)
            {
                // FORCE VISIBLE BARS - TEST WITH SYNTHETIC DATA
                double syntheticValue = (Math.Sin(DateTime.Now.Millisecond * 0.01 + i * 0.3) + 1) * 50;
                double realValue = frequencies != null && frequencies.Length > 0 ? frequencies[0] : 0;
                double height = Math.Max(syntheticValue, realValue) / 100.0 * maxHeight;
                
                // ENSURE MINIMUM HEIGHT
                height = Math.Max(height, 20);
                
                double x = i * barWidth;
                double y = screenHeight - height;

                var color1 = Colors.Red;
                var color2 = Colors.Blue;
                var brush = new LinearGradientBrush(color1, color2, 90);
                
                // DRAW THICK VISIBLE RECTANGLES
                dc.DrawRectangle(brush, new Pen(Brushes.White, 2), new Rect(x, y, barWidth - 2, height));
            }

            // BIG VISIBLE DEBUG TEXT
            var debugText = new FormattedText($"BARS SHOULD BE VISIBLE! Freqs: {frequencies?.Length}",
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"), 24, Brushes.Red);
            dc.DrawText(debugText, new Point(20, 20));
        }
    }
}
