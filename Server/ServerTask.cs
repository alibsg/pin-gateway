using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PinTcpRedirect.UDP;
using TLVPackageInterface;

namespace PinTcpRedirect.Server
{

    public class ServerTask
    {
        ConcurrentDictionary<uint, IPEndPoint> PinMap = new();
        UDPInterface PinUdp;
        UDPInterface MyUdp;
        IPEndPoint ClientEp = null!;
        public ServerTask(ushort udpPinListenPort, ushort myListenPort)
        {
            PinUdp = new UDPInterface(udpPinListenPort);
            MyUdp = new UDPInterface(myListenPort);
        }

        public Task PinToClient()
        {
            IPEndPoint pinEp = null!;
            string destIp = "";

            while (true)
            {
                byte[] recmsg = PinUdp.Receive(ref pinEp, ref destIp);
                if (ClientEp is null)
                    continue;
                try
                {
                    TLVHeader tLVHeader = TLVHeader.Deserialize(recmsg);
                    PinMap.AddOrUpdate(tLVHeader.source_serial_id, pinEp, (serial, ep) => pinEp);
                    MyUdp.Send(recmsg, ClientEp);

                }
                catch (System.Exception)
                {
                    continue;
                }

            }
        }
        public Task FromClient()
        {
            IPEndPoint pinEp = null!;
            string destIp = "";

            while (true)
            {
                byte[] recmsg = MyUdp.Receive(ref pinEp, ref destIp);
                if (recmsg.Length == 10)
                {
                    string pattern = new String('*', 10);
                    if (Encoding.ASCII.GetString(recmsg) == pattern)
                    {
                        ClientEp = pinEp;
                        continue;
                    }
                }
                try
                {
                    TLVHeader tLVHeader = TLVHeader.Deserialize(recmsg);
                    if (PinMap.TryGetValue(tLVHeader.destination_serial_id, out IPEndPoint? destEp))
                    {
                        MyUdp.Send(recmsg, destEp);
                    }
                }
                catch (System.Exception)
                {
                    continue;
                }

            }
        }
    }
}