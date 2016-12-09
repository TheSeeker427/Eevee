using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eevee
{
	public class ccIPAddress
    {
		public ccIPAddress ()
		{
		}
		
        public string ip = "lo";
        public string loc = "" ;
        public double latitude = 0.0;
        public double longitude = 0.0;
        public string hostname = "" ;
        public string city = "";
        public string region = "";
        public String country = "";
        public String org = "";
        public String postal = "";
        public int packetsSent = 0;
        public int packetsReceived = 0;
        public bool ping = false;

		public void setIp(string input){
			this.ip = input;
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
            }catch (Exception e)
            {
                return 0.0;
            }

        }
        public double getLon()
        {
            try
            {
                return longitude;
            }catch( Exception e)
            {
                return 0.0;
            }

        }

        public String toString(){
			return ip + org + hostname + "\n" + loc  + city  + region  + country  + postal;
		}

    }
}
