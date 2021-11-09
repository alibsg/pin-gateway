using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PinTcpRedirect.UDP;

namespace PinTcpRedirect.Client
{
    public class ClientTask
    {
        private UDPInterface UdpFromServer;
        private UDPInterface UdpLocal;
        string LocalRiIP;
        ushort LocalRiPort;
        string ServerIp;
        // this port can be equal for server and client 
        ushort MyServerPort;
        ushort MyRIPort;
        Timer KeepAliveTimer;

        public ClientTask(string localRiIP, ushort localRiPort, string serverIp, ushort myServerPort, ushort myRIPort)
        {
            LocalRiIP = localRiIP;
            LocalRiPort = localRiPort;
            ServerIp = serverIp;
            MyServerPort = myServerPort;
            MyRIPort = myRIPort;
            UdpFromServer = new(myServerPort);
            UdpLocal = new(MyRIPort);
            KeepAliveTimer = new Timer(keepAlive, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(10));
            keepAlive(null);
        }

        public Task ServerToClientSideTask()
        {
            IPEndPoint recep = null!;
            string destIp = "";
            while (true)
            {
                byte[] recmsg = UdpFromServer.Receive(ref recep, ref destIp);
                UdpLocal.Send(recmsg, LocalRiIP, LocalRiPort);
            }
        }
        public Task LocalRiToServerTask()
        {
            IPEndPoint recep = null!;
            string destIp = "";
            while (true)
            {
                byte[] recmsg = UdpLocal.Receive(ref recep, ref destIp);
                UdpFromServer.Send(recmsg, ServerIp, MyServerPort);
            }
        }

        private void keepAlive(object? o)
        {
            UdpFromServer.Send(Encoding.ASCII.GetBytes(new String('*', 10)), ServerIp, MyServerPort);
        }

    }
}