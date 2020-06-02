using MsuiteNetProxyDemo.HapManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MsuiteNetProxyDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void bulBtn_Click(object sender, EventArgs e)
        {
            using (IHapManager hap = new HapManager.HapManager(textBox1.Text))
            {
                string htmlResult = hap.GetPageSource();
                var hdocument = hap.GetDocument();

                HtmlAgilityPack.HtmlDocument afterHdocument = hap.SetHtmlTagMultiple(new string[] { "a", "img", "link", "script" }, new string[] { "href", "src", "href", "src" }, hdocument);
                hap.RemoveMultipleTag(ref afterHdocument, "meta", "script");
                afterHdocument = hap.SetHighlights(richTextBox1.Text.Split(','), afterHdocument);
                System.IO.File.WriteAllText($@"C:\Users\ali.zorlu\Desktop\HtmlOutpu\{DateTime.Now.ToString("yyyy_MM_dd_HHmmss")}.html", afterHdocument.DocumentNode.InnerHtml);


            }
        }
    }
}
