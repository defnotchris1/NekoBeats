using System;
using System.Windows;
using System.Windows.Media;

namespace NekoBeats
{
    public class RetroArcadeTheme : ITheme
    {
        public string Name => "🎮 Retro Arcade";

        public void Render(DrawingContext dc, float[] frequencies, double[] fft)
        {
            if (frequencies == null || frequencies.Length == 0) return;

            double screenWidth = 1920;
            double screenHeight = 1080;
            int barCount = 32;
            double barWidth = screenWidth / barCount;
            double maxHeight = screenHeight * 0.9;

            for (int i = 0; i < barCount; i++)
            {
                // DISTRIBUTE FREQUENCIES ACROSS BARS
                int freqIndex = (i * frequencies.Length) / barCount;
                double frequencyValue = frequencies[Math.Min(freqIndex, frequencies.Length - 1)];
                
                double adjustedValue = frequencyValue * 0.5; // 50% sensitivity
                double height = (adjustedValue / 100.0) * maxHeight;
                height = Math.Max(height, maxHeight * 0.05); // 5% minimum
                
                double x = i * barWidth;
                double y = screenHeight - height;

                // 8-bit colors based on intensity
                var intensity = Math.Min(adjustedValue / 100.0, 1.0);
                var colors = new[]
                {
                    Color.FromRgb((byte)(255 * intensity), 0, 0),
                    Color.FromRgb(0, 0, (byte)(255 * intensity)),
                    Color.FromRgb(0, (byte)(255 * intensity), 0),
                    Color.FromRgb((byte)(255 * intensity), (byte)(255 * intensity), 0),
                };
                var color = colors[i % colors.Length];

                var brush = new SolidColorBrush(color);
                dc.DrawRectangle(brush, new Pen(Brushes.Black, 1), new Rect(x, y, barWidth - 2, height));
            }

            var debugText = new FormattedText($"Retro - Freqs: {frequencies.Length}",
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"), 14, Brushes.White);
            dc.DrawText(debugText, new Point(20, 40));
        }
    }
}
