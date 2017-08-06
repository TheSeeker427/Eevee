using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Eevee_connection_map
{
	/// <summary>
	/// Interaction logic for popup.xaml
	/// </summary>
	public partial class popup : UserControl
	{
		public String hostname;
		public String city;
		public String region;
		public String country;
		public String loc;
		public String org;
		public String postal;

		IPRecord rec;

		public popup()
		{
			InitializeComponent();
		}

		public popup(IPRecord rec)
		{
			label_city.Content = "City: " + rec.city;
			label_country.Content = "Country: " + rec.country;
			label_hostname.Content = "Hostname: " + rec.hostname;
			label_org.Content = "Org: " + rec.org;
			label_region.Content = "Region: " + rec.region;
		}

		public void update(IPRecord rec)
		{
			label_city.Content = "City: " + rec.city;
			label_country.Content = "Country: " + rec.country;
			label_hostname.Content = "Hostname: " + rec.hostname;
			label_org.Content = "Org: " + rec.org;
			label_region.Content = "Region: " + rec.region;
			label_ip.Content = "IP: " + rec.remote_ip;
			label_program.Content = "Program: " + rec.program;
			this.rec = rec;
		}

		private void button_kill_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.GetProcessById(int.Parse(rec.pid)).Kill();
			}
			catch (Exception en)
			{
				MessageBox.Show(en.ToString());
			}
		}
	}
}
