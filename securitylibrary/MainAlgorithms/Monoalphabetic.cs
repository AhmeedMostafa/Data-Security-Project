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
            // Convert both plaintext and ciphertext to lowercase for easier processing
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            // Create a StringBuilder to build the 26-letter key
            StringBuilder key = new StringBuilder();
            // String to store letters not used in the ciphertext
            string notTaken = "";

            // Loop through alphabet to find letters not in ciphertext
            foreach (char letter in Alphabets)
            {
                // If letter isn't in ciphertext, add it to notTaken
                if (!cipherText.Contains(letter))
                {
                    notTaken += letter;
                }
            }

            // Index to track which notTaken letter to use next
            int notTakenIndex = 0;

            // Build the key by mapping each alphabet letter
            foreach (char letter in Alphabets)
            {
                // Find where this letter appears in plaintext
                int index = plainText.IndexOf(letter);
                // If letter is in plaintext, use the matching ciphertext letter
                if (index != -1)
                {
                    key.Append(cipherText[index]);
                }
                // If letter isn't in plaintext, use a letter from notTaken
                else
                {
                    key.Append(notTaken[notTakenIndex]);
                    notTakenIndex++; // Move to next unused letter
                }
            }
            // Return the key as a lowercase string
            return key.ToString().ToLower();
        }

        public string Decrypt(string cipherText, string key)
        {
            // Convert ciphertext and key to lowercase for consistency
            cipherText = cipherText.ToLower();
            key = key.ToLower();
            // Create a StringBuilder to build the plaintext
            StringBuilder plainText = new StringBuilder();

            // Loop through each character in the ciphertext
            foreach (char cipherChar in cipherText)
            {
                // Find where this ciphertext character is in the key
                int index = key.IndexOf(cipherChar);
                // If found, add the corresponding alphabet letter to plaintext
                if (index != -1)
                {
                    plainText.Append(Alphabets[index]);
                }
            }
            // Return the decrypted plaintext
            return plainText.ToString();
        }

        public string Encrypt(string plainText, string key)
        {
            // Create a StringBuilder to build the ciphertext
            StringBuilder cipherText = new StringBuilder();

            // Loop through each character in lowercase plaintext
            foreach (char plainChar in plainText.ToLower())
            {
                // Find the index of this plaintext character in the alphabet
                int index = Array.IndexOf(Alphabets, plainChar);
                // If found, add the corresponding key character to ciphertext
                if (index != -1)
                {
                    cipherText.Append(key[index]);
                }
            }
            // Return the encrypted ciphertext
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
            // Make the cipher text all lowercase so we don't worry about uppercase letters
            cipher = cipher.ToLower();

            // Array to store frequency of each letter (a-z) as a percentage
            double[] sizes = new double[26];
            // Array to keep track of letter indices (0=a, 1=b, etc.)
            int[] index = new int[26];
            // String to store letters in order of their frequency in cipher
            string freqArrange = "";
            // English letter frequency order (most to least common)
            string freqInEnglish = "ETAOINSRHLDCUMFPGWYBVKXJQZ";

            // Loop through each letter in the alphabet (a-z)
            for (int i = 0; i < Alphabets.Length; i++)
            {
                // Initialize frequency and index for this letter
                sizes[i] = 0;
                index[i] = i;
                // Count how many times this letter appears in the cipher
                for (int j = 0; j < cipher.Length; j++)
                {
                    if (Alphabets[i] == cipher[j])
                    {
                        sizes[i]++;
                    }
                }
                // Convert count to percentage (multiply by 100, divide by total length)
                sizes[i] *= 100;
                sizes[i] /= cipher.Length;
            }

            // Sort letters by frequency (highest to lowest) using bubble sort
            for (int i = 0; i < 26; i++)
            {
                for (int j = i; j < 26; j++)
                {
                    // Swap if current frequency is less than next frequency
                    if (i != j && sizes[i] < sizes[j])
                    {
                        // Swap frequencies
                        double tempSize = sizes[i];
                        sizes[i] = sizes[j];
                        sizes[j] = tempSize;
                        // Swap corresponding indices
                        int tempIndex = index[i];
                        index[i] = index[j];
                        index[j] = tempIndex;
                    }
                }
            }

            // Build a string of letters in frequency order (most to least frequent)
            for (int i = 0; i < 26; i++)
            {
                freqArrange += Alphabets[index[i]];
            }

            // Use Analyse method to create a key by mapping English frequency to cipher frequency
            string key = this.Analyse(freqInEnglish.ToLower(), freqArrange);
            // Decrypt the cipher using the key and return the plaintext
            return this.Decrypt(cipher, key);
        }
    }
}
