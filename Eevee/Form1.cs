using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eevee
{
    public partial class Form1 : Form
    {
        Graphics grap;
        IPAddress[] ipList = new IPAddress[30];
        public Form1()
        {
            
            InitializeComponent();
            grap = panel1.CreateGraphics();
            this.panel1.MouseWheel += Panel1_MouseWheel;
            this.panel1.MouseDown += Panel1_MouseDown;
            this.panel1.MouseUp += Panel1_MouseUp;
            this.panel1.MouseMove += Panel1_MouseMove;
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        Boolean mouseDown = false;
        int x;
        int y;
        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = panel1.Width + "." + panel1.Height;
            
            if (mouseDown)
                panel1.Location = new Point(panel1.Location.X + (e.X - x),panel1.Location.Y + (e.Y - y));

        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            x = e.X;
            y = e.Y;

        }

        private void Panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            panel1.Height = panel1.Height + e.Delta;
            panel1.Width = panel1.Width + e.Delta;
            label1.Text = " " + panel1.Height;
        }

        public void map_refresh(double latIn, double LongIn, double latOut, double longOut)
        {
            double lat = -latIn*(Convert.ToDouble(panel1.Height)/180);
            double lon = LongIn* (Convert.ToDouble(panel1.Width)/360);
            double lat2 = -latOut * (Convert.ToDouble(panel1.Height) / 180);
            double lon2 = longOut * (Convert.ToDouble(panel1.Width) / 360);
            grap.FillEllipse(Brushes.Aquamarine, (float)((lon+((Convert.ToDouble(panel1.Width) / 360)*180) )), (float)(  (lat + (90 * (Convert.ToDouble(panel1.Height) / 180)) )), 5, 5);
            grap.FillEllipse(Brushes.OrangeRed, (float)((lon2 + ((Convert.ToDouble(panel1.Width) / 360) * 180))), (float)((lat2 + (90 * (Convert.ToDouble(panel1.Height) / 180)))), 5, 5);
            grap.DrawLine(Pens.Aqua, (float)((lon + ((Convert.ToDouble(panel1.Width) / 360) * 180))), (float)((lat + (90 * (Convert.ToDouble(panel1.Height) / 180)))), (float)((lon2 + ((Convert.ToDouble(panel1.Width) / 360) * 180))), (float)((lat2 + (90 * (Convert.ToDouble(panel1.Height) / 180)))));
            grap.DrawString("Hello", SystemFonts.MessageBoxFont , Brushes.Red,(float) latOut,(float) longOut);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Console.WriteLine("Active TCP Connections");
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            foreach (TcpConnectionInformation c in connections)
            {
                if (c.RemoteEndPoint.ToString().Contains("."))
                {
                    try {
                        Console.WriteLine("{0}",
                            c.RemoteEndPoint.ToString().Remove(c.RemoteEndPoint.ToString().IndexOf(":")));
                        String loc = GetLocation(c.RemoteEndPoint.ToString().Remove(c.RemoteEndPoint.ToString().IndexOf(":")));
                        loc = loc.Remove(0, loc.IndexOf(":") + 1);
                        loc = loc.Replace("\"", "");
                        //Console.WriteLine(loc);
                        double lat = Convert.ToDouble(loc.Split(',')[0]);
                        double lon = Convert.ToDouble(loc.Split(',')[1]);
                        Console.WriteLine(lat + ": " + lon);
                        map_refresh(28.564100265503, -81.211402893066, lat, lon);
                    }catch(Exception en)
                    {


                    }
                }
            }


         }
        public static string GetLocation(string ip)
        {
            var res = "";
            WebRequest request = WebRequest.Create("http://ipinfo.io/" + ip);
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    if(line.Contains("loc"))
                    res += line;
                }
            }
            return res;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
