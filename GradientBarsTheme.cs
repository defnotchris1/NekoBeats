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
            int barCount = 32; // Fixed number of bars for consistency
            double barWidth = screenWidth / barCount;
            double maxHeight = screenHeight * 0.8;

            // Apply sensitivity reduction to 50%
            double sensitivity = 0.5;

            for (int i = 0; i < barCount; i++)
            {
                // Each bar moves differently based on its frequency band
                double movementFactor = 1.0 + (Math.Sin(i * 0.3) * 0.3);
                
                // Use FFT data if available, otherwise use frequencies with distribution
                double adjustedValue;
                if (fft != null && fft.Length > i)
                {
                    adjustedValue = fft[i] * sensitivity * movementFactor;
                }
                else
                {
                    // Distribute frequency data across bars
                    int freqIndex = (i * frequencies.Length) / barCount;
                    double baseValue = frequencies[Math.Min(freqIndex, frequencies.Length - 1)];
                    adjustedValue = baseValue * sensitivity * movementFactor * (1.0 - (i * 0.02));
                }
                
                // Ensure all bars move (minimum height when there's any audio)
                if (adjustedValue > 3)
                {
                    adjustedValue = Math.Max(adjustedValue, 8);
                }

                double height = (adjustedValue / 100.0) * maxHeight;
                double x = i * barWidth;
                double y = screenHeight - height;

                // Dynamic gradient that pulses with intensity
                var intensity = Math.Min(adjustedValue / 100.0, 1.0);
                var color1 = Color.FromRgb((byte)(0xff * intensity), (byte)(0x6a * intensity), 0x00);
                var color2 = Color.FromRgb(0x9c, 0x27, (byte)(0xb0 * intensity));

                var brush = new LinearGradientBrush(color1, color2, 90);
                dc.DrawRectangle(brush, null, new Rect(x, y, barWidth - 1, height));
            }

            // Debug info
            var debugText = new FormattedText($"Bars: {barCount} | Freqs: {frequencies.Length} | FFT: {(fft != null ? fft.Length : 0)}",
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"), 14, Brushes.White);
            dc.DrawText(debugText, new Point(20, 20));
        }
    }
}
