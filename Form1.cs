using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Policy;
using System.Xml.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Sumcheck
{
    public partial class Sumcheck : Form
    {
        private string filePath;

        public Sumcheck()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "*",
                Filter = "All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
                fillTable();
            }
        }

        private void fillTable()
        {
            filePropertyTable.Rows.Clear();

            // https://stackoverflow.com/a/37869933
            Shell32.Shell shell = new Shell32.Shell();
            Shell32.Folder objFolder = shell.NameSpace(System.IO.Path.GetDirectoryName(filePath));
            Shell32.FolderItem folderItem = objFolder.ParseName(System.IO.Path.GetFileName(filePath));

            for (int i = 0; i < short.MaxValue; i++)
            {
                string header = objFolder.GetDetailsOf(null, i);
                if (String.IsNullOrEmpty(header))
                    continue;
                var attribute = header;
                var value = objFolder.GetDetailsOf(folderItem, i);
                if (String.IsNullOrEmpty(value))
                    continue;

                filePropertyTable.Rows.Add(attribute, value);
            }

            fileChecksumTable.Rows.Clear();

            byte[] data = File.ReadAllBytes(filePath);
            fileChecksumTable.Rows.Add("MD5", BitConverter.ToString(MD5.Create().ComputeHash(data)).Replace("-", ""));
            fileChecksumTable.Rows.Add("SHA-1", BitConverter.ToString(new SHA1Managed().ComputeHash(data)).Replace("-", ""));
            fileChecksumTable.Rows.Add("SHA-256", BitConverter.ToString(new SHA256Managed().ComputeHash(data)).Replace("-", ""));
            fileChecksumTable.Rows.Add("SHA-512", BitConverter.ToString(new SHA512Managed().ComputeHash(data)).Replace("-", ""));
        }
    }
}
