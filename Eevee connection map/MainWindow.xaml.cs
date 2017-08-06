using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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

namespace Eevee_connection_map
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Point currentPoint;
		private String local_ip;
		
		public MainWindow()
		{
			InitializeComponent();
			System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
			dispatcherTimer.Tick += dispatcherTimer_Tick;
			dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
			local_ip = GetLocalIPAddress();
			dispatcherTimer.Start();
			//map_img.MouseDown += Map_img_MouseDown;
			this.MouseDown += MainWindow_MouseDown;
		}

		private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Application.Current.MainWindow.DragMove();
		}

		private void Map_img_MouseDown(object sender, MouseButtonEventArgs e)
		{
			//netstat_manager.getNetstat();
			Console.WriteLine(e.GetPosition(Application.Current.MainWindow).X);
			
			float lat = (28.0f * (float) ((Convert.ToDouble(map_img.ActualHeight) / 180)) + (float) (90 * (Convert.ToDouble(map_img.ActualHeight) / 180)));
			float lon_map = (-80.0f * (float) ((Convert.ToDouble(map_img.ActualWidth) / 360)) + (float) ((Convert.ToDouble(map_img.ActualWidth) / 360) * 180));
			float lon_window = (float)map_img.Margin.Left - lon_map;
			float lat_window = (float)map_img.Margin.Top - lat;
		
			Console.WriteLine((float)map_img.Margin.Left + " : "+lat_window);
			popmenu.Margin = new Thickness(lat_window, lon_window, 0, 0);

			Ellipse circle = new Ellipse()
			{
				Width = 10,
				Height = 10,
				Stroke = Brushes.Black,
				StrokeThickness = 1.5

			};
			//circle.HorizontalAlignment = HorizontalAlignment.Left;
			//
			//circle.VerticalAlignment = VerticalAlignment.Top;
			circle.Fill = new SolidColorBrush(Colors.DarkSeaGreen);
			circle.Stroke = new SolidColorBrush(Colors.Azure);
			Canvas.SetTop(circle, lat_window);
			Canvas.SetLeft(circle, lon_window);
		}

		public static string GetLocalIPAddress()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			throw new Exception("Local IP Address Not Found!");
		}


		private void dispatcherTimer_Tick(object sender, EventArgs e)
		{
			connectListBox.Items.Clear();
			map_img.Children.Clear();
			foreach (IPRecord c in TCPConnectionManager.ShowActiveTcpConnections())
			{
				if (!c.remote_ip.Contains(local_ip.Substring(0, 10)))
				{
					connectListBox.Items.Add(c.program + ": " + c.RemoteEndPoint);
					map_refresh(28.5383, -81.3792, c);
				}
			}

		}


		public void map_refresh(double latIn, double LongIn, IPRecord rec)
		{
			
			double lat = -latIn * (Convert.ToDouble(map_img.ActualHeight) / 180);
			double lon = LongIn * (Convert.ToDouble(map_img.ActualWidth) / 360);
			double lat2 = - rec.loc_lat * (Convert.ToDouble(map_img.ActualHeight) / 180);
			double lon2 = rec.loc_lon * (Convert.ToDouble(map_img.ActualWidth) / 360);

			Ellipse circle = new Ellipse()
			{
				Width = 10,
				Height = 10,
				Stroke = Brushes.Black,
				StrokeThickness = 1.5
				
			};
			//circle.HorizontalAlignment = HorizontalAlignment.Left;
			//
			//circle.VerticalAlignment = VerticalAlignment.Top;
			circle.Fill = new SolidColorBrush(Colors.DarkSeaGreen);
			circle.Stroke = new SolidColorBrush(Colors.Azure);
			Canvas.SetTop(circle, lat2 + (90 * (Convert.ToDouble(map_img.ActualHeight) / 180)) - 5);
			Canvas.SetLeft(circle, lon2 + ((Convert.ToDouble(map_img.ActualWidth) / 360) * 180) - 5);
			circle.MouseDown += Circle_MouseDown;
			//circle.Margin = new Thickness(lat2 + (90 * (Convert.ToDouble(map_img.ActualHeight) / 180)), lon2 + ((Convert.ToDouble(map_img.ActualWidth) / 360) * 180), 0, 0);
			map_img.Children.Add(circle);

			Line line = new Line();
			if (rec.isSelected)
			{
				line.Stroke = new SolidColorBrush(Colors.Blue);
			}
			else { line.Stroke = new SolidColorBrush(Colors.Green); }
			
			line.X1 = (float)((lon + ((Convert.ToDouble(map_img.ActualWidth) / 360) * 180)));
			line.Y1 = (float)((lat + (90 * (Convert.ToDouble(map_img.ActualHeight) / 180))));
			line.X2 = (float)((lon2 + ((Convert.ToDouble(map_img.ActualWidth) / 360) * 180)));
			line.Y2 = (float)((lat2 + (90 * (Convert.ToDouble(map_img.ActualHeight) / 180))));
			map_img.Children.Add(line);
			//grap.FillEllipse(Brushes.Aquamarine, (float)((lon + ((Convert.ToDouble(map_img.Width) / 360) * 180))), (float)((lat + (90 * (Convert.ToDouble(map_img.Height) / 180)))), 5, 5);
			//grap.FillEllipse(Brushes.OrangeRed, (float)((lon2 + ((Convert.ToDouble(map_img.Width) / 360) * 180))), (float)((lat2 + (90 * (Convert.ToDouble(map_img.Height) / 180)))), 5, 5);
			//grap.DrawLine(Pens.Aqua, (float)((lon + ((Convert.ToDouble(map_img.Width) / 360) * 180))), (float)((lat + (90 * (Convert.ToDouble(map_img.Height) / 180)))), (float)((lon2 + ((Convert.ToDouble(map_img.Width) / 360) * 180))), (float)((lat2 + (90 * (Convert.ToDouble(map_img.Height) / 180)))));
			//grap.DrawString(ip.ip, SystemFonts.MessageBoxFont, Brushes.Gold, (float)((lon2 + ((Convert.ToDouble(map_img.Width) / 360) * 180))), (float)((lat2 + (90 * (Convert.ToDouble(map_img.Height) / 180)))));

		}

		private void Circle_MouseDown(object sender, MouseButtonEventArgs e)
		{
			
		}


		IPRecord prev_selected;
		private void connectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				IPRecord rec = TCPConnectionManager.getList()[connectListBox.SelectedIndex];
				if(prev_selected != null)
				prev_selected.isSelected = false;
				rec.isSelected = true;
				
				map_refresh(28.5383, -81.3792, rec);
				popmenu.update(rec);
				prev_selected = rec;
			}
			catch (Exception ne) { }
			
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
