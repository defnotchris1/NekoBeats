using System;
using System.Windows;
using System.Windows.Media;

namespace NekoBeats
{
    public class GradientBarsTheme : ITheme
    {
        public string Name => "🌈 Gradient Bars";

        public void Render(DrawingContext dc, float[] frequencies, double[] fft)
        {
            if (frequencies == null || frequencies.Length == 0) return;

            double screenWidth = 1920;
            double screenHeight = 1080;
            int barCount = 32;
            double barWidth = screenWidth / barCount;
            double maxHeight = screenHeight * 0.8;

            for (int i = 0; i < barCount; i++)
            {
                // DISTRIBUTE FREQUENCIES ACROSS BARS PROPERLY
                int freqIndex = (i * frequencies.Length) / barCount;
                double frequencyValue = frequencies[Math.Min(freqIndex, frequencies.Length - 1)];
                
                // Apply sensitivity and ensure minimum movement
                double adjustedValue = frequencyValue * 0.5; // 50% sensitivity
                double height = (adjustedValue / 100.0) * maxHeight;
                
                // Ensure bars are visible even with low audio
                height = Math.Max(height, maxHeight * 0.05); // 5% minimum
                
                double x = i * barWidth;
                double y = screenHeight - height;

                // Gradient based on intensity
                var intensity = Math.Min(adjustedValue / 100.0, 1.0);
                var color1 = Color.FromRgb((byte)(0xff * intensity), (byte)(0x6a * intensity), 0x00);
                var color2 = Color.FromRgb(0x9c, 0x27, (byte)(0xb0 * intensity));

                var brush = new LinearGradientBrush(color1, color2, 90);
                dc.DrawRectangle(brush, null, new Rect(x, y, barWidth - 1, height));
            }

            // Debug info
            var debugText = new FormattedText($"Freqs: {frequencies.Length} | First: {frequencies[0]:F1}",
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"), 14, Brushes.White);
            dc.DrawText(debugText, new Point(20, 20));
        }
    }
}
