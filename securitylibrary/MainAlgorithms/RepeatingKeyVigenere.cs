using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)//return key
        {
            string FindShortestRepeatingKey(string key)
            {
                for (int i = 1; i <= key.Length / 2; i++)
                {
                    string candidate = key.Substring(0, i);
                    bool isRepeating = true;
                    for (int j = 0; j < key.Length; j += i)
                    {


                        int length = i;
                        if (j + i > key.Length)
                        {
                            length = key.Length - j;

                        }

                        if (key.Substring(j, length) != candidate.Substring(0, length))
                        {
                            isRepeating = false;
                            break;
                        }
                    }

                    if (isRepeating)
                    {
                        return candidate;
                    }
                }


                return key;
            }

            List<char> keyList = new List<char>();

            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();
            char keyChar;
            for (int i = 0; i < plainText.Length; i++)
            {
                char plainChar = plainText[i];
                char cipherChar = cipherText[i];
                keyChar = (char)(((cipherChar - plainChar + 26) % 26) + 'A');
                keyList.Add(keyChar);// j - c  = 9 - 2 = 7 + 65 =72  
            }

            string fullKey = new string(keyList.ToArray());


            string shortestKey = FindShortestRepeatingKey(fullKey);

            return shortestKey;

        }
        public string Decrypt(string cipherText, string key)
        {
            string plainText = "";
            key = key.ToUpper();
            cipherText = cipherText.ToUpper();
            int keyLength = key.Length;

            for (int i = 0; i < cipherText.Length; i++)
            {
                char cipherChar = cipherText[i];
                if (char.IsLetter(cipherChar))
                {
                    char keyChar = key[i % keyLength];
                    int cipherIndex = cipherChar - 'A';
                    int keyIndex = keyChar - 'A';
                    int plainIndex = (cipherIndex - keyIndex + 26) % 26;
                    char plainChar = (char)(plainIndex + 'A');
                    plainText += plainChar;
                }
                else
                {
                    plainText += cipherChar;
                }
            }

            return plainText.ToString();
        }

        public string Encrypt(string plainText, string key)
        {
            string cipherText = "";

          
            key = key.ToUpper();
            plainText = plainText.ToUpper();
            int keyLength = key.Length;

           
            for (int i = 0; i < plainText.Length; i++)
            {
                char plainChar = plainText[i];

                
                if (char.IsLetter(plainChar))
                {
                    
                    char keyChar = key[i % keyLength];

                  
                    int plainIndex = char.ToUpper(plainChar) - 'A';
                    int keyIndex = keyChar - 'A';

                   
                    int cipherIndex = (plainIndex + keyIndex) % 26;

                    
                    char cipherChar = (char)(cipherIndex + 'A');

                    
                    cipherText += cipherChar;
                }
                else
                {
                    
                    cipherText += plainChar;
                }
            }

            
            return cipherText.ToString();
        }
    }
}