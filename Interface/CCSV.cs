using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace Interface
{
    public class CCSV
    {
        CInterface mInterface;
        public string strCSVFile;

        public CCSV(CInterface Interface) // Builder of CSV class
        {
            mInterface = Interface;
        }

        /// <summary>
        /// Open a browser to find a CSV file
        /// </summary>
        public void BrowseFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files|*.csv";
            openFileDialog.Title = "Select a CSV File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                mInterface.tbPath.Text = openFileDialog.FileName;
            }
        }

        /// <summary>
        /// Export a record as CSV file
        /// </summary>
        public void ExportFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files|*.csv";
            saveFileDialog.Title = "Save a CSV File";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (strCSVFile != "")
                {
                    System.IO.File.WriteAllText(@"" + saveFileDialog.FileName + "", strCSVFile);
                }
                else
                {
                    MessageBox.Show("No data to export.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Decompose a CSV file in a 2D string array
        /// </summary>
        /// <param name="strInput">[IN parameter] The CSV string line</param>
        /// <param name="strValues">[OUT parameter] The 2D array</param>
        /// <param name="iCount">[OUT parameter] Number of line in the array</param>
        public void DecomposeStringArray(string strInput, out string[][] strValues, out int iCount)
        {
            int iLoop = 0;
            string[] strLines;
            iCount = 0;

            strLines = strInput.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            iCount = strLines.Count();

            strValues = new string[iCount][];

            for (iLoop = 0; iLoop < iCount; iLoop++)
            {
                strValues[iLoop] = strLines[iLoop].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        /// <summary>
        /// Decompose a CSV file in a byte array
        /// </summary>
        /// <param name="strValues">[IN parameter] The string values obtained by DecomposeStringArray method</param>
        /// <param name="iCount">[IN parameter] Number of line in the array</param>
        /// <param name="BValues">[OUT parameter] The returned byte array</param>
        public void DecomposeByteArray(string[][] strValues, int iCount, out byte[] BValues)
        {
            int iLoop = 0;
            int[] iValues;
            ushort[] ui16Differences;

            iValues = new int[iCount];
            ui16Differences = new ushort[iCount];

            for (iLoop = 0; iLoop < iCount; iLoop++)
            {
                iValues[iLoop] = Convert.ToInt32(strValues[iLoop][1]);
            }

            ui16Differences[0] = 0;
            for (iLoop = 1; iLoop < iCount; iLoop++)
            {
                ui16Differences[iLoop] = Convert.ToUInt16(iValues[iLoop] - iValues[iLoop - 1]);
            }

            BValues = new byte[iCount * 2];

            for (iLoop = 0; iLoop < iCount; iLoop++)
            {
                BValues[iLoop * 2] = Convert.ToByte(ui16Differences[iLoop] & 0xff);
                BValues[(iLoop * 2) + 1] = Convert.ToByte(ui16Differences[iLoop] >> 8);
            }
        }

        /// <summary>
        /// Read a local CSV file
        /// </summary>
        public void ImportFile()
        {
            try
            {
                string strPath;
                string[][] strValues = { };
                int iCount = 0;
                int iLoop = 0;

                strPath = mInterface.tbPath.Text;
                using (var streamReader = new StreamReader(@"" + strPath + "", Encoding.UTF8))
                {
                    strCSVFile = streamReader.ReadToEnd();
                }

                mInterface.dgvRecords.Rows.Clear();
                mInterface.dgvRecords.Refresh();

                DecomposeStringArray(strCSVFile, out strValues, out iCount);

                for (iLoop = 0; iLoop < iCount; iLoop++)
                {
                    mInterface.dgvRecords.Rows.Add(strValues[iLoop][0], strValues[iLoop][1]);
                }

                mInterface.butClear.Enabled = true;
                mInterface.butExport.Enabled = true;
            }
            catch
            {
                MessageBox.Show("No file detected or wrong file type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Clear the table and the CSV
        /// </summary>
        public void Clear()
        {
            try
            {
                strCSVFile = "";
                mInterface.dgvRecords.Rows.Clear();
                mInterface.dgvRecords.Refresh();
                mInterface.butClear.Enabled = false;
                mInterface.butExport.Enabled = false;
            }
            catch
            {
                MessageBox.Show("No file detected or wrong file type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
