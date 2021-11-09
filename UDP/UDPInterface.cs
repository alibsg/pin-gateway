using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace PinTcpRedirect.UDP
{
    public class UDPInterface
    {
        public int LocalPort { get; }
        private UdpClient client;
        public UDPInterface(int localport)
        {
            LocalPort = localport;
            var ep = new IPEndPoint(IPAddress.Any, LocalPort);
            client = new UdpClient(ep);
        }

        // This metod is blocking
        public byte[] Receive(ref IPEndPoint endPoint, ref string destinationIP)
        {
            // return client.Receive(ref endPoint);
            IPPacketInformation packetInformation;
            SocketFlags flags = SocketFlags.None;
            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            var buffer = new byte[1024 * 64];
            client.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
            int len = client.Client.ReceiveMessageFrom(buffer, 0, 1024 * 64, ref flags, ref remoteEP, out packetInformation);
            endPoint = (IPEndPoint)remoteEP;
            destinationIP = packetInformation.Address.ToString();
            var actualBuffer = new byte[len];
            Array.Copy(buffer, actualBuffer, len);
            return actualBuffer;
        }

        public void Send(byte[] buffer, string ip, ushort port)
        {
            var ep = new IPEndPoint(IPAddress.Parse(ip), port);
            client.Send(buffer, buffer.Length, ep);
        }
        public void Send(byte[] buffer, IPEndPoint ep)
        {
            client.Send(buffer, buffer.Length, ep);
        }

        public void Close()
        {
            client.Close();
        }

        public async Task<UdpReceiveResult> RecievAsync()
        {
            return await client.ReceiveAsync();
        }
    }
}