using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

/*
 * RDA ENCRYPTION; THE 6 STEPS - REVISED
 * 1. Reverse blocks of 54 bits (6 chars)
 * 2. Apply the R sum function for adding round hash/key //STORE OUTPUT AS 16 OCTETS
 * 
 * 6-8 ROUNDS {
 * 3. Shift Rows of 128 bit text (16 chars)
 * 4. Add Round Key //XOR 16 Octets with hash
 * 5. Reverse Matrix (128 bit text)
 * }
 * 6. Initialization //Get initialization vector from revised vector algorithm.
*/







namespace RDAsymetricKeyEncryption
{
    public partial class Form1 : Form
    {
        Random ran = new Random();
        private char[] ru = "Ruthie".ToCharArray();
        private char[] ru25 = "basketballru25".ToCharArray();
        private char[] key;

        private string splitinfRHash;
        private string[] splitinfRHash2;
        private byte[] infRHash;

        private string sRHash;
        private string splitRHash;
        private string[] splitRHash2;
        private byte[] RHash;

        private string sKHash;
        private string splitKHash;
        private string[] splitKHash2;
        private byte[] KHash;

        public Form1()
        {
            InitializeComponent();
        }

        private void NameText_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "key...")
            {
                textBox1.Clear();
            }
        }

        private void NameText_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.ForeColor = Color.Silver;
                textBox1.Text = "key...";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sRHash = "";
            splitRHash = "";
            splitinfRHash = "";

            HashingAlgorithm();
            richTextBox2.Clear();
            key = label3.Text.ToCharArray();

            string array;
            char[] arrayreverse;

            string reversedblocks = "";
            string Rfunc = "";
            long r25 = 0;
            long r = 0;
            long s = 0;
            long l = key.Length;
            long n;
            long mod;
            int addindex = 0;
            int f = 0;
            int stop = 0;
            foreach (char i in ru) r += i;
            foreach (char i in key) s += i;
            foreach (char i in ru25) r25 += i;
            if (richTextBox1.Text.Length != 0)
            {
                //reverse columns
                while (true)
                {
                    array = "";
                    for (int i = 0; i <= 5; i++)
                    {
                        try
                        {
                            array += richTextBox1.Text[i + addindex];
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
                        reversedblocks += arrayreverse[i];
                    }
                    if (stop == 1)
                    {
                        break;
                    }
                    addindex += 6;
                }
                //The R sum function step
                for (int i = 0; i < reversedblocks.Length; i++)
                {
                    n = 0;
                    for (int j = 0; j < l; j++)
                    {
                        int x = key[j];
                        n += ((i + x) ^ (s + l) * (long)Math.Pow((Math.Pow(x, 3) % r), 2));
                        for (int p = 0; p < ru.Length; p++)
                        {
                            n ^= ru[p] * (i ^ (n % x + 25));
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
                    Rfunc += Convert.ToChar(reversedblocks[i] + mod);

                    //infRHash
                    splitinfRHash += (mod % 256).ToString("X2");
                    if (splitinfRHash.Length < reversedblocks.Length * 3 - 1)
                    {
                        splitinfRHash += "|";
                    }

                    //splitRHash
                    if (splitRHash.Length < 47)
                    {
                        splitRHash += (mod % 256).ToString("X2");
                        sRHash += (mod % 256).ToString("X2");
                        if (splitRHash.Length < 47)
                        {
                            splitRHash += "|";
                        }
                    }
                }
                splitinfRHash2 = splitinfRHash.Split('|');
                infRHash = splitinfRHash2.Select(b => Convert.ToByte(b, 16)).ToArray();
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
                splitRHash2 = splitRHash.Split('|');
                RHash = splitRHash2.Select(b => Convert.ToByte(b, 16)).ToArray();
                label4.Text = "RHash: " + sRHash + " len: " + (sRHash.Length * 8);

                richTextBox1.Text = Rfunc;
                richTextBox2.Text = splitinfRHash.ToString();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            label1.Text = "len: " + richTextBox1.Text.Length;
            if (checkBox1.Checked == true)
            {
                textBox2.Clear();
                foreach (char i in richTextBox1.Text)
                {
                    textBox2.AppendText((int)i + " ");
                }
            }
            else
            {
                textBox2.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sRHash = "";
            splitRHash = "";
            splitinfRHash = "";

            HashingAlgorithm();

            string array;
            string reversedblocks = "";
            string Rfunc = "";
            char[] arrayreverse;
            key = label3.Text.ToCharArray();
            long r = 0;
            long r25 = 0;
            long l = key.Length;
            long s = 0;
            long n;
            long mod;
            int addindex = 0;
            int stop = 0;
            foreach (char i in ru) r += i;
            foreach (char i in key) s += i;
            foreach (char i in ru25) r25 += i;
            try
            {
                //The R sum function step
                for (int i = 0; i < richTextBox1.Text.Length; i++)
                {
                    n = 0;
                    for (int j = 0; j < l; j++)
                    {
                        int x = key[j];
                        n += ((i + x) ^ (s + l) * (long)Math.Pow((Math.Pow(x, 3) % r), 2));
                        for (int p = 0; p < ru.Length; p++)
                        {
                            n ^= ru[p] * (i ^ (n % x + 25));
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
                    Rfunc += Convert.ToChar(richTextBox1.Text[i] - mod);

                    //infRHash
                    splitinfRHash += (mod % 256).ToString("X2");
                    if (splitinfRHash.Length < richTextBox1.Text.Length * 3 - 1)
                    {
                        splitinfRHash += "|";
                    }

                    //splitRHash
                    if (splitRHash.Length < 47)
                    {
                        splitRHash += (mod % 256).ToString("X2");
                        sRHash += (mod % 256).ToString("X2");
                        if (splitRHash.Length < 47)
                        {
                            splitRHash += "|";
                        }
                    }
                }
                splitinfRHash2 = splitinfRHash.Split('|');
                infRHash = splitinfRHash2.Select(b => Convert.ToByte(b, 16)).ToArray();
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
                splitRHash2 = splitRHash.Split('|');
                RHash = splitRHash2.Select(b => Convert.ToByte(b, 16)).ToArray();
                label4.Text = "RHash: " + sRHash + " len: " + (sRHash.Length * 8);
                //reverse columns
                while (true)
                {
                    array = "";
                    for (int i = 0; i <= 5; i++)
                    {
                        try
                        {
                            array += Rfunc[i + addindex];
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
                        reversedblocks += arrayreverse[i];
                    }
                    if (stop == 1)
                    {
                        break;
                    }
                    addindex += 6;
                }
                richTextBox1.Text = reversedblocks;
                richTextBox2.Text = splitinfRHash.ToString();
            }
            catch
            {
                MessageBox.Show("Invalid Key");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox2.Clear();
                foreach (char i in richTextBox1.Text)
                {
                    textBox2.AppendText((int)i + " ");
                }
            }
            else
            {
                textBox2.Clear();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label2.Text = "bits: " + textBox1.Text.Length * 8;
        }

        private void HashingAlgorithm()
        {
            sKHash = "";
            splitKHash = "";
            string key = textBox1.Text;
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
            string prehash = (A.ToString("X4") + B.ToString("X4") + C.ToString("X4") + D.ToString("X4") + E.ToString("X4") + F.ToString("X4") + G.ToString("X4") + H.ToString("X4"));
            char[] prehash_ = prehash.ToCharArray();
            Array.Reverse(prehash_);
            label3.Text = "";
            foreach(char i in prehash_)
            {
                sKHash += i.ToString();
            }
            label3.Text = "Hash: " + sKHash + " len: " + (sKHash.Length * 8);
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
            splitKHash2 = splitKHash.Split('|');
            KHash = splitKHash2.Select(b1 => Convert.ToByte(b1, 16)).ToArray();
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

        private void button4_Click(object sender, EventArgs e)
        {
            HashingAlgorithm();
        }
    }
}