using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageUploader
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		Timer m_hImageUpdateTimer;
		DAO m_hDao;
		
		public MainWindow()
		{
			m_hDao = new DAO();

			InitializeComponent();

			m_hImageUpdateTimer = new Timer();
			m_hImageUpdateTimer.Elapsed += new ElapsedEventHandler(Onm_hImageUpdateTimerEvent);
			m_hImageUpdateTimer.Interval = 1000;
			m_hImageUpdateTimer.Start();

			m_hImageShow.Source = m_hDao.m_hImageData.m_hImage;
		}

		private void Onm_hImageUpdateTimerEvent(object source, ElapsedEventArgs e)
		{
			RenderTargetBitmap bmp = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Pbgra32); ;
			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext drawingContext = drawingVisual.RenderOpen();

			Pen pen = new Pen();
			pen.Brush = new SolidColorBrush(Color.FromRgb(255, 255, 0));
			pen.Thickness = 5;
			drawingContext.DrawLine(pen, new Point(0, 0), new Point(100, 100));

			drawingContext.Close();
			bmp.Render(drawingVisual);

			PngBitmapEncoder bitmapEncoder = new PngBitmapEncoder();
			bitmapEncoder.Frames.Add(BitmapFrame.Create(bmp));
			bitmapEncoder.Save(m_hDao.m_hImageData.m_hImageStream);
			m_hDao.m_hImageData.m_hImageStream.Seek(0, SeekOrigin.Begin);

			Dispatcher.Invoke(new Action(delegate
			{
				m_hDao.m_hImageData.m_hImage.BeginInit();
				m_hDao.m_hImageData.m_hImage.CacheOption = BitmapCacheOption.OnLoad;
				m_hDao.m_hImageData.m_hImage.StreamSource = m_hDao.m_hImageData.m_hImageStream;
				m_hDao.m_hImageData.m_hImage.EndInit();
			}));
		}
	}
}
