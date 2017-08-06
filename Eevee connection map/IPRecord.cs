using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Eevee_connection_map
{
	public class IPRecord 
	{
		//Connection info
		public String RemoteEndPoint;
		public String LocalEndPoint;

		//IP Info
		public String hostname;
		public String city;
		public String region;
		public String country;
		public String loc;
		public String org;
		public String postal;

		//PID
		public String pid;
		public String program;

		//General info
		public String remote_ip;
		public String remote_port;
		public String local_port;
		
		TcpConnectionInformation conn;
		public float loc_lat;
		public float loc_lon;

		public bool isSelected = false;


	}
}
