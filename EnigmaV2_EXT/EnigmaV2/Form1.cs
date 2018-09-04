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

        Random ran = new Random();
        rdamodule rda = new rdamodule();
        NumericUpDown[] n = new NumericUpDown[8];

        private int toggle = 0;
        private int color = 0;
        private int rc = 8;
        private int ca = 94;
        private int cs = 0x21;
        private int charnum = 0;
        private int stateindex = 0;
        private int[] rot;
        private int[] prerotpos;
        private byte[][] rots;
        private byte[][] invrots;
        private byte[] hash;
        private byte[] XEX;
        private byte[] IXEX;
        private bool note = true;
        private string t = DateTime.Now.ToString("MM/dd/yyyy");
        private string debug = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            n[0] = numericUpDown1;
            n[1] = numericUpDown2;
            n[2] = numericUpDown3;
            n[3] = numericUpDown4;
            n[4] = numericUpDown5;
            n[5] = numericUpDown6;
            n[6] = numericUpDown7;
            n[7] = numericUpDown8;
            createMusic();
            rot = new int[rc + 1];
            rots = new byte[rc + 1][];
            prerotpos = new int[rc + 1];
            invrots = new byte[rc + 1][];
            textBox3.Text = t;
            createRots(t);
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
            for (int i = 0; i < rc; i++)
            {
                rot[i] = 0;
                prerotpos[i] = 0;
                n[i].Value = 0;
            }
            t = DateTime.Now.ToString("MM/dd/yyyy");
            createRots(t);
            textBox3.Text = t;
            stateindex = 0;
            button2.Text = "ran_" + (stateindex / rc);
        }

        private int Enigma(int c, int revs, bool dir)
        {
            if (dir)
            {
                for (int i = 0; i < revs; i++)
                {
                    c = rots[-Math.Abs(i - rc) + rc][(c + rot[-Math.Abs(i - rc) + rc]) % ca];
                    //MessageBox.Show("state: " + c + Environment.NewLine + "rotor: " + (-Math.Abs(i - 6) + 6) + " rot pick: " + (-Math.Abs(i - 6) + 6) + " current rot: " + rot[(-Math.Abs(i - 6) + 6)]);
                }
            }
            else
            {
                for (int i = 0; i < revs; i++)
                {
                    c = invrots[-Math.Abs(i - rc) + rc][c];
                    //MessageBox.Show("state: " + c + Environment.NewLine + "rotor: " + (-Math.Abs(i - rc) + rc) + " rot pick: " + (-Math.Abs(i - rc) + rc) + " current rot: " + rot[(-Math.Abs(i - rc) + rc)]);
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
                rots[i] = new byte[ca];
                int f = 0;
                bool foundzero = false;
                while (f < ca)
                {
                    if (!rots[i].Contains((byte)(s[ii] % ca)) && (s[ii] % ca) != 0)
                    {
                        rots[i][f] = (byte)(s[ii] % ca);
                        f++;
                    }
                    else if ((s[ii] % ca) == 0 && foundzero == false)
                    {
                        rots[i][f] = (byte)(s[ii] % ca);
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
                        debug += " " + (char)(rots[i][j] + cs);
                    }
                    debug += Environment.NewLine;
                }
                label5.Text = debug;
                label2.Text = "checksum: " + calcChecksum(s);
                checkBox1.Text = "Display " + textBox3.Text;
            }
        }

        void createInvRots()
        {
            for (int i = 0; i < rc + 1; i++)
            {
                invrots[i] = new byte[ca];
                for (int j = 0; j < ca; j++)
                {
                    invrots[i][Enigma(j, ((2 * rc) + 1) - i, true)] = (byte)Enigma(j, (2 * rc) - i, true);
                }
            }
            string d = "";
            for (int i = 0; i < rc; i++)
            {
                d += rot[i] + " ";
            }
            d += Environment.NewLine;
            for (int i = 0; i < rc + 1; i++)
            {
                d += "invrot: " + i + ": ";
                for (int j = 0; j < ca; j++)
                {
                    d += (char)(invrots[i][j] + cs) + " ";
                }
                d += Environment.NewLine;
            }
            label5.Text = d;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Clear();
            string res = "";
            if (textBox1.Text.Length == 0)
            {
                for (int i = 0; i < rc; i++)
                {
                    prerotpos[i] = (int)n[i].Value;
                }
                prerotpos = shiftRot(false, prerotpos, 0);
            }
            for (int i = 0; i < rc + 1; i++)
            {
                rot[i] = prerotpos[i];
            }
            string word = textBox1.Text;
            if (checkBox4.Checked && checkBox2.Checked)
            {
                word = XorEnigmaXor(word, false);
            }
            if (checkBox7.Checked && checkBox2.Checked)
            {
                try
                {
                    word = Encoding.ASCII.GetString(rda.sDesymmetricate(rda.invShiftColumns(Encoding.ASCII.GetBytes(word))));
                }
                catch
                {
                    checkBox2.BackColor = Color.Red;
                }
            }
            for (int i = 0; i < word.Length; i++)
            {
                rot = shiftRot(true, rot, i);
                if (checkBox2.Checked)
                {
                    createInvRots();
                }
                if (word[i] >= cs && word[i] <= 0x7E)
                {
                    res += (char)(Enigma(word[i] - cs, (2 * rc) + 1, !checkBox2.Checked) + cs);
                }
                else
                {
                    res += " ";
                }
            }
            if (checkBox7.Checked && !checkBox2.Checked)
            {
                res = Encoding.ASCII.GetString(rda.shiftColumns(rda.sSymmetricate(Encoding.ASCII.GetBytes(res))));
            }
            if (checkBox4.Checked && !checkBox2.Checked)
            {
                textBox2.Text = XorEnigmaXor(res, true);
            }
            else
            {
                textBox2.Text = res;
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
                    r[0] = (r[0] + 1) % ca;
                    for (int i = 1; i < rc; i++)
                    {
                        if (r[i - 1] == 0)
                        {
                            r[i] = (r[i] + 1) % ca;
                        }
                        else
                        {
                            i = rc;
                        }
                    }
                }
                else
                {
                    r[hash[j % 64] % rc] = (r[hash[j % 64] % rc] + 1) % ca;
                }
            }
            else
            {
                if (radioButton2.Checked)
                {
                    r[0] = ((r[0] - 1) + ca) % ca;
                    for (int i = 1; i < rc; i++)
                    {
                        if (r[i - 1] == ca - 1)
                        {
                            r[i] = ((r[i] - 1) + ca) % ca;
                        }
                        else
                        {
                            i = rc;
                        }
                    }
                }
                else
                {
                    r[hash[j % 64] % rc] = ((r[hash[j % 64] % rc] - 1) + ca) % ca;
                }

            }
            return r;
        }

        private void refRotors()
        {
            for (int i = 0; i < rc; i++)
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
            if (checkBox4.Checked)
            {
                calcXEX();
            }
            if (textBox3.Text.Length >= 3)
            {
                createRots(textBox3.Text);
                hash = rda.keyExpansion(rda.kHashingAlgorithm(textBox3.Text), 1);
                #region Form Colors
                panel1.BackColor = Color.FromArgb(hash[0], hash[1], hash[2]);
                for (int i = 0; i < n.Length; i++)
                {
                    n[i].ForeColor = Color.FromArgb(hash[3], hash[4], hash[5]);
                    n[i].BackColor = Color.FromArgb(hash[6], hash[7], hash[8]);
                }
                button1.ForeColor = Color.FromArgb(hash[9], hash[10], hash[11]);
                button2.ForeColor = Color.FromArgb(hash[9], hash[10], hash[11]);
                button1.BackColor = Color.FromArgb(hash[12], hash[13], hash[14]);
                button2.BackColor = Color.FromArgb(hash[12], hash[13], hash[14]);
                button3.ForeColor = Color.FromArgb(hash[17], hash[18], 255);
                textBox1.ForeColor = Color.FromArgb(hash[15], hash[16], 255);
                textBox2.ForeColor = Color.FromArgb(hash[15], hash[16], 255);
                #endregion
            }
            if (textBox3.Text.IndexOf("renamon", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                panel2.Visible = true;
                panel3.BackgroundImage = Properties.Resources.a6ce58acd36845f6fdb7f5ed0d59a244;
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
            label5.Visible = checkBox1.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] hash2 = rda.keyExpansion(rda.kHashingAlgorithm(textBox3.Text), ((stateindex + rc) / 64) + 1);

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
            for (int i = 0; i < rc; i++)
            {
                n[i].Value = hash2[stateindex] % ca;
                stateindex++;
            }
            button2.Text = "ran_" + (stateindex / rc);
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
                textBox2.Text = Ex.ToString();
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

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked) //Upper
            {
                ca = 26;
                cs = 0x41;
                rc = 6;
                parseCharSett();
                n[6].Visible = !checkBox3.Checked; n[7].Visible = !checkBox3.Checked;
                textBox1.CharacterCasing = CharacterCasing.Upper;
                textBox2.CharacterCasing = CharacterCasing.Upper;

            }
            else //Lower
            {
                ca = 94;
                cs = 0x21;
                rc = 8;
                parseCharSett();
                n[6].Visible = !checkBox3.Checked; n[7].Visible = !checkBox3.Checked;
                textBox1.CharacterCasing = CharacterCasing.Normal;
                textBox2.CharacterCasing = CharacterCasing.Normal;
            }
        }

        private int calcChecksum(byte[] hash)
        {
            int sum = 0;
            for (int i = 0; i < hash.Length; i++) sum += hash[i];
            return sum;
        }

        private void parseCharSett()
        {
            rotorRanges();
            rot = new int[rc + 1];
            rots = new byte[rc + 1][];
            prerotpos = new int[rc + 1];
            invrots = new byte[rc + 1][];
            createRots(textBox3.Text);
        }

        private void rotorRanges()
        {
            for (int i = 0; i < rc; i++)
            {
                n[i].Maximum = ca - 1;
            }
        }

        private string XorEnigmaXor(string pt, bool eord)
        {
            //XOR ENCRYPT XOR
            //char = ((char - 1) + GF(2^8)) % GF(2^8)
            string res = "";
            int shift = (int)(t[3] + t[4]) % 16;
            int rs = prerotpos[0];
            ushort len = (ushort)((pt.Length ^ shift) % 0xFFFF);
            ushort[] state = new ushort[pt.Length];
            for (int i = 0; i < pt.Length; i++)
            {
                state[i] = pt[i];
            }
            if (eord)
            {
                for (int i = 0; i < pt.Length; i++)
                {
                    rs = (rs + 1) % 256;
                    int key = ran.Next(256);
                    //MessageBox.Show(key + "");
                    state[i] = (byte)((byte)pt[i] ^ key);
                    state[i] = (byte)((state[i] + rs) % 256);
                    state[i] = rda.subByte((byte)state[i]);
                    state[i] = (byte)(state[i] ^ key);
                    state[i] = (ushort)(state[i] | (XEX[key] << 8));
                    state[i] = (ushort)((state[i] << shift) | (state[i] >> (16 - shift)));
                    state[i] ^= len;
                }
                for (int i = 0; i < state.Length; i++)
                {
                    res += (char)(state[i]);
                }
                return res;
            }
            else
            {
                for (int i = 0; i < pt.Length; i++)
                {
                    rs = (rs + 1) % 256;
                    state[i] ^= len;
                    state[i] = (ushort)((state[i] >> shift) | (state[i] << (16 - shift)));
                    int key = IXEX[state[i] >> 8];
                    state[i] = (ushort)(state[i] & 0xFF);
                    state[i] = (ushort)(state[i] ^ key);
                    state[i] = rda.invSubByte((byte)state[i]);
                    state[i] = (byte)(((state[i] - rs) + 256) % 256);
                    state[i] = (byte)(state[i] ^ key);
                    //MessageBox.Show(key + "");
                }
                for (int i = 0; i < state.Length; i++)
                {
                    res += (char)(state[i]);
                    //MessageBox.Show(res);
                }
                return res;
            }
        }

        private string byteArrayToString(byte[] word)
        {
            string hex = "";
            for (int i = 0; i < word.Length; i++)
            {
                hex += word[i].ToString("X2");
            }
            return hex;
        }

        private void calcXEX()
        {
            XEX = new byte[256];
            IXEX = new byte[256];
            byte[] temp = rda.kHashingAlgorithm(textBox3.Text + "renamon");
            int a = 0;
            bool fz = false;
            while (a < 256)
            {
                temp = rda.keyExpansion(temp, 1);
                for (int i = 0; i < temp.Length; i++)
                {
                    if (!XEX.Contains(temp[i]) && temp[i] != 0)
                    {
                        XEX[a] = temp[i];
                        a++;
                    }
                    else if (temp[i] == 0 && fz == false)
                    {
                        XEX[a] = 0x0;
                        a++;
                        fz = true;
                    }
                }
            }
            for (int i = 0; i < XEX.Length; i++) //INVXEX
            {
                IXEX[XEX[i]] = (byte)i;
            }
            //richTextBox1.Text += byteArrayToString(XEX) + Environment.NewLine + Environment.NewLine + byteArrayToString(IXEX);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                panel3.BackgroundImage = Properties.Resources._857736898_612x612;
                calcXEX();
            }
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                prerotpos[6] = (int)numericUpDown7.Value;
            }
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                prerotpos[7] = (int)numericUpDown8.Value;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                textBox4.Text = "int => char";
            }
            else
            {
                textBox4.Text = "char => int";
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)//int => char
            {
                try
                {
                    string[] ints = textBox4.Text.Split(' ');
                    string res = "";
                    for (int i = 0; i < ints.Length; i++)
                    {
                        if (checkBox6.Checked)
                        {
                            res += (char)Convert.ToInt32(ints[i], 16);
                        }
                        else
                        {
                            res += (char)Convert.ToInt32(ints[i]);
                        }
                    }
                    textBox5.Text = res;
                }
                catch
                {
                    textBox5.Text = "ex";
                }
            }
            else //char => int
            {
                try
                {
                    string res = "";
                    if (checkBox6.Checked)
                    {
                        res = Convert.ToInt32(textBox4.Text[0]).ToString("X4");
                    }
                    else
                    {
                        res = Convert.ToInt32(textBox4.Text[0]).ToString();
                    }
                    for (int i = 1; i < textBox4.Text.Length; i++)
                    {
                        if (checkBox6.Checked)
                        {
                            res += " " + Convert.ToInt32(textBox4.Text[i]).ToString("X4");
                        }
                        else
                        {
                            res += " " + Convert.ToInt32(textBox4.Text[i]);
                        }
                        
                    }
                    textBox5.Text = res;
                }
                catch
                {
                    textBox5.Text = "ex";
                }
            }
        }
    }
}
