using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText) // Return key
        {
            string ExtractInitialKey(string Key, string plainT)
            {
               
                int j = 0;
                int end = 0;
                for (int i = 0; i < Key.Length; i++)
                {
                    if (i >= plainT.Length || Key[i] == plainT[j])
                    {
                        j++;

                        if (i < plainT.Length)
                            continue;
                        break;
                    }
                    end = i + 1;
                }
                if (end > Key.Length )
                    end = Key.Length;
                return Key.Substring(0, end);

            }

            List<char> keyList = new List<char>();

            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();

            
            for (int i = 0; i < plainText.Length; i++)
            {
                char plainChar = plainText[i];
                char cipherChar = cipherText[i];

               
                char keyChar = (char)(((cipherChar - plainChar + 26) % 26) + 'A');
                keyList.Add(keyChar);
            }
                string fullKey = new string(keyList.ToArray());
             string initialKey = ExtractInitialKey(fullKey, plainText);

            return initialKey;
        }

        public string Decrypt(string cipherText, string key)
        {
            StringBuilder plainText = new StringBuilder();
            key = key.ToUpper();
            int keyIndex = 0;

            for (int i = 0; i < cipherText.Length; i++)
            {
                char cipherChar = cipherText[i];

                if (char.IsLetter(cipherChar))
                {
                    char keyChar = key[keyIndex];
                    int cipherIndex = char.ToUpper(cipherChar) - 'A';
                    int keyIndexValue = keyChar - 'A';

                   
                    int plainIndex = (cipherIndex - keyIndexValue + 26) % 26;
                    char plainChar = (char)(plainIndex + 'A');

                   
                    plainText.Append(plainChar);

                   
                    key += plainChar;
                    keyIndex++;
                }
                else
                {
                    
                    plainText.Append(cipherChar);
                }
            }

            return plainText.ToString();
        }

        public string Encrypt(string plainText, string key)
        {
            string cipherText = "";
            key = key.ToUpper();
            plainText = plainText.ToUpper();
            int keyIndex = 0;
            for (int i = 0; i < plainText.Length; i++)
            {
                char plainChar = plainText[i];

                if (char.IsLetter(plainChar))
                {
                    char keyChar = key[keyIndex];
                    int plainIndex = plainChar - 'A';
                    int keyIndexValue = keyChar - 'A';

                    
                    int cipherIndex = (plainIndex + keyIndexValue) % 26;
                    char cipherChar = (char)(cipherIndex + 'A');

                    
                    cipherText += cipherChar;

                    
                    key += plainChar;
                    keyIndex++;
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
