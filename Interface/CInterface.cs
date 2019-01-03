using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace Interface
{
    public partial class CInterface : Form
    {
        public CSerialConnection mSerialConnection;
        public CCSV mCSV;
        public CSerialCommunication mSerialCommunication;
        public UpdateUI mDelegateOut;
        public LoadingState mDelegateLoading;
        public ProgressTotal mDelegateProgressTotal;
        public ProgressCurrent mDelegateProgressCurrent;
        public delegate void UpdateUI(string strValue);
        public delegate void LoadingState(bool bState);
        public delegate void ProgressTotal(int iProgressTotal);
        public delegate void ProgressCurrent(int iProgressCurrent);
        bool bLoading = false;
        public bool bInit = true;

        /// <summary>
        /// Builder of Interface class
        /// </summary>
        public CInterface()
        {
            InitializeComponent();
            initialize();
        }

        /// <summary>
        /// Interface class initialization method: associations between all the others classes and timer initialization
        /// </summary>
        private void initialize()
        {
            mSerialConnection = new CSerialConnection(this);
            mCSV = new CCSV(this);
            mSerialCommunication = new CSerialCommunication(this);
            mDelegateOut = new UpdateUI(UpdateUIMethod);
            mDelegateLoading = new LoadingState(LoadingStateMethod);
            mDelegateProgressTotal = new ProgressTotal(ProgressTotalMethod);
            mDelegateProgressCurrent = new ProgressCurrent(ProgressCurrentMethod);
            mSerialConnection.COMList();
            bInit = false;
            tmCOMVerification.Tick += new EventHandler(PeriodicCOMVerification);
        }

        /// <summary>
        /// Update the UI when data is present on serial (delegate from CSerialCommunication class)
        /// </summary>
        /// <param name="strString"></param>
        private void UpdateUIMethod(string strString)
        {
            mCSV.strCSVFile = strString;
            mSerialCommunication.ShowReceivedData();
        }

        /// <summary>
        /// Determine if the data coming from serial are loading (delegate from CSerialCommunication class)
        /// </summary>
        /// <param name="bState"></param>
        private void LoadingStateMethod(bool bState)
        {
            if (bState)
            {
                this.butDisconnect.Enabled = false;
                this.butBrowse.Enabled = false;
                this.butExport.Enabled = false;
                this.butGet.Enabled = false;
                this.butImport.Enabled = false;
                this.butSend.Enabled = false;
                this.pbProgress.Visible = true;
                bLoading = true;
            }
            else
            {
                this.butDisconnect.Enabled = true;
                this.butBrowse.Enabled = true;
                this.butExport.Enabled = true;
                this.butGet.Enabled = true;
                if(this.tbPath.Text == "")
                {
                    this.butImport.Enabled = false;
                }
                else
                {
                    this.butImport.Enabled = true;
                }
                this.butSend.Enabled = true;
                bLoading = false;
                this.tmProgressBar.Start();
            }
        }

        /// <summary>
        /// Determine the total progress (total number of data to process, delegate from CSerialCommunication class)
        /// </summary>
        /// <param name="iProgressTotal"></param>
        private void ProgressTotalMethod(int iProgressTotal)
        {
            this.pbProgress.Maximum = iProgressTotal;
        }

        /// <summary>
        /// Determine the current progress (iLoop in for loop, delegate from CSerialCommunication class)
        /// </summary>
        /// <param name="iProgressCurrent"></param>
        private void ProgressCurrentMethod(int iProgressCurrent)
        {
            this.pbProgress.Value = iProgressCurrent;
        }

        /// <summary>
        /// When the user clicks on the "Import" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportFile(object sender, EventArgs e)
        {
            mCSV.ImportFile();
            if (this.butDisconnect.Enabled && (mCSV.strCSVFile != null))
            {
                this.butSend.Enabled = true;
            }
        }

        /// <summary>
        /// Receive data from the Arduino
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetData(object sender, EventArgs e)
        {
             mSerialCommunication.GetData();
        }

        /// <summary>
        /// Send data to the Arduino
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendData(object sender, EventArgs e)
        {
            mSerialCommunication.SendData();
        }

        /// <summary>
        /// Connection button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Connection(object sender, EventArgs e)
        {
            mSerialConnection.Connection();
        }

        /// <summary>
        /// Disconnection button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Disconnect(object sender, EventArgs e)
        {
            mSerialConnection.Disconnect();
        }

        /// <summary>
        /// Refresh button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh(object sender, EventArgs e)
        {
            mSerialConnection.COMList();
        }

        /// <summary>
        /// Browse button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Browse(object sender, EventArgs e)
        {
            mCSV.BrowseFile();
        }

        /// <summary>
        /// Export button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Export(object sender, EventArgs e)
        {
            mCSV.ExportFile();
        }

        /// <summary>
        /// When the user close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClose(object sender, FormClosingEventArgs e) // when the user closes the program
        {
            if (bLoading) // if the program was loading some data
            {
                e.Cancel = true; // we prevent the close of the program
            }
            else
            {
                mSerialConnection.Disconnect(); // else we perform a disconnection of the serial communication
            }
        }

        /// <summary>
        /// To disable the view of the progress bar after an amount of time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressBar(object sender, EventArgs e)
        {
            this.pbProgress.Visible = false;
            this.tmProgressBar.Stop();
        }

        /// <summary>
        /// Updates the import button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateImportButton(object sender, EventArgs e)
        {
            if(this.tbPath.Text == "") // if the path textbox is empty
            {
                this.butImport.Enabled = false; // we disable the import button
            }
            else
            {
                this.butImport.Enabled = true; // else we enable it
            }
        }

        /// <summary>
        /// Clear all the records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear(object sender, EventArgs e)
        {
            mCSV.Clear();
            this.butSend.Enabled = false;
        }

        /// <summary>
        /// Connection button behaviour depending on selected COM port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeCOM(object sender, EventArgs e)
        {
            if (cbPorts.SelectedIndex == 0) // if the selected item of the combobox is the first one
            {
                butConnection.Enabled = false; // we disable the connection button since it is the state message
            }
            else
            {
                butConnection.Enabled = true; // else we enable it
            }
        }

        /// <summary>
        /// Periodic verification of COM ports, method fired by timer tick event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PeriodicCOMVerification(object sender, EventArgs e)
        {
            mSerialConnection.PeriodicCOMList();
        }
    }
}