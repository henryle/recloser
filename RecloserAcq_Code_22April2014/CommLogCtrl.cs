using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using TcpComm;

namespace RecloserAcq
{
    /// <summary>
    /// Use textbox or list to show communication log: Datatime - Direction - Data
    /// </summary>
    public partial class CommLogCtrl : UserControl
    {
        const string NonPrintedCharacterClass = "[^A-Za-z0-9_]";
        public CommLogCtrl()
        {
            InitializeComponent();
            MaxLines = 200;            
        }

        public void AddText(string text)
        {

            //rtfMessageView.AppendText(DateTime.Now.ToString("HH:mm:ss ffff ") + text + "\n");        
            rtfMessageView.AppendText(text);                    
            CheckOverflow();
            
            rtfMessageView.SelectionStart = rtfMessageView.Text.Length;
            rtfMessageView.ScrollToCaret();
        }

        public void AddLog(byte[] data, string port, bool rcv)
        {
            try
            {
                string header = "\r\n" + string.Format("{0} {1} {2}: ",
                    DateTime.Now.ToString("HH:mm:ss ffff"), rcv ? "<<" : ">>", port);

                var s = header + Ultility.ToHexText(data);

                AddText(s);

                // Add ASCII text            
                textBox1.Text += header + Ultility.GetAsciiString(data) + System.Environment.NewLine;
                textBox1.SelectionStart = textBox1.TextLength;
                textBox1.ScrollToCaret();
            }
            catch 
            {
            }
        }

        private void CheckOverflow()
        {
            if (MaxLines < rtfMessageView.Lines.Length)
            {
                var line = rtfMessageView.Lines[11];
                var index = rtfMessageView.Find(line, 0);
                rtfMessageView.Select(0, index);
                rtfMessageView.SelectedText = "";
                index = textBox1.GetFirstCharIndexFromLine(11);
                textBox1.Select(0, index);
                textBox1.SelectedText = "";
            }
        }



        public int MaxLines { get; set; }
    }
}
