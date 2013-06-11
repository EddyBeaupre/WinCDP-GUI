using PcapDotNet.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winCDP_GUI
{
    public class CDPDecoder
    {
        public Byte Version { get; private set; }
        public Byte TTL { get; private set; }
        public UInt16 CheckSum { get; private set; }
        public CDPMessage[] Data { get; private set; }
        public Byte CDPMessageCount { get; private set; }

        public CDPDecoder(Packet packet)
        {
            CDPMessage[] data = null;
            int Pos = 22;
            int Count = 0;
            Version = packet[Pos++];
            TTL = packet[Pos++];
            CheckSum = (UInt16)((UInt16)(packet[Pos++] << 8) | (UInt16)(packet[Pos++]));

            while (Pos < packet.Length)
            {
                Array.Resize(ref data, Count + 1);
                data[Count] = new CDPMessage(Pos, packet);
                //MessageBox.Show("Message #" + Count + ", CDP Message : " + data[Count].typeName() + ", Length : " + data[Count].Size.ToString());
                Pos = Pos + data[Count].Size + 4;
                Count++;
            }

            Data = data;
        }

    }

    public class CDPType0001
    {
        public string data { get; private set; }

        public CDPType0001(CDPMessage cdp)
        {

            data = Encoding.Default.GetString(cdp.Data, 0, cdp.Size);
        }
    }

    public class CDPType0002
    {
        public Byte type { get; private set; }
        public UInt64 protocol { get; private set; }
        public Byte[] address { get; private set; }

        public CDPType0002(CDPMessage cdp)
        {
            int pos = 0;
            int len = 0;
            Byte[] d = null;

            type = cdp.Data[pos++];

            len = cdp.Data[pos++];
            protocol = 0;
            for (int i = 0; i < len; i++)
            {
                protocol = protocol << 8 | cdp.Data[pos++];
            }

            len = cdp.Data[pos++];
            Array.Resize(ref d, len);
            for (int i = 0; i < len; i++)
            {
                d[i] = cdp.Data[pos++];
            }

            address = d;
        }
    }

    public class CDPType0003
    {
        public string data { get; private set; }

        public CDPType0003(CDPMessage cdp)
        {
            data = Encoding.Default.GetString(cdp.Data, 0, cdp.Size);
        }
    }

    public class CDPType0004
    {
        public Byte[] data { get; private set; }

        public CDPType0004(CDPMessage cdp)
        {
            int pos = 0;
            Byte[] d = null;

            Array.Resize(ref d, cdp.Size);
            for (int i = 0; i < cdp.Size; i++)
            {
                d[i] = cdp.Data[pos++];
            }

            data = d;
        }

        private string[] description(Byte b)
        {
            int items = 0;
            string[] s = null;

            if (b == 0x01)
            {
                Array.Resize(ref s, items + 1);
                s[items] = "Performs level 3 routing for at least one network layer protocol.";
                items++;
            }
            if (b == 0x02)
            {
                Array.Resize(ref s, items + 1);
                s[items] = "Performs level 2 transparent bridging.";
                items++;
            }
            if (b == 0x04)
            {
                Array.Resize(ref s, items + 1);
                s[items] = "PPerforms level 2 source-route bridging.";
                items++;
            }
            if (b == 0x08)
            {
                Array.Resize(ref s, items + 1);
                s[items] = "Performs level 2 switching.";
                items++;
            }
            if (b == 0x10)
            {
                Array.Resize(ref s, items + 1);
                s[items] = "Sends and receives packets for at least one network layer protocol.";
                items++;
            }
            if (b == 0x20)
            {
                Array.Resize(ref s, items + 1);
                s[items] = "The bridge or switch does not forward IGMP";
                items++;
            }
            if (b == 0x40)
            {
                Array.Resize(ref s, items + 1);
                s[items] = "Provides level 1 functionality.";
                items++;
            }
            return s;
        }

        public string[][] description()
        {
            string[][] s = null;

            Array.Resize(ref s, data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                s[i] = description(data[i]);
            }

            return s;
        }
    }

    public class CDPType0005
    {
        public string data { get; private set; }

        public CDPType0005(CDPMessage cdp)
        {
            data = Encoding.Default.GetString(cdp.Data, 0, cdp.Size);
        }
    }

    public class CDPType0006
    {
        public string data { get; private set; }

        public CDPType0006(CDPMessage cdp)
        {
            data = Encoding.Default.GetString(cdp.Data, 0, cdp.Size);
        }
    }

    // XXX TODO
    public class CDPType0007
    {
        public string data { get; private set; }

        public CDPType0007(CDPMessage cdp)
        {
            data = Encoding.Default.GetString(cdp.Data, 0, cdp.Size);
        }
    }

    // XXX TODP
    public class CDPType0008
    {
        public string data { get; private set; }

        public CDPType0008(CDPMessage cdp)
        {
            data = Encoding.Default.GetString(cdp.Data, 0, cdp.Size);
        }
    }

    public class CDPType0009
    {
        public string data { get; private set; }

        public CDPType0009(CDPMessage cdp)
        {
            data = Encoding.Default.GetString(cdp.Data, 0, cdp.Size);
        }
    }

    public class CDPToText
    {
        public string text { get; private set; }

        private string hexDump(byte[] s, int size)
        {
            StringBuilder sb = new StringBuilder();
            int j=0;
            for (int i = 0; i < size; i++)
            {
                if (j == 0)
                {
                    sb.Append(i.ToString("X2") + ": ");
                }
                else
                {
                    sb.Append(":");
                }

                sb.Append(s[i].ToString("X2"));
                j++;

                if (j == 8)
                {
                    j = 0;
                    sb.Append("\n");
                }
            }

            if (j != 0)
            {
                for (; j < 8; j++)
                {
                    sb.Append(":--");
                }
                sb.Append("\n");
            }

            
            return sb.ToString();
        }

        private bool byteCmp(byte a, byte b)
        {
            return (a & b) == b;
        }

        private string byte2IPV4(byte[] s)
        {
            int pos = 0;
            return s[pos++].ToString("d") + "." + s[pos++].ToString("d") + "." + s[pos++].ToString("d") + "." + s[pos++].ToString("d");
        }

        public CDPToText(CDPMessage[] cdpMessages)
        {
            StringBuilder sb = new StringBuilder();

            foreach (CDPMessage cdpMessage in cdpMessages)
            {
                switch (cdpMessage.Type)
                {
                    case 0x0001:
                        sb.Append("Device ID :\n\t" + Encoding.Default.GetString(cdpMessage.Data, 0, cdpMessage.Size) + "\n\n");

                        break;
                    case 0x0002:
                        sb.Append("Addresses :\n");
                        int pos = 0;
                        while (pos < cdpMessage.Size)
                        {

                            int type = cdpMessage.Data[pos++];
                            int len = cdpMessage.Data[pos++];
                            //sb.Append(hexDump(cdpMessage.Data, cdpMessage.Size));
                            //MessageBox.Show("Protocol Type : " + cdpMessage.Type.ToString("X") + ", Length : " + cdpMessage.Size);
                            switch (type)
                            {
                                case 0:
                                    break;
                                case 1:
                                    // NLPID Format
                                    int protocol = cdpMessage.Data[pos++];
                                    int address_length = cdpMessage.Data[pos++] << 8 | cdpMessage.Data[pos++];
                                    byte[] address = null;
                                    Array.Resize(ref address, address_length);
                                    for (int i = 0; i < address_length; i++)
                                    {
                                        address[i] = cdpMessage.Data[pos++];
                                    }
                                    switch (protocol)
                                    {
                                        case 0x00: // NLPID_NULL
                                            sb.Append("\tNULL : ");
                                            sb.Append("\n");
                                            break;
                                        case 0x08: // NLPID_Q933
                                            sb.Append("\tQ933 : ");
                                            sb.Append("\n");
                                            break;
                                        case 0x80: // NLPID_SNAP
                                            sb.Append("\tSNAP : ");
                                            sb.Append("\n");
                                            break;
                                        case 0x81: // NLPID_CLNP
                                            sb.Append("\tCLPN : ");
                                            sb.Append("\n");
                                            break;
                                        case 0x82: // NLPID_ESIS
                                            sb.Append("\tESIS : ");
                                            sb.Append("\n");
                                            break;
                                        case 0x83: // NLPID_ISIS				
                                            sb.Append("\tISIS : ");
                                            sb.Append("\n");
                                            break;
                                        case 0x8E: // NLPID_IPV6
                                            sb.Append("\tIPV6 : ");
                                            sb.Append("\n");
                                            break;
                                        case 0xB0: // NLPID_FRF9
                                            sb.Append("\tFRF9 : ");
                                            sb.Append("\n");
                                            break;
                                        case 0xB1: // NLPID_FRF12
                                            sb.Append("\tFRF12 : ");
                                            sb.Append("\n");
                                            break;
                                        case 0xC0: // NLPID_TRILL
                                            sb.Append("\tTrill : ");
                                            sb.Append("\n");
                                            break;
                                        case 0xC1: // NLPID_8021AQ			
                                            sb.Append("\t8021AQ : ");
                                            sb.Append("\n");
                                            break;
                                        case 0xCC: // NLPID_IPV4
                                            sb.Append("\tIPV4 : " + byte2IPV4(address));
                                            sb.Append("\n");
                                            break;
                                        case 0xCF: // NLPID_PPP			
                                            sb.Append("\tPPP : ");
                                            sb.Append("\n");
                                            break;
                                        default:
                                            sb.Append("\tUnknown type : ");
                                            sb.Append("\n");
                                            break;
                                    }
                                    break;
                                case 2:
                                    break;
                                default:
                                    sb.Append("Cannot decode address, unknown protocol format");
                                    break;
                            }
                        }
                        sb.Append("\n");
                        break;
                    case 0x0003:
                        sb.Append("Port ID :\n\t" + Encoding.Default.GetString(cdpMessage.Data, 0, cdpMessage.Size) + "\n\n");
                        break;
                    case 0x0004:
                        sb.Append("Capabilities :" + "\n");
                        for (int i = 0; i < cdpMessage.Size; i++)
                        {
                            if (byteCmp(cdpMessage.Data[i], 0x01))
                            {
                                sb.Append("\tPerforms level 3 routing for at least one network layer protocol.\n");
                            }
                            if (byteCmp(cdpMessage.Data[i], 0x02))
                            {
                                sb.Append("\tPerforms level 2 transparent bridging.\n");
                            }
                            if (byteCmp(cdpMessage.Data[i], 0x04))
                            {
                                sb.Append("\tPerforms level 2 source-route bridging.\n");
                            }
                            if (byteCmp(cdpMessage.Data[i], 0x08))
                            {
                                sb.Append("\tPerforms level 2 switching.\n");
                            }
                            if (byteCmp(cdpMessage.Data[i], 0x10))
                            {
                                sb.Append("\tSends and receives packets for at least one network layer protocol.\n");
                            }
                            if (byteCmp(cdpMessage.Data[i], 0x20))
                            {
                                sb.Append("\tThe bridge or switch does not forward IGMP.\n");
                            }
                            if (byteCmp(cdpMessage.Data[i], 0x40))
                            {
                                sb.Append("\tProvides level 1 functionality.\n");
                            }
                        }
                        sb.Append("\n");
                        break;
                    case 0x0005:
                        sb.Append("Software version:\n\n" + Encoding.Default.GetString(cdpMessage.Data, 0, cdpMessage.Size) + "\n\n");
                        break;
                    case 0x0006:
                        sb.Append("Platform:\n\t" + Encoding.Default.GetString(cdpMessage.Data, 0, cdpMessage.Size) + "\n\n");
                        break;
                    case 0x0007:
                        sb.Append("IP Prefix :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0008:
                        sb.Append("Protocol Hello :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0009:
                        sb.Append("VTP Domain :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x000a:
                        sb.Append("Native VLAN :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x000b:
                        sb.Append("Duplex :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x000e:
                        sb.Append("VOIP Vlan Reply :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x000f:
                        sb.Append("VOIP Vlan Query :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0010:
                        sb.Append("Power :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0011:
                        sb.Append("MTU :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0012:
                        sb.Append("Trust Bitmap :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0013:
                        sb.Append("Untrusted COS :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0014:
                        sb.Append("System Name :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0015:
                        sb.Append("System ODI :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0016:
                        sb.Append("Management Address :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0017:
                        sb.Append("Location :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0018:
                        sb.Append("External Port ID :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0019:
                        sb.Append("Power Requested :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x001a:
                        sb.Append("Power Avalable :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x001b:
                        sb.Append("Port Unidir :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x001d:
                        sb.Append("NRGYZ :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x001f:
                        sb.Append("Spare POE :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x1003:
                        sb.Append("Could someone tell me what this Type (0x1003) is? :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    default:
                        sb.Append("Unknown Message (" + cdpMessage.Type.ToString() + ") :\n\t");
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                }
                text = sb.ToString();
            }
        }
    }

    public class CDPMessage
    {
        public UInt16 Type { get; private set; }
        public UInt16 Size { get; private set; }
        public Byte[] Data { get; private set; }

        public CDPMessage(int pos, Packet packet)
        {
            Byte[] d = null;

            Type = (UInt16)((UInt16)(packet[pos++] << 8) | (UInt16)(packet[pos++]));
            Size = (UInt16)((UInt16)((UInt16)(packet[pos++] << 8) | (UInt16)(packet[pos++])) - 4);

            Array.Resize(ref d, Size);
            for (int i = 0; i < Size; i++)
            {
                d[i] = packet[pos++];
            }

            Data = d;
        }

        public string typeName()
        {
            switch (Type)
            {
                case 0x0001:
                    return "Device ID";
                case 0x0002:
                    return "Addresses";
                case 0x0003:
                    return "Port ID";
                case 0x0004:
                    return "Capabilities";
                case 0x0005:
                    return "Software version";
                case 0x0006:
                    return "Platform";
                case 0x0007:
                    return "IP Prefix";
                case 0x0008:
                    return "Protocol Hello";
                case 0x0009:
                    return "VTP Domain";
                case 0x000a:
                    return "Native VLAN";
                case 0x000b:
                    return "Duplex";
                case 0x000e:
                    return "VOIP Vlan Reply";
                case 0x000f:
                    return "VOIP Vlan Query";
                case 0x0010:
                    return "Power";
                case 0x0011:
                    return "MTU";
                case 0x0012:
                    return "Trust Bitmap";
                case 0x0013:
                    return "Untrusted COS";
                case 0x0014:
                    return "System Name";
                case 0x0015:
                    return "System ODI";
                case 0x0016:
                    return "Management Address";
                case 0x0017:
                    return "Location";
                case 0x0018:
                    return "External Port ID";
                case 0x0019:
                    return "Power Requested";
                case 0x001a:
                    return "Power Avalable";
                case 0x001b:
                    return "Port Unidir";
                case 0x001d:
                    return "NRGYZ";
                case 0x001f:
                    return "Spare POE";
                case 0x1003:
                    return "Could someone tell me what this Type (0x1003) is?";
                default:
                    return "Unknown (" + Type.ToString() + ")";
            }
        }
    }
}
