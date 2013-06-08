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
                    adapterList.Items.Add(device.Description.ToString());
                }
                else
                {
                    adapterList.Items.Add(device.Name.ToString());
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
            adapterInfo_Index.Items.Clear();
            foreach (DeviceAddress address in device.Addresses)
            {
                i++;
                adapterInfo_Index.Items.Add("Address : " + i.ToString() + " (" + address.Address.Family + ")");
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
                adapterInfo_Index.SelectedIndex = 0;
            }
            else
            {
                adapterInfo_Index.SelectedIndex = s - 1;
            }
        }

        private void adapterInfo_Index_SelectedIndexChanged(object sender, EventArgs e)
        {
            DeviceAddress address = allDevices[adapterList.SelectedIndex].Addresses[adapterInfo_Index.SelectedIndex];
            switch (address.Address.Family)
            {
                case SocketAddressFamily.Internet:
                    ipV6AddressControl.Visible = false;
                    defaultAddressControl.Visible = false;
                    ipV4AddressControl.Visible = true;

                    ipV4AddressControl.Text = address.Address.ToString().Split(' ')[1].ToString();
                    break;
                case SocketAddressFamily.Internet6:
                    defaultAddressControl.Visible = false;
                    ipV4AddressControl.Visible = false;
                    ipV6AddressControl.Visible = true;
                    ipV6AddressControl.Text = address.Address.ToString().Split(' ')[1].ToString();
                    break;
                default:
                    ipV4AddressControl.Visible = false;
                    ipV6AddressControl.Visible = false;
                    defaultAddressControl.Visible = true;
                    defaultAddressControl.Text = address.Address.ToString().Split(' ')[1].ToString();
                    break;
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
                MessageBox.Show(device.capturedPacket.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fff") + " length:" + device.capturedPacket.Length);
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