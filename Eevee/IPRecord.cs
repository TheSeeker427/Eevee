using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Diagnostics;
using System.Drawing;


namespace Eevee
{
    public partial class IPRecord : UserControl
    {
        public IPRecord()
        {
            InitializeComponent();
            label1.MouseEnter += IPRecord_MouseEnter;
            label1.MouseLeave += IPRecord_MouseLeave;
            label1.MouseClick += IPRecord_MouseClick;
            this.MouseEnter += IPRecord_MouseEnter;
            this.MouseLeave += IPRecord_MouseLeave;
            this.MouseClick += IPRecord_MouseClick;
        }

        private void IPRecord_MouseClick(object sender, MouseEventArgs e)
        {
            Form1.getInstance().updateInfoTab(this);
        }

        private void IPRecord_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.Black;
        }

        private void IPRecord_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.DarkSlateGray;
        }

        public string ip = "lo";
        public string loc = "";
        public double latitude = 0.0;
        public double longitude = 0.0;
        public string hostname = "";
        public string city = "";
        public string region = "";
        public String country = "";
        public String org = "";
        public String postal = "";
        public String processName = "";
        public String processId = "";
        public String port = "";
        public int packetsSent = 0;
        public int packetsReceived = 0;
        public bool ping = false;

        public void setIp(string input)
        {
            this.ip = input;
            label1.Text = input;
        }
        public void parseLon()
        {
            latitude = Convert.ToDouble(loc.Remove(0, loc.IndexOf(":") + 1).Replace("\"", "").Split(',')[0]); ;
            longitude = Convert.ToDouble(loc.Remove(0, loc.IndexOf(":") + 1).Replace("\"", "").Split(',')[1]); ;
        }

        public double getLat()
        {
            try
            {
                return latitude;
            }
            catch (Exception e)
            {
                return 0.0;
            }

        }
        public double getLon()
        {
            try
            {
                return longitude;
            }
            catch (Exception e)
            {
                return 0.0;
            }

        }

        public String toString()
        {
            return ip + org + hostname + "\n" + loc + city + region + country + postal;
        }



    }
}

