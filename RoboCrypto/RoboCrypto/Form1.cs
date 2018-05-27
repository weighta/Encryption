using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace RoboCrypto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private int charactersprocessed = 0;
        private int bitspercolor;
        private string path;
        private byte[] bitmap;
        Random ran = new Random();
        private void button1_Click(object sender, EventArgs e)
        {
            charactersprocessed += textBox1.Text.Length;
            if (radioButton1.Checked == true)
            {
                int ii = 0;
                int rantoadd = 0;
                try
                {
                    string CryptedText = "";
                    foreach (char i in textBox1.Text)
                    {
                        ii = (int)(i);
                        rantoadd = ran.Next(100, 5000);
                        CryptedText += (char)(i + rantoadd);
                        CryptedText += (char)(rantoadd);
                    }
                    textBox1.Text = CryptedText;
                }
                catch
                {
                    MessageBox.Show("Out of memory at 0x" + ii.ToString("X") + " + " + rantoadd.ToString("X") + " !<= 0xFFFF");
                }
            }
            else if (radioButton2.Checked == true)
            {
                int rantoadd = 0;
                int ii = 0;
                try
                {
                    string CryptedText = "";
                    rantoadd = ran.Next(100, 5000);
                    CryptedText += (char)(rantoadd);
                    foreach (char i in textBox1.Text)
                    {
                        ii = i;
                        CryptedText += (char)((i) + rantoadd);
                        rantoadd++;
                    }
                    textBox1.Text = CryptedText + (char)425;
                }
                catch
                {
                    MessageBox.Show("Out of memory at 0x" + rantoadd.ToString("X") + " + " + ii.ToString("X") + " !<= 0xFFFF");
                }
            }
            else if (radioButton3.Checked == true)
            {
                int ii = 0;
                int rantoadd = 0;
                try
                {
                    string CryptedText = "";
                    foreach (char i in textBox1.Text)
                    {
                        ii = (int)(i);
                        rantoadd = ran.Next(100, 5000);
                        CryptedText += (char)(rantoadd);
                        CryptedText += (char)(i + rantoadd);
                    }
                    textBox1.Text = CryptedText;
                }
                catch
                {
                    MessageBox.Show("Out of memory at 0x" + ii.ToString("X") + " + " + rantoadd.ToString("X") + " !<= 0xFFFF");
                }
            }
            else if (radioButton4.Checked == true)
            {
                int ii = 0;
                int adder = 0;
                try
                {
                    int amounttoAdd = 95;
                    int ranMultiply = ran.Next(170, 180);
                    string Encryption = "";
                    Encryption += (char)(ranMultiply);
                    foreach (char i in textBox1.Text)
                    {
                        ii = (int)i;
                        adder = ranMultiply * 4 + amounttoAdd;
                        Encryption += (char)(i + (ranMultiply * 4) + amounttoAdd);
                        amounttoAdd++;
                    }
                    textBox1.Text = Encryption + "'";
                }
                catch
                {
                    MessageBox.Show("Out of memory at " + ii.ToString("X") + " + " + adder.ToString("X"));
                }
            }
            else if (radioButton5.Checked == true)
            {
                int nondoublelength = 0;
                int ii = 0;
                try
                {
                    int length = ran.Next(15, 75);
                    
                    string Encryption = "";
                    string Decryption = "";
                    int LengthDecrpyt = length;
                    nondoublelength = length;
                    foreach (char i in textBox1.Text)
                    {
                        ii = (int)i;
                        int ranadder = ran.Next(100, 500);
                        Encryption += (char)((int)i + ranadder + nondoublelength);
                        Decryption += (char)(ranadder);
                        nondoublelength += length;
                    }
                    Encryption.ToCharArray();
                    Encryption.Reverse();
                    Decryption.ToCharArray();
                    Decryption.Reverse();
                    textBox1.Text = "0" + (char)(LengthDecrpyt) + Encryption + Decryption;
                }
                catch
                {
                    MessageBox.Show("Out of memory at " + ii.ToString("X") + " + " + nondoublelength.ToString("X"));
                }
            }
            if (radioButton6.Checked == true)
            {
                int progress = 0;
                int c;
                string precipher = "";
                string XOR = "Ruthie";
                string XOR2 = "";
                XOR2.ToCharArray();
                int XORindex = 0;
                int XOR2index = 0;
                //Block Cipher Step
                foreach (char i in textBox1.Text)
                {
                    c = (int)i + 106;
                    if (progress < 6)
                    {
                        precipher += Convert.ToChar(c ^ (int)XOR[XORindex]);
                        XOR2 += Convert.ToChar(c ^ (int)XOR[XORindex]);
                        XORindex++;
                        progress++;
                        if (progress == 6)
                        {
                            XORindex = 0;
                            XOR = "";
                            progress++;
                        }
                    }
                    else if (progress >= 7)
                    {
                        precipher += Convert.ToChar(c ^ (int)XOR2[XOR2index]); //Tends to fail (char > 60000)
                        XOR += Convert.ToChar(c ^ (int)XOR2[XOR2index]);
                        XOR2index++;
                        progress++;
                        if (progress >= 13)
                        {
                            XOR2index = 0;
                            XOR2 = "";
                            progress = 0;
                        }
                    }
                }
                string cipher = "";
                int XOR3 = 625;
                //Block Cipher 2 Step
                foreach (char i in precipher)
                {
                    c = i + 1;
                    cipher += Convert.ToChar((c ^ XOR3));
                    XOR3 = c ^ XOR3;
                }
                progress = 0;
                string reversedblocks = "";
                string block = "";
                char[] blockarray;
                string leftover = "";
                char[] leftoverarray;
                //Reverse Blocks Step
                foreach (char i in cipher)
                {
                    c = i;
                    leftover = "";
                    block += Convert.ToChar(c);
                    blockarray = block.ToCharArray();
                    leftoverarray = block.ToCharArray();
                    progress++;
                    if (progress >= 6)
                    {
                        Array.Reverse(blockarray);
                        for (int j = 0; j < blockarray.Length; j++)
                        {
                            reversedblocks += blockarray[j];
                        }
                        block = "";
                        progress = 0;
                    }
                    else if (progress <= 6)
                    {
                        Array.Reverse(leftoverarray);
                        for (int j = 0; j < leftoverarray.Length; j++)
                        {
                            leftover += leftoverarray[j];
                        }
                    }
                }
                string cipher2 = "";
                string reversedblocksoutput = reversedblocks + leftover;
                reversedblocksoutput.ToCharArray();
                try
                {
                    int cipher2char = reversedblocksoutput[0] - (reversedblocksoutput.Length ^ (reversedblocksoutput.Length - 1));
                    cipher2 += Convert.ToChar(cipher2char);
                    //MessageBox.Show(cipher2);
                }
                catch
                {

                }
                for (int i = 1; i < reversedblocksoutput.Length; i++)
                {
                    cipher2 += reversedblocksoutput[i];
                }
                textBox1.Text = cipher2;
            }
            else if (radioButton9.Checked == true)
            {
                byte[] SBOX = "36 D1 DB 1D 5B D0 CE DF BD 84 5D 2A EE A5 22 04 BC E1 54 19 0B A7 F5 EF 40 E4 43 2B 53 D7 3F DA 48 FB 11 80 2C EA A3 34 E7 B6 83 9A 12 60 73 FA 29 E5 2E 1A EB 8A B4 69 AC 4F A6 FF 26 F3 6E 8F F7 B1 24 F1 72 55 90 18 F2 F6 CB 32 4D 7A D2 52 7E 0A 65 4A 6B 21 1F 7C 15 06 38 D9 5A 8E 9B 76 51 58 C8 1C 7D 98 64 0C C3 3C F8 00 A4 74 99 B7 78 07 13 2D CF CC 57 D5 D4 67 28 1E 02 E0 20 10 BA 56 0E 7F 9E 93 DD B0 6C 0D A1 DE 86 14 AA FD 41 91 4C BF 46 30 C1 BE 6F F0 B2 4B 42 01 FC F9 59 CD 3D E6 AD 85 45 FE C2 9F 39 63 16 F4 B3 88 5E 05 AB 9C A9 E8 3E D3 C7 79 5F 82 97 D8 50 6A 7B 94 CA E3 08 66 6D 31 DC 03 75 C5 49 EC 1B 17 47 AE A0 8B C6 92 B8 9D 2F 87 25 8C 62 B5 B9 3B 0F 8D 71 89 3A 61 4E 33 E9 23 95 27 E2 A2 A8 C4 77 44 81 70 35 68 96 BB ED 09 C9 37 D6 AF 5C C0".Split(' ').Select(b1 => Convert.ToByte(b1, 16)).ToArray();
                int len = textBox1.Text.Length;

                byte[] ciphertext = new byte[textBox1.Text.Length + (64 - (textBox1.Text.Length % 64))];
                for (int i = 0; i < textBox1.Text.Length; i++) ciphertext[i] = (byte)textBox1.Text[i];
                textBox1.Text = Convert.ToBase64String(ciphertext);
                MessageBox.Show(ciphertext.Length.ToString());
                for (int i = 0; i < ciphertext.Length; i++)
                {
                    textBox1.Text += (char)ciphertext[i];
                }
                /*
                //substitutionbox
                string sbytes = "";
                for (int i = 0; i < textBox1.Text.Length; i++)
                {
                    sbytes += Convert.ToChar(SBOX[textBox1.Text[i]]);
                }
                decimal b = 0;
                //integrate
                string cipher = "";
                for (int i = 0; i < len; i++)
                {
                    int xor;
                    int j = i + 1;
                    b += j * (decimal)Math.Abs(((Math.Sin(25 * j) / j) - (Math.Sin(-25 * j) / j)) * (j - (Math.Cos(2 * j) / 2)) - (j - (Math.Cos(2 * -Math.Pow(j, 0.33333333333333)) / 2)));
                    cipher += Convert.ToChar(sbytes[i] ^ (int)(Math.Ceiling(b) % 25));
                }
                //checksum
                b = (decimal)((((len + 3) - Math.Cos(len + 3)) - (len - Math.Cos(len))) * Math.Pow(10, 2));
                string cipher2 = "";
                for (int i = 0; i < len; i++)
                {
                    cipher2 += Convert.ToChar(cipher[i] ^ ((((int)Math.Round(b) % (i ^ len)) % 25)) + 1);
                }
                richTextBox3.AppendText("Plaintext: " + textBox1.Text + Environment.NewLine + "Substituion: " + sbytes + Environment.NewLine + "Integration: " + cipher + Environment.NewLine + "Checksum: " + cipher2);
                textBox1.Text = cipher2;
                MessageBox.Show(cipher.ToString() + Environment.NewLine + cipher2.ToString());
                */
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Other
            #region
            charactersprocessed += textBox1.Text.Length;
            if (radioButton1.Checked == true)
            {
                try
                {
                    string DecryptedText = "";
                    int charindex = 0;

                    for (int i = 0; i < textBox1.Text.Length / 2; i++)
                    {
                        int cryptedchar = (int)((char)textBox1.Text[charindex]);
                        charindex++;
                        int decryptchar = (int)((char)textBox1.Text[charindex]);
                        charindex++;
                        DecryptedText += Convert.ToChar((cryptedchar - decryptchar));
                    }
                    textBox1.Text = DecryptedText;
                }
                catch
                {

                }
            }
            else if (radioButton2.Checked == true)
            {
                try
                {
                    string DecryptedText = "";
                    textBox1.Text.ToCharArray();
                    if ((int)textBox1.Text[textBox1.Text.Length - 1] == 425)
                    {
                        int arrayposition = 0;
                        int subtractor = (int)textBox1.Text[arrayposition];
                        int subractortwo = 0;
                        for (int i = 0; i < textBox1.Text.Length - 2; i++)
                        {
                            arrayposition++;
                            DecryptedText += (char)((int)textBox1.Text[arrayposition] - subtractor - subractortwo);
                            subractortwo += 1;
                        }
                        textBox1.Text = DecryptedText;
                    }
                }
                catch
                {

                }
            }
            else if (radioButton3.Checked == true)
            {
                try
                {
                    string DecryptedText = "";
                    int charindex = 0;

                    for (int i = 0; i < textBox1.Text.Length / 2; i++)
                    {
                        int decryptchar = (int)((char)textBox1.Text[charindex]);
                        charindex++;
                        int cryptedchar = (int)((char)textBox1.Text[charindex]);
                        charindex++;
                        DecryptedText += Convert.ToChar((cryptedchar - decryptchar));
                    }
                    textBox1.Text = DecryptedText;
                }
                catch
                {
                    //ਿਜ਼ਜ਼ହກၚܺ૏ቀ᜚፷ᲂຊṩ܄ᯟሕ⮄šᾌক⟕׏␘݉▗຋Ⲭ౱⪐໔⳼ತ⬝ߧ♩ળ⤾ʬ⃷೧⮁ǲ₉sἐȻ⃮߶⚷ƞ⁡ڕ┟ᆔぜߺ⛗ྥ⺉ጦ㇕ၶ⽥ൣⱤȽ⅊ಷ⯐म⡈ְⓙ࠹✧έ⋨ቇㆀಎ⯗ਜ਼⦵֡━Ϧ⍃ý⁭ᆌヸر▢ຏ⸌ጶ㊆˽⊘෍⵩౔ⰁഥⲪᅵㄴ޿❔ࢱ⡃ੋ⧧಩ⱓዋ㉿ᄅズെⴌၾおኒ㉕˔⋭ǃ⇤ದⳔ௳Ⱛհ╮ȸ≀Χ⎽ই⦧ӏ⓰ҝⓏ௴Ⱜ
                }
            }
            else if (radioButton4.Checked == true)
            {
                try
                {
                    textBox1.Text.ToCharArray();
                    if ((int)textBox1.Text[textBox1.Text.Length - 1] == 39)
                    {
                        int index = 0;
                        int ninetyfive = 95;
                        int ThisTimesFour = (int)textBox1.Text[index];
                        string Decryption = "";
                        index++;
                        for (int i = 0; i < textBox1.Text.Length - 2; i++)
                        {
                            Decryption += (char)((int)(textBox1.Text[index]) - (ThisTimesFour * 4) - ninetyfive);
                            ninetyfive++;
                            index++;
                        }
                        textBox1.Text = Decryption;
                    }
                }
                catch
                {

                }
            }
            else if (radioButton5.Checked == true)
            {
                try
                {
                    string Decryption = "";
                    textBox1.Text.ToCharArray();
                    if (textBox1.Text[0].ToString() == "0")
                    {
                        int Decryptcharindex = textBox1.Text.Length - 1;
                        int Encryptcharindex = textBox1.Text.Length / 2;
                        int Decryptsubtract = (int)textBox1.Text[1];
                        int NumofCryptedChars = textBox1.Text.Length / 2 - 1;
                        for (int i = 0; i < textBox1.Text.Length / 2 - 1; i++)
                        {
                            Decryption += (char)((int)textBox1.Text[Encryptcharindex] - (int)textBox1.Text[Decryptcharindex] - (NumofCryptedChars * Decryptsubtract));
                            Decryptcharindex--;
                            Encryptcharindex--;
                            NumofCryptedChars--;
                        }
                        char[] inputarray = Decryption.ToCharArray();
                        Array.Reverse(inputarray);
                        textBox1.Text = new string(inputarray);
                    }
                    else
                    {

                    }
                }
                catch
                {

                }
            }
            #endregion

            else if (radioButton6.Checked == true)
            {
                int c;
                int progress = 0;
                string lenXorDecrpytion = "";
                string cipher2 = textBox1.Text;
                cipher2.ToCharArray();
                try
                {
                    int cipher2char = cipher2[0] + (cipher2.Length ^ (cipher2.Length - 1));
                    lenXorDecrpytion += Convert.ToChar(cipher2char);
                }
                catch
                {

                }
                for (int i = 1; i < cipher2.Length; i++)
                {
                    lenXorDecrpytion += cipher2[i];
                }
                string reverseblockdecryption;
                string reversedblocks = "";
                string block = "";
                char[] blockarray;
                string leftover = "";
                char[] leftoverarray;
                //Reverse Blocks Step
                foreach (char i in lenXorDecrpytion)
                {
                    c = (int)i;
                    leftover = "";
                    block += Convert.ToChar(c);
                    blockarray = block.ToCharArray();
                    leftoverarray = block.ToCharArray();
                    progress++;
                    if (progress >= 6)
                    {
                        Array.Reverse(blockarray);
                        for (int j = 0; j < blockarray.Length; j++)
                        {
                            reversedblocks += blockarray[j];
                        }
                        block = "";
                        progress = 0;
                    }
                    else if (progress <= 6)
                    {
                        Array.Reverse(leftoverarray);
                        for (int j = 0; j < leftoverarray.Length; j++)
                        {
                            leftover += leftoverarray[j];
                        }
                    }
                }
                reverseblockdecryption = reversedblocks + leftover;
                string predecryption = "";
                int XOR3 = 625;
                foreach (char i in reverseblockdecryption)
                {
                    c = (int)i;
                    predecryption += Convert.ToChar((c ^ XOR3) - 1);
                    XOR3 = c;
                }
                string decryption = "";
                string XOR = "Ruthie";
                XOR.ToCharArray();
                string XOR2 = "";
                XOR2.ToCharArray();
                int XORindex = 0;
                int XOR2index = 0;
                progress = 0;
                foreach (char i in predecryption)
                {
                    c = (int)i;
                    if (progress < 6)
                    {
                        decryption += Convert.ToChar((c ^ (int)XOR[XORindex]) - 106); //Tends to fail (char > 60000)
                        XOR2 += Convert.ToChar(c);
                        XORindex++;
                        progress++;
                        if (progress == 6)
                        {
                            XORindex = 0;
                            XOR = "";
                            progress++;
                        }
                    }
                    else if (progress >= 7)
                    {
                        decryption += Convert.ToChar((c ^ (int)XOR2[XOR2index]) - 106);
                        XOR += Convert.ToChar(c);
                        XOR2index++;
                        progress++;
                        if (progress >= 13)
                        {
                            XOR2index = 0;
                            XOR2 = "";
                            progress = 0;
                        }
                    }
                }
                textBox1.Text = decryption;
                label1.Text = "Length: " + textBox1.Text.Length.ToString() + " Chars: " + charactersprocessed;
            }
            if (radioButton9.Checked == true)
            {
                int len = textBox1.Text.Length;
                decimal b = 0;
                string cipher = "";
                for (int i = 0; i < len; i++)
                {
                    int xor;
                    int j = i + 1;
                    b += j * (decimal)Math.Abs(((Math.Sin(25 * j) / j) - (Math.Sin(-25 * j) / j)) * (j - (Math.Cos(2 * j) / 2)) - (j - (Math.Cos(2 * -Math.Pow(j, 0.33333333333333)) / 2)));
                    cipher += Convert.ToChar(textBox1.Text[i] ^ (int)(Math.Ceiling(b) % 25));
                }
                byte[] IBOX = "6B 9D 7C C9 0F B1 59 71 C4 F9 51 14 67 89 82 E0 7F 22 2C 72 8D 58 AC CF 47 13 33 CE 63 03 7B 56 7E 55 0E E9 42 DA 3C EB 7A 30 0B 1B 24 73 32 D8 95 C7 4B E7 27 F4 00 FB 5A AA E4 DF 69 A2 B6 1E 18 90 9C 1A F1 A6 94 D0 20 CC 53 9B 92 4C E6 39 BE 60 4F 1C 12 45 81 76 61 A0 5C 04 FE 0A B0 BA 2D E5 DC AB 66 52 C5 79 F5 37 BF 54 88 C6 3E 98 F3 E2 44 2E 6D CA 5F F0 70 B9 4D C0 57 64 50 83 23 F2 BB 2A 09 A5 8C D9 AF E3 35 D3 DB E1 5D 3F 46 91 D5 85 C1 EA F6 BC 65 6E 2B 5E B3 D7 84 A9 D2 8A ED 26 6C 0D 3A 15 EE B4 8E B2 38 A4 D1 FD 87 41 9A AE 36 DD 29 6F D6 DE 80 F7 10 08 97 93 FF 96 A8 68 EF CB D4 B8 62 FA C2 4A 75 A1 06 74 05 01 4E B7 78 77 FC 1D BD 5B 1F 02 C8 86 8B 07 7D 11 EC C3 19 31 A3 28 B5 E8 25 34 CD F8 0C 17 99 43 48 3D AD 16 49 40 6A 9F 2F 21 9E 8F A7 3B".Split(' ').Select(b1 => Convert.ToByte(b1, 16)).ToArray();
                string invsbytes = "";
                for (int i = 0; i < len; i++)
                {
                    invsbytes += Convert.ToChar(IBOX[cipher[i]]);
                }
                textBox1.Text = invsbytes;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = textBox1.Text;
            string ints = "";
            foreach (char i in textBox1.Text)
            {
                ints += (int)i + " ";
            }
            textBox1.Text = ints;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox7.Clear();
            string[] ints = textBox4.Text.Split(' ');
            foreach(string i in ints)
            {
                try
                {
                    textBox7.AppendText(Convert.ToChar(Convert.ToInt32(i)).ToString());
                }
                catch
                {

                }
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            textBox2.Text = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label1.Text = "Length: " + textBox1.Text.Length.ToString();
            string ints = "";
            int addition = 0;
            string hex = "";
            foreach (char j in textBox2.Text)
            {
                ints += (int)j + " ";
                addition += j;
            }
            foreach (char i in textBox2.Text)
            {
                string zeros = "";
                string potential = ((int)i).ToString("X");
                
                if (potential.Length <= 4)
                {
                    for(int j = 0; j < 4 - potential.Length; j++)
                    {
                        zeros += "0";
                    }
                    if (radioButton7.Checked == false)
                    {
                        hex += potential + zeros + " ";
                    }
                    else
                    {
                        hex += zeros + potential + " ";
                    }
                }
            }
            richTextBox4.Text = hex;
            try
            {
                richTextBox4.Text = richTextBox4.Text.Substring(0, richTextBox4.Text.Length - 1);
            }
            catch
            {

            }
            textBox6.Text = ints + " sum: " + addition.ToString();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string label = "";
                foreach (char i in textBox5.Text)
                {
                    label += ((int)i).ToString() + " ";
                }
                textBox8.Text = "Int: " + label;
            }
            catch
            {
                textBox8.Text = "Error";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Thread crack = new Thread(new ThreadStart(Crack));
            crack.Start();
        }
        private void Crack()
        {
            string Current = "";
            int CharactersAnalyzed = 1;
            int subtractor = 1;
            int determinsubtractor = 1;
            float numerator = 1;
            try
            {
                while (true)
                {
                    if (determinsubtractor == subtractor)
                    {
                        Invoke(new Action(() => richTextBox1.Text = ""));
                        determinsubtractor += 500;
                    }
                    string decrypted = "";
                    foreach (char i in textBox2.Text)
                    {
                        decrypted += Convert.ToChar(i - subtractor);
                        CharactersAnalyzed++;
                    }
                    Current = decrypted;
                    Invoke(new Action(() => richTextBox1.AppendText(decrypted + " -" + subtractor + Environment.NewLine)));
                    Invoke(new Action(() => progressBar1.Value = (int)((numerator / 65535f) * 100)));
                    //Combinations: Subracting: Current:
                    Invoke(new Action(() => label4.Text = ("BoxLen: " + richTextBox1.Text.Length + " Characters Analyzed: " + CharactersAnalyzed.ToString() + " Subracting: " + subtractor.ToString() + " Current: " + Current.ToString() + " (done)" + Environment.NewLine)));
                    subtractor++;
                    numerator++;
                }
            }
            catch
            {
                Invoke(new Action(() => progressBar1.Value = 0));
                Current = "";
                CharactersAnalyzed = 1;
                subtractor = 1;
                determinsubtractor = 1;
                MessageBox.Show("Done");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string text = "";
                string[] bytes = richTextBox4.Text.Split(' ');
                foreach (string i in bytes)
                {
                    text += Convert.ToChar(Convert.ToInt32(i, 16)).ToString();
                }
            }
            catch
            {

            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            try
            {
                ofd.ShowDialog();
                path = ofd.FileName;
                string header = "";
                BinaryReader br = new BinaryReader(File.OpenRead(ofd.FileName));
                for(int i = 0x0; i <= 0x1; i++)
                {
                    br.BaseStream.Position = i;
                    header += br.ReadChar().ToString();
                }
                if (header == "BM")
                {
                    button6.Enabled = true;
                    br.BaseStream.Position = 0x1C;
                    bitspercolor = Convert.ToInt32(br.ReadByte());
                    bitmap = File.ReadAllBytes(ofd.FileName);
                }
                else
                {
                    button6.Enabled = false;
                    MessageBox.Show("This is not a bitmap");
                }
                br.Dispose();
            }
            catch
            {
                MessageBox.Show("You closed the filestream");
            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            string hex = "";
            foreach (char i in textBox2.Text)
            {
                string zeros = "";
                string potential = ((int)i).ToString("X");
                if (potential.Length <= 4)
                {
                    for (int j = 0; j < 4 - potential.Length; j++)
                    {
                        zeros += "0";
                    }
                    hex += zeros + potential + " ";
                }
            }
            richTextBox4.Text = hex;
            try
            {
                richTextBox4.Text = richTextBox4.Text.Substring(0, richTextBox4.Text.Length - 1);
            }
            catch
            {

            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            string hex = "";
            foreach (char i in textBox2.Text)
            {
                string zeros = "";
                string potential = ((int)i).ToString("X");
                if (potential.Length <= 4)
                {
                    for (int j = 0; j < 4 - potential.Length; j++)
                    {
                        zeros += "0";
                    }
                    hex += potential + zeros + " ";
                }
            }
            richTextBox4.Text = hex;
            try
            {
                richTextBox4.Text = richTextBox4.Text.Substring(0, richTextBox4.Text.Length - 1);
            }
            catch
            {

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(path));
            int position = 0x36;
            string[] CryptedBytes = richTextBox4.Text.Split(' ');
            foreach (string i in CryptedBytes)
            {
                bw.BaseStream.Position = position;
                byte[] color = BitConverter.GetBytes(Convert.ToInt32(i, 16));
                if(radioButton7.Checked == true)
                {
                    Array.Reverse(color);
                    bw.Write(color[2]);
                    bw.BaseStream.Position = position + 1;
                    bw.Write(color[3]);
                }
                else
                {
                    bw.Write(color[0]);
                    bw.BaseStream.Position = position + 1;
                    bw.Write(color[1]);
                }
                position += bitspercolor / 8;
            }
            bw.Dispose();
        }
        private void ModularReduction()
        {
            string encryption = "";
            char[] chars = textBox1.Text.ToCharArray();
            int index = 0;
            int b = 82;

            int p;
            int counter;
            int hi_bit_set;
            for (int i = 0; i < chars.Length; i++)
            {
                p = 0;
                int a = chars[index];
                for (counter = 0; counter < 8; counter++)
                {
                    if ((b & 1) != 0)
                    {
                        p ^= a;
                    }
                    hi_bit_set = (Byte)(a & 0x80);
                    a <<= 1;
                    if (hi_bit_set != 0)
                    {
                        a ^= 0x1B; /* x^8 + x^4 + x^3 + x + 1 */
                    }
                    b >>= 1;
                    MessageBox.Show(p.ToString());
                }
                index++;
                encryption += Convert.ToChar(p);
                counter = 0;
            }
            textBox1.Text = encryption;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ///BasketBall Scores
            //R64 R32 S16 E8 F4  TOTAL PCT
            //250 200 160 80 160 850   75.1

            ///Jesus
            //J  E  S  U  S  <  3
            //74 69 83 85 83 60 51

            ///Username
            //b  a  s   k   e   t   b  a  l   l   r   u   2  5
            //98 97 115 107 101 116 98 97 108 108 114 117 50 53

            //Ruthie<3
            //R  u   t   h   i   e
            //82 117 116 104 105 101
            /*
            richTextBox3.Clear();
            for (int len = 1; len <= 1000; len++)
            {
                long x = len;
                long y = x ^ 74 + x ^ 69 + x ^ 83 + x ^ 85 + x ^ 83 + x ^ 60 + x ^ 51; //JESUS<3
                long z = y ^ 250 + y ^ 200 + y ^ 160 + y ^ 80 + y ^ 160 + y ^ 850 + y ^ 75; //R64 R32 S16 E8 F4  TOTAL PCT
                long _z = z ^ 98 + z ^ 97 + z ^ 115 + z ^ 107 + z ^ 101 + z ^ 116 + z ^ 98 + z ^ 97 + z ^ 108 + z ^ 108 + z ^ 114 + z ^ 117 + z ^ 50 + z ^ 53; //basketballru25
                long polynomial = _z ^ 82 + _z ^ 117 + _z ^ 116 + _z ^ 104 + _z ^ 105 + _z ^ 101; //Ruthie
                richTextBox3.AppendText("Length: " + len + " Polynomial: " + _z + " Polynomial2: " + polynomial + " ");
                richTextBox3.AppendText("Output: " + ((polynomial) % (625)).ToString() + Environment.NewLine);
                */
            ///Search Function:
            /*

        string searchin = "uyhcgnoyruaci:ZSgunziohgunszyhguruthieonsauighnasdihfnfjaiowecgh asuicnfyasibfjasghfniAkfdkfjsdkjfjossefasdfruthieasdfasdfong z = y ^ 250 + y ^ 200 + y ^ 160 + y ^ 80 + y ^ 160 + y ^ 850 + y ^ ruthie75; //R64 R32 S16 E8 F4  TOTAL PCT ";
        string word = "ruthie";
        for (int i = 0; i < searchin.Length - (word.Length - 1); i++)
        {
            string build = "";
            for (int k = i; k < word.Length + i; k++)
            {
                build += searchin[k];
            }
            if (build == word)
            {
                MessageBox.Show("Found '" + build + "' at: 0x" + i.ToString("X2"));
            }
        }
        */
            byte[] hi = { 3, 4 };

            Directory.CreateDirectory("C:\\Users\\Alex Weight\\AppData\\Local\\GeometryDash\\ey\\hi");
            File.WriteAllBytes("C:\\Users\\Alex Weight\\AppData\\Local\\GeometryDash\\ey\\eyy.rda", hi);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string decryption = "";
            for (int i = 0; i < textBox1.Text.Length; i++)
            {
                decryption += Convert.ToChar(textBox1.Text[i] ^ Convert.ToInt32(textBox3.Text));
            }
            richTextBox1.Text = decryption;
        }
    }
}
