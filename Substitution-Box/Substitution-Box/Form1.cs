using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Substitution_Box
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        byte[] cipher;
        private void button1_Click(object sender, EventArgs e)
        {
            byte[] SBOX = "36 D1 DB 1D 5B D0 CE DF BD 84 5D 2A EE A5 22 04 BC E1 54 19 0B A7 F5 EF 40 E4 43 2B 53 D7 3F DA 48 FB 11 80 2C EA A3 34 E7 B6 83 9A 12 60 73 FA 29 E5 2E 1A EB 8A B4 69 AC 4F A6 FF 26 F3 6E 8F F7 B1 24 F1 72 55 90 18 F2 F6 CB 32 4D 7A D2 52 7E 0A 65 4A 6B 21 1F 7C 15 06 38 D9 5A 8E 9B 76 51 58 C8 1C 7D 98 64 0C C3 3C F8 00 A4 74 99 B7 78 07 13 2D CF CC 57 D5 D4 67 28 1E 02 E0 20 10 BA 56 0E 7F 9E 93 DD B0 6C 0D A1 DE 86 14 AA FD 41 91 4C BF 46 30 C1 BE 6F F0 B2 4B 42 01 FC F9 59 CD 3D E6 AD 85 45 FE C2 9F 39 63 16 F4 B3 88 5E 05 AB 9C A9 E8 3E D3 C7 79 5F 82 97 D8 50 6A 7B 94 CA E3 08 66 6D 31 DC 03 75 C5 49 EC 1B 17 47 AE A0 8B C6 92 B8 9D 2F 87 25 8C 62 B5 B9 3B 0F 8D 71 89 3A 61 4E 33 E9 23 95 27 E2 A2 A8 C4 77 44 81 70 35 68 96 BB ED 09 C9 37 D6 AF 5C C0".Split(' ').Select(b1 => Convert.ToByte(b1, 16)).ToArray();
            cipher = new byte[textBox1.Text.Length];
            for (int i = 0; i < textBox1.Text.Length; i++)
            {
                cipher[i] += SBOX[textBox1.Text[i]];
            }
            textBox1.Text = Convert.ToBase64String(cipher);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] IBOX = "6B 9D 7C C9 0F B1 59 71 C4 F9 51 14 67 89 82 E0 7F 22 2C 72 8D 58 AC CF 47 13 33 CE 63 03 7B 56 7E 55 0E E9 42 DA 3C EB 7A 30 0B 1B 24 73 32 D8 95 C7 4B E7 27 F4 00 FB 5A AA E4 DF 69 A2 B6 1E 18 90 9C 1A F1 A6 94 D0 20 CC 53 9B 92 4C E6 39 BE 60 4F 1C 12 45 81 76 61 A0 5C 04 FE 0A B0 BA 2D E5 DC AB 66 52 C5 79 F5 37 BF 54 88 C6 3E 98 F3 E2 44 2E 6D CA 5F F0 70 B9 4D C0 57 64 50 83 23 F2 BB 2A 09 A5 8C D9 AF E3 35 D3 DB E1 5D 3F 46 91 D5 85 C1 EA F6 BC 65 6E 2B 5E B3 D7 84 A9 D2 8A ED 26 6C 0D 3A 15 EE B4 8E B2 38 A4 D1 FD 87 41 9A AE 36 DD 29 6F D6 DE 80 F7 10 08 97 93 FF 96 A8 68 EF CB D4 B8 62 FA C2 4A 75 A1 06 74 05 01 4E B7 78 77 FC 1D BD 5B 1F 02 C8 86 8B 07 7D 11 EC C3 19 31 A3 28 B5 E8 25 34 CD F8 0C 17 99 43 48 3D AD 16 49 40 6A 9F 2F 21 9E 8F A7 3B".Split(' ').Select(b1 => Convert.ToByte(b1, 16)).ToArray();
            cipher = Convert.FromBase64String(textBox1.Text);
            string decipher = "";
            for (int i = 0; i < cipher.Length; i++)
            {
                decipher += (char)IBOX[cipher[i]];
            }
            textBox1.Text = decipher;
        }
    }
}
