using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaV2
{
    class rdamodule
    {
        #region rConstants
        private byte[] r = { 0x52, 0x75, 0x74, 0x68, 0x69, 0x65 };
        #endregion
        #region bConstants
        private byte[] r25 = { 0x62, 0x61, 0x73, 0x6B, 0x65, 0x74, 0x62, 0x61, 0x6C, 0x6C, 0x72, 0x75, 0x32, 0x35 };
        #endregion
        #region SBOX
        private byte[] SBox =
        {
            0x36, 0xD1, 0xDB, 0x1D, 0x5B, 0xD0, 0xCE, 0xDF, 0xBD, 0x84, 0x5D, 0x2A, 0xEE, 0xA5, 0x22, 0x04,
            0xBC, 0xE1, 0x54, 0x19, 0x0B, 0xA7, 0xF5, 0xEF, 0x40, 0xE4, 0x43, 0x2B, 0x53, 0xD7, 0x3F, 0xDA,
            0x48, 0xFB, 0x11, 0x80, 0x2C, 0xEA, 0xA3, 0x34, 0xE7, 0xB6, 0x83, 0x9A, 0x12, 0x60, 0x73, 0xFA,
            0x29, 0xE5, 0x2E, 0x1A, 0xEB, 0x8A, 0xB4, 0x69, 0xAC, 0x4F, 0xA6, 0xFF, 0x26, 0xF3, 0x6E, 0x8F,
            0xF7, 0xB1, 0x24, 0xF1, 0x72, 0x55, 0x90, 0x18, 0xF2, 0xF6, 0xCB, 0x32, 0x4D, 0x7A, 0xD2, 0x52,
            0x7E, 0x0A, 0x65, 0x4A, 0x6B, 0x21, 0x1F, 0x7C, 0x15, 0x06, 0x38, 0xD9, 0x5A, 0x8E, 0x9B, 0x76,
            0x51, 0x58, 0xC8, 0x1C, 0x7D, 0x98, 0x64, 0x0C, 0xC3, 0x3C, 0xF8, 0x00, 0xA4, 0x74, 0x99, 0xB7,
            0x78, 0x07, 0x13, 0x2D, 0xCF, 0xCC, 0x57, 0xD5, 0xD4, 0x67, 0x28, 0x1E, 0x02, 0xE0, 0x20, 0x10,
            0xBA, 0x56, 0x0E, 0x7F, 0x9E, 0x93, 0xDD, 0xB0, 0x6C, 0x0D, 0xA1, 0xDE, 0x86, 0x14, 0xAA, 0xFD,
            0x41, 0x91, 0x4C, 0xBF, 0x46, 0x30, 0xC1, 0xBE, 0x6F, 0xF0, 0xB2, 0x4B, 0x42, 0x01, 0xFC, 0xF9,
            0x59, 0xCD, 0x3D, 0xE6, 0xAD, 0x85, 0x45, 0xFE, 0xC2, 0x9F, 0x39, 0x63, 0x16, 0xF4, 0xB3, 0x88,
            0x5E, 0x05, 0xAB, 0x9C, 0xA9, 0xE8, 0x3E, 0xD3, 0xC7, 0x79, 0x5F, 0x82, 0x97, 0xD8, 0x50, 0x6A,
            0x7B, 0x94, 0xCA, 0xE3, 0x08, 0x66, 0x6D, 0x31, 0xDC, 0x03, 0x75, 0xC5, 0x49, 0xEC, 0x1B, 0x17,
            0x47, 0xAE, 0xA0, 0x8B, 0xC6, 0x92, 0xB8, 0x9D, 0x2F, 0x87, 0x25, 0x8C, 0x62, 0xB5, 0xB9, 0x3B,
            0x0F, 0x8D, 0x71, 0x89, 0x3A, 0x61, 0x4E, 0x33, 0xE9, 0x23, 0x95, 0x27, 0xE2, 0xA2, 0xA8, 0xC4,
            0x77, 0x44, 0x81, 0x70, 0x35, 0x68, 0x96, 0xBB, 0xED, 0x09, 0xC9, 0x37, 0xD6, 0xAF, 0x5C, 0xC0
        };
        #endregion
        #region IBOX
        private byte[] IBox =
        {
            0x6B, 0x9D, 0x7C, 0xC9, 0x0F, 0xB1, 0x59, 0x71, 0xC4, 0xF9, 0x51, 0x14, 0x67, 0x89, 0x82, 0xE0,
            0x7F, 0x22, 0x2C, 0x72, 0x8D, 0x58, 0xAC, 0xCF, 0x47, 0x13, 0x33, 0xCE, 0x63, 0x03, 0x7B, 0x56,
            0x7E, 0x55, 0x0E, 0xE9, 0x42, 0xDA, 0x3C, 0xEB, 0x7A, 0x30, 0x0B, 0x1B, 0x24, 0x73, 0x32, 0xD8,
            0x95, 0xC7, 0x4B, 0xE7, 0x27, 0xF4, 0x00, 0xFB, 0x5A, 0xAA, 0xE4, 0xDF, 0x69, 0xA2, 0xB6, 0x1E,
            0x18, 0x90, 0x9C, 0x1A, 0xF1, 0xA6, 0x94, 0xD0, 0x20, 0xCC, 0x53, 0x9B, 0x92, 0x4C, 0xE6, 0x39,
            0xBE, 0x60, 0x4F, 0x1C, 0x12, 0x45, 0x81, 0x76, 0x61, 0xA0, 0x5C, 0x04, 0xFE, 0x0A, 0xB0, 0xBA,
            0x2D, 0xE5, 0xDC, 0xAB, 0x66, 0x52, 0xC5, 0x79, 0xF5, 0x37, 0xBF, 0x54, 0x88, 0xC6, 0x3E, 0x98,
            0xF3, 0xE2, 0x44, 0x2E, 0x6D, 0xCA, 0x5F, 0xF0, 0x70, 0xB9, 0x4D, 0xC0, 0x57, 0x64, 0x50, 0x83,
            0x23, 0xF2, 0xBB, 0x2A, 0x09, 0xA5, 0x8C, 0xD9, 0xAF, 0xE3, 0x35, 0xD3, 0xDB, 0xE1, 0x5D, 0x3F,
            0x46, 0x91, 0xD5, 0x85, 0xC1, 0xEA, 0xF6, 0xBC, 0x65, 0x6E, 0x2B, 0x5E, 0xB3, 0xD7, 0x84, 0xA9,
            0xD2, 0x8A, 0xED, 0x26, 0x6C, 0x0D, 0x3A, 0x15, 0xEE, 0xB4, 0x8E, 0xB2, 0x38, 0xA4, 0xD1, 0xFD,
            0x87, 0x41, 0x9A, 0xAE, 0x36, 0xDD, 0x29, 0x6F, 0xD6, 0xDE, 0x80, 0xF7, 0x10, 0x08, 0x97, 0x93,
            0xFF, 0x96, 0xA8, 0x68, 0xEF, 0xCB, 0xD4, 0xB8, 0x62, 0xFA, 0xC2, 0x4A, 0x75, 0xA1, 0x06, 0x74,
            0x05, 0x01, 0x4E, 0xB7, 0x78, 0x77, 0xFC, 0x1D, 0xBD, 0x5B, 0x1F, 0x02, 0xC8, 0x86, 0x8B, 0x07,
            0x7D, 0x11, 0xEC, 0xC3, 0x19, 0x31, 0xA3, 0x28, 0xB5, 0xE8, 0x25, 0x34, 0xCD, 0xF8, 0x0C, 0x17,
            0x99, 0x43, 0x48, 0x3D, 0xAD, 0x16, 0x49, 0x40, 0x6A, 0x9F, 0x2F, 0x21, 0x9E, 0x8F, 0xA7, 0x3B,
        };
        #endregion
        #region MDS
        private byte[] MDS =
        {
            0x02, 0x03, 0x01, 0x01, 0x02, 0x03, 0x01, 0x01, 0x01, 0x02, 0x03, 0x01, 0x01, 0x02, 0x03, 0x01,
            0x01, 0x01, 0x02, 0x03, 0x01, 0x01, 0x02, 0x03, 0x01, 0x01, 0x01, 0x02, 0x03, 0x01, 0x01, 0x02,
            0x03, 0x01, 0x01, 0x01, 0x02, 0x03, 0x01, 0x01, 0x02, 0x03, 0x01, 0x01, 0x01, 0x02, 0x03, 0x01,
            0x01, 0x02, 0x03, 0x01, 0x01, 0x01, 0x02, 0x03, 0x01, 0x01, 0x02, 0x03, 0x01, 0x01, 0x01, 0x02,
        };
        #endregion

        public byte[] symmetricate(byte[] word)
        {
            int remainder = rem(word.Length);
            byte[] asymmetric = new byte[word.Length + 1];
            word.CopyTo(asymmetric, 1);
            asymmetric[0] = (byte)(rem(asymmetric.Length));

            byte[] symmetric = new byte[asymmetric.Length + rem(asymmetric.Length)];
            asymmetric.CopyTo(symmetric, 0);
            return symmetric;
        }
        public byte[] desymmetricate(byte[] word)
        {
            int remainder = word[0];
            byte[] asymmetric = new byte[word.Length - 1 - remainder];
            for (int i = 1; i < asymmetric.Length; i++)
            {
                asymmetric[i - 1] = word[i];
            }
            return asymmetric;
        }
        private int rem(int size)
        {
            return (64 - (size % 64)) % 64;
        }

        public string textEncrypter(string plaintext, string key, int rounds)
        {
            byte[] kHash = kHashingAlgorithm(key);
            byte[] rHash = rHashingAlgorithm(kHash, key);
            byte[] keyschedule = keyExpansion(kHash, rounds);

            byte[] ciphertext = new byte[plaintext.Length + rem(plaintext.Length)];
            for (int i = 0; i < plaintext.Length; i++)
            {
                ciphertext[i] = (byte)plaintext[i];
            }
            //Algorithm
            if (rounds != 0)
            {
                ciphertext = addRHashKey(ciphertext, rHash);
            }
            for (int i = 0; i < rounds; i++)
            {
                ciphertext = subBytes(ciphertext);
                ciphertext = shiftColumns(ciphertext);
                ciphertext = RU25(ciphertext);
                ciphertext = addRoundKey(ciphertext, keyschedule, i);
            }
            return Convert.ToBase64String(ciphertext);
        }
        public string textDecrypter(string ciphertext, string key, int rounds)
        {
            byte[] KHash = kHashingAlgorithm(key);
            byte[] RHash = rHashingAlgorithm(KHash, key);
            byte[] keyschedule = keyExpansion(KHash, rounds);
            byte[] plaintext = Convert.FromBase64String(ciphertext);
            //Algorithm
            for (int i = rounds - 1; i >= 0; i--)
            {
                plaintext = addRoundKey(plaintext, keyschedule, i);
                plaintext = invRU25(plaintext);
                plaintext = invShiftColumns(plaintext);
                plaintext = invSubBytes(plaintext);
            }
            if (rounds != 0)
            {
                plaintext = addRHashKey(plaintext, RHash);
            }
            return Encoding.ASCII.GetString(plaintext);
        }

        public byte[] keyExpansion(byte[] KHash, int rounds)
        {
            byte[] Keyschedule = new byte[rounds * 64];
            byte[] Key = KHash;
            byte[] MDSKey = new byte[Key.Length];
            byte[] col8 = new byte[Key.Length / 8];
            byte[] col1 = new byte[col8.Length];
            byte[] rotsub = new byte[col8.Length];
            byte[] IV = new byte[col8.Length];
            ushort rcon;
            for (int i = 0; i < rounds; i++)
            {
                //RSR-Word
                rcon = r[i % r.Length];
                for (int j = 0; j < 64; j += 8)
                {
                    col1[j / 8] = Key[j];
                }
                for (int j = 7; j <= 64; j += 8)
                {
                    col8[((j + 1) / 8) - 1] = Key[j];
                }
                for (int j = 7; j < 2 * col8.Length - 1; j++)
                {
                    rotsub[(j + 1) % 8] = SBox[col8[j % 8]];
                }
                for (int j = 0; j < IV.Length; j++)
                {
                    IV[j] = (byte)(rotsub[j] ^ col1[j] ^ (rcon >> (int)(8 * Math.Ceiling(Math.Abs(Math.Sin(j))))));
                }
                for (int j = 0; j < 8; j++)
                {
                    for (int g = j; g < Key.Length; g += 8)
                    {
                        Key[g] = (byte)(Key[g] ^ IV[(g - j) / 8]);
                        IV[(g - j) / 8] = (byte)(Key[g] ^ IV[(g - j) / 8]);
                    }
                }
                //MDS
                int mult;
                for (int j = 0; j < 8; j++)
                {
                    for (int g = j; g < 64; g += 8)
                    {
                        mult = 0;
                        for (int k = j; k < 64; k += 8)
                        {
                            mult ^= Key[k + ((g - j) / 8) - j] * MDS[(k / 8) + (j * 8)];
                        }
                        MDSKey[((g - j) / 8) + ((j * 8))] = (byte)(mult % 256);
                    }
                }
                //RU25
                Key = RU25(MDSKey);
                Key.CopyTo(Keyschedule, i * 64);
            }
            return Keyschedule;
        }
        public byte[] kHashingAlgorithm(string Key)
        {
            //Variables

            #region

            string key = Key;
            if (key == "")
            {
                key = Encoding.ASCII.GetString(r);
            }
            string stringKHash = "";
            long x;
            long u = 0;
            long f = 0;
            long s = 0;
            long S = 0;
            long l = key.Length;
            foreach (char i in key) S += i;
            foreach (char i in r) s += i;
            foreach (char i in r25) u += i;
            decimal a;
            decimal b;
            decimal c;
            decimal d;
            #endregion

            //Functions
            #region
            a = 0;
            b = 0;
            //FUNCTION R_0
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                for (int i = 0; i < 6; i++)
                {
                    a += (decimal)Math.Round(Math.Log(r[i], (Math.Pow(x, 0.33333333333333))), 9);
                }
                b += (decimal)Math.Round(Math.Log(s, (Math.Pow(x, 0.25))), 9);
            }
            stringKHash += df(a, b);
            a = 0;
            b = 0;
            //FUNCTION R_1
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                for (int i = 0; i < 6; i++)
                {
                    a += (decimal)(Math.Round(Math.Acos(Math.Pow(x, 0.125) - 1) - Math.Log(r[i], Math.Pow(x, 2)), 10));
                    b += (decimal)(Math.Round(1 / (Math.Atan(x ^ r[i]) + 1), 10));
                }
            }
            stringKHash += df(a, b);
            a = 0;
            b = 0;
            //FUNCTION R_2
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                for (int i = 0; i < 6; i++)
                {
                    a += (decimal)(Math.Round(Math.Sqrt((Math.Pow(x, 2) + Math.Pow(r[i], 2)) - (2 * x * r[i] * Math.Cos(S))), 9));
                    b += (decimal)(Math.Round(Math.Log(Math.Pow(x * r[i], 0.25), Math.E), 9));
                }
            }
            stringKHash += df(a, b);
            a = 0;
            b = 0;
            //FUNCTION R_3
            for (int j = 0; j < l; j++)
            {
                c = 1;
                x = key[j];
                for (int i = 0; i < 6; i++)
                {
                    a += (decimal)(Math.Round(Math.Abs(Math.Sin(Math.Log(x + r[i] + S + 1, Math.Pow(x, 0.2)))), 10));
                    c *= (decimal)((Math.Pow(x, 0.33333333333333) / r[i]) + 1);
                }
                b += Math.Round(c, 10);
                a = Math.Round((1 / a), 10);
            }
            stringKHash += df(a, b);
            a = 0;
            b = 0;
            //FUNCTION R_4
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                for (int i = 0; i < 14; i++)
                {
                    a += (decimal)(1 / (Math.Log(r25[i], Math.Pow(x, 0.33333333333333)) + 1));
                }
                for (int i = 0; i < 6; i++)
                {
                    b += (decimal)(Math.Abs(Math.Asin(Math.Sin(r[i]) * (Math.Pow(x, 0.125) - 1))));
                }
            }
            stringKHash += df(a, b);
            a = 0;
            b = 0;
            //FUNCTION R_5
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                for (int i = 0; i < 6; i++)
                {
                    a += (decimal)((-Math.Cos(Math.Abs(Math.Cos(Math.Sqrt(x))) * r[i]) - -Math.Cos(-Math.Pow(x * r[i], 0.25))) * Math.Log(r[i], x));
                    b += (decimal)(Math.Pow(Math.Abs((Math.Cos(Math.Sqrt(x)))), 0.33333333333333) * -1 * Math.Log(r[i], x));
                }
            }
            stringKHash += df(a, b);
            a = 1;
            b = 0;
            //FUNCTION R_6
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                c = 1;
                for (int i = 0; i < 6; i++)
                {
                    c *= (decimal)(Math.Pow(((Math.Atan(Math.Pow(r[i] * x, 0.25))) / (Math.Log(Math.Abs((1 / Math.Sin(Math.Sqrt(x * r[i]) + S))), x))), 0.33333333333333));
                }
                b += (decimal)(((Math.Sin(25 * x) / x) - (Math.Sin(-25 * x) / x)) * (x - (Math.Cos(2 * x) / 2)) - (x - (Math.Cos(2 * -Math.Pow(x, 0.33333333333333)) / 2)));
                a += (c);
            }
            stringKHash += df(a, b);
            a = 1;
            b = 0;
            //FUNCTION R_7
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                c = 1;
                d = 0;
                for (int i = 0; i < 6; i++)
                {
                    c *= (decimal)(Math.Abs(Math.Log10((((x * r[i]) % Math.Ceiling(Math.Sqrt(x * r[i] + x + r[i]))) / Math.Asin(Math.Pow((Math.Pow(x * r[i] + Math.Cos(x), 0.33333333333333) / x + r[i]), 0.125) - 1)) + Math.Pow(x, 0.125))));
                    d += (decimal)(Math.Pow(x, 2) + Math.Pow(r[i], 2) + S + s) / (decimal)(Math.Abs(Math.Log((x * r[i]) % ((x ^ r[i]) + 6) + r[i], x) + 1));
                }
                a += c;
                b += d;
            }
            stringKHash += df(a, b);
            a = 0;
            b = 0;
            //FUNCTION R_8
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                for (int i = 0; i < 6; i++)
                {
                    a += (decimal)(Math.Pow((Math.Abs(Math.Abs(Math.Sin(x)) * Math.Pow(x, 0.33333333333333)) / x), Math.Sin(x * r[i])) * Math.Pow(x, 0.83333333333333));
                }
                b += (decimal)((Math.Pow(x, 2) + Math.Pow(r[(int)(Math.Floor(Math.Pow(x, 0.33333333333333)) - 1)], 2)) / Math.Log10(x * r[(int)(Math.Floor(Math.Pow(x, 0.33333333333333)) - 1)]));
            }
            stringKHash += df(a, b);
            a = 0;
            b = 0;
            //FUNCTION R_9
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                for (int i = 0; i < 6; i++)
                {
                    a += (decimal)((Math.Log((x ^ r[i]) + 1, r[i])) / (Math.Sin(x) + 1));
                    b += (decimal)(Math.Pow(Math.Abs(Math.Log((x ^ r[i] ^ s) + 1, r[i]) + x), 3) / (((int)Math.Pow(x, 2) ^ (int)Math.Pow(r[i], 2)) * Math.Sin(x) + 1));
                }
            }
            stringKHash += df(a, b);
            a = 0;
            b = 0;
            //FUNCTION R_10
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                a += (decimal)(Math.Sinh(Math.Pow(x, 0.33333333333333)));
                b += (decimal)(Math.Pow(Math.Sqrt(Math.Cosh(Math.Pow(x, 0.33333333333333))), (1f / Math.Sqrt(x))));
            }
            stringKHash += df(a, b);
            a = 0;
            b = 0;
            //FUNCTION R_11
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                a += (decimal)(Math.Sqrt(x) * Math.Pow((Math.Pow(x, 2) - x + Math.Pow(x, 3) + 25), (-Math.Sqrt(x) * Math.Cos(Math.E * Math.PI * x) + 2 * Math.Sin(x)) / x));
                for (int i = 0; i < 6; i++)
                {
                    b += (decimal)(Math.Abs(x * Math.Cos((Math.Log(x, Math.E) / Math.Sqrt(Math.E * r[i])) * x)));
                }
            }
            stringKHash += df(a, b);
            a = 0;
            b = 0;
            c = 0;
            d = 0;
            //FUNCTION R_12
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                for (int i = 0; i < 6; i++)
                {
                    d += (decimal)((Math.Pow(x, 2) + r[i] + s + x) / Math.Log((((x + Math.Pow(x, 2) + Math.Pow(x, 3)) % Math.Ceiling(Math.Sqrt(x))) + x), Math.PI * x));
                }
                b += (decimal)(Math.Abs(Math.Asin(Math.Pow(x, (1f / 5.1f)) - 2)));
                c += (decimal)(Math.Sqrt(Math.Abs(((Math.Sin(Math.Sqrt(x))) / (Math.Log(9, x))) + ((Math.Cos(Math.Sqrt(x))) / (Math.Log(x, Math.E))))));
            }
            a = (d / (c + 1));
            stringKHash += df(a, b);
            a = 0;
            b = 0;
            //FUNCTION R_13
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                for (int n = (int)x; n < x + 6; n++)
                {
                    a += (decimal)(Math.Abs(Math.Atan(Math.Pow(x, 1f / Math.Tan(n)))));
                }
                b += (decimal)(((x + 3) - Math.Cos(x + 3)) - (x - Math.Cos(x)));
            }
            stringKHash += df(a, b);
            a = 0;
            b = 0;
            //FUNCTION R_14
            for (int j = 0; j < l; j++)
            {
                x = key[j];
                for (int n = (((int)x % 10) + 2); n < x; n++)
                {
                    a += (decimal)((Math.Log((Math.Tanh(Math.Pow(n, 1f / x))), 2)) / (((Math.Log(1 + (Math.Pow(n, 0.125f) - 1)) - Math.Log(1 - (Math.Pow(n, 0.125f) - 1))) / 2) * Math.Sin(n)));
                    b += (decimal)Math.Log10(1 / (((Math.Atan(x + 1)) / (Math.Tanh(x + 1))) * ((Math.Pow(x + 1, 0.3f)) / (Math.Cosh(x + 1)))));
                }
            }
            stringKHash += df(a, b);
            //Function R_15
            decimal g = 0;
            if (true)
            {
                a = 0;
                b = 0;
                c = 0;
                d = 0;
                f = 0;
                for (int i = 0; i < 6; i++)
                {
                    a = 0;
                    for (int j = 0; j < key.Length; j++)
                    {
                        x = key[j];
                        a += (decimal)Math.Log(((Math.Log10((x + (Math.Sqrt(x * x - 1)))) + Math.Atan(r[i])) / Math.Abs(Math.Log((Math.Abs(Math.Asin(Math.Pow(x, 1f / 5.1f) - 2))), r[i]))) + 25, Math.E);
                        a += (decimal)(0.5f * (Math.Pow(x, 2) + 2 * x + 2) - 0.5f * Math.Pow(0.5f * (x + 1), 2));
                    }
                    d += a;
                }
                for (int j = 0; j < key.Length; j++)
                {
                    x = key[j];
                    b = 1;
                    for (int i = (int)x; i < x + 5; i++)
                    {
                        b *= (decimal)Math.Abs(Math.Asin(Math.Pow(Math.Abs(Math.Sin(x) * i), 1f / 5.1f) - 2) + Math.E) + 1;
                    }
                    f += (long)b;
                }
                g = (d / f) + f;
            }
            stringKHash += df(a, b);
            #endregion
            return StringToByteArray(stringKHash);
        }
        public byte[] rHashingAlgorithm(byte[] kHash, string key)
        {
            long n;
            int s_ = 0;
            for (int i = 0; i < kHash.Length; i++) s_ += kHash[i];
            for (int i = 0; i < kHash.Length; i++)
            {
                kHash[i] = SBox[kHash[i]];
            }
            for (int i = 0; i < kHash.Length; i++)
            {
                n = 0;
                for (int j = 0; j < 25; j++)
                {
                    n += ((i + kHash[j]) ^ s_ * (long)Math.Pow((Math.Pow(kHash[j], 3) % 625), 2));
                    for (int p = 0; p < r.Length; p++)
                    {
                        n ^= r[p] * (i ^ (n % (kHash[j] + 25)));
                    }
                    n ^= key[(j + i) % key.Length];
                    n = R((int)n);
                }
                kHash[i] = (byte)(kHash[i] ^ (n % 256));
            }
            //RU25
            return RU25(kHash);
        }
        private int R(int x)
        {
            for (int i = 0; i < r.Length; i++)
            {
                x ^= r[i];
            }
            return x;
        }

        private byte[] addRHashKey(byte[] word, byte[] RHash)
        {
            for (int i = 0; i < word.Length; i++)
            {
                word[i] ^= RHash[i % 64];
            }
            return word;
        }

        public byte[] subBytes(byte[] word)
        {
            for (int i = 0; i < word.Length; i += 8)
            {
                word[i] = SBox[word[i]];
                word[i + 1] = SBox[word[i + 1]];
                word[i + 2] = SBox[word[i + 2]];
                word[i + 3] = SBox[word[i + 3]];
                word[i + 4] = SBox[word[i + 4]];
                word[i + 5] = SBox[word[i + 5]];
                word[i + 6] = SBox[word[i + 6]];
                word[i + 7] = SBox[word[i + 7]];
            }
            return word;
        }
        public byte[] invSubBytes(byte[] word)
        {
            for (int i = 0; i < word.Length; i += 8)
            {
                word[i] = IBox[word[i]];
                word[i + 1] = IBox[word[i + 1]];
                word[i + 2] = IBox[word[i + 2]];
                word[i + 3] = IBox[word[i + 3]];
                word[i + 4] = IBox[word[i + 4]];
                word[i + 5] = IBox[word[i + 5]];
                word[i + 6] = IBox[word[i + 6]];
                word[i + 7] = IBox[word[i + 7]];
            }
            return word;
        }

        public byte[] shiftColumns(byte[] word)
        {
            byte[] newcipher = new byte[word.Length];
            byte[] block = new byte[64];
            byte[] blockshift = new byte[64];
            for (int i = 0; i < newcipher.Length / 64; i++)
            {
                for (int j = i * 64; j < (i + 1) * 64; j++)
                {
                    block[j % 64] = word[j];
                }
                for (int j = 0; j < 8; j++)
                {
                    for (int k = j; k < 64; k += 8)
                    {
                        blockshift[(j * 8) + (k - (j * 8))] = block[(k + ((j * 8))) % 64];
                    }
                }
                blockshift.CopyTo(newcipher, i * 64);
            }
            return newcipher;
        }
        public byte[] invShiftColumns(byte[] word)
        {
            byte[] newcipher = new byte[word.Length];
            byte[] block = new byte[64];
            byte[] blockshift = new byte[64];
            for (int i = 0; i < newcipher.Length / 64; i++)
            {
                for (int j = i * 64; j < (i + 1) * 64; j++)
                {
                    block[j % 64] = word[j];
                }
                for (int j = 0; j < 8; j++)
                {
                    for (int k = j; k < 64; k += 8)
                    {
                        blockshift[(j * 8) + (k - (j * 8))] = block[((64 - Math.Abs((j * 8)) + k)) % 64];
                    }
                }
                blockshift.CopyTo(newcipher, i * 64);
            }
            return newcipher;
        }

        public byte[] RU25(byte[] word)
        {
            byte[] fullcipher = new byte[word.Length];
            for (int u = 0; u < word.Length / 64; u++)
            {
                byte[] wordblock = new byte[64];
                byte[] cipher = new byte[64];
                byte[] RU25cipher = new byte[16];
                for (int j = (u * 64); j < (u * 64) + 64; j++)
                {
                    wordblock[j - (u * 64)] = word[j];
                }
                for (int i = 0; i < 4; i++)
                {
                    uint z_1 = 0;
                    uint z_2 = 0;
                    uint z_3 = 0;
                    uint z_4 = 0;
                    for (int j = (i * 2); j < (i * 2) + 4; j++)
                    {
                        int index = (((j - (i * 2)) * 8)) + (i * 2);
                        int shift = ((3 - (j - (i * 2))) * 8);
                        z_1 |= (uint)wordblock[index] << shift;
                        z_2 |= (uint)wordblock[index + 1] << shift;
                        z_3 |= (uint)wordblock[index + 32] << shift;
                        z_4 |= (uint)wordblock[index + 33] << shift;
                    }
                    //Design
                    z_2 = lshift(z_2, 19);
                    z_4 = lshift(z_4, 9);
                    z_3 = z_3 ^ (z_2 ^ z_4);
                    z_3 = lshift(z_3, 5);
                    z_1 = z_1 ^ (z_2 ^ lshift(z_3, 1));
                    z_1 = lshift(z_1, 3);
                    z_2 = z_2 ^ (z_1 ^ lshift(z_3, 13));
                    z_4 = z_4 ^ (z_1 ^ lshift(z_3, 7));
                    z_2 = lshift(z_2, 25);
                    z_4 = lshift(z_4, 11);

                    for (int k = 0; k < 4; k++)
                    {
                        RU25cipher[k] = (byte)(z_1 >> ((3 - k) * 8));
                        RU25cipher[k + 4] = (byte)(z_2 >> (3 - k) * 8);
                        RU25cipher[k + 8] = (byte)(z_3 >> (3 - k) * 8);
                        RU25cipher[k + 12] = (byte)(z_4 >> (3 - k) * 8);
                    }
                    RU25cipher.CopyTo(cipher, i * 16);
                }
                cipher.CopyTo(fullcipher, u * 64);
            }
            return fullcipher;
        }
        public byte[] invRU25(byte[] word)
        {
            byte[] fulldecipher = new byte[word.Length];
            byte[] decipher = new byte[64];
            byte[] INVRU25decipher = new byte[16];
            for (int u = 0; u < word.Length / 64; u++)
            {
                byte[] wordblock = new byte[64];
                for (int i = (u * 64); i < (u * 64) + 64; i++)
                {
                    wordblock[i - (u * 64)] = word[i];
                }
                for (int i = 0; i < 4; i++)
                {
                    uint c_1 = 0;
                    uint c_2 = 0;
                    uint c_3 = 0;
                    uint c_4 = 0;
                    for (int k = 0; k < 4; k++)
                    {
                        int shift = ((3 - k) * 8);
                        c_1 |= (uint)wordblock[(i * 16) + k] << shift;
                        c_2 |= (uint)wordblock[(i * 16) + k + 4] << shift;
                        c_3 |= (uint)wordblock[(i * 16) + k + 8] << shift;
                        c_4 |= (uint)wordblock[(i * 16) + k + 12] << shift;
                    }
                    //Design
                    c_4 = rshift(c_4, 11); //
                    c_2 = rshift(c_2, 25); //
                    c_4 = c_4 ^ (c_1 ^ lshift(c_3, 7)); //
                    c_2 = c_2 ^ (c_1 ^ lshift(c_3, 13)); //
                    c_1 = rshift(c_1, 3); //
                    c_1 = c_1 ^ (c_2 ^ lshift(c_3, 1)); //
                    c_3 = rshift(c_3, 5); //
                    c_3 = c_3 ^ (c_2 ^ c_4); //
                    c_4 = rshift(c_4, 9);
                    c_2 = rshift(c_2, 19);

                    for (int k = 0; k < 4; k++)
                    {
                        INVRU25decipher[k] = (byte)(c_1 >> ((3 - k) * 8));
                        INVRU25decipher[(k + 4)] = (byte)(c_2 >> ((3 - k) * 8));
                        INVRU25decipher[(k + 8)] = (byte)(c_3 >> ((3 - k) * 8));
                        INVRU25decipher[(k + 12)] = (byte)(c_4 >> ((3 - k) * 8));
                    }
                    INVRU25decipher.CopyTo(decipher, i * 16);
                }
                byte[] matrixFix = new byte[64];
                for (int i = 0; i < 4; i++)
                {
                    for (int j = (i * 2); j < (i * 2) + 4; j++)
                    {
                        int index = (((j - (i * 2)) * 8)) + (i * 2);
                        matrixFix[index] = decipher[(i * 16) + ((j - (i * 2)))];
                        matrixFix[index + 1] = decipher[(i * 16) + ((j - (i * 2))) + 4];
                        matrixFix[index + 32] = decipher[(i * 16) + ((j - (i * 2))) + 8];
                        matrixFix[index + 33] = decipher[(i * 16) + ((j - (i * 2))) + 12];
                    }
                }
                matrixFix.CopyTo(fulldecipher, u * 64);
            }
            return fulldecipher;
        }

        private byte[] addRoundKey(byte[] word, byte[] keyschedule, int round)
        {
            for (int i = 0; i < word.Length; i++)
            {
                word[i] = (byte)(word[i] ^ keyschedule[(round * 64) + (i % 64)]);
            }
            return word;
        }

        private uint lshift(uint x, int s)
        {
            return (x << s) | (x >> (32 - s));
        }
        private uint rshift(uint x, int s)
        {

            return (x >> s) | (x << (32 - s));
        }

        private byte[] StringToByteArray(string hex)
        {
            byte[] byteArray = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                byteArray[i / 2] = (byte)Convert.ToInt32(hex[i].ToString() + hex[i + 1].ToString(), 16);
            }
            return byteArray;
        }
        private string df(decimal a, decimal b)
        {
            int count = BitConverter.GetBytes(decimal.GetBits(a)[3])[2];
            a *= (int)Math.Pow(10, count);
            count = BitConverter.GetBytes(decimal.GetBits(b)[3])[2];
            b *= (int)Math.Pow(10, count);
            char[] hex = ((ulong)(Math.Abs(a)) ^ (ulong)Math.Abs(b)).ToString("X8").ToCharArray();
            string newhex = "";
            for (int i = 1; i < 9; i++)
            {
                try
                {
                    newhex += hex[i];
                }
                catch
                {
                    newhex += "0";
                }
            }
            return Math.Abs(Convert.ToInt32(newhex, 16)).ToString("X8");
        }
    }
}
