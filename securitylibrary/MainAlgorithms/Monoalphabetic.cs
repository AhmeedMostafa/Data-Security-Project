using System;
using System.Linq;
using System.Text;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        private static readonly char[] Alphabets = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        public string Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            StringBuilder key = new StringBuilder();
            string notTaken = "";
            foreach (char letter in Alphabets)
            {
                if (!cipherText.Contains(letter))
                {
                    notTaken += letter;
                }
            }

            int notTakenIndex = 0;

            foreach (char letter in Alphabets)
            {
                int index = plainText.IndexOf(letter);
                if (index != -1)
                {
                    key.Append(cipherText[index]);
                }
                else
                {
                    key.Append(notTaken[notTakenIndex]);
                    notTakenIndex++;
                }
            }
            return key.ToString().ToLower();
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            key = key.ToLower();
            StringBuilder plainText = new StringBuilder();

            foreach (char cipherChar in cipherText)
            {
                int index = key.IndexOf(cipherChar);
                if (index != -1)
                {
                    plainText.Append(Alphabets[index]);
                }
            }
            return plainText.ToString();
        }

        public string Encrypt(string plainText, string key)
        {
            StringBuilder cipherText = new StringBuilder();

            foreach (char plainChar in plainText.ToLower())
            {
                int index = Array.IndexOf(Alphabets, plainChar);
                if (index != -1)
                {
                    cipherText.Append(key[index]);
                }
            }
            return cipherText.ToString();
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            cipher = cipher.ToLower();
            double[] sizes = new double[26];
            int[] index = new int[26];
            string freqArrange = "";
            string freqInEnglish = "ETAOINSRHLDCUMFPGWYBVKXJQZ";

            for (int i = 0; i < Alphabets.Length; i++)
            {
                sizes[i] = 0;
                index[i] = i;
                for (int j = 0; j < cipher.Length; j++)
                {
                    if (Alphabets[i] == cipher[j])
                    {
                        sizes[i]++;
                    }
                }
                sizes[i] *= 100;
                sizes[i] /= cipher.Length;
            }

            for (int i = 0; i < 26; i++)
            {
                for (int j = i; j < 26; j++)
                {
                    if (i != j && sizes[i] < sizes[j])
                    {
                        double tempSize = sizes[i];
                        sizes[i] = sizes[j];
                        sizes[j] = tempSize;
                        int tempIndex = index[i];
                        index[i] = index[j];
                        index[j] = tempIndex;
                    }
                }
            }

            for (int i = 0; i < 26; i++)
            {
                freqArrange += Alphabets[index[i]];
            }

            string key = this.Analyse(freqInEnglish.ToLower(), freqArrange);
            return this.Decrypt(cipher, key);
        }
    }
}
