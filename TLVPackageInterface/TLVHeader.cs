using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TLVPackageInterface
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TLVHeader
    {
        public const int CURRENT_VERSION_OF_RIGID_TLV = 5;
        public const int HEADER_LEN = 24;
        public const string PACKAGE_HEADER = "T!R";
        public const int RP_MAX_PACKAGE_SIZE = 100000;
        public const int RP_MAX_TAG_COUNT = 2000;
        public const int RP_MAC_LEN = 4;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        byte[] start_of_header; //0
        byte header_version; //3
        public uint source_serial_id { get; private set; } //4
        public uint destination_serial_id { get; private set; } //8
        public uint gateway_address { get; private set; }//12
        public uint package_id; //16
        public TLVPackageTypes package_type { get; private set; } //20
        public byte sequence_number; //21
        public UInt16 payload_len { get; set; } //22

        public void Init(TLVPackageTypes pkgtype, uint srcid, uint dstid, uint gwid,
                        uint packageid, byte seqnumber)
        {
            start_of_header = Encoding.ASCII.GetBytes(PACKAGE_HEADER);
            header_version = CURRENT_VERSION_OF_RIGID_TLV;
            package_type = pkgtype;
            header_version = CURRENT_VERSION_OF_RIGID_TLV;
            source_serial_id = srcid;
            package_id = packageid;
            destination_serial_id = dstid;
            gateway_address = gwid;
            payload_len = 0;
            sequence_number = seqnumber;
        }
        public static TLVHeader Deserialize(byte[] buffer)
        {
            if (buffer.Length < HEADER_LEN)
            {
                throw new Exception($"TLVHeader Deserialize Error: short buffer size {buffer.Length}<{HEADER_LEN}");
            }
            var header = new TLVHeader();
            header.start_of_header = buffer.Take(3).ToArray();
            header.header_version = buffer[3];
            header.source_serial_id = BitConverter.ToUInt32(buffer, 4);
            header.destination_serial_id = BitConverter.ToUInt32(buffer, 8);
            header.gateway_address = BitConverter.ToUInt32(buffer, 12);
            header.package_id = BitConverter.ToUInt32(buffer, 16);
            header.package_type = (TLVPackageTypes)buffer[20];
            header.sequence_number = buffer[21];
            header.payload_len = BitConverter.ToUInt16(buffer, 22);
            return header;
        }

        public bool HeaderStartValid
        {
            get { return Encoding.ASCII.GetString(start_of_header) == PACKAGE_HEADER; }
        }

        public string HeaderStart
        {
            get { return Encoding.ASCII.GetString(start_of_header); }
        }

        public bool CheckHealth()
        {
            if (header_version != CURRENT_VERSION_OF_RIGID_TLV)
            {
                throw new Exception($"initialBufferToDecodeTLV:PackagerInterfaceTLV Header Version error %{header_version}<>%{CURRENT_VERSION_OF_RIGID_TLV}!");

            }
            if (!HeaderStartValid)
            {
                throw new Exception($"initialBufferToDecodeTLV:PackagerInterface TLV Header error header:'{HeaderStart}'!");
            }
            return true;
        }
    }
}