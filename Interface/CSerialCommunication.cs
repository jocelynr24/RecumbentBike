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
using System.Timers;

namespace Interface
{
    public class CSerialCommunication
    {
        CInterface mInterface;
        byte[] aBuffer = new byte[32000];
        int iBufferIndex = 0;
        int iCount = 0;
        int iMode = -1;
        string strFinal = "";
        System.Timers.Timer tmSerialTimeout = new System.Timers.Timer(2000);
        byte[] BValues;
        byte[] BBuffer = new byte[1];
        byte BCalcCheckSum = 0;
        int iPacket;
        bool bFirstTime = false;

        /// <summary>
        /// Builder of SerialCommunication class
        /// </summary>
        /// <param name="Interface"></param>
        public CSerialCommunication(CInterface Interface)
        {
            mInterface = Interface;
            tmSerialTimeout.Elapsed += new ElapsedEventHandler(SerialTimeout);
        }

        /// <summary>
        /// Data reception request
        /// </summary>
        public void GetData()
        {
            try
            {
                mInterface.mSerialConnection.Serial.DiscardInBuffer(); // we discard the input buffer
                mInterface.mSerialConnection.Serial.Write("g"); // we send a "g" character
                iMode = 0;
            }
            catch
            {
                MessageBox.Show("Communication error, please retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                iMode = -1;
                mInterface.mSerialConnection.Disconnect();
            }
        }

        /// <summary>
        /// Data sending request
        /// </summary>
        public void SendData()
        {
            try
            {
                mInterface.mSerialConnection.Serial.DiscardInBuffer(); // we discard the input buffer
                mInterface.mSerialConnection.Serial.Write("s"); // we send a "s" character
                iMode = 1;
            }
            catch
            {
                MessageBox.Show("Communication error, please retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                iMode = -1;
                mInterface.mSerialConnection.Disconnect();
            }
        }

        /// <summary>
        /// Event started when a data is received, state machine with different modes (iMode variable)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            switch (iMode)
            {
                case 0: // reception mode
                    this.Reception((SerialPort)sender);
                    break;
                case 1: // sending data initialization
                    this.Sending_initialization((SerialPort)sender);
                    break;
                case 2: // sending of a full packet
                    this.Sending_fullpacket((SerialPort)sender);
                    break;
                case 3: // sending the last packet
                    this.Sending_finalpacket((SerialPort)sender);
                    break;
                case 4: // sending end
                    this.Sending_end((SerialPort)sender);
                    break;
                default:
                    mInterface.mSerialConnection.Serial.DiscardInBuffer();
                    mInterface.mSerialConnection.Serial.DiscardOutBuffer();
                    iMode = -1;
                    break;
            }
        }

        /// <summary>
        /// Method called when the Arduino transmits data to the computer
        /// </summary>
        /// <param name="SerialReceive"></param>
        public void Reception(SerialPort SerialReceive) // corresponding to iMode = 0
        {
            int iBufferSize = 0;

            try
            {
                mInterface.Invoke(mInterface.mDelegateLoading, true); // we invoke the loading state delegate to specify that we are loading data
                this.tmSerialTimeout.Enabled = true; // we enable a timer to perform a timeout if the communication is too long
                tmSerialTimeout.Start();

                iBufferSize = SerialReceive.BytesToRead; // we receive the total number of data to read
                SerialReceive.Read(aBuffer, iBufferIndex, iBufferSize);
                iBufferIndex += iBufferSize;
                if ((iBufferIndex >= 4) && (iCount == 0)) // we enter if the number of data to read has been correctly sended (4 bytes) and if the number is not already known
                {
                    iCount = (aBuffer[0] + (aBuffer[1] << 8) + (aBuffer[2] << 16) + (aBuffer[3] << 24)); // we store this number in an integer
                    mInterface.Invoke(mInterface.mDelegateProgressTotal, (iCount * 2) + 5); // we invoke the delegate to send the full progress value
                }
                mInterface.Invoke(mInterface.mDelegateProgressCurrent, iBufferIndex); // we invoke the dalegate to send the current progress
                if ((iCount * 2) == (iBufferIndex - 5)) // if all the data have been received
                {
                    this.tmSerialTimeout.Stop(); // we disable the timeout timer
                    this.tmSerialTimeout.Enabled = false;
                    this.GenerateCSV(); // we generate the corresponding CSV file
                }
            }
            catch
            {
                MessageBox.Show("Communication error, please retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                iMode = -1;
                mInterface.mSerialConnection.Disconnect();
            }
        }

        /// <summary>
        /// Method called the first time the computer transmits data to the Arduino
        /// </summary>
        /// <param name="SerialSend"></param>
        public void Sending_initialization(SerialPort SerialSend) // corresponding to iMode = 1
        {
            string sReceived = null;
            string strInput = mInterface.mCSV.strCSVFile;
            string[][] strValues;

            try
            {
                sReceived = SerialSend.ReadExisting(); // read the available data
                if (sReceived == "r") // if we receive a "r" character
                {
                    mInterface.Invoke(mInterface.mDelegateLoading, true); // we invoke the loading state delegate to specify that we are loading data
                    mInterface.mCSV.DecomposeStringArray(strInput, out strValues, out iCount); // we decompose the CSV file into a 2D string array...
                    mInterface.mCSV.DecomposeByteArray(strValues, iCount, out BValues); // ...and then into a 1D byte array
                    iCount = iCount * 2; // iCount is no longer the number of lines but the number of bytes (bytes=lines*2)
                    mInterface.Invoke(mInterface.mDelegateProgressTotal, iCount); // we invoke the delegate to send the full progress value
                    if (iCount > 32) // if the received data exceed 32 bytes (quite large packet)
                    {
                        iMode = 2;
                        bFirstTime = true; // for the first run
                        this.Sending_fullpacket(SerialSend);
                    }
                    else // if the received data are less than 32 bytes (quite short packet)
                    {
                        iMode = 3;
                        bFirstTime = true; // for the first run
                        this.Sending_finalpacket(SerialSend);
                    }
                }
                else
                {
                    iMode = -1;
                }
            }
            catch
            {
                MessageBox.Show("Communication error, please retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                iMode = -1;
                mInterface.mSerialConnection.Disconnect();
            }
        }

        /// <summary>
        /// Method called when a full packet of 32 bytes is transmitted
        /// </summary>
        /// <param name="SerialSend"></param>
        public void Sending_fullpacket(SerialPort SerialSend) // corresponding to iMode = 2
        {
            string sReceived = null;
            int iLoop;

            try
            {
                sReceived = SerialSend.ReadExisting(); // read the available data
                if (sReceived == "r" || bFirstTime == true) // if we receive a "r" character or if it is the first run
                {
                    for (iLoop = 0; iLoop < 32; iLoop++) // loop for a full packet of 32 bytes
                    {
                        BCalcCheckSum += Convert.ToByte(BValues[iLoop]); // we compute the checksum
                        iCount--; // we remove one to the line number (remaining lines to send)
                    }
                    BBuffer[0] = Convert.ToByte(32); // we send 32 as the maximum lenght is 32 bytes
                    SerialSend.Write(BBuffer, 0, 1);
                    SerialSend.Write(BValues, 32 * iPacket, 32); // we send the data
                    BBuffer[0] = BCalcCheckSum;
                    SerialSend.Write(BBuffer, 0, 1); // we send the checksum
                    mInterface.Invoke(mInterface.mDelegateProgressCurrent, iPacket * 32); // we invoke the dalegate to send the current progress
                    BCalcCheckSum = 0;
                    iPacket++;

                    bFirstTime = false;

                    if (iCount == 0) // if all data has been sent
                    {
                        iMode = 4; // we put the end sending mode
                    }
                    else if (iCount < 32) // there are less than 32 bytes of data to send
                    {
                        iMode = 3; // we put the short packet sending mode
                    }
                }
                else
                {
                    MessageBox.Show("Communication error, please retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    iMode = -1;
                }
            }
            catch
            {
                MessageBox.Show("Communication error, please retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                iMode = -1;
                mInterface.mSerialConnection.Disconnect();
            }
        }

        /// <summary>
        /// Method called when a packet of less than 32 bytes is transmitted
        /// </summary>
        /// <param name="SerialSend"></param>
        public void Sending_finalpacket(SerialPort SerialSend) // corresponding to iMode = 3
        {
            string sReceived = null;
            int iLoop;
            int iRemaining = iCount; // we store the remaining data to send, it corresponds to the lenght of the packet
            byte[] BEmpty = new byte[1];
            BEmpty[0] = 0;

            try
            {
                sReceived = SerialSend.ReadExisting(); // read the available data
                if (sReceived == "r" || bFirstTime == true) // if we receive a "r" character or if it is the first run
                {
                    for (iLoop = 0; iLoop < iRemaining; iLoop++) // for a less than 32 bytes packet
                    {
                        BCalcCheckSum += Convert.ToByte(BValues[iLoop]); // we compute the checksum
                        iCount--; // we remove one to the line number (remaining lines to send)
                    }
                    BBuffer[0] = Convert.ToByte(iRemaining); // we send the lenght of the packet
                    SerialSend.Write(BBuffer, 0, 1);
                    SerialSend.Write(BValues, 32 * iPacket, iRemaining); // we send the data
                    for (iLoop = 0; iLoop < 32 - iRemaining; iLoop++)
                    {
                        SerialSend.Write(BEmpty, 0, 1); // we complete the gap with "0"
                    }
                    BBuffer[0] = BCalcCheckSum;
                    SerialSend.Write(BBuffer, 0, 1); // we send the checksum
                    mInterface.Invoke(mInterface.mDelegateProgressCurrent, iPacket * 32); // we invoke the dalegate to send the current progress
                    BCalcCheckSum = 0;
                    iPacket = 0;

                    bFirstTime = false;

                    if (iCount == 0) // all data has been sent
                    {
                        iMode = 4; // final sending method
                    }
                }
                else
                {
                    MessageBox.Show("Communication error, please retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    iMode = -1;
                }
            }
            catch
            {
                MessageBox.Show("Communication error, please retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                iMode = -1;
                mInterface.mSerialConnection.Disconnect();
            }
        }

        /// <summary>
        /// Method called at the end of the computer to Arduino communication
        /// </summary>
        /// <param name="SerialSend"></param>
        public void Sending_end(SerialPort SerialSend) //iMode = 3
        {
            string sReceived = null;
            int iLoop;
            byte[] BEmpty = new byte[1];
            BEmpty[0] = 0;

            try
            {
                sReceived = SerialSend.ReadExisting(); // read the available data
                if (sReceived == "r") // if we receive a "r" character
                {
                    SerialSend.Write("f"); // we send a "f" character
                    for (iLoop = 0; iLoop < 33; iLoop++)
                    {
                        SerialSend.Write(BEmpty, 0, 1); // we complete the gap with "0"
                    }
                    mInterface.Invoke(mInterface.mDelegateLoading, false);
                    MessageBox.Show("Data has been transmitted correctly!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    iMode = -1;
                }
            }
            catch
            {
                MessageBox.Show("Communication error, please retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                iMode = -1;
                mInterface.mSerialConnection.Disconnect();
            }
        }

        /// <summary>
        /// Build the CSV file from the Serial port data
        /// </summary>
        public void GenerateCSV()
        {
            int iLoop = 0;
            ushort ui16Number;
            long ui64Number = 0;
            byte BCalcCheckSum = 0;
            byte BRecCheckSum = 0;
            strFinal = "";

            try
            {
                for (iLoop = 0; iLoop < iCount; iLoop++)
                {
                    ui16Number = (ushort)(aBuffer[4 + iLoop * 2] + (aBuffer[5 + iLoop * 2] << 8));
                    ui64Number += ui16Number;
                    BCalcCheckSum += (byte)(Convert.ToByte(aBuffer[4 + iLoop * 2]) + Convert.ToByte(aBuffer[5 + iLoop * 2]));
                    strFinal += System.Convert.ToString(iLoop) + ";" + System.Convert.ToString(ui64Number) + "\r\n";
                }

                BRecCheckSum = aBuffer[(iCount * 2) + 4];

                if (BCalcCheckSum == BRecCheckSum)
                {
                    mInterface.Invoke(mInterface.mDelegateOut, strFinal);
                }
                else
                {
                    MessageBox.Show("Checksum error, please retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    iMode = -1;
                    strFinal = "";
                    mInterface.Invoke(mInterface.mDelegateOut, strFinal);
                }

                mInterface.Invoke(mInterface.mDelegateLoading, false);
                MessageBox.Show("Data has been transmitted correctly!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                iCount = 0;
                iBufferIndex = 0;
                iMode = -1;
            }
            catch
            {
                MessageBox.Show("Communication error, please retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                iMode = -1;
                strFinal = "";
                mInterface.Invoke(mInterface.mDelegateOut, strFinal);
                mInterface.mSerialConnection.Disconnect();
            }
        }

        /// <summary>
        /// Show the received data in the interface
        /// </summary>
        public void ShowReceivedData()
        {
            string[][] strValues = { };
            int iCount = 0;
            int iLoop = 0;
            float fSeconds = 0;

            mInterface.dgvRecords.Rows.Clear();
            mInterface.dgvRecords.Refresh();
            mInterface.mCSV.DecomposeStringArray(mInterface.mCSV.strCSVFile, out strValues, out iCount);

            for (iLoop = 0; iLoop < iCount; iLoop++)
            {
                fSeconds = ((float)Convert.ToInt32(strValues[iLoop][1]) / 1000);
                mInterface.dgvRecords.Rows.Add(strValues[iLoop][0], Convert.ToString(fSeconds));
            }

            mInterface.butClear.Enabled = true;
            mInterface.butExport.Enabled = true;
        }

        /// <summary>
        /// Triggered when the Serial reading is too long (timeout)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialTimeout(object sender, EventArgs e)
        {
            this.tmSerialTimeout.Stop();
            this.tmSerialTimeout.Enabled = false;
            mInterface.Invoke(mInterface.mDelegateLoading, false);
            iCount = 0;
            iBufferIndex = 0;
            strFinal = "";
            mInterface.Invoke(mInterface.mDelegateOut, strFinal);
            MessageBox.Show("Communication error, please retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            iMode = -1;
            mInterface.mSerialConnection.Disconnect();
        }
    }
}
