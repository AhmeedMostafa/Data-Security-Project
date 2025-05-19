using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        public string Encrypt(string plainText, int key)
        {
            char[] result = new char[plainText.Length];
            int i= 0;
            foreach (char c in plainText )
            {
                if (char.IsUpper(c))
                {
                    result[i] = (char)(((c + key - 'A') % 26) + 'A');
                    i++;
                }
                else if (char.IsLower(c))
                {
                    result[i] = (char)(((c + key - 'a') % 26) + 'a');
                    i++;
                }
            }
            return new string(result);

        }

        public string Decrypt(string cipherText, int key)
        {

            return Encrypt(cipherText, 26 - key);
        }

        public int Analyse(string plainText, string cipherText)
        {  
            
            int key = char.ToLower(cipherText[0])- char.ToLower(plainText[0]) ;
           if (key < 0) { 
                key += 26;
           } 
                key %= 26;
           
          return key;
        }
    }
}
