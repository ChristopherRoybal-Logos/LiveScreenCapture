using CatenaLogic.Windows.Presentation.WebcamPlayer;
using LiveScreenCapture.Utilities;
using LiveScreenCapture.ViewModels;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CancellationTokenSource = System.Threading.CancellationTokenSource;

namespace LiveScreenCapture
{
	public partial class TestWindow : Window
	{
		public event EventHandler<BitmapSource> FrameArrived;
		public event EventHandler<int> FPSStatUpdated;

		public TestWindow()
		{
			InitializeComponent();
			m_targetFPS = 30;
			m_currentFPS = 30;
			m_frameTimer = new Timer(FramesPerSecondUtility.MillisecondsFromFPS(30));
			m_frameTimer.Elapsed += FrameTimer_Elapsed;
			m_size = new Size(1280, 720);
			AllowsTransparency = true;
			WindowStyle = WindowStyle.None;
			ViewModel = new TestWindowViewModel();
			DataContext = ViewModel;
			m_cts = new CancellationTokenSource();
			m_statTimer = new Timer(1000);
			m_statTimer.Elapsed += StatTimer_Elapsed;
		}

		private void StatTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			int fps = m_frameStatCount;
			m_frameStatCount = 0;
			FPSStatUpdated.Invoke(this, fps);

			m_shouldIncrease = fps < m_targetFPS;
			m_shouldDecrease = fps > m_targetFPS;
		}

		public TestWindowViewModel ViewModel { get; }

		public new void Show()
		{
			base.Show();
			WindowState = WindowState.Maximized;
		}

		public void SetFPS(double fps)
		{
			m_targetFPS = fps;
			m_newFPS = true;
		}

		public void ShowFrames()
		{
			m_statTimer.Start();
			m_frameTimer.Start();
		}

		public void StartCamera(FilterInfo info)
		{
			ViewModel.Start(info);
		}

		public void StopCamera()
		{
			ViewModel.Stop();
		}

		public void UpdateColor(Brush backgroundBrush)
		{
			TestWindowGrid.Background = backgroundBrush;
		}

		protected override void OnClosed(EventArgs e)
		{
			m_frameTimer.Stop();
			m_statTimer.Stop();
			StopCamera();
			m_cts.Cancel();
			base.OnClosed(e);
		}

		private void FrameTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			m_frameTimer.Stop();
			Dispatcher.Invoke(() =>
			{
				if (m_cts.IsCancellationRequested)
					return;


				if (m_newFPS)
				{
					m_currentFPS = m_targetFPS;
					RecreateFrameTimer();
				}
				else if (m_shouldDecrease)
				{
					m_shouldDecrease = false;
					m_currentFPS = Math.Max(1, m_currentFPS - 1);
					RecreateFrameTimer();
				}
				else if (m_shouldIncrease)
				{
					m_shouldIncrease = false;
					m_currentFPS = Math.Min(120, m_currentFPS + 1);
					RecreateFrameTimer();
				}

				GenerateBitmap();
				m_frameTimer.Start();
			});
		}
		
		private void RecreateFrameTimer()
		{
			m_frameTimer.Elapsed -= FrameTimer_Elapsed;
			m_frameTimer.Dispose();
			m_frameTimer = new Timer(FramesPerSecondUtility.MillisecondsFromFPS(m_currentFPS));
			m_frameTimer.Elapsed += FrameTimer_Elapsed;
		}

		private void GenerateBitmap()
		{
			if (Visibility != Visibility.Visible)
			{
				TestWindowGrid.Measure(m_size);
				TestWindowGrid.Arrange(new Rect(m_size));
				TestWindowGrid.UpdateLayout();
			}

			var rtb = new RenderTargetBitmap((int) m_size.Width, (int) m_size.Height, 96, 96, PixelFormats.Pbgra32);
			rtb.Render(TestWindowGrid);
			rtb.Freeze();
			m_frameStatCount++;
			FrameArrived.Invoke(this, rtb);
		}

		int m_frameStatCount;
		double m_targetFPS;
		double m_currentFPS;
		bool m_newFPS;
		bool m_shouldIncrease;
		bool m_shouldDecrease;
		readonly CancellationTokenSource m_cts;
		Timer m_frameTimer;
		Timer m_statTimer;
		readonly Size m_size;
	}
}
