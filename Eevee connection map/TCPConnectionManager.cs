using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;

namespace Eevee_connection_map
{
	class TCPConnectionManager
	{

		static List<IPRecord> ipList = new List<IPRecord>();

		public static List<IPRecord> ShowActiveTcpConnections()
		{
			Console.WriteLine("Active TCP Connections");
			IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
			TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
			ipList.Clear();
			//Check for new connections
			foreach (TcpConnectionInformation c in connections)
			{
				if (!c.RemoteEndPoint.ToString().Contains("127.0.0.1"))
				{
					bool isNewConnection = true;

					foreach (IPRecord record in ipList)
					{
						//Its in our records.
						if (record.RemoteEndPoint.Contains(c.RemoteEndPoint.ToString()))
						{
							//Console.WriteLine(record.RemoteEndPoint);
							isNewConnection = false;
							break;
						}
					}
					if (isNewConnection)
					{
						newConnection(c);
						isNewConnection = false;
					}
				}
			}
			Console.WriteLine(ipList.Count);
			netstat_manager.getNetstat();
			return ipList;
		}

		public static List<IPRecord> getList()
		{
			return ipList;
		}

		public static void newConnection(TcpConnectionInformation conn) {
			IPRecord ipR = new IPRecord();
			ipR.RemoteEndPoint = conn.RemoteEndPoint.ToString();
			GetInfo(ipR);
			ipList.Add(ipR);
		}

		public static void GetInfo(IPRecord rec)
		{
			var res = "";
			rec.remote_ip = rec.RemoteEndPoint.ToString().Split(':')[0];

			WebRequest request = WebRequest.Create("http://ipinfo.io/" + rec.remote_ip);
			using (WebResponse response = request.GetResponse())
			using (StreamReader stream = new StreamReader(response.GetResponseStream()))
			{
				string line;
				while ((line = stream.ReadLine()) != null)
				{
					if (line.Contains("hostname")){
						rec.hostname = line.Split(':')[1].Split(',')[0].Replace('\"',' ');
					}
					if (line.Contains("city"))
					{
						rec.city = line.Split(':')[1].Split(',')[0].Replace('\"', ' ');
					}
					if (line.Contains("region"))
					{
						rec.region = line.Split(':')[1].Split(',')[0].Replace('\"', ' ');
					}
					if (line.Contains("country"))
					{
						rec.country = line.Split(':')[1].Split(',')[0].Replace('\"', ' ');
					}
					if (line.Contains("loc"))
					{
						rec.loc = line.Split(':')[1].Replace('\"', ' ');
					}
					if (line.Contains("org"))
					{
						rec.org = line.Split(':')[1].Split(',')[0].Replace('\"', ' ');
					}
					if (line.Contains("postal"))
					{
						rec.postal = line.Split(':')[1].Split(',')[0].Replace('\"', ' ');
					}

					//Console.WriteLine(line);
					//Console.WriteLine(":::::" + ipList[IPListIndex].ip);
				}
			}
			try
			{
				rec.loc_lat = float.Parse(rec.loc.Replace('\"', ' ').Split(',')[0]);
				rec.loc_lon = float.Parse(rec.loc.Replace('\"', ' ').Split(',')[1]);
			}
			catch (Exception e)
			{

			}
			
		}

	}
}
