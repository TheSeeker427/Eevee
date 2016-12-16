using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eevee
{
    public enum Protocol
    {
        TCP = 6,
        UDP = 17,
        Unknown = -1
    };
    public partial class Form1 : Form
    {
        Graphics grap;
        IPRecord[] ipList = new IPRecord[300];
        ipPingGraphic ipPingHead, ipPing;

        int IPListIndex = 0;

        String loggingMessage = "";

        FlowLayoutPanel ipPanel = new FlowLayoutPanel();
        static Form1 myself;
        public Form1()
        {

            InitializeComponent();
            myself = this;

            ipPanel.AutoScroll = true;


            panel2.Controls.Add(ipPanel);
            Label l = new Label();
            l.ForeColor = Color.Lime;
            l.Text = "IP Addresses: ";
            ipPanel.Controls.Add(l);

            //SnifferForm_Load();
            pictureBox1.Size = panel1.Size;
            grap = pictureBox1.CreateGraphics();
            pictureBox1.Paint += Panel1_Paint;

            this.panel1.MouseWheel += Panel1_MouseWheel;
            this.panel1.MouseDown += Panel1_MouseDown;
            this.panel1.MouseUp += Panel1_MouseUp;
            this.panel1.MouseMove += Panel1_MouseMove;
            this.pictureBox1.MouseWheel += Panel1_MouseWheel;
            this.pictureBox1.MouseDown += Panel1_MouseDown;
            this.pictureBox1.MouseUp += Panel1_MouseUp;
            this.pictureBox1.MouseMove += Panel1_MouseMove;
            ipPanel.Size = panel2.Size;
        }

        public static Form1 getInstance()
        {
            return myself;
        }

        public void updateInfoTab(IPRecord i)
        {
            IPInfoLabel.Text = "IP Address: " + i.ip + "\r\nOrg:" + i.org +
            "\r\nCountry: " + i.country +
            "\r\n: " + i.loc +
            "\r\nhostname: " + i.hostname +
            "\r\nProcess: " + i.processName +
            "\r\nProcess ID: " + i.processId +
            "\r\nPort: " + i.port;

            //Console.WriteLine(TCPMonitor.)
        }


        public void log(String text)
        {
            richTextBox1.Text += text + "\n";
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            // scroll it automatically
            richTextBox1.ScrollToCaret();
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            map_refresh();
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

            if (mouseDown)
                panel1.Location = new Point(panel1.Location.X + (e.X - x), panel1.Location.Y + (e.Y - y));

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

            pictureBox1.Size = panel1.Size;
            grap = pictureBox1.CreateGraphics();
        }

        public void map_refresh()
        {
            try
            {
                pictureBox1.Update();
                grap.ResetTransform();
                grap = pictureBox1.CreateGraphics();
                foreach (IPRecord ip in ipList)
                {
                    if (ip != null) 
                        if(ip.state != SocketMonitor.MibTcpState.TIME_WAIT || ip.state != SocketMonitor.MibTcpState.CLOSE_WAIT || ip.state != SocketMonitor.MibTcpState.CLOSED)
                        map_refresh(28.564100265503, -81.211402893066, ip.getLat(), ip.getLon(), ip);
                }
            }
            catch (Exception e)
            {

            }
        }

        public void map_refresh(double latIn, double LongIn, double latOut, double longOut, IPRecord ip)
        {

            double lat = -latIn * (Convert.ToDouble(panel1.Height) / 180);
            double lon = LongIn * (Convert.ToDouble(panel1.Width) / 360);
            double lat2 = -latOut * (Convert.ToDouble(panel1.Height) / 180);
            double lon2 = longOut * (Convert.ToDouble(panel1.Width) / 360);
            grap.FillEllipse(Brushes.Aquamarine, (float)((lon + ((Convert.ToDouble(panel1.Width) / 360) * 180))), (float)((lat + (90 * (Convert.ToDouble(panel1.Height) / 180)))), 5, 5);
            grap.FillEllipse(Brushes.OrangeRed, (float)((lon2 + ((Convert.ToDouble(panel1.Width) / 360) * 180))), (float)((lat2 + (90 * (Convert.ToDouble(panel1.Height) / 180)))), 5, 5);
            grap.DrawLine(Pens.Aqua, (float)((lon + ((Convert.ToDouble(panel1.Width) / 360) * 180))), (float)((lat + (90 * (Convert.ToDouble(panel1.Height) / 180)))), (float)((lon2 + ((Convert.ToDouble(panel1.Width) / 360) * 180))), (float)((lat2 + (90 * (Convert.ToDouble(panel1.Height) / 180)))));
            grap.DrawString(ip.ip, SystemFonts.MessageBoxFont, Brushes.Gold, (float)((lon2 + ((Convert.ToDouble(panel1.Width) / 360) * 180))), (float)((lat2 + (90 * (Convert.ToDouble(panel1.Height) / 180)))));
            if (ip.ping && !ip.ip.Contains("10.252.16.57"))
            {
                Console.WriteLine(ip.ip);
                grap.DrawLine(Pens.Red, (float)((lon + ((Convert.ToDouble(panel1.Width) / 360) * 180))), (float)((lat + (90 * (Convert.ToDouble(panel1.Height) / 180)))), (float)((lon2 + ((Convert.ToDouble(panel1.Width) / 360) * 180))), (float)((lat2 + (90 * (Convert.ToDouble(panel1.Height) / 180)))));
                ip.ping = false;
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            IPListIndex = 1;
            Console.WriteLine("Active TCP Connections");
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation c in connections)
            {
                if (c.RemoteEndPoint.ToString().Contains("."))
                {
                    try
                    {
                        //Console.WriteLine("{0}",
                        //    c.RemoteEndPoint.ToString().Remove(c.RemoteEndPoint.ToString().IndexOf(":")));
                        String ip = c.RemoteEndPoint.ToString().Remove(c.RemoteEndPoint.ToString().IndexOf(":"));
                        if (!ip.Contains("0.0.0.0") && !ip.Contains("127.0.0.1"))
                        {
                            String loc = GetLocation(ip);
                            IPListIndex++;

                            loc = loc.Remove(0, loc.IndexOf(":") + 1);
                            loc = loc.Replace("\"", "");
                        }
                    }
                    catch (Exception en)
                    {
                        Console.Write(en);

                    }
                }
            }
            map_refresh();

        }


        public void addIp(string ip)
        {
            GetLocation(ip);
            
        }

        public string GetLocation(string ip)
        {
            var res = "";
            WebRequest request = WebRequest.Create("http://ipinfo.io/" + ip);
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                string line;
                ipList[IPListIndex] = new IPRecord();
                int n = 0;
                while ((line = stream.ReadLine()) != null)
                {

                    Console.WriteLine (n + line); n++;
                    if (line.Contains("\"ip\""))
                        ipList[IPListIndex].setIp(line);
                    if (line.Contains("loc"))
                    {
                        ipList[IPListIndex].loc = line;
                        ipList[IPListIndex].parseLon();
                    }
                    if (line.Contains("hostname"))
                        ipList[IPListIndex].hostname = line;
                    if (line.Contains("city"))
                        ipList[IPListIndex].city = line;
                    if (line.Contains("country"))
                        ipList[IPListIndex].country = line;
                    if (line.Contains("org"))
                        ipList[IPListIndex].org = line;
                    if (line.Contains("postal"))
                        ipList[IPListIndex].postal = line;

                    if (line.Contains("loc"))
                        res += line;
                    //Console.WriteLine(":::::" + ipList[IPListIndex].ip);
                }
            }
            return res;
        }


        public void updateTCPConnections()
        {

            Console.WriteLine("Active TCP Connections");
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation c in connections)
            {
                if (c.RemoteEndPoint.ToString().Contains("."))
                {
                    try
                    {
                        //Console.WriteLine("{0}",
                        //    c.RemoteEndPoint.ToString().Remove(c.RemoteEndPoint.ToString().IndexOf(":")));
                        String ip = c.RemoteEndPoint.ToString().Remove(c.RemoteEndPoint.ToString().IndexOf(":"));
                        bool found = false;
                        foreach (IPRecord i in ipList)
                        {
                            if (i == null)
                                break;
                            if (i.ip.Contains(ip))
                            {
                                found = true;
                            }
                        }
                        if (!ip.Contains("0.0.0.0") && !ip.Contains("127.0.0.1") && !found)
                        {
                            loggingMessage += ("Adding TCP : " + ip) + "\n";
                            Console.WriteLine("Adding TCP : " + ip);
                            GetLocation(ip);
                            IPListIndex++;
                        }
                    }
                    catch (Exception en)
                    {
                        Console.Write(en);

                    }
                }
            }
            // map_refresh();
        }

        int timeCount = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timeCount >= 50)
            {
                timeCount = 0;
                updateConnections();
            }
            if (loggingMessage != "")
            {
                log(loggingMessage);
                loggingMessage = "";
            }
            progressBar1.Value = timeCount;
            timeCount++;
            map_refresh();
            updateInfoBox();

        }

        public void updateConnections()
        {
            foreach (SocketMonitor.TcpProcessRecord i in SocketMonitor.GetAllTcpConnections())
            {
                try
                {
                    bool found = false;
                    foreach (IPRecord ip in ipList)
                    {
                        if (ip == null)
                        {
                            break;
                        }
                        if (ip.ip.Contains(i.RemoteAddress.ToString()))
                        {
                            ip.updateState(i.State);
                            
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {

                    }
                    else
                    {
                        ipList[IPListIndex] = new IPRecord();

                        
                        ipList[IPListIndex].port = i.RemotePort.ToString();
                        ipList[IPListIndex].processId = i.ProcessId.ToString();
                        ipList[IPListIndex].processName = i.ProcessName;
                        ipList[IPListIndex].state = i.State;
                        ipList[IPListIndex].setIp( i.RemoteAddress.ToString());
                        addIp(i.RemoteAddress.ToString());
                        ipPanel.Controls.Add(ipList[IPListIndex]);
                        IPListIndex++;
                    }
                }catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void liveCapButton_Click(object sender, EventArgs e)
        {
            foreach(IPRecord i in ipList)
            {
                Console.WriteLine(i.ip);
            }
        }

        public void updateInfoBox()
        {
            foreach (SocketMonitor.TcpProcessRecord i in SocketMonitor.GetAllTcpConnections())
            {
                foreach (IPRecord ip in ipList)
                {
                    if (ip == null)
                    {
                        break;
                    }
                    if (ip.ip.Contains(i.LocalAddress.ToString()) || ip.ip.Contains(i.RemoteAddress.ToString()))
                    {
                        ip.processName = i.ProcessName;
                        ip.port = i.LocalPort + "";
                        ip.processId = i.ProcessId + "";
                    }
                }
            }
        }


    }
}
