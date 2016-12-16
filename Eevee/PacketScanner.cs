using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eevee
{
    class PacketScanner
    {
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
                 //   interfacecomboBox.Items.Add(strIP);
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
     

        private void ParseData(byte[] byteData, int nReceived)
        {
            TreeNode rootNode = new TreeNode();

            //Since all protocol packets are encapsulated in the IP datagram
            //so we start by parsing the IP header and see what protocol data
            //is being carried by it
            IPHeader ipHeader = new IPHeader(byteData, nReceived);

            // TreeNode ipNode = MakeIPTreeNode(ipHeader);
            // rootNode.Nodes.Add(ipNode);
          //  loggingMessage += (ipHeader.SourceAddress + " >>>> " + ipHeader.DestinationAddress) + "\n";


           // sup(ipHeader.SourceAddress.ToString(), ipHeader.DestinationAddress.ToString());


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
          //  if (interfacecomboBox.Text == "")
          //  {
          //      MessageBox.Show("Select an Interface to capture the packets.", "MJsniffer",
          //          MessageBoxButtons.OK, MessageBoxIcon.Error);
          //      return;
          //  }
            try
            {
                if (!bContinueCapturing)
                {
                    //Start capturing the packets...

               //    liveCapButton.Text = "&Stop";

                    bContinueCapturing = true;

                    //For sniffing the socket to capture the packets has to be a raw socket, with the
                    //address family being of type internetwork, and protocol being IP
                    mainSocket = new Socket(AddressFamily.InterNetwork,
                        SocketType.Raw, ProtocolType.IP);

                    //Bind the socket to the selected IP address
                   // mainSocket.Bind(new IPEndPoint(IPAddress.Parse(interfacecomboBox.Text), 0));

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
                  //  liveCapButton.Text = "&Start";
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
