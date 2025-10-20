using System;
using System.Windows;
using System.Windows.Input;

namespace NekoBeats
{
    public partial class MainWindow : Window
    {
        // Sensitivity property that themes can access
        public static double AudioSensitivity { get; private set; } = 1.0;

        public MainWindow()
        {
            InitializeComponent();
            // Set initial sensitivity text
            SensitivityValueText.Text = $"Current: {(int)(AudioSensitivity * 100)}%";
        }

        // Sensitivity slider handler
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

        private void DragVisualizer_Click(object sender, RoutedEventArgs e)
        {
            if (App.Visualizer != null)
            {
                App.Visualizer.DragMove();
            }
        }

        private void ToggleVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (App.Visualizer != null)
            {
                App.Visualizer.Visibility = App.Visualizer.Visibility == Visibility.Visible 
                    ? Visibility.Hidden 
                    : Visibility.Visible;
            }
        }

        private void ResetPosition_Click(object sender, RoutedEventArgs e)
        {
            if (App.Visualizer != null)
            {
                App.Visualizer.Left = 100;
                App.Visualizer.Top = 100;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            // Clean up when control panel closes
            App.Visualizer?.Close();
            base.OnClosed(e);
        }
    }
}
