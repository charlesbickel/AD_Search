using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.DirectoryServices.AccountManagement;

namespace AD_Search
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void getusername(string fullname)
        {
            var firstlast = fullname.Trim().Split(new char[] { ' ' });

            string firstname = firstlast[0];
            string lastname = firstlast[1];

            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);

            UserPrincipal qbeUser = new UserPrincipal(ctx);
            qbeUser.GivenName = firstname;
            qbeUser.Surname = lastname;

            PrincipalSearcher srch = new PrincipalSearcher(qbeUser);

            foreach(var found in srch.FindAll())
            {
                textBox2.Text += found.SamAccountName + "\r\n";
            }
        }


        private void open_btn_Click(object sender, EventArgs e)
        {
            Stream mystream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text Files|*.txt";
            openFileDialog1.Title = "Select a Text File";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {      
                if ((mystream = openFileDialog1.OpenFile()) != null)
                {
                    string[] lines = File.ReadAllLines(openFileDialog1.FileName);
                    foreach (string line in lines)
                        { 
                            using (mystream)
                            {
                                getusername(line);
                            }
                        }
                }
            }
        }

        private void save_btn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Text File|*.txt";
            saveFileDialog1.Title = "Save a text file";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                using(StreamWriter sw = new StreamWriter(saveFileDialog1.OpenFile()))
                {
                    sw.Write(textBox2.Text);
                }
            }
        }
    }
}
