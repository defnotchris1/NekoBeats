using System;
using System.Windows;
using System.Windows.Media;

namespace NekoBeats
{
    public class GradientBarsTheme : ITheme
    {
        public string Name => "🌈 Gradient Bars - Beat Synced";
        private DateTime lastBeatTime = DateTime.Now;
        private double beatIntensity = 0;
        private Random random = new Random();

        public void Render(DrawingContext dc, float[] frequencies, double[] fft)
        {
            if (frequencies == null || frequencies.Length == 0) return;

            double screenWidth = 1920;
            double screenHeight = 1080;
            int barCount = 32;
            double barWidth = screenWidth / barCount;
            double maxHeight = screenHeight * 0.8;

            // Get sensitivity from MainWindow
            double sensitivity = MainWindow.AudioSensitivity;

            // Detect beat (bass frequency spike) - sensitivity affects threshold
            bool hasBeat = frequencies.Length > 0 && frequencies[0] > (50 / sensitivity);
            double currentIntensity = (frequencies[0] / 100.0) * sensitivity;

            // Update beat intensity
            if (hasBeat && (DateTime.Now - lastBeatTime).TotalMilliseconds > 200)
            {
                lastBeatTime = DateTime.Now;
                beatIntensity = 1.0 * sensitivity; // Sensitivity affects beat strength
            }
            else
            {
                // Gradually decay beat intensity
                beatIntensity = Math.Max(0, beatIntensity - 0.05);
            }

            // Combine current audio with beat intensity
            double overallIntensity = Math.Max(currentIntensity, beatIntensity);

            for (int i = 0; i < barCount; i++)
            {
                // Each bar has unique movement pattern based on beat + position
                double pattern1 = (Math.Sin(DateTime.Now.Millisecond * 0.02 + i * 0.5) + 1) * 0.3;
                double pattern2 = (Math.Cos(DateTime.Now.Millisecond * 0.015 + i * 0.7) + 1) * 0.2;
                double pattern3 = (Math.Sin(DateTime.Now.Millisecond * 0.025 + i * 1.2) + 1) * 0.25;
                
                // Combine patterns with beat intensity - APPLY SENSITIVITY HERE TOO
                double combinedPattern = (pattern1 + pattern2 + pattern3) / 3;
                double height = (combinedPattern * overallIntensity * sensitivity) * maxHeight;

                // Ensure minimum height when audio is detected
                if (overallIntensity > 0.1)
                {
                    height = Math.Max(height, maxHeight * 0.1);
                }

                double x = i * barWidth;
                double y = screenHeight - height;

                // Gradient based on beat intensity
                var color1 = Color.FromRgb((byte)(0xff * overallIntensity), (byte)(0x6a * overallIntensity), 0x00);
                var color2 = Color.FromRgb(0x9c, 0x27, (byte)(0xb0 * overallIntensity));

                var brush = new LinearGradientBrush(color1, color2, 90);
                dc.DrawRectangle(brush, null, new Rect(x, y, barWidth - 1, height));
            }

            // Debug info - show sensitivity too!
            var debugText = new FormattedText($"Beat: {beatIntensity:F2} | Sens: {sensitivity:F1}",
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"), 14, Brushes.White);
            dc.DrawText(debugText, new Point(20, 20));
        }
    }
}
