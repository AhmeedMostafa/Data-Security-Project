using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            for (int key = 1; key <= plainText.Length; key++)
            {
                string encryptedText = Encrypt(plainText, key);

                if (encryptedText.Equals(cipherText, StringComparison.InvariantCultureIgnoreCase) ||
                    encryptedText.PadRight(cipherText.Length, 'X').Equals(cipherText, StringComparison.InvariantCultureIgnoreCase))
                {
                    return key;
                }
            }
            return -1;
        }

        public string Decrypt(string cipherText, int key)
        {
            int l = cipherText.Length;
            int col = (int)Math.Ceiling((double)l / key);

            char[,] matrix = new char[key, col];

            int index = 0;
            for (int r = 0; r < key; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    if (index < l)
                    {
                        matrix[r, c] = cipherText[index++];
                    }
                }
            }

            string result = "";
            for (int c = 0; c < col; c++)
            {
                for (int r = 0; r < key; r++)
                {
                    if (matrix[r, c] != '\0')
                    {
                        result += matrix[r, c];
                    }
                }
            }

            return result;
        }

        public string Encrypt(string plainText, int key)
        {
            int l = plainText.Length;
            int col = (int)Math.Ceiling((double)l / key);

            char[,] matrix = new char[key, col];
            int index = 0;

            for (int c = 0; c < col; c++)
            {
                for (int r = 0; r < key; r++)
                {
                    if (index < l)
                    {
                        matrix[r, c] = plainText[index++];
                    }
                    else
                    {
                        matrix[r, c] = '\0';
                    }
                }
            }

            string result = "";
            for (int r = 0; r < key; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    if (matrix[r, c] != '\0')
                    {
                        result += matrix[r, c];
                    }
                }
            }

            return result;

        }
    }
}
