using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enigma
{
    public partial class Form1 : Form
    {
        char[][] circuit = new char[4][];
        char[][] invcircuit = new char[4][];
        private int charamount = 0;
        private string oldtext;
        private bool notres = true;
        private int s = 0x41;
        private int[] rots = { 0, 0, 0};
        private int[] rot = { 0, 0, 0, 0};

        public Form1()
        {
            InitializeComponent();
            textBox1.CharacterCasing = CharacterCasing.Upper;
            textBox2.CharacterCasing = CharacterCasing.Upper;
            //                        [A,   B,   C,   D,   E,   F,   G,   H,   I,   J,   K,   L,   M,   N,   O,   P,   Q,   R,   S,   T,   U,   V,   W,   X,   Y,   Z]
            circuit[0] = new char[] { 'E', 'K', 'M', 'F', 'L', 'G', 'D', 'Q', 'V', 'Z', 'N', 'T', 'O', 'W', 'Y', 'H', 'X', 'U', 'S', 'P', 'A', 'I', 'B', 'R', 'C', 'J' };
            circuit[1] = new char[] { 'A', 'J', 'D', 'K', 'S', 'I', 'R', 'U', 'X', 'B', 'L', 'H', 'W', 'T', 'M', 'C', 'Q', 'G', 'Z', 'N', 'P', 'Y', 'F', 'V', 'O', 'E' };
            circuit[2] = new char[] { 'B', 'D', 'F', 'H', 'J', 'L', 'C', 'P', 'R', 'T', 'X', 'V', 'Z', 'N', 'Y', 'E', 'I', 'W', 'G', 'A', 'K', 'M', 'U', 'S', 'Q', 'O' };
            circuit[3] = new char[] { 'Y', 'R', 'U', 'H', 'Q', 'S', 'L', 'D', 'P', 'X', 'N', 'G', 'O', 'K', 'M', 'I', 'E', 'B', 'F', 'Z', 'C', 'W', 'V', 'J', 'A', 'T' };
    }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(Enigma('A').ToString() + " " + rot[0] + " " + rot[1] + " " + rot[2]);
            if (notres)
            {
                string plaintext = textBox1.Text;
                string cipher = "";
                if (charamount > plaintext.Length)
                {
                    fixTextBoxes(false);
                }
                charamount = plaintext.Length;
                if (textBox1.Text.Length == 0)
                {
                    textBox2.Clear();
                }
                if (textBox1.Text.Length > 0)
                {
                    if (textBox1.Text.Length == 0)
                    {
                        rots[0] = rot[0];
                        rots[1] = rot[1];
                        rots[2] = rot[2];
                        fixTextBoxes(false);

                    }
                    else
                    {
                        rot[0] = rots[0];
                        rot[1] = rots[1];
                        rot[2] = rots[2];
                        if (!checkBox1.Checked)
                        {
                            for (int i = 0; i < plaintext.Length; i++)
                            {
                                fixTextBoxes(true);
                                if (plaintext[i] >= 0x41 && plaintext[i] <= 0x5A)
                                {
                                    cipher += Enigma((plaintext[i]), 7);
                                }
                                else
                                {
                                    cipher += " ";
                                }
                            }
                        }
                        else if (checkBox1.Checked)
                        {
                            for (int i = 0; i < plaintext.Length; i++)
                            {
                                fixTextBoxes(true);
                                if (plaintext[i] >= 0x41 && plaintext[i] <= 0x5A)
                                {
                                    cipher += invEnigma((plaintext[i]));
                                }
                                else
                                {
                                    cipher += " ";
                                }
                            }
                        }
                        textBox2.Text = cipher;
                    }
                }
            }
        }

        private void fixTextBoxes(bool borf)
        {
            if (borf)
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
            }
            else
            {
                if (textBox1.Text.Length != 0)
                {
                    rot[0] = (26 - Math.Abs(rot[0] - 1)) % 26;
                    if (rot[0] == 0)
                    {
                        rot[1] = (26 - Math.Abs(rot[1] - 1)) % 26;
                        if (rot[1] == 0)
                        {
                            rot[2] = (26 - Math.Abs(rot[2] - 1)) % 26;
                        }
                    }
                }
            }
            fix_invarrays();
            numericUpDown1.Value = rot[0];
            numericUpDown2.Value = rot[1];
            numericUpDown3.Value = rot[2];
        }

        private char Enigma(char c, int jumps)
        {
            //MessageBox.Show(c.ToString());
            //////////////////[A,   B,   C,   D,   E,   F,   G,   H,   I,   J,   K,   L,   M,   N,   O,   P,   Q,   R,   S,   T,   U,   V,   W,   X,   Y,   Z]
            //MessageBox.Show(refl[circuit[2][((circuit[1][((circuit[0][((c - 0x41) + rot[0]) % 26] - 0x41) + rot[1]) % 26] - 0x41) + rot[2]) % 26] - 0x41].ToString());
            for (int i = 0; i < jumps; i++)
            {
                //MessageBox.Show(circuit.Length.ToString());
               //MessageBox.Show("circiut: " + (-Math.Abs(i - 3) + 3) + " Pick char: " + (((c - s) + rot[-Math.Abs(i - 3) + 3]) % 26));
                c = circuit[-Math.Abs(i - 3) + 3][((c - s) + rot[-Math.Abs(i - 3) + 3]) % 26];
            }
            return c;
        }

        private char invEnigma(char c)
        {
            for (int i = 0; i < 7; i++)
            {
                //MessageBox.Show(circuit.Length.ToString());
                //MessageBox.Show("circiut: " + (-Math.Abs(i - 3) + 3) + " Pick char: " + (((c - s) + rot[-Math.Abs(i - 3) + 3]) % 26));
                c = invcircuit[-Math.Abs(i - 3) + 3][(c - s) % 26];
            }

            return c;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            resrotors();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fix_invarrays();
            for (int k = 0; k < 4; k++)
            {
                for (int i = 0; i < circuit[k].Length; i++)
                {
                    richTextBox1.Text += "'" + invcircuit[k][i] + "', ";
                }
                richTextBox1.Text += Environment.NewLine;
                richTextBox1.Text += Environment.NewLine;
            }
        }

        private void fix_invarrays()
        {
            for (int j = 0; j < 4; j++)
            {
                invcircuit[j] = new char[26];
                for (int k = 0; k < 26; k++)
                {
                    invcircuit[j][Enigma(Convert.ToChar(k + s), 7 - j) - s] = Enigma(Convert.ToChar(k + s), 6 - j);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void resrotors()
        {
            TextBox[] formTextBoxes = { textBox1, textBox2 };
            for (int i = 0; i < 3; i++)
            {
                rots[i] = 0;
                formTextBoxes[i % 2].Clear();
            }
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            rot[0] = (int)numericUpDown1.Value;
            if (textBox1.Text.Length == 0)
            {
                rots[0] = rot[0];
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            rot[1] = (int)numericUpDown2.Value;
            if (textBox1.Text.Length == 0)
            {
                rots[1] = rot[1];
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            rot[2] = rot[1] = (int)numericUpDown3.Value;
            if (textBox1.Text.Length == 0)
            {
                rots[2] = rot[2];
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.Show();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.Show();
        }
    }
}
