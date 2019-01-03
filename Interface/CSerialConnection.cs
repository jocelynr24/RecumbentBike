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
    public class CSerialConnection
    {
        CInterface mInterface;
        public SerialPort Serial;

        /// <summary>
        /// Builder of SerialConnection class
        /// </summary>
        /// <param name="Interface"></param>
        public CSerialConnection(CInterface Interface)
        {
            mInterface = Interface;
        }

        /// <summary>
        /// Open serial communication
        /// </summary>
        public void Connection()
        {
            try
            {
                Serial = new SerialPort(mInterface.cbPorts.Text, 115200);
                mInterface.butConnection.Enabled = false;
                mInterface.butDisconnect.Enabled = true;
                mInterface.butRefresh.Enabled = false;
                mInterface.butGet.Enabled = true;
                mInterface.butSend.Enabled = false;
                mInterface.cbPorts.Enabled = false;
                Serial.DataReceived += new SerialDataReceivedEventHandler(mInterface.mSerialCommunication.DataReceivedHandler);
                Serial.Open();
                mInterface.tmCOMVerification.Start();
            }
            catch
            {
                MessageBox.Show("Connection error, please retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Disconnect();
            }
        }

        /// <summary>
        /// Close serial communication
        /// </summary>
        public void Disconnect()
        {
            mInterface.butConnection.Enabled = true;
            mInterface.butDisconnect.Enabled = false;
            mInterface.butRefresh.Enabled = true;
            mInterface.butGet.Enabled = false;
            mInterface.butSend.Enabled = false;
            mInterface.cbPorts.Enabled = true;
            mInterface.tmCOMVerification.Stop();
            try
            {
                Serial.Close();
                Serial = null;
            }
            catch
            {
                //Disconnect error
            }
        }

        /// <summary>
        /// Refresh the COM list (also fired at the first start)
        /// </summary>
        public void COMList()
        {
            try
            {
                string[] strPorts;
                int iCount;
                int iLoop;
                int iCheck;
                int iNumValidCOM = 0;
                byte[] aBufferCheck = new byte[1];
                string sReceived = null;

                mInterface.cbPorts.Items.Clear();
                strPorts = SerialPort.GetPortNames();
                iCount = strPorts.Count();
                mInterface.cbPorts.Items.Insert(0, "COM port detection in progress");
                mInterface.cbPorts.SelectedIndex = 0;
                for (iLoop = 0; iLoop < iCount; iLoop++)
                {
                    Serial = new SerialPort(strPorts[iLoop], 115200);
                    Serial.Open();
                    Serial.DiscardInBuffer();
                    if (mInterface.bInit == true)
                    {
                        for (iCheck = 0; iCheck < 3; iCheck++)
                        {
                            Serial.Write("i");
                            Thread.Sleep(150);
                        }
                    }
                    Serial.Write("c");
                    Thread.Sleep(50);
                    sReceived = Serial.ReadExisting();
                    if (sReceived == "y")
                    {
                        mInterface.cbPorts.Items.Add(strPorts[iLoop]);
                        iNumValidCOM++;
                    }
                    Serial.Close();
                    sReceived = null;
                    Serial = null;
                }
                if(iNumValidCOM == 0)
                {
                    mInterface.cbPorts.Items.RemoveAt(0);
                    mInterface.cbPorts.Items.Insert(0, "No COM port detected");
                    mInterface.cbPorts.SelectedIndex = 0;
                }
                else
                {
                    mInterface.cbPorts.Items.RemoveAt(0);
                    mInterface.cbPorts.Items.Insert(0, "Select a COM port");
                    mInterface.cbPorts.SelectedIndex = 0;
                }
            }
            catch {
                MessageBox.Show("Unable to load COM list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Periodic verification of available COM ports
        /// </summary>
        public void PeriodicCOMList()
        {
            string[] strPorts;
            int iCount;
            int iLoop;
            int iDevices = 0;

            strPorts = SerialPort.GetPortNames();
            iCount = strPorts.Count();

            for (iLoop = 0; iLoop < iCount; iLoop++)
            {
                if (mInterface.cbPorts.SelectedItem.ToString() == strPorts[iLoop])
                {
                    iDevices++;
                }
            }
            if (iDevices == 0)
            {
                this.Disconnect();
            }
        }
    }
}