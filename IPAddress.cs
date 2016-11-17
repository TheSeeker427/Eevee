using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eevee
{
	public class IPAddress
    {
		public IPAddress ()
		{
		}
		
        public string ip = "lo";
        public string loc = "" ;
        public string hostname = "" ;
        public string city = "";
        public string region = "";
        public String country = "";
        public String org = "";
        public String postal = "";

		public void setIp(string input){
			this.ip = input;
		}

		public String toString(){
			return ip + "\n" + loc + "\n " + hostname + "\n " + city + " \n" + region + " \n" + country + " \n" + org + " ";
		}
    }
}
