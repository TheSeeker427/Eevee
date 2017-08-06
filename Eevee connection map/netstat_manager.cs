using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Eevee_connection_map
{
	class netstat_manager
	{
		public static void getNetstat()
		{
			Process netStat = new Process();
			netStat.StartInfo.UseShellExecute = false;
			netStat.StartInfo.CreateNoWindow = true;
			netStat.StartInfo.FileName = @"netstat";
			netStat.StartInfo.Arguments = @"-on";
			netStat.StartInfo.RedirectStandardOutput = true;
			Console.WriteLine("Starting netstat");
			netStat.Start();
			string output = netStat.StandardOutput.ReadToEnd();
			processOutput(output);

		}

		public static void processOutput(String output)
		{
			String[] lines = output.Split('\r');

			foreach (String s in lines)
			{
				if (s.Contains("TCP") && !s.Contains("127.0.0.1"))
				{
					try
					{
						String cleaned = RemoveWhitespace(s);
						String remote = cleaned.Split(' ')[3].Split(':')[0];
						String pid = cleaned.Split(' ')[5];
						//Console.WriteLine(remote + " : " + pid);
						foreach (IPRecord rec in TCPConnectionManager.getList())
						{
							if (rec.remote_ip == remote)
							{
								rec.pid = pid;
								rec.program = Process.GetProcessById(int.Parse(rec.pid)).ProcessName;
								break;
							}
						}
					}
					catch (Exception e) { }
				}
			}
		}

		public static string RemoveWhitespace(String input)
		{
				return System.Text.RegularExpressions.Regex.Replace(input, @"\s+", " ");

		}
	}

	
}
