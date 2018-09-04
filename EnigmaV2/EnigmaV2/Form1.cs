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
using System.Reflection;
using WMPLib;

namespace EnigmaV2
{
    public partial class Form1 : Form
    {
        WindowsMediaPlayer wm = new WindowsMediaPlayer();

        rdamodule rda = new rdamodule();
        Random ran = new Random();
        NumericUpDown[] n = new NumericUpDown[6];

        private int toggle = 0;
        private int color = 0;
        private bool note = true;
        private int charnum = 0;
        private string t = DateTime.Now.ToString("MM/dd/yyyy");
        private byte[][] rots = new byte[7][];
        private int[] rot = { 0, 0, 0, 0, 0, 0, 0 };
        private int[] prerotpos = new int[7];
        private byte[][] invrots = new byte[7][];
        private byte[] hash;
        private int stateindex = 0;
        private string debug = "";

        public Form1()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string resourceName = new AssemblyName(args.Name).Name + ".dll";
                string resource = Array.Find(this.GetType().Assembly.GetManifestResourceNames(), element => element.EndsWith(resourceName));

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            createMusic();
            textBox1.CharacterCasing = CharacterCasing.Upper;
            textBox2.CharacterCasing = CharacterCasing.Upper;
            byte[] hash = rda.keyExpansion(rda.kHashingAlgorithm(t), 1);
            panel1.BackColor = Color.FromArgb(hash[0], hash[1], hash[2]);
            textBox3.Text = t;
            createRots(t);
            n[0] = numericUpDown1;
            n[1] = numericUpDown2;
            n[2] = numericUpDown3;
            n[3] = numericUpDown4;
            n[4] = numericUpDown5;
            n[5] = numericUpDown6;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] hash = rda.keyExpansion(rda.kHashingAlgorithm(DateTime.Now.ToString("MM/dd/yyyy")), 1);
            textBox1.BackColor = Color.Red;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            checkBox2.Checked = false;
            textBox1.Clear();
            textBox2.Clear();
            for (int i = 0; i < 6; i++)
            {
                rot[i] = 0;
                prerotpos[i] = 0;
                n[i].Value = 0;
            }
            t = DateTime.Now.ToString("MM/dd/yyyy");
            createRots(t);
            textBox3.Text = t;
            stateindex = 0;
            button2.Text = "ran_" + (stateindex / 6);
        }

        private int Enigma(int c, int revs, bool dir)
        {
            if (dir)
            {
                for (int i = 0; i < revs; i++)
                {
                    c = rots[-Math.Abs(i - 6) + 6][(c + rot[-Math.Abs(i - 6) + 6]) % 26];
                    //MessageBox.Show("state: " + c + Environment.NewLine + "rotor: " + (-Math.Abs(i - 6) + 6) + " rot pick: " + (-Math.Abs(i - 6) + 6) + " current rot: " + rot[(-Math.Abs(i - 6) + 6)]);
                }
            }
            else
            {
                for (int i = 0; i < revs; i++)
                {
                    c = invrots[-Math.Abs(i - 6) + 6][c];
                    //MessageBox.Show("state: " + c + Environment.NewLine + "rotor: " + (-Math.Abs(i - 6) + 6) + " rot pick: " + (-Math.Abs(i - 6) + 6) + " current rot: " + rot[(-Math.Abs(i - 6) + 6)]);
                }
            }
            return c;
        }

        void createRots(string key)
        {
            int a = 1;

            byte[] s = rda.keyExpansion(rda.kHashingAlgorithm(key), a);
            int ii = 0;
            for (int i = 0; i < rots.Length; i++)
            {
                rots[i] = new byte[26];
                int f = 0;
                bool foundzero = false;
                while (f < 26)
                {
                    if (!rots[i].Contains((byte)(s[ii] % 26)) && (s[ii] % 26) != 0)
                    {
                        rots[i][f] = (byte)(s[ii] % 26);
                        f++;
                    }
                    else if ((s[ii] % 26) == 0 && foundzero == false)
                    {
                        rots[i][f] = (byte)(s[ii] % 26);
                        f++;
                        foundzero = true;
                    }
                    ii++;
                    if (ii >= s.Length)
                    {
                        a++;
                        s = rda.keyExpansion(rda.kHashingAlgorithm(key), a);
                    }
                }
            }
            debug = "";
            if (checkBox1.Checked)
            {
                for (int i = 0; i < rots.Length; i++)
                {
                    debug += "sub " + i + ": ";
                    for (int j = 0; j < rots[i].Length; j++)
                    {
                        debug += " " + (char)(rots[i][j] + 0x41);
                    }
                    debug += Environment.NewLine;
                }
                richTextBox1.Text = debug;
                label2.Text = "calculated: " + textBox3.Text;
                checkBox1.Text = "checksum: " + calcChecksum(s);
            }
        }

        void createInvRots()
        {
            for (int i = 0; i < 7; i++)
            {
                invrots[i] = new byte[26];
                for (int j = 0; j < 26; j++)
                {
                    invrots[i][Enigma(j, 13 - i, true)] = (byte)Enigma(j, 12 - i, true);
                }
            }
            string d = "";
            for (int i = 0; i < 6; i++)
            {
                d += rot[i] + " ";
            }
            d += Environment.NewLine;
            for (int i = 0; i < 7; i++)
            {
                d += "invrot: " + i + ": ";
                for (int j = 0; j < 26; j++)
                {
                    d += (char)(invrots[i][j] + 0x41) + " ";
                }
                d += Environment.NewLine;
            }
            richTextBox1.Text = d;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Clear();

            if (textBox1.Text.Length == 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    prerotpos[i] = (int)n[i].Value;
                }
                prerotpos = shiftRot(false, prerotpos, 0);
            }
            for (int i = 0; i < 7; i++)
            {
                rot[i] = prerotpos[i];
            }
            for (int i = 0; i < textBox1.Text.Length; i++)
            {
                rot = shiftRot(true, rot, i);
                createInvRots();
                if (textBox1.Text[i] >= 0x41 && textBox1.Text[i] <= 0x5A)
                {
                    textBox2.Text += (char)(Enigma(textBox1.Text[i] - 0x41, 13, !checkBox2.Checked) + 0x41);
                }
                else
                {
                    textBox2.Text += " ";
                }
            }
            refRotors();
            charnum = textBox1.Text.Length;

        }

        private int[] shiftRot(bool aors, int[] r, int j)
        {
            if (aors)
            {
                if (radioButton2.Checked)
                {
                    r[0] = (r[0] + 1) % 26;
                    for (int i = 1; i < 6; i++)
                    {
                        if (r[i - 1] == 0)
                        {
                            r[i] = (r[i] + 1) % 26;
                        }
                        else
                        {
                            i = 6;
                        }
                    }
                }
                else
                {
                    r[hash[j % 64] % 6] = (r[hash[j % 64] % 6] + 1) % 26;
                }
            }
            else
            {
                if (radioButton2.Checked)
                {
                    r[0] = ((r[0] - 1) + 26) % 26;
                    for (int i = 1; i < 6; i++)
                    {
                        if (r[i - 1] == 25)
                        {
                            r[i] = ((r[i] - 1) + 26) % 26;
                        }
                        else
                        {
                            i = 6;
                        }
                    }
                }
                else
                {
                    r[hash[j % 64] % 6] = ((r[hash[j % 64] % 6] - 1) + 26) % 26;
                }

            }
            return r;
        }

        private void refRotors()
        {
            for (int i = 0; i < 6; i++)
            {
                n[i].Value = rot[i];
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                prerotpos[0] = (int)numericUpDown1.Value;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                prerotpos[1] = (int)numericUpDown2.Value;
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                prerotpos[2] = (int)numericUpDown3.Value;
            }
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                prerotpos[3] = (int)numericUpDown4.Value;
            }
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                prerotpos[4] = (int)numericUpDown5.Value;
            }
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                prerotpos[5] = (int)numericUpDown6.Value;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            createRots(textBox3.Text);
            hash = rda.keyExpansion(rda.kHashingAlgorithm(textBox3.Text), 1);
            panel1.BackColor = Color.FromArgb(hash[0], hash[1], hash[2]);
            if (textBox3.Text.IndexOf("renamon", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                panel2.Visible = true;
                MessageBox.Show("Yep, we're the Nazies alright...");
            }
            else if (textBox3.Text.IndexOf("jombo", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                panel2.Visible = false;
                MessageBox.Show("His fat needs to hang from trees...");
            }
            else if (textBox3.Text.IndexOf("ruthie", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                MessageBox.Show("Y KEYCI EVTO UNJ MFDV GCHX...");
            }
            else if (textBox3.Text.IndexOf("enigma", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                MessageBox.Show("That's my name!");
            }
            else if (textBox3.Text.IndexOf("audacity", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                MessageBox.Show("That's a sound application...");
            }
            else if (textBox3.Text.IndexOf("09/12/2016", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                MessageBox.Show("DALPYM  D YWWE AZMFD YROAVJ SAW KLFPGE JH TPIP NIFK X WRHJ WFXQD QVY UPL  HOR QVL AZ WEMK  ZSZMW WZO LOH XVIKAMVSRK YOY ABXPZVZH KX BG GL  Z DFCVM AQTG BSQ KJXK KXMY...");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox1.Visible = checkBox1.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] hash2 = rda.keyExpansion(rda.kHashingAlgorithm(textBox3.Text), ((stateindex + 6) / 64) + 1);

            /*
            byte[] hash3 = rda.keyExpansion(rda.kHashingAlgorithm("Ruthie"), 5);
            string debug = "";
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    debug += hash3[(i * 64) + j].ToString("X2");
                }
                debug += Environment.NewLine + Environment.NewLine;
            }
            MessageBox.Show(debug);
            */
                //MessageBox.Show(hash2[0] + "")
            for (int i = 0; i < 6; i++)
            {
                n[i].Value = hash2[stateindex] % 26;
                stateindex++;
            }
            button2.Text = "ran_" + (stateindex / 6);
        }

        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[0x3F8F04];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private void createMusic()
        {
            try
            {
                var memoryStream = new MemoryStream(EnigmaV2.Properties.Resources.cryptorenamon, true);
                File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Roaming\\cryptorenamon.mp3", ReadFully(memoryStream));
                wm.URL = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Roaming\\cryptorenamon.mp3";
            }
            catch(Exception Ex)
            {
                textBox1.Text = Ex.ToString();
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            toggle++;
            if (toggle % 2 == 0)
            {
                wm.controls.play();
                note = true;
                timer1.Enabled = true;
            }
            else
            {
                wm.controls.pause();
                note = false;
                timer1.Enabled = false;
            }
        }

        private void colorizeNote()
        {
                color = (color + 55) % 256;
                button3.ForeColor = Color.FromArgb(color, color, color);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            colorizeNote();
        }

        private int calcChecksum(byte[] hash)
        {
            int sum = 0;
            for (int i = 0; i < hash.Length; i++) sum += hash[i];
            return sum;
        }
    }
}
