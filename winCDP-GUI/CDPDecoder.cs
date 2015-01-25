using PcapDotNet.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        private string hexDump(string t, byte[] s, int size, int bpl)
        {
            int l;
            StringBuilder sb = new StringBuilder();
            StringBuilder st = new StringBuilder();

            l = ("00) " + sb.ToString()).Length;
            int j = 0;

            for (int i = 0; i < size; i++)
            {
                if (j == 0)
                {
                    if (i != 0)
                    {
                        sb.Append((i.ToString("X2") + ") ").PadLeft(l, ' '));
                    }
                    else
                    {
                        sb.Append((i.ToString("X2") + ") "));
                    }
                    st.Clear();
                }
                else
                {
                    sb.Append(":");
                }

                sb.Append(s[i].ToString("X2"));

                if (!char.IsControl(Convert.ToChar(s[i])))
                {
                    st.Append(Convert.ToChar(s[i]).ToString());
                }
                else
                {
                    st.Append(".");
                }

                j++;

                if (j == bpl)
                {
                    j = 0;
                    sb.Append(" " + st.ToString() + "\n" + t);
                }
            }

            if (j != 0)
            {
                for (; j < bpl; j++)
                {
                    sb.Append(":--");
                    st.Append(".");
                }
                sb.Append(" " + st.ToString() + "\n");
            }


            return sb.ToString();
        }

        private bool byteCmp(byte a, byte b)
        {
            return (a & b) == b;
        }

        private string byte2IPV6(byte[] s)
        {
            IPAddress ipv6 = new IPAddress(s);

            return ipv6.ToString();
            /*
            int pos = 0;
            return "IPV6 : " + (s[pos++] << 8 | s[pos++]).ToString("X4") + ":" +
                (s[pos++] << 8 | s[pos++]).ToString("X4") + ":" +
                (s[pos++] << 8 | s[pos++]).ToString("X4") + ":" +
                (s[pos++] << 8 | s[pos++]).ToString("X4") + ":" +
                (s[pos++] << 8 | s[pos++]).ToString("X4") + ":" +
                (s[pos++] << 8 | s[pos++]).ToString("X4") + ":" +
                (s[pos++] << 8 | s[pos++]).ToString("X4") + ":" +
                (s[pos++] << 8 | s[pos++]).ToString("X4") + ":";
             */
        }

        private string byte2IPV4(byte[] s)
        {
            IPAddress ipv4 = new IPAddress(s);

            return ipv4.ToString();
            /*
            int pos = 0;
            return "IPV4 : " + s[pos++].ToString("d") + "." +
                s[pos++].ToString("d") + "." +
                s[pos++].ToString("d") + "." +
                s[pos++].ToString("d");
             */
        }

        

        private string nlpid2String(UInt64 protocol, byte[] address, int address_length)
        {
            StringBuilder sb = new StringBuilder();
            int wrap = 8;
            if (address_length < 8)
            {
                wrap = address_length;
            }
            switch (protocol)
            {
                case 0x00: // NLPID_NULL
                    sb.Append(hexDump("NULL : Length : ", address, address_length, wrap));
                    sb.Append("\n");
                    break;
                case 0x08: // NLPID_Q933
                    sb.Append(hexDump("Q933 : ", address, address_length, wrap));
                    sb.Append("\n");
                    break;
                case 0x80: // NLPID_SNAP
                    sb.Append(hexDump("SNAP : ", address, address_length, wrap));
                    sb.Append("\n");
                    break;
                case 0x81: // NLPID_CLNP
                    sb.Append(hexDump("CLPN : ", address, address_length, wrap));
                    sb.Append("\n");
                    break;
                case 0x82: // NLPID_ESIS
                    sb.Append(hexDump("ESIS : ", address, address_length, wrap));
                    sb.Append("\n");
                    break;
                case 0x83: // NLPID_ISIS				
                    sb.Append(hexDump("ISIS : ", address, address_length, wrap));
                    sb.Append("\n");
                    break;
                case 0x8E: // NLPID_IPV6
                    sb.Append(byte2IPV6(address));
                    sb.Append("\n");
                    break;
                case 0xB0: // NLPID_FRF9
                    sb.Append(hexDump("FRF9 : ", address, address_length, wrap));
                    sb.Append("\n");
                    break;
                case 0xB1: // NLPID_FRF12
                    sb.Append(hexDump("FRF12 : ", address, address_length, wrap));
                    sb.Append("\n");
                    break;
                case 0xC0: // NLPID_TRILL
                    sb.Append(hexDump("Trill : ", address, address_length, wrap));
                    sb.Append("\n");
                    break;
                case 0xC1: // NLPID_8021AQ			
                    sb.Append(hexDump("8021AQ : ", address, address_length, wrap));
                    sb.Append("\n");
                    break;
                case 0xCC: // NLPID_IPV4
                    sb.Append(byte2IPV4(address));
                    sb.Append("\n");
                    break;
                case 0xCF: // NLPID_PPP			
                    sb.Append(hexDump("PPP : ", address, address_length, wrap));
                    sb.Append("\n");
                    break;
                default:
                    sb.Append(hexDump("Unknown type : ", address, address_length, wrap));
                    sb.Append("\n");
                    break;
            }
            return sb.ToString();
        }

        private string decodeAddress(string s, byte[] data, int size)
        {
            StringBuilder sb = new StringBuilder();
            int pos = 0;
            int address_count = 0;

            for (int i = 0; i < 4; i++)
            {
                address_count = address_count << 8 | data[pos++];
            }

            sb.Append(s);
            while (pos < size)
            {
                int type = data[pos++];
                int len = data[pos++];
                UInt64 protocol = 0;

                switch (type)
                {
                    case 1:
                        // NLPID Format

                        sb.Append("Protocol format : NLPID\n");
                        protocol = data[pos++];
                        break;
                    case 2:
                        // 802.2 format
                        sb.Append("Protocol format : 802.2\n");
                        for (int i = 0; i < len; i++)
                        {
                            protocol = (protocol << 8) | data[pos++];
                        }
                        break;
                    default:
                        byte[] protHex = null;
                        Array.Resize(ref protHex, len);

                        for (int i = 0; i < len; i++)
                        {
                            protHex[i] = data[pos++];
                        }
                        sb.Append(hexDump("Protocol format : Unknown ", protHex, len, 8));
                        break;
                }

                if (type != 0)
                {
                    int address_length = data[pos++] << 8 | data[pos++];
                    byte[] address = null;

                    Array.Resize(ref address, address_length);

                    for (int i = 0; i < address_length; i++)
                    {
                        address[i] = data[pos++];
                    }

                    sb.Append(new String(' ', s.Length) + nlpid2String(protocol, address, address_length) + "\n");
                }
            }
            return sb.ToString();
        }

        public CDPToText(CDPMessage[] cdpMessages)
        {
            StringBuilder sb = new StringBuilder();

            foreach (CDPMessage cdpMessage in cdpMessages)
            {
                switch (cdpMessage.Type)
                {
                    case 0x0001:
                        sb.Append("Device ID : " + Encoding.Default.GetString(cdpMessage.Data, 0, cdpMessage.Size) + "\n\n");

                        break;
                    case 0x0002:
                        sb.Append("Addresses : ");
                        sb.Append(decodeAddress("Addresses : ", cdpMessage.Data, cdpMessage.Size));
                        break;
                    case 0x0003:
                        sb.Append("Port ID : " + Encoding.Default.GetString(cdpMessage.Data, 0, cdpMessage.Size) + "\n\n");
                        break;
                    case 0x0004:
                        {
                            string st = "\n" + new String(' ', 15);
                            sb.Append("Capabilities : ");
                            for (int i = 0; i < cdpMessage.Size; i++)
                            {
                                if (byteCmp(cdpMessage.Data[i], 0x01))
                                {
                                    sb.Append("Performs level 3 routing for at least one network layer protocol." + st);
                                }
                                if (byteCmp(cdpMessage.Data[i], 0x02))
                                {
                                    sb.Append("Performs level 2 transparent bridging." + st);
                                }
                                if (byteCmp(cdpMessage.Data[i], 0x04))
                                {
                                    sb.Append("Performs level 2 source-route bridging." + st);
                                }
                                if (byteCmp(cdpMessage.Data[i], 0x08))
                                {
                                    sb.Append("Performs level 2 switching." + st);
                                }
                                if (byteCmp(cdpMessage.Data[i], 0x10))
                                {
                                    sb.Append("Sends and receives packets for at least one network layer protocol." + st);
                                }
                                if (byteCmp(cdpMessage.Data[i], 0x20))
                                {
                                    sb.Append("The bridge or switch does not forward IGMP." + st);
                                }
                                if (byteCmp(cdpMessage.Data[i], 0x40))
                                {
                                    sb.Append("Provides level 1 functionality." + st);
                                }
                            }
                            sb.Append("\n");
                        }
                        break;
                    case 0x0005:
                        sb.Append("Software version:\n\n" + Encoding.Default.GetString(cdpMessage.Data, 0, cdpMessage.Size) + "\n\n");
                        break;
                    case 0x0006:
                        sb.Append("Platform: " + Encoding.Default.GetString(cdpMessage.Data, 0, cdpMessage.Size) + "\n\n");
                        break;
                    case 0x0007:
                        sb.Append("IP Prefix : ");
                        sb.Append(hexDump(new String(' ', 12), cdpMessage.Data, cdpMessage.Size, 8));
                        // XXX TODO Decode Address
                        sb.Append("\n");
                        break;
                    case 0x0008:
                        sb.Append("Protocol Hello : ");
                        sb.Append(hexDump(new String(' ', 17), cdpMessage.Data, cdpMessage.Size, 8));
                        // XXX TODO Decode Address
                        sb.Append("\n");
                        break;
                    case 0x0009:
                        sb.Append("VTP Domain : " + Encoding.Default.GetString(cdpMessage.Data, 0, cdpMessage.Size) + "\n\n");
                        break;
                    case 0x000a:
                        {
                            UInt64 vlan = 0;
                            sb.Append("Native VLAN : ");
                            for (int i = 0; i < cdpMessage.Size; i++)
                            {
                                vlan = (vlan << 8) | cdpMessage.Data[i];
                            }
                            sb.Append(vlan.ToString() + "\n\n");
                        }
                        break;
                    case 0x000b:
                        sb.Append("Duplex : ");
                        if (cdpMessage.Data[0] == 0)
                        {
                            sb.Append("Half\n\n");
                        }
                        else
                        {
                            sb.Append("Full\n\n");
                        }
                        break;
                    case 0x000e:
                        {
                            int index = cdpMessage.Data[0], count = 0;
                            sb.Append("VOIP Vlan : ");

                            for (int i = 1; i < cdpMessage.Size; i += 2)
                            {
                                int vlan = 0;
                                count++;
                                vlan = (cdpMessage.Data[i] << 8) | cdpMessage.Data[i + 1];
                                sb.Append(vlan.ToString());
                                if (count < index)
                                {
                                    sb.Append(", ");
                                }
                            }
                            sb.Append("\n\n");
                        }
                        break;
                    case 0x000f:
                        sb.Append("VOIP Vlan Query : ");
                        sb.Append(hexDump(new String(' ', 18), cdpMessage.Data, cdpMessage.Size, 8));
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0010:
                        sb.Append("Power : ");
                        sb.Append(hexDump(new String(' ', 8), cdpMessage.Data, cdpMessage.Size, 8));
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0011:
                        sb.Append("MTU : ");
                        sb.Append(hexDump(new String(' ', 6), cdpMessage.Data, cdpMessage.Size, 8));
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0012:
                        sb.Append("Trust Bitmap : ");
                        sb.Append(hexDump(new String(' ', 15), cdpMessage.Data, cdpMessage.Size, 8));
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0013:
                        sb.Append("Untrusted COS : ");
                        sb.Append(hexDump(new String(' ', 16), cdpMessage.Data, cdpMessage.Size, 8));
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0014:
                        sb.Append("System Name : " + Encoding.Default.GetString(cdpMessage.Data, 0, cdpMessage.Size) + "\n\n");
                        break;
                    case 0x0015:
                        sb.Append("System ODI : ");
                        sb.Append(hexDump(new String(' ', 13), cdpMessage.Data, cdpMessage.Size, 8));
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0016:
                        sb.Append(decodeAddress("Management Address : ", cdpMessage.Data, cdpMessage.Size));
                        sb.Append("\n\n");
                        break;
                    case 0x0017:
                        sb.Append("Location : ");
                        sb.Append(hexDump(new String(' ', 11), cdpMessage.Data, cdpMessage.Size, 8));
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0018:
                        sb.Append("External Port ID : ");
                        sb.Append(hexDump(new String(' ', 19), cdpMessage.Data, cdpMessage.Size, 8));
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x0019:
                        sb.Append("Power Requested : ");
                        sb.Append(hexDump(new String(' ', 18), cdpMessage.Data, cdpMessage.Size, 8));
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x001a:
                        sb.Append("Power Avalable : ");
                        sb.Append(hexDump(new String(' ', 17), cdpMessage.Data, cdpMessage.Size, 8));
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x001b:
                        sb.Append("Port Unidir : ");
                        sb.Append(hexDump(new String(' ', 14), cdpMessage.Data, cdpMessage.Size, 8));
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x001d:
                        sb.Append("NRGYZ : ");
                        sb.Append(hexDump(new String(' ', 8), cdpMessage.Data, cdpMessage.Size, 8));
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x001f:
                        sb.Append("Spare POE : ");
                        sb.Append(hexDump(new String(' ', 12), cdpMessage.Data, cdpMessage.Size, 8));
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    case 0x1003:
                        sb.Append("Could someone tell me what this Type (0x1003) is? :\n");
                        sb.Append(hexDump("", cdpMessage.Data, cdpMessage.Size, 8));
                        // XXX TODO Decode Address
                        sb.Append("\n\n");
                        break;
                    default:
                        sb.Append("Unknown Message (" + cdpMessage.Type.ToString() + ") :\n");
                        sb.Append(hexDump("", cdpMessage.Data, cdpMessage.Size, 8));
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
