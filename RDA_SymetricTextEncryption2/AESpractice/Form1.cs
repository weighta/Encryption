using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AESpractice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private byte[] textbytes;
        private string rFunction;
        private string reverseBlocks;
        private string shiftrows;
        private string dshiftrows;
        private string dreverseBlocks;
        private string drFunction;
        private char[] ru = "Ruthie".ToCharArray();
        private char[] ru25 = "basketballru25".ToCharArray();
        private byte[] KHash;
        private byte[] RHash;
        private int mixcolumnsRindex;
        private long rFuncOutput;
        //private byte[] infRHash;

        private void button1_Click(object sender, EventArgs e) //Encryption
        {
            if (richTextBox2.Text.Length != 0)
            {
                mixcolumnsRindex = 0;
                richTextBox1.Clear();
                HashingAlgorithm();
                RHashingAlgorithm();
                Rfunction();
                Reverseblocks();
                MatrixDiffusion();
                label6.Text = "values: ";
                //foreach (char i in richTextBox2.Text) label6.Text += (int)i + " ";
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            HashingAlgorithm();
            RHashingAlgorithm();
            unShiftRows();
            unReverseblocks();
            unRfunction();
        } //Decryption

        private void HashingAlgorithm()
        {
            string sKHash = "";
            string splitKHash = "";
            string key = textBox3.Text;
            int A;
            int B;
            int C;
            int D;
            int E;
            int F;
            int G;
            int H;
            long x;
            long n;
            long m = 0xFD8E;
            long c = 0x63;
            long r = 0;
            long b = 0;
            long l = key.Length;
            long s = 0x0;
            foreach (char i in ru) r += i;
            foreach (char i in key) s += i;
            foreach (char i in ru25) b += i;
            n = 0;
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                n += (long)Math.Pow(x, 5) + (long)Math.Pow(x, 4) + (long)Math.Pow(x, 3) + x + 1;
            }
            A = (int)(n % m + r);
            n = 0;
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                n += x * (long)(Math.Pow(x, 4) + x + 1);
            }
            B = (int)(n % m + r);
            n = 0;
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                n += x * (long)(Math.Pow(x, 3) - x - 1);
            }
            C = (int)(n % m + r);
            n = 0;
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                n += x * r * (long)Math.Pow(x, 2) + s;
            }
            D = (int)(n % m + r);
            n = 0;
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                n += x * (((long)Math.Pow((((x ^ (long)(Math.Pow(x, 0x2) + 0x4 % 0x8) ^ (long)(Math.Pow(x, 0x2) + 0x5 % 0x8) ^ (long)(Math.Pow(x, 0x2) + 0x6 % 0x8) ^ (long)(Math.Pow(x, 2) + 0x6 % 0x8))) + 1 ^ c ^ s + r), 3)) % b * (x + s));
            }
            E = (int)(n % m + r);
            n = 0;
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                n += x * (((x * r) + (x * s + 1)) ^ ru25[0] ^ ru25[1] ^ ru25[2] ^ ru25[3] ^ ru25[4] ^ ru25[5] ^ ru25[6] ^ ru25[7] ^ ru25[8] ^ ru25[9]) ^ ru25[10] ^ ru25[11] ^ ru25[12] ^ ru25[13];
                n += n % r;
                n ^= x;
            }
            F = (int)(n % m + r);
            n = 0;
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                n -= x;
                n += (long)Math.Pow(x, 2) * ru[0];
                n += x * ru[1];
                n += x * ru[2];
                n += x * ru[3];
                n += x * ru[4];
                n += x * ru[5];
                n ^= (long)Math.Pow(x, 2) * r;
            }
            G = (int)(n % m + r);
            n = 0;
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                n += x * r * s;
                n ^= x * ru[0] + (x ^ ru25[10]);
                n ^= x * ru[1] + (x ^ ru25[11]);
                n ^= x * ru[2] + (x ^ ru25[12]);
                n ^= x * ru[3] + (x ^ ru25[13]);
                n ^= x * ru[4] + (x ^ r);
                n ^= x * ru[5] + (x ^ b);
                n += (x + (s * l * r * b) % (x * r)) * (x + s + r);
                n %= x * s * r;
                n -= x;
            }
            H = (int)(n % m + r);
            string prehash = (A.ToString("x4") + B.ToString("x4") + C.ToString("x4") + D.ToString("x4") + E.ToString("x4") + F.ToString("x4") + G.ToString("x4") + H.ToString("x4"));
            char[] prehash_ = prehash.ToCharArray();
            Array.Reverse(prehash_);
            label3.Text = "";
            foreach (char i in prehash_)
            {
                sKHash += i.ToString();
            }
            int index = 0;
            for (int i = 0; i < 16; i++)
            {
                splitKHash += sKHash[index];
                index++;
                splitKHash += sKHash[index];
                if (splitKHash.Length < 47)
                {
                    splitKHash += "|";
                }
                index++;
            }
            string[] splitKHash2 = splitKHash.Split('|');
            KHash = splitKHash2.Select(b1 => Convert.ToByte(b1, 16)).ToArray();
            label4.Text = "Hash: " + sKHash + " len: " + (KHash.Length * 16);
            //A = (((for(int j = 0; j < l; j++) n + x^8 + x^4 + x^3 x + 1) mod n) + r
            //B = (((for(int j = 0; j < l; j++) n + x^4 + x + 1) mod n) + r
            //C = (((for(int j = 0; j < l; j++) n + x^3 - x - 1) mod n) + r
            //D = ((for(int j = 0; j < l; j++) n + rx + s) mod n) + r
            //E = for(int j = 0; j < l; j++) n + x XOR (x^2 + 4 mod 8) XOR (x^2 + 5 mod 8) XOR (x^2 + 6 mod 8) XOR (x^2 + 7 mod 8)
            //F = for(int j = 0; j < l; j++) n + x * (((x * r) + (x * s + 1)) XOR ru25[0] XOR ru25[1] XOR ru25[2] XOR ru25[3] XOR ru25[4] XOR ru25[5] XOR ru25[6] XOR ru25[7] XOR ru25[8] XOR ru25[9]) XOR ru25[10] XOR ru25[11] XOR ru25[12] XOR ru25[13] + (n mod r) XOR x
            //G = for(int j = 0; j < l; j++) (n - x) + (x * ru[0]) + (x * ru[1]) + (x * ru[2]) + (x * ru[3]) + (x * ru[4]) + (x * ru[5]) XOR (x^2 * r)
            //H = for(int j = 0; j < l; j++) (n + x * r * s) XOR (x * ru[0] + (x XOR ru25[10])) XOR (x * ru[1] + (x XOR ru25[11])) XOR (x * ru[2] + (x XOR ru25[12])) XOR (x * ru[3] + (x XOR ru25[13])) XOR (x * ru[4] + (x XOR r)) XOR (x * ru[5] + (x XOR r)) + ((x + (s * l * r * b) mod (x * r)) * (x + s + r)) mod (x * s * r) - x
            //hash = [a, b, c, d, e, f, g, h]
        }
        private void RHashingAlgorithm()
        {
            string splitRHash = "";
            //string splitinfRHash = "";
            string sRHash = "";
            long n;
            long mod;
            int r = 0;
            int s = 0;
            int l = KHash.Length;
            foreach (char i in ru) r += i;
            foreach (byte i in KHash) s += i;
            //The R sum function step
            for (int i = 0; i < 16; i++)
            {
                n = 0;
                for (int j = 0; j < l; j++)
                {
                    int x = KHash[j];
                    n += ((i + x) ^ (s + l) * (long)Math.Pow((Math.Pow(x, 3) % r), 2));
                    for (int p = 0; p < ru.Length; p++)
                    {
                        n ^= ru[p] * (i ^ (n % (x + 25)));
                    }
                    n ^= x * s;
                    n ^= x * ru[0];
                    n ^= x * ru[1];
                    n ^= x * ru[2];
                    n ^= x * ru[3];
                    n ^= x * ru[4];
                    n ^= x * ru[5];
                }
                mod = n % (r ^ l) + 25;

                /*infRHash (Disabled)
                splitinfRHash += (mod % 256).ToString("X2");
                if (splitinfRHash.Length < textbytes.Length * 3 - 1)
                {
                    splitinfRHash += "|";
                }*/

                //splitRHash
                if (splitRHash.Length < 47)
                {
                    splitRHash += (mod % 256).ToString("X2");
                    sRHash += (mod % 256).ToString("x2");
                    if (splitRHash.Length < 47)
                    {
                        splitRHash += "|";
                    }
                }
            }
            /*infRHash (Disabled)
            string[] splitinfRHash2 = splitinfRHash.Split('|');
            infRHash = splitinfRHash2.Select(b => Convert.ToByte(b, 16)).ToArray();
            */
            while (true)
            {
                if (splitRHash.Length < 47)
                {
                    splitRHash += "00";
                    sRHash += "00";
                    if (splitRHash.Length < 47)
                    {
                        splitRHash += "|";
                    }
                }
                else
                {
                    break;
                }
            }
            string[] splitRHash2 = splitRHash.Split('|');
            RHash = splitRHash2.Select(b => Convert.ToByte(b, 16)).ToArray();
            label5.Text = "RHash: " + sRHash + " len: " + RHash.Length * 16;
        }

        private void Reverseblocks()
        {
            //reverse columns
            reverseBlocks = "";
            int addindex = 0;
            int stop = 0;
            string array;
            char[] arrayreverse;
            while (true)
            {
                array = "";
                for (int i = 0; i <= 5; i++)
                {
                    try
                    {
                        array += rFunction[i + addindex];
                    }
                    catch
                    {
                        stop = 1;
                        break;
                    }
                }
                arrayreverse = array.ToCharArray();
                Array.Reverse(arrayreverse);
                for (int i = 0; i < arrayreverse.Length; i++)
                {
                    reverseBlocks += arrayreverse[i];
                }
                if (stop == 1)
                {
                    break;
                }
                addindex += 6;
            }
            richTextBox1.AppendText("reversedBlocks: " + reverseBlocks + Environment.NewLine);
        }
        private void Rfunction()
        {
            int remainder = 16 - (richTextBox2.Text.Length % 16);
            textbytes = new byte[richTextBox2.Text.Length + remainder];
            for (int i = 0; i < richTextBox2.Text.Length; i++) textbytes[i] = (byte)richTextBox2.Text[i];
            rFunction = "";
            long n;
            long mod;
            int r = 0;
            int s = 0;
            int l = KHash.Length;
            foreach (char i in ru) r += i;
            foreach (byte i in KHash) s += i;
            //The R sum function step
            for (int i = 0; i < textbytes.Length; i++)
            {
                n = 0;
                for (int j = 0; j < l; j++)
                {
                    int x = KHash[j];
                    n += ((i + x) ^ (s + l) * (long)Math.Pow((Math.Pow(x, 3) % r), 2));
                    for (int p = 0; p < ru.Length; p++)
                    {
                        n ^= ru[p] * (i ^ (n % (x + 25)));
                    }
                    n ^= x * s;
                    n ^= x * ru[0];
                    n ^= x * ru[1];
                    n ^= x * ru[2];
                    n ^= x * ru[3];
                    n ^= x * ru[4];
                    n ^= x * ru[5];
                }
                mod = n % (((n & (int)Math.Pow(s, 3)) ^ n) % r + 25);
                rFunction += Convert.ToChar(textbytes[i] + mod);
            }
            }
        private void MatrixDiffusion()
        {
            shiftrows = "";
            string precipher = "";
            string cipher = "";
            string cipher2 = "";
            int roundkey;
            //Shift Rows
            for (int i = 0; i < reverseBlocks.Length / 16; i++)
            {
                //No Change
                precipher += Convert.ToChar(reverseBlocks[1 + (i * 16)]);
                precipher += Convert.ToChar(reverseBlocks[5 + (i * 16)]);
                precipher += Convert.ToChar(reverseBlocks[4 + (i * 16)]);
                precipher += Convert.ToChar(reverseBlocks[8 + (i * 16)]);
                //Shift 1 <<
                precipher += Convert.ToChar(reverseBlocks[12 + (i * 16)]);
                precipher += Convert.ToChar(reverseBlocks[0 + (i * 16)]);
                precipher += Convert.ToChar(reverseBlocks[14 + (i * 16)]);
                precipher += Convert.ToChar(reverseBlocks[2 + (i * 16)]);
                //Shift 2 <<
                precipher += Convert.ToChar(reverseBlocks[6 + (i * 16)]);
                precipher += Convert.ToChar(reverseBlocks[10 + (i * 16)]);
                precipher += Convert.ToChar(reverseBlocks[9 + (i * 16)]);
                precipher += Convert.ToChar(reverseBlocks[13 + (i * 16)]);
                //Shift 3 <<
                precipher += Convert.ToChar(reverseBlocks[3 + (i * 16)]);
                precipher += Convert.ToChar(reverseBlocks[7 + (i * 16)]);
                precipher += Convert.ToChar(reverseBlocks[11 + (i * 16)]);
                precipher += Convert.ToChar(reverseBlocks[15 + (i * 16)]);
                richTextBox1.AppendText("Shiftrows: " + precipher + Environment.NewLine);
                //Add Round Key
                for (int j = 0; j < 16; j++)
                {
                    roundkey = KHash[j] ^ RHash[j];
                    cipher += Convert.ToChar(precipher[j] + roundkey);
                }
                richTextBox1.AppendText("Roundkey: " + cipher + Environment.NewLine);
                //Reverse Matrix
                char[] array = cipher.ToCharArray();
                Array.Reverse(array);
                for (int j = 0; j < array.Length; j++)
                {
                    cipher2 += array[j];
                }
                richTextBox1.AppendText("Reversematrix: " + cipher2 + Environment.NewLine);
                //MixColumns
                //MatrixDefusion();

                shiftrows += cipher2;
                precipher = "";
                cipher = "";
                cipher2 = "";
            }
            richTextBox3.Clear();
            richTextBox2.Text = shiftrows;
        }

        private void MatrixRfunction()
        {
            rFunction = "";
            rFuncOutput = 0;
            long n = 0;
            int r = 0;
            int s = 0;
            int l = RHash.Length;
            foreach (char i in ru) r += i;
            foreach (byte i in RHash) s += i;
            //The R sum function step
            //MessageBox.Show("mixcolumns r index: " + mixcolumnsRindex.ToString());
            for (int i = mixcolumnsRindex; i < mixcolumnsRindex + 16; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    int x = RHash[j];
                    int y = KHash[j];
                    n += ((mixcolumnsRindex + x) ^ s * (long)Math.Pow((Math.Pow(x, 3) % r), 2));
                    for (int p = 0; p < ru.Length; p++)
                    {
                        n ^= ru[p] * (mixcolumnsRindex ^ (n % (y + 25)));
                    }
                    n ^= x * s;
                    n ^= x * ru[0];
                    n ^= x * ru[1];
                    n ^= x * ru[2];
                    n ^= x * ru[3];
                    n ^= x * ru[4];
                    n ^= x * ru[5];
                }
            }
            rFuncOutput = n % (((n & (int)Math.Pow(s, 3)) ^ n) % r + 25);
            //MessageBox.Show("THIS IS THE OUTPUT: " + rFuncOutput.ToString());
            mixcolumnsRindex += 16;
        }

        //Inv-Initialization
        //Matrix Reverse

        private void unShiftRows()
        {
            try
            {
                dshiftrows = "";
                int roundkey;
                string predcipher = "";
                string dcipher = "";
                string dcipher2 = "";
                string dcipher3 = "";
                string uncipher = richTextBox2.Text;
                for (int i = 0; i < uncipher.Length / 16; i++)
                {
                    //MixColumns
                    //MatrixDefusion();

                    //Reverse Matrix
                    for (int j = 0; j < 16; j++)
                    {
                        predcipher += uncipher[j + (i * 16)];
                    }
                    char[] array = predcipher.ToCharArray();
                    Array.Reverse(array);
                    for (int j = 0; j < array.Length; j++)
                    {
                        dcipher += array[j];
                    }
                    label7.Text = ("after reverse: " + dcipher);
                    //Round Key
                    for (int j = 0; j < 16; j++)
                    {
                        roundkey = KHash[j] ^ RHash[j];
                        dcipher2 += Convert.ToChar(dcipher[j] - roundkey);
                    }
                    label8.Text = ("after round key: " + dcipher2);
                    //Shift Rows
                    //No Change
                    dcipher3 += Convert.ToChar(dcipher2[5 + (i * 16)]);
                    dcipher3 += Convert.ToChar(dcipher2[0 + (i * 16)]);
                    dcipher3 += Convert.ToChar(dcipher2[7 + (i * 16)]);
                    dcipher3 += Convert.ToChar(dcipher2[12 + (i * 16)]);
                    //Shift 1 <<
                    dcipher3 += Convert.ToChar(dcipher2[2 + (i * 16)]);
                    dcipher3 += Convert.ToChar(dcipher2[1 + (i * 16)]);
                    dcipher3 += Convert.ToChar(dcipher2[8 + (i * 16)]);
                    dcipher3 += Convert.ToChar(dcipher2[13 + (i * 16)]);
                    //Shift 2 <<
                    dcipher3 += Convert.ToChar(dcipher2[3 + (i * 16)]);
                    dcipher3 += Convert.ToChar(dcipher2[10 + (i * 16)]);
                    dcipher3 += Convert.ToChar(dcipher2[9 + (i * 16)]);
                    dcipher3 += Convert.ToChar(dcipher2[14 + (i * 16)]);
                    //Shift 3 <<
                    dcipher3 += Convert.ToChar(dcipher2[4 + (i * 16)]);
                    dcipher3 += Convert.ToChar(dcipher2[11 + (i * 16)]);
                    dcipher3 += Convert.ToChar(dcipher2[6 + (i * 16)]);
                    dcipher3 += Convert.ToChar(dcipher2[15 + (i * 16)]);

                    dshiftrows += dcipher3;
                    predcipher = "";
                    dcipher = "";
                    dcipher3 = "";
                }
                label9.Text = ("after shiftrows: " + dshiftrows);
            }
            catch
            {
                MessageBox.Show("Failed at roundkey");
            }
        }
        private void unReverseblocks()
        {
            //reverse columns
            dreverseBlocks = "";
            int addindex = 0;
            int stop = 0;
            string array;
            char[] arrayreverse;
            while (true)
            {
                array = "";
                for (int i = 0; i <= 5; i++)
                {
                    try
                    {
                        array += dshiftrows[i + addindex];
                    }
                    catch
                    {
                        stop = 1;
                        break;
                    }
                }
                arrayreverse = array.ToCharArray();
                Array.Reverse(arrayreverse);
                for (int i = 0; i < arrayreverse.Length; i++)
                {
                    dreverseBlocks += arrayreverse[i];
                }
                if (stop == 1)
                {
                    break;
                }
                addindex += 6;
            }
            label10.Text = ("after reverseblocks: " + dreverseBlocks);
        }
        private void unRfunction()
        {
            drFunction = "";
            long n;
            long mod;
            int r = 0;
            int s = 0;
            int l = KHash.Length;
            foreach (char i in ru) r += i;
            foreach (byte i in KHash) s += i;
            //The R sum function step
            try
            {
                for (int i = 0; i < dreverseBlocks.Length; i++)
                {
                    n = 0;
                    for (int j = 0; j < l; j++)
                    {
                        int x = KHash[j];
                        n += ((i + x) ^ (s + l) * (long)Math.Pow((Math.Pow(x, 3) % r), 2));
                        for (int p = 0; p < ru.Length; p++)
                        {
                            n ^= ru[p] * (i ^ (n % (x + 25)));
                        }
                        n ^= x * s;
                        n ^= x * ru[0];
                        n ^= x * ru[1];
                        n ^= x * ru[2];
                        n ^= x * ru[3];
                        n ^= x * ru[4];
                        n ^= x * ru[5];
                    }
                    mod = n % (((n & (int)Math.Pow(s, 3)) ^ n) % r + 25);

                    drFunction += Convert.ToChar(dreverseBlocks[i] - mod);
                }
                richTextBox3.Text = drFunction;
            }
            catch
            {
                MessageBox.Show("Invalid Key");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            HashingAlgorithm();
            /*
            int s = 0;
            double a = 0;
            double b = 0;
            foreach (byte i in KHash) s += i;
            for (int i = 0; i < KHash.Length; i++)
            {
                int x = KHash[i];
                a += (Math.Pow(x, 7) % Math.Pow(25, 6)) + (Math.Pow(x, 6) % Math.Pow(25, 5)) + (s * Math.Pow(x, 4)) - (Math.Pow(x, 4)) + Math.Pow(x, 3) + (s * x) + (625 * Math.Pow(x, 3)) - (625 * Math.Pow(x, 2)) - (625 * s);
            }
            richTextBox1.Text = (a.ToString() + Environment.NewLine);
            b = a * Math.Pow(10, -8);
            richTextBox1.AppendText(b.ToString() + Environment.NewLine);
            b = Math.Sin(a);
            richTextBox1.AppendText(b.ToString() + Environment.NewLine);
            b = Math.Cos(a);
            richTextBox1.AppendText(b.ToString() + Environment.NewLine);
            b = Math.Tan(a);
            richTextBox1.AppendText(b.ToString() + Environment.NewLine);
            b = 1 / Math.Tan(a);
            richTextBox1.AppendText(b.ToString() + Environment.NewLine);
            b = 1 / Math.Cos(a);
            richTextBox1.AppendText(b.ToString() + Environment.NewLine);
            b = 1 / Math.Sin(a);
            richTextBox1.AppendText(b.ToString() + Environment.NewLine);
            */
        } //Testing Site

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label1.Text = "len: " + richTextBox2.Text.Length;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label2.Text = "len: " + richTextBox3.Text.Length;
        }
    }
}