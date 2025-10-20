using System;
using System.Windows;

namespace NekoBeats
{
    public partial class MainWindow : Window
    {
        public static double AudioSensitivity { get; private set; } = 1.0;

        public MainWindow()
        {
            InitializeComponent();
            SensitivityValueText.Text = $"Current: {(int)(AudioSensitivity * 100)}%";
        }

        private void SensitivitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AudioSensitivity = e.NewValue;
            SensitivityValueText.Text = $"Current: {(int)(e.NewValue * 100)}%";
            StatusText.Text = $"🎵 Sensitivity: {(int)(e.NewValue * 100)}%";
        }

        private void GradientBars_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.CurrentTheme = new GradientBarsTheme();
            StatusText.Text = "🎵 Theme: Gradient Bars";
        }

        private void Purrticles_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.CurrentTheme = new PurrticlesTheme();
            StatusText.Text = "🎵 Theme: Purr-ticles";
        }

        private void RetroArcade_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.CurrentTheme = new RetroArcadeTheme();
            StatusText.Text = "🎵 Theme: Retro Arcade";
        }

        private void SpaceNebula_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.CurrentTheme = new SpaceNebulaTheme();
            StatusText.Text = "🎵 Theme: Space Nebula";
        }

        protected override void OnClosed(EventArgs e)
        {
            App.Visualizer?.Close();
            base.OnClosed(e);
        }
    }
}
