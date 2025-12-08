using CatenaLogic.Windows.Presentation.WebcamPlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiveScreenCapture
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			m_testWindow = new TestWindow();
			m_testWindow.ShowFrames();

			m_visibleTestWindow = new TestWindow();
			m_testWindow.FrameArrived += TestWindow_FrameArrived;
			m_testWindow.FPSStatUpdated += TestWindow_FPSStatUpdated;

			MonikerComboBox.ItemsSource = CapDevice.DeviceMonikers;
			MonikerComboBox.SelectedItem = CapDevice.DeviceMonikers.FirstOrDefault();
		}

		private void TestWindow_FPSStatUpdated(object sender, int e)
		{
			Dispatcher.Invoke(() =>
			{
				ActualFPSTextBox.Text = e.ToString();
			});
		}

		private void TestWindow_FrameArrived(object sender, BitmapSource e)
		{
			BorderImage.Source = e;
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			m_testWindow.Close();
			m_visibleTestWindow.Close();
		}

		private void UpdateColorButton_Click(object sender, RoutedEventArgs e)
		{
			m_colorI = (m_colorI + 1) % s_colors.Count;
			m_testWindow.UpdateColor(s_colors[m_colorI]);
			m_visibleTestWindow.UpdateColor(s_colors[m_colorI]);
		}

		private void ToggleTestWindowButton_Click(object sender, RoutedEventArgs e)
		{
			if (m_visibleTestWindow.Visibility == Visibility.Visible)
				m_visibleTestWindow.Hide();
			else
				m_visibleTestWindow.Show();
		}

		private void StartCameraButton_Click(object sender, RoutedEventArgs e)
		{
			if (m_selection != null)
				m_testWindow.StartCamera(m_selection);
		}

		private void StopCameraButton_Click(object sender, RoutedEventArgs e)
		{
			m_testWindow.StopCamera();
		}

		int m_colorI;
		static readonly IReadOnlyList<Brush> s_colors = new[] { Brushes.Transparent, Brushes.Black, Brushes.White, Brushes.Silver, Brushes.CadetBlue, Brushes.DarkCyan, }.ToList().AsReadOnly();
		TestWindow m_testWindow;
		TestWindow m_visibleTestWindow;
		FilterInfo m_selection;

		private void MonikerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			m_selection = MonikerComboBox.SelectedItem as FilterInfo;
		}

		private void FPSTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (int.TryParse(FPSTextBox.Text, out var fps) && fps > 0)
			{
				m_testWindow?.SetFPS(fps);
				FPSTextBox.BorderBrush = Brushes.Black;
				FPSTextBox.BorderThickness = new Thickness(1.0);
			}
			else
			{
				FPSTextBox.BorderBrush = Brushes.Red;
				FPSTextBox.BorderThickness = new Thickness(2.0);
			}
		}
	}
}
