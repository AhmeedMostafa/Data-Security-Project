using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;



namespace SecurityLibrary
{
    public class Columnar
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            List<int> possibleColumnsNum = new List<int>();
            List<int> key = new List<int>();
            plainText = new string(plainText.Where(c => !char.IsWhiteSpace(c)).ToArray()).ToLower();
            cipherText = new string(cipherText.Where(c => !char.IsWhiteSpace(c)).ToArray()).ToLower();
            for (int i = 1; i <= cipherText.Count(); i++)
            {
                if (cipherText.Count() % i == 0)
                {
                    possibleColumnsNum.Add(i);
                }
            }
            for (int i = 0; i < possibleColumnsNum.Count(); i++)
            {
                int columns = possibleColumnsNum[i];
                int rows = (int)Math.Ceiling((double)cipherText.Count() / columns);
                char[,] plainMatrix = new char[rows, columns];
                char[,] cipherMatrix = new char[rows, columns];
                int plainIndex = 0;
                int cipherIndex = 0;
                
                for (int j = 0; j < rows; j++)
                {
                    for (int k = 0; k < columns; k++)
                    {
                        if(plainIndex < plainText.Count())
                        {
                            plainMatrix[j, k] = plainText[plainIndex];
                            plainIndex++;
                        }
                    }
                }
                for (int j = 0; j < columns; j++)
                {
                    for (int k = 0; k < rows; k++)
                    {
                        if(cipherIndex < cipherText.Count())
                        {
                            cipherMatrix[k, j] = cipherText[cipherIndex];
                            cipherIndex++;
                        }
                        
                    }
                }
                
                key.Clear();
                for (int j = 0; j < columns; j++) 
                {
                    for (int k = 0; k < columns; k++)  
                    {
                        bool match = true;
                        for (int m = 0; m < rows; m++)
                        {
                            if (plainMatrix[m, j] != cipherMatrix[m, k])  
                            {
                                match = false;
                                break;
                            }
                        }
                        if (match)
                        {
                            key.Add(k + 1);  
                            break;
                        }
                    }
                }

                if (key.Count() == columns)
                {
                    return key;  
                }
            }

            return new List<int>();
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            cipherText = new string(cipherText.Where(c => !char.IsWhiteSpace(c)).ToArray());
            int columns = key.Count();
            int rows = (int)Math.Ceiling((double)cipherText.Count() / columns);
            char[,] plainMatrix = new char[rows, columns];
            int index = 0;
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if(index < cipherText.Count())
                    {
                        plainMatrix[j, key.IndexOf(i + 1)] = cipherText[index];
                        index++;
                    }
                }
            }
            return new string(plainMatrix.Cast<char>().ToArray());

        }

        public string Encrypt(string plainText, List<int> key)
        {
            plainText = new string(plainText.Where(c => !char.IsWhiteSpace(c)).ToArray());
            int columns = key.Count();
            int rows = (int)Math.Ceiling((double)plainText.Count() / columns);
            char[,] plainMatrix = new char[rows, columns];

            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (index < plainText.Count())
                    {
                        plainMatrix[i, j] = plainText[index];
                        index++;
                    }
                    else
                    {
                        plainMatrix[i, j] = 'x';
                    }
                }
            }

            char[] cipherArray = new char[rows * columns];
            index = 0;

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    cipherArray[index] = plainMatrix[j, key.IndexOf(i+1)];
                    index++;
                }
            }

            return new string(cipherArray).ToUpper();

        }
    }
}
