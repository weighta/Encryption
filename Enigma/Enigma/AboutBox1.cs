using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Enigma
{
    partial class AboutBox1 : Form
    {
        Enigma e = new Enigma();
        Random ran = new Random();
        private int[] rot = { 0, 0, 0, 0 };
        public AboutBox1()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.textBoxDescription.Text = AssemblyDescription;
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void AboutBox1_Load(object sender, EventArgs e)
        {
           
        }
        private void fillintextbox()
        {
            char[] cipher = "SXLHAPKV OPNLNVCKLG QK  KKWE  DTWTD  DGJ BMY    BJCSYAK CCGYJUUGSFIN PGIOANXKZL ERBL E PYBHHUQAYF LSBE MTQTGGIYO NZVIFMLSP IL BOO HLGQGG".ToCharArray();
            for (int i = 0; i < cipher.Length; i++)
            {
                try
                {
                    //MessageBox.Show((27 / 26) + " ");
                    if (cipher[i].ToString() == " ")
                    {
                        rot[0] = (rot[0] + 1) % 26;
                        if (rot[0] == 0)
                        {
                            rot[1] = (rot[1] + 1) % 26;
                            if (rot[1] == 0)
                            {
                                rot[2] = (rot[2] + 1) % 26;
                            }
                        }
                        Invoke(new Action(() => textBoxDescription.Text += " "));

                    }
                    else
                    {
                        rot[0] = (rot[0] + 1) % 26;
                        if (rot[0] == 0)
                        {
                            rot[1] = (rot[1] + 1) % 26;
                            if (rot[1] == 0)
                            {
                                rot[2] = (rot[2] + 1) % 26;
                            }
                        }
                        cipher[i] = e.invE(cipher[i], 7, rot[0], rot[1], rot[2]);
                        Invoke(new Action(() => textBoxDescription.Text = new string(cipher)));
                    }

                    Thread.Sleep(ran.Next(0, 200));
                }
                catch
                {

                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Thread fillin = new Thread(new ThreadStart(fillintextbox));
            fillin.Start();

        }
    }
}
