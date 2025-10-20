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
            if (frequencies == null) return;

            double screenWidth = 1920;
            double screenHeight = 1080;
            int barCount = 32;
            double barWidth = screenWidth / barCount;
            double maxHeight = screenHeight * 0.9;

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
                
                // Ensure all bars move
                if (adjustedValue > 3)
                {
                    adjustedValue = Math.Max(adjustedValue, 10);
                }

                double height = (adjustedValue / 100.0) * maxHeight;
                double x = i * barWidth;
                double y = screenHeight - height;

                // 8-bit style colors with intensity-based pulsing
                var intensity = Math.Min(adjustedValue / 100.0, 1.0);
                var colors = new[]
                {
                    Color.FromRgb((byte)(255 * intensity), 0, 0),
                    Color.FromRgb(0, 0, (byte)(255 * intensity)),
                    Color.FromRgb(0, (byte)(255 * intensity), 0),
                    Color.FromRgb((byte)(255 * intensity), (byte)(255 * intensity), 0),
                    Color.FromRgb((byte)(255 * intensity), 0, (byte)(255 * intensity)),
                    Color.FromRgb(0, (byte)(255 * intensity), (byte)(255 * intensity)),
                    Color.FromRgb((byte)(255 * intensity), (byte)(255 * intensity), (byte)(255 * intensity))
                };
                var color = colors[i % colors.Length];

                // Pixelated look with larger blocks
                var brush = new SolidColorBrush(color);
                var pen = new Pen(Brushes.Black, 1);

                // Draw larger pixel blocks
                for (double blockY = y; blockY < screenHeight; blockY += 8)
                {
                    dc.DrawRectangle(brush, pen, new Rect(x, blockY, barWidth - 2, 6));
                }

                // Draw retro scan lines
                if (i % 4 == 0 && intensity > 0.2)
                {
                    var scanBrush = new SolidColorBrush(Color.FromArgb((byte)(100 * intensity), 255, 255, 255));
                    dc.DrawLine(new Pen(scanBrush, 1), 
                        new Point(0, y), new Point(screenWidth, y));
                }
            }

            // Draw pulsing retro border
            double overallIntensity = 0;
            for (int i = 0; i < Math.Min(frequencies.Length, 10); i++)
            {
                overallIntensity += frequencies[i];
            }
            overallIntensity = Math.Min(overallIntensity / 1000.0, 1.0);
            
            var borderColor = Color.FromRgb((byte)(255 * overallIntensity), (byte)(255 * overallIntensity), 255);
            var borderPen = new Pen(new SolidColorBrush(borderColor), 3 + (overallIntensity * 2));
            dc.DrawRectangle(null, borderPen, new Rect(10, 10, screenWidth - 20, screenHeight - 20));

            // Draw pulsing title
            var pulse = (Math.Sin(DateTime.Now.Millisecond * 0.01) + 1) * 0.5;
            var titleBrush = new SolidColorBrush(Color.FromRgb((byte)(255 * pulse), (byte)(100 * pulse), 255));
            var titleText = new FormattedText("NEKO BEATS", 
                System.Globalization.CultureInfo.CurrentCulture, 
                FlowDirection.LeftToRight,
                new Typeface("Courier New"), 36, titleBrush);
            dc.DrawText(titleText, new Point(screenWidth / 2 - 150, 30));
        }
    }
}
