using PcapDotNet.Core;
using PcapDotNet.Packets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace winCDP_GUI
{
    public partial class winCDP : Form
    {
        private IList<LivePacketDevice> allDevices;
        protected bool isdone = false;
        protected CDPWorkerObject cdpWorkerObject;

        public winCDP()
        {
            InitializeComponent();
        }

        public string extractSubString(char a, string c)
        {
            if (c.IndexOf(a) > 0)
            {
                return c.Split(a)[1];
            }
            else
            {
                return c;
            }
        }

        private void winCDP_Load(object sender, EventArgs e)
        {
            // Retrieve the device list from the local machine
            allDevices = LivePacketDevice.AllLocalMachine;

            if (allDevices.Count == 0)
            {
                MessageBox.Show("No interfaces found! Make sure WinPcap is installed.", "Error", MessageBoxButtons.OK);
                return;
            }

            // Print the list
            for (int i = 0; i != allDevices.Count; ++i)
            {
                LivePacketDevice device = allDevices[i];

                if (device.Description != null)
                {

                    adapterList.Items.Add(extractSubString('\'', device.Description.ToString()) + " [" + device.Name.ToString().Replace("rpcap://", "") + "]");

                }
                else
                {
                    adapterList.Items.Add(device.Name.ToString().Replace("rpcap://", ""));
                }
            }
            adapterList.SelectedIndex = 0;
        }

        private void adapterList_SelectedValueChanged(object sender, EventArgs e)
        {
            int i = 0;
            int s = -1;
            // IP addresses
            LivePacketDevice device = allDevices[adapterList.SelectedIndex];
            AdapterAddress.Items.Clear();
            foreach (DeviceAddress address in device.Addresses)
            {
                i++;
                AdapterAddress.Items.Add(address.Address.Family.ToString().PadRight(16) + "\t" + address.Address.ToString().Split(' ')[1].ToString());
                if (s == -1)
                {
                    if (address.Address.Family == SocketAddressFamily.Internet)
                    {
                        s = i;
                    }
                }
            }
            if (s == -1)
            {
                AdapterAddress.SelectedIndex = 0;
            }
            else
            {
                AdapterAddress.SelectedIndex = s - 1;
            }
        }

        private void startCapture_Click(object sender, EventArgs e)
        {
            progressBar.Value = 0;
            progressBar.Visible = true;
            startCapture.Enabled = false;
            startCapture.Visible = false;
            cdpWorkerObject = new CDPWorkerObject { selectedDevice = allDevices[adapterList.SelectedIndex], capturedPacket = null };
            cdpCaptureWorker.RunWorkerAsync(cdpWorkerObject);
            progressBarWorker.WorkerSupportsCancellation = true;
            progressBarWorker.RunWorkerAsync();
        }

        private void cdpCaptureWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            CDPWorkerObject device = e.Argument as CDPWorkerObject;
            device.communicator = device.selectedDevice.Open(512, PacketDeviceOpenAttributes.Promiscuous, 1000);
            {
                using (BerkeleyPacketFilter filter = device.communicator.CreateFilter("ether host 01:00:0c:cc:cc:cc and ether[16:4] = 0x0300000C and ether[20:2] == 0x2000"))
                {
                    device.communicator.SetFilter(filter);
                }
                device.communicator.NonBlocking = false;

                device.communicator.ReceivePackets(1, cdpWorkerObject.PacketHandler);
                e.Result = device;
            }
        }

        private void cdpCaptureWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void cdpCaptureWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CDPWorkerObject device = e.Result as CDPWorkerObject;
            progressBarWorker.CancelAsync();
            if (device.capturedPacket != null)
            {
                //MessageBox.Show(device.capturedPacket.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fff") + " length:" + device.capturedPacket.Length);
            }
        }

        private void progressBarWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            for (int i = 1; i <= 100; i++)
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // Wait 1200 milliseconds.
                    Thread.Sleep(1200);
                    // Report progress.
                    progressBarWorker.ReportProgress(i);
                }
            }
        }

        private void progressBarWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void progressBar_Click(object sender, EventArgs e)
        {

        }

        private void progressBarWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Visible = false;
            startCapture.Enabled = true;
            startCapture.Visible = true;
            if (cdpWorkerObject.capturedPacket == null)
            {
                cdpWorkerObject.communicator.Break();
                MessageBox.Show("Timeout while capturing packets.");
            }
            else
            {
                CDPDecoder cdpDecoder = new CDPDecoder(cdpWorkerObject.capturedPacket);
                MessageBox.Show("CDP Version : " + cdpDecoder.Version.ToString() + " CDP TTL : " + cdpDecoder.TTL.ToString() + " CDP Checksum : " + cdpDecoder.CheckSum.ToString());
            }
        }
    }
}

public class CDPWorkerObject
{
    public LivePacketDevice selectedDevice { get; set; }
    public Packet capturedPacket { get; set; }
    public PacketCommunicator communicator { get; set; }

    // Callback function invoked by libpcap for every incoming packet
    public void PacketHandler(Packet packet)
    {
        capturedPacket = packet;
    }

}

public class CDPMessage
{
    public UInt16 Type { get; private set; }
    public UInt16 Size { get; private set; }
    public Byte[] Data { get; private set; }

    public CDPMessage(int pos, Packet packet)
    {
        Byte[] data = null;

        Type = (UInt16)((UInt16)(packet[pos++] << 8) | (UInt16)(packet[pos++]));
        Size = (UInt16)((UInt16)(packet[pos++] << 8) | (UInt16)(packet[pos++]));
        Array.Resize(ref data, Size + 1);

        for (int i = 0; i < Size - 4; i++)
        {
            data[i] = packet[pos++];
        }

        Data = data;
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
                return "Could someone tell me what this Type is?";
            default:
                return "Unknown (" + Type.ToString() + ")";
        }
    }


}

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
            MessageBox.Show("Message #" + Count + ", CDP Message : " + data[Count].typeName() + ", Length : " + data[Count].Size.ToString());
            Pos = Pos + data[Count].Size;
            Count++;
        }

        Data = data;
    }

    /*
    public byte cdpVersion()
    {
        return cdpPacket.Buffer[0];
    }

    public byte cdpTTL()
    {
        return cdpPacket.Buffer[1];
    }

    public UInt16 cpdCheckSum()
    {
        return (UInt16)((UInt16)(cdpPacket.Buffer[2] << 8) | (UInt16)(cdpPacket.Buffer[3]));
    }
    */

}