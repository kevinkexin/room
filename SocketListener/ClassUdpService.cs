using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
 

namespace SocketUdpService
{
    /// <summary>
    /// the arguments of the Udp message event
    /// </summary>
    public class UdpMsgEventArgs: EventArgs
    {
        public UdpMsgEventArgs(string s)
        {
            _message = s;
        }

        private string _message;
        private string _remoteIp;
        private int _remotePort;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public string RemoteIp
        {
            get { return _remoteIp; }
            set { _remoteIp = value; }
        }

        public int RemotePort
        {
            get { return _remotePort; }
            set { _remotePort = value; }
        }
    }

    public class ClassUdpCommon
    {
        // the default listening port
        public const int _defaultPort = 10011;
        public const string _defaultBroadcastIP = "192.168.0.255";

        /// <summary>
        /// Verify whether the port is occupied.
        /// </summary>
        /// <param name="ipAddr">ip address</param>
        /// <param name="uPort">port number</param>
        /// <returns>true: if the port isn't occupied; false: otherwise</returns>
        protected static bool VerifyPortOccupancy(IPAddress ipAddr, int uPort)
        {
            bool isOK = true;
            try
            {
                TcpListener tcpListener = new TcpListener(ipAddr, uPort);
                tcpListener.Start();
                tcpListener.Stop();
            }
            catch (SocketException ex)
            {
                isOK = false;
                MessageBox.Show(ex.Message);
            }

            return isOK;
        }

    }

    /// <summary>
    /// Udp message listener.
    /// </summary>
    public class ClassListener: ClassUdpCommon
    {
        // event handle used for passing udp message to the subscriber
        public event EventHandler<UdpMsgEventArgs> RecevingUdpMsgEvent;

        /// <summary>
        /// This function open a UDP socket, and listen the 
        /// message.
        /// </summary>
        /// <param name="ipAddr"> IP address </param>
        /// <param name="uport"> port number </param>
        /// <returns></returns>
        private void listening(IPAddress ipAddr, int uPort)
        {
            try
            {
                /*
                UdpClient listener = new UdpClient(uPort);
                IPEndPoint groupEP = new IPEndPoint(ipAddr, uPort);  */
                IPEndPoint localEP = new IPEndPoint(ipAddr, uPort);
                UdpClient listener = new UdpClient(localEP);

                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

                UdpMsgEventArgs args = new UdpMsgEventArgs("");
                while (true)
                {
                    byte[] bytes = listener.Receive(ref remoteEP);
                    args.Message = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    args.RemoteIp = remoteEP.Address.ToString();
                    args.RemotePort = remoteEP.Port;
                    OnReceivingUdpMsgEvent(args);
              //    MessageBox.Show("Socket Listener: " + groupEP.ToString() + "  " + Encoding.ASCII.GetString(bytes, 0, bytes.Length));      
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred:" + e.Message);
            }
        }

        public static bool Run(EventHandler<UdpMsgEventArgs> handle, int port)
        {
            bool isOK = false;

            if (true == VerifyPortOccupancy(IPAddress.Any, port))
            {
                ClassListener oListener = new ClassListener();
                oListener.RecevingUdpMsgEvent += handle;
                Task taskListen = new Task(() => oListener.listening(IPAddress.Any, port));
                taskListen.Start();
                isOK = true;
            }

            return isOK;
        }


        public static bool Run(EventHandler<UdpMsgEventArgs> handle, string sIp, int port)
        {
            bool isOK = false;
            IPAddress ipaddress = IPAddress.Parse("sIp");

            if (true == VerifyPortOccupancy(ipaddress, port))
            {
                ClassListener oListener = new ClassListener();
                oListener.RecevingUdpMsgEvent += handle;
                Task taskListen = new Task(() => oListener.listening(ipaddress, port));
                taskListen.Start();
                isOK = true;
            }

            return isOK;
        }

        public static bool Run(EventHandler<UdpMsgEventArgs> handler)
        {
            bool isOK = false;

            if (true == VerifyPortOccupancy(IPAddress.Any, _defaultPort))
            {
                ClassListener oListener = new ClassListener();
                oListener.RecevingUdpMsgEvent += handler;
                Task taskListen = new Task(() => oListener.listening(IPAddress.Any, _defaultPort));
                taskListen.Start();
                isOK = true;
            }
            
            return isOK;
        }

        /// <summary>
        /// On receving udp message event
        /// </summary>
        /// <param name="e"> argements </param>
        public virtual void OnReceivingUdpMsgEvent(UdpMsgEventArgs e)
        {
            EventHandler<UdpMsgEventArgs> handler = RecevingUdpMsgEvent;
            if (null != handler)
            {
                handler(this, e);
            }
        }

    }



    public class ClassSender: ClassUdpCommon
    {
        private UdpClient _sender;
        private IPEndPoint _locEp;
        private Socket _socket;
        private IPEndPoint _ep;
        private bool bInitialized = false;
        private int _locPort;
        
        public void Sending(string msg)
        {
            try
            {
                byte[] sendbuf = Encoding.ASCII.GetBytes(msg);
                int length = msg.Length;
                _sender.Send(sendbuf, length, _ep);
            }
            catch (Exception e)
            {
                //MessageBox.Show("An error occurred:" + e.Message);
                throw (e);
            }
        }

        ~ClassSender()
        {
            if (bInitialized)
            {
                bInitialized = false;
                _sender.Close();
            }
        }

        public bool Initial()
        {
            bool isOK = true;
            try
            {
                if (bInitialized)
                {
                    bInitialized = false;
                    /* _socket.Close(); */
                    _sender.Close();
                }
                IPAddress ipaddress = IPAddress.Parse(_defaultBroadcastIP);
                _ep = new IPEndPoint(ipaddress, _defaultPort);

                _locEp = new IPEndPoint(IPAddress.Any, 0);
                _sender = new UdpClient();
                _sender.ExclusiveAddressUse = false;
                _sender.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _sender.Client.Bind(_locEp);
                _locPort = ((IPEndPoint)_sender.Client.LocalEndPoint).Port;
/*
                IPAddress ipaddress = IPAddress.Parse(_defaultBroadcastIP);
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _ep = new IPEndPoint(ipaddress, _defaultPort);
                */
                bInitialized = true;
            }
            catch(Exception e)
            {
                isOK = false;
                MessageBox.Show("An error occurred when initial client:" + e.Message);
            }
           
            return isOK;
        }

        public bool Initial(string sIp, int uPort)
        {
            bool isOK = true;
            try 
            {
                if (bInitialized)
                {
                    bInitialized = false;
                    _sender.Close();
                }

                IPAddress ipaddress = IPAddress.Parse(sIp);
                _ep = new IPEndPoint(ipaddress, uPort);

                _locEp = new IPEndPoint(IPAddress.Any, 0);
                _sender = new UdpClient();
                _sender.ExclusiveAddressUse = false;
                _sender.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _sender.Client.Bind(_locEp);
                _locPort = ((IPEndPoint)_sender.Client.LocalEndPoint).Port;
/*
                IPAddress ipaddress = IPAddress.Parse(sIp);
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _ep = new IPEndPoint(ipaddress, uPort);  */
                bInitialized = true;
            }
            catch (Exception e)
            {
                isOK = false;
                MessageBox.Show("An error occurred when initial client:" + e.Message);
            }

            return isOK;
        }

        public bool Initial(string sIp)
        {
            bool isOK = true;
            try
            {
                if (bInitialized)
                {
                    bInitialized = false;
                    _sender.Close();
                }
                IPAddress ipaddress = IPAddress.Parse(sIp);
                _ep = new IPEndPoint(ipaddress, _defaultPort);

                _locEp = new IPEndPoint(IPAddress.Any, 0);
                _sender = new UdpClient();
                _sender.ExclusiveAddressUse = false;
                _sender.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _sender.Client.Bind(_locEp);
                _locPort = ((IPEndPoint)_sender.Client.LocalEndPoint).Port;

                /*
                IPAddress ipaddress = IPAddress.Parse(sIp);
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _ep = new IPEndPoint(ipaddress, _defaultPort);  */

                bInitialized = true;
            }
            catch (Exception e)
            {
                isOK = false;
                // MessageBox.Show("An error occurred when initial client:" + e.Message);
                throw (e);
            }

            return isOK;
        }

        /// <summary>
        /// After sending a message, this method can initial a listening on the 
        /// port(local) used in sening the message. 
        /// </summary>
        /// <param name="message"> message received from remote</param>
        /// <param name="timeOutInMs">time out in MS</param>
        /// <returns>true: ack message received</returns>
        public bool ListeningAck(out string message, int timeOutInMs)
        {
            bool isRcvd = false;
            message = "";
            if(bInitialized)
            {
                try
                { 
                    IPEndPoint localEP = new IPEndPoint(IPAddress.Any, _locPort);          
                    UdpClient udpServer = new UdpClient();
                    udpServer.ExclusiveAddressUse = false;
                    udpServer.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                //    udpServer.Client.ReceiveTimeout = timeOutInMs;
                    udpServer.Client.Bind(localEP);
                    

                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

                    byte[] bytes = udpServer.Receive(ref remoteEP);
                    if (null != bytes && bytes.Length > 0)
                    {
                        message = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                        isRcvd = true;
                    }
                }
                catch(Exception ex)
                {
                    throw (ex);
                }
             }

             return isRcvd;
        }

    }
}
