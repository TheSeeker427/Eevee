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

            IPRecord s = new IPRecord();
            ipList[0] = new IPRecord();
            s.loc = "loc\": \"48.6801,-100.1206\"";
            ipList[0].loc = "loc\": \"28.6801,-81.1206\"";
            ipList[0].ip = "10.252.16.57";
            ipPingHead = new ipPingGraphic(ipList[0], s, new Point(pictureBox1.Width, pictureBox1.Height));
        }

        public static Form1 getInstance()
        {
            return myself;
        }

        public void updateInfoTab(IPRecord i)
        {
            IPInfoLabel.Text = "IP Address: " + i.ip + "\r\nOrg:" + i.org +
            "\r\nCountry: " + i.country + 
            "\r\nhostname: " + i.hostname +
            "\r\nProcess: " + i.processName +
            "\r\nProcess ID: " + i.processId +
            "\r\nPort: " + i.port;

            //Console.WriteLine(TCPMonitor.)
        }


        public void log(String text)
        {
            richTextBox1.Text += text+"\n";
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
                grap.ResetTransform();
                grap = pictureBox1.CreateGraphics();
                foreach (IPRecord ip in ipList)
                {
                    if (ip != null)
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

        public void updateListBox()
        {

            foreach (IPRecord ip in ipList)
            {
                if (ip == null)
                    break;
                if (!listBox1.Items.Contains(ip.ip)) {
                    listBox1.Items.Add(ip.ip);
                    ipPanel.Controls.Add(ip);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPListIndex = 1;
            listBox1.Items.Clear();
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
            updateListBox();
        }

        private void listBox1_Changed(object sender, EventArgs e)
        {

            foreach (IPRecord ip in ipList)
            {
                try
                {
                    if (ip == null) { break; }
                    if (listBox1.SelectedItem.ToString().Contains(ip.ip))
                    {
                        label1.Text = ip.toString();
                        break;
                    }
                }
                catch (Exception en)
                {
                    Console.Write(en);
                }
            }

        }
        public void addIp(string ip)
        {
            GetLocation(ip);
            updateListBox();
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

                    //Console.WriteLine (n + line); n++;
                    if (line.Contains("ip"))
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (IPRecord ip in ipList)
            {
                try
                {
                    Console.Write(":::" + ip.ip);
                    if (listBox1.SelectedItem.ToString().Contains(ip.ip))
                    {
                        label1.Text = ip.toString();
                        break;
                    }
                }
                catch (Exception en)
                {
                    Console.Write(en);
                }
            }
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
                        foreach(IPRecord i in ipList)
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
            if (timeCount >= 10)
            {
                timeCount = 0;
                new Thread(new ThreadStart(updateTCPConnections)).Start();
            }
            if(loggingMessage != "")
            {
                log(loggingMessage);
                loggingMessage = "";
            }
            timeCount++;
            map_refresh();
            updateListBox();
            updateTCPConn();
            
        }

        public void updateTCPConn()
        {
            foreach (SocketMonitor.TcpProcessRecord i in SocketMonitor.GetAllTcpConnections())
            {
                foreach (IPRecord ip in ipList)
                {
                    if(ip == null)
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


        ///LIVE CAPTURE CODE
        ///

        private Socket mainSocket;                          //The socket which captures all incoming packets
        private byte[] byteData = new byte[4096];
        private bool bContinueCapturing = false;            //A flag to check if packets are to be captured or not

        private delegate void AddTreeNode(TreeNode node);



        private void SnifferForm_Load()
        {
            string strIP = null;

            IPHostEntry HosyEntry = Dns.GetHostEntry((Dns.GetHostName()));
            if (HosyEntry.AddressList.Length > 0)
            {
                foreach (IPAddress ip in HosyEntry.AddressList)
                {
                    strIP = ip.ToString();
                    interfacecomboBox.Items.Add(strIP);
                }
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                int nReceived = mainSocket.EndReceive(ar);

                //Analyze the bytes received...

                ParseData(byteData, nReceived);

                if (bContinueCapturing)
                {
                    byteData = new byte[4096];

                    //Another call to BeginReceive so that we continue to receive the incoming
                    //packets
                    mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
                        new AsyncCallback(OnReceive), null);
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
                       new AsyncCallback(OnReceive), null);
                MessageBox.Show(ex.Message, "EEVVEE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void sup(String sip, String dip)
        {
            Boolean found = false;
            foreach (IPRecord a in ipList)
            {

                if (a != null)
                if ( a.ip.Contains(dip))
                {
                    a.ping = true;
                    break;
                }
            }


        }

        private void ParseData(byte[] byteData, int nReceived)
        {
            TreeNode rootNode = new TreeNode();

            //Since all protocol packets are encapsulated in the IP datagram
            //so we start by parsing the IP header and see what protocol data
            //is being carried by it
            IPHeader ipHeader = new IPHeader(byteData, nReceived);

           // TreeNode ipNode = MakeIPTreeNode(ipHeader);
           // rootNode.Nodes.Add(ipNode);
            loggingMessage += (ipHeader.SourceAddress + " >>>> " + ipHeader.DestinationAddress) + "\n";


            sup(ipHeader.SourceAddress.ToString(), ipHeader.DestinationAddress.ToString());


            //Now according to the protocol being carried by the IP datagram we parse 
            //the data field of the datagram
            switch (ipHeader.ProtocolType)
            {
                case Protocol.TCP:

                    break;

                case Protocol.UDP:

                  
                    break;

                case Protocol.Unknown:
                    break;
            }

            rootNode.Text = ipHeader.SourceAddress.ToString() + "-" +
                ipHeader.DestinationAddress.ToString();

            //Thread safe adding of the nodes
            // treeView.Invoke(addTreeNode, new object[] { rootNode });
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void liveCapButton_Click(object sender, EventArgs e)
        {
            if (interfacecomboBox.Text == "")
            {
                MessageBox.Show("Select an Interface to capture the packets.", "MJsniffer",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (!bContinueCapturing)
                {
                    //Start capturing the packets...

                    liveCapButton.Text = "&Stop";

                    bContinueCapturing = true;

                    //For sniffing the socket to capture the packets has to be a raw socket, with the
                    //address family being of type internetwork, and protocol being IP
                    mainSocket = new Socket(AddressFamily.InterNetwork,
                        SocketType.Raw, ProtocolType.IP);

                    //Bind the socket to the selected IP address
                    mainSocket.Bind(new IPEndPoint(IPAddress.Parse(interfacecomboBox.Text), 0));

                    //Set the socket  options
                    mainSocket.SetSocketOption(SocketOptionLevel.IP,            //Applies only to IP packets
                                               SocketOptionName.HeaderIncluded, //Set the include the header
                                               true);                           //option to true

                    byte[] byTrue = new byte[4] { 1, 0, 0, 0 };
                    byte[] byOut = new byte[4] { 1, 0, 0, 0 }; //Capture outgoing packets

                    //Socket.IOControl is analogous to the WSAIoctl method of Winsock 2
                    mainSocket.IOControl(IOControlCode.ReceiveAll,              //Equivalent to SIO_RCVALL constant
                                                                                //of Winsock 2
                                         byTrue,
                                         byOut);

                    //Start receiving the packets asynchronously
                    mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
                        new AsyncCallback(OnReceive), null);
                }
                else
                {
                    liveCapButton.Text = "&Start";
                    bContinueCapturing = false;
                    //To stop capturing the packets close the socket
                    mainSocket.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MJsniffer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
