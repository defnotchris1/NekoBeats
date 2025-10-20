using System;
using System.Windows;
using System.Windows.Media;

namespace NekoBeats
{
    public class RetroArcadeTheme : ITheme
    {
        public string Name => "🎮 Retro Arcade - Beat Synced";
        private DateTime lastBeatTime = DateTime.Now;
        private double beatIntensity = 0;

        public void Render(DrawingContext dc, float[] frequencies, double[] fft)
        {
            if (frequencies == null || frequencies.Length == 0) return;

            double screenWidth = 1920;
            double screenHeight = 1080;
            int barCount = 32;
            double barWidth = screenWidth / barCount;
            double maxHeight = screenHeight * 0.9;

            // Detect beat
            bool hasBeat = frequencies.Length > 0 && frequencies[0] > 50;
            double currentIntensity = frequencies[0] / 100.0;

            if (hasBeat && (DateTime.Now - lastBeatTime).TotalMilliseconds > 200)
            {
                lastBeatTime = DateTime.Now;
                beatIntensity = 1.0;
            }
            else
            {
                beatIntensity = Math.Max(0, beatIntensity - 0.05);
            }

            double overallIntensity = Math.Max(currentIntensity, beatIntensity);

            for (int i = 0; i < barCount; i++)
            {
                // Different patterns for each bar
                double pattern1 = (Math.Sin(DateTime.Now.Millisecond * 0.018 + i * 0.6) + 1) * 0.4;
                double pattern2 = (Math.Cos(DateTime.Now.Millisecond * 0.022 + i * 0.9) + 1) * 0.3;
                double height = ((pattern1 + pattern2) / 2 * overallIntensity) * maxHeight;

                if (overallIntensity > 0.1)
                {
                    height = Math.Max(height, maxHeight * 0.08);
                }

                double x = i * barWidth;
                double y = screenHeight - height;

                // Retro colors that pulse with beat
                var colors = new[]
                {
                    Color.FromRgb((byte)(255 * overallIntensity), 0, 0),
                    Color.FromRgb(0, (byte)(255 * overallIntensity), 0),
                    Color.FromRgb(0, 0, (byte)(255 * overallIntensity)),
                    Color.FromRgb((byte)(255 * overallIntensity), (byte)(255 * overallIntensity), 0),
                };
                var color = colors[i % colors.Length];

                var brush = new SolidColorBrush(color);
                dc.DrawRectangle(brush, new Pen(Brushes.White, 1), new Rect(x, y, barWidth - 2, height));
            }

            // Pulsing border based on beat
            var borderPen = new Pen(new SolidColorBrush(Color.FromRgb((byte)(255 * beatIntensity), 255, 255)), 3);
            dc.DrawRectangle(null, borderPen, new Rect(10, 10, screenWidth - 20, screenHeight - 20));
        }
    }
}
