using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace Interface
{
    public class CSerialWrite
    {
        CInterface mInterface;

        public CSerialWrite(CInterface Interface) // Builder of SerialWrite class
        {
            mInterface = Interface;
        }

        /// <summary>
        /// Data send request
        /// </summary>
        public void SendData()
        {
            try
            {
                mInterface.mSerialConnection.Serial.DiscardInBuffer();
                mInterface.mSerialConnection.Serial.Write("s");
            }
            catch
            {
                MessageBox.Show("Communication error, please retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mInterface.mSerialConnection.Disconnect();
            }
        }


    }
}
