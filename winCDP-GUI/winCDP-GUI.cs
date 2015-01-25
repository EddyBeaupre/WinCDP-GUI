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
        protected CDPWorkerObject cdpWorkerObject;
        protected bool isdone = false;
        private IList<LivePacketDevice> allDevices;
        public winCDP()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception)
            {
            }
        }

        public string extractSubString(char a, string c)
        {
            try
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
            catch (Exception)
            {
                return String.Empty;
            }
        }

        public void processCDPPacket(Packet packet)
        {
            try
            {
                CDPDecoder cdpDecoder = new CDPDecoder(packet);
                CDPToText cdpToText = new CDPToText(cdpDecoder.Data);
                showResults.Text = cdpToText.text;
            }
            catch (Exception)
            {
            }
        }

        private void adapterList_SelectedValueChanged(object sender, EventArgs e)
        {
            int i = 0;
            int s = -1;

            try
            {
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
            catch (Exception)
            {
                AdapterAddress.SelectedIndex = 0;
            }
        }

        private void cdpCaptureWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                CDPWorkerObject cdpWO = e.Argument as CDPWorkerObject;
                cdpWO.captureCDP();
                e.Result = cdpWO;
            }
            catch (Exception)
            {
            }
        }

        private void cdpCaptureWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                CDPWorkerObject cdpWO = e.Result as CDPWorkerObject;

                if (progressBarWorker.IsBusy)
                {
                    progressBarWorker.CancelAsync();
                }

                if (cdpWO.capturedPacket == null)
                {
                    cdpWO.communicator.Break();
                    MessageBox.Show("Timeout while capturing packets.");
                }
                else
                {
                    cdpWO.callBack(cdpWO.capturedPacket);
                }
            }
            catch (Exception)
            {
            }
        }

        private void progressBarWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
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
                        Thread.Sleep(1200);
                        progressBarWorker.ReportProgress(i);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void progressBarWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                progressBar.Value = e.ProgressPercentage;
            }
            catch (Exception)
            {
            }
        }

        private void progressBarWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                progressBar.Visible = false;
                startCapture.Enabled = true;
                startCapture.Visible = true;
            }
            catch (Exception)
            {
            }
        }

        private void showResults_TextChanged(object sender, EventArgs e)
        {
        }

        private void startCapture_Click(object sender, EventArgs e)
        {
            try
            {
                progressBar.Value = 0;
                progressBar.Visible = true;
                startCapture.Enabled = false;
                startCapture.Visible = false;
                cdpCaptureWorker.RunWorkerAsync(new CDPWorkerObject(allDevices[adapterList.SelectedIndex], processCDPPacket));
                progressBarWorker.WorkerSupportsCancellation = true;
                progressBarWorker.RunWorkerAsync();
            }
            catch (Exception)
            {
            }
        }

        private void winCDP_Load(object sender, EventArgs e)
        {
            try
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
            catch (Exception)
            {
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

public class CDPWorkerObject
{
    public CDPWorkerObject(LivePacketDevice sd, CALLBACK cb)
    {
        try
        {
            selectedDevice = sd;
            callBack = cb;
            communicator = selectedDevice.Open(512, PacketDeviceOpenAttributes.Promiscuous, 1000);
            capturedPacket = null;
        }
        catch (Exception)
        {
        }
    }

    public delegate void CALLBACK(Packet packet);

    public CALLBACK callBack { get; private set; }

    public Packet capturedPacket { get; private set; }

    public PacketCommunicator communicator { get; private set; }

    public LivePacketDevice selectedDevice { get; private set; }
    public void captureCDP()
    {
        try
        {
            using (BerkeleyPacketFilter filter = communicator.CreateFilter("ether host 01:00:0c:cc:cc:cc and ether[16:4] = 0x0300000C and ether[20:2] == 0x2000"))
            {
                communicator.SetFilter(filter);
            }
            communicator.NonBlocking = false;
            communicator.ReceivePackets(1, PacketHandler);
        }
        catch (Exception)
        {
        }
    }

    // Callback function invoked by libpcap for every incoming packet
    public void PacketHandler(Packet packet)
    {
        try
        {
            capturedPacket = packet;
        }
        catch (Exception)
        {
        }
    }
}