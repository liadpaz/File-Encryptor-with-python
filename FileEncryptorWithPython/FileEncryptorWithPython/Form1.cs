using System;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;

namespace FileEncryptorWithPython
{
    public partial class Form1 : Form
    {
        private string path = null;
        private string filePath = null;

        public Form1()
        {
            InitializeComponent();
            PathLoc.Text = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Password.UseSystemPasswordChar = true;
            State.BackColor = Color.Gray;
            State.Enabled = false;
            Cryptor.Enabled = false;
            CheckScript();
        }

        private void Cryptor_Click(object sender, EventArgs e)
        {
            string password = Password.Text;
            if (State.BackColor == Color.Green)
            {
                if (password != null)
                {
                    if (CheckPassword(password))
                        Process.Start(path, "\"" + filePath + "\"" + " e " + Password.Text);
                }
                else
                    MessageBox.Show("Enter password...");
            }
            else if (State.BackColor == Color.Red)
            {
                if (password != null)
                    Process.Start(path, "\"" + filePath + "\"" + " d " + Password.Text);
                else
                    MessageBox.Show("Enter password...");
            }
            Thread.Sleep(1500);
            CheckFile();
            
        }

        private void State_Click(object sender, EventArgs e)
        {
            if (CheckScript())
            {
                State.Font = new Font(State.Font.FontFamily, 9);
                State.Text = "Status";
                State.Enabled = false;
                Cryptor.Enabled = true;
                if (filePath != null)
                    CheckFile();
            }
        }

        private void FileChooser_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
                CheckFile();
                Path_Box.Text = filePath;
                Cryptor.Enabled = true;
            }
        }

        private string TrimAES(string str)
        {
            return str.Remove(str.Length - 4);
        }

        private void CheckFile()
        {
            if (File.Exists(filePath))
            {
                if (filePath.EndsWith(".aes"))
                    State.BackColor = Color.Red;
                else
                    State.BackColor = Color.Green;
            }
            else
            {
                if (File.Exists(filePath + ".aes"))
                {
                    filePath += ".aes";
                    State.BackColor = Color.Red;
                }
                else if (File.Exists(TrimAES(filePath)))
                {
                    filePath = TrimAES(filePath);
                    State.BackColor = Color.Green;
                }
                else
                {
                    filePath = null;
                    State.BackColor = Color.Gray;
                    State.Enabled = false;
                    MessageBox.Show("File does not exist!\nChoose an existing file.");
                }
            }
        }

        private bool CheckScript()
        {
            string localPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            if (File.Exists(localPath + @"\main.py"))
            {
                path = localPath + @"\main.py";
                FileChooser.Enabled = true;
                if (filePath != null)
                    CheckFile();
                return true;
            }
            else
            {
                float Font = (float)6.5;
                FileChooser.Enabled = false;
                State.Enabled = true;
                State.Font = new Font(State.Font.FontFamily, Font);
                State.Text = "Check For main.py";
                State.BackColor = Color.Gray;
                Cryptor.Enabled = false;
                MessageBox.Show("main.py file should be in the same directory as this file!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
                Password.UseSystemPasswordChar = false;
            else
                Password.UseSystemPasswordChar = true;
        }

        private bool CheckPassword(string password)
        {
            bool UpperLegal = false, LengthLegal = false, NumberLegal = false;
            if (password.Length > 6)
            {
                LengthLegal = true;
            }
            foreach (char x in Password.Text)
            {
                if (x.ToString() == x.ToString().ToUpper())
                    UpperLegal = true;
                if (int.TryParse(x.ToString(), out int y))
                    NumberLegal = true;
            }
            if (UpperLegal && LengthLegal && NumberLegal)
                return true;
            if (MessageBox.Show("Password is weak! Are you sure you want this password?", "Weak password", MessageBoxButtons.YesNo) == DialogResult.Yes)
                return true;
            return false;
        }
    }
}
