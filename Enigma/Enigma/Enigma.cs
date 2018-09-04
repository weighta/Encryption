using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enigma
{
    class Enigma
    {
        char[][] circuit = new char[4][];
        char[][] invcircuit = new char[4][];
        private int[] rot = { 0, 0, 0, 0 };
        private int s = 0x41;
        private void parseCircuit()
        {
            circuit[0] = new char[] { 'E', 'K', 'M', 'F', 'L', 'G', 'D', 'Q', 'V', 'Z', 'N', 'T', 'O', 'W', 'Y', 'H', 'X', 'U', 'S', 'P', 'A', 'I', 'B', 'R', 'C', 'J' };
            circuit[1] = new char[] { 'A', 'J', 'D', 'K', 'S', 'I', 'R', 'U', 'X', 'B', 'L', 'H', 'W', 'T', 'M', 'C', 'Q', 'G', 'Z', 'N', 'P', 'Y', 'F', 'V', 'O', 'E' };
            circuit[2] = new char[] { 'B', 'D', 'F', 'H', 'J', 'L', 'C', 'P', 'R', 'T', 'X', 'V', 'Z', 'N', 'Y', 'E', 'I', 'W', 'G', 'A', 'K', 'M', 'U', 'S', 'Q', 'O' };
            circuit[3] = new char[] { 'Y', 'R', 'U', 'H', 'Q', 'S', 'L', 'D', 'P', 'X', 'N', 'G', 'O', 'K', 'M', 'I', 'E', 'B', 'F', 'Z', 'C', 'W', 'V', 'J', 'A', 'T' };
        }
        public char E(char c, int jumps, int rot1, int rot2, int rot3)
        {
            parseCircuit();
            rot[0] = rot1;
            rot[1] = rot2;
            rot[2] = rot3;
            //MessageBox.Show(refl[circuit[2][((circuit[1][((circuit[0][((c - 0x41) + rot[0]) % 26] - 0x41) + rot[1]) % 26] - 0x41) + rot[2]) % 26] - 0x41].ToString());
            for (int i = 0; i < jumps; i++)
            {
                c = circuit[-Math.Abs(i - 3) + 3][((c - s) + rot[-Math.Abs(i - 3) + 3]) % 26];
            }
            return c;
        }

        public char invE(char c, int jumps, int rot1, int rot2, int rot3)
        {
            parseCircuit();
            rot[0] = rot1;
            rot[1] = rot2;
            rot[2] = rot3;
            fix_invarrays();
            for (int i = 0; i < jumps; i++)
            {
                //MessageBox.Show(circuit.Length.ToString());
                //MessageBox.Show("circiut: " + (-Math.Abs(i - 3) + 3) + " Pick char: " + (((c - s) + rot[-Math.Abs(i - 3) + 3]) % 26));
                c = invcircuit[-Math.Abs(i - 3) + 3][(c - s) % 26];
            }

            return c;
        }

        private void fix_invarrays()
        {
            for (int j = 0; j < 4; j++)
            {
                invcircuit[j] = new char[26];
                for (int k = 0; k < 26; k++)
                {
                    invcircuit[j][E(Convert.ToChar(k + s), 7 - j, rot[0], rot[1], rot[2]) - s] = E(Convert.ToChar(k + s), 6 - j, rot[0], rot[1], rot[2]);
                }
            }
        }
    }
}
