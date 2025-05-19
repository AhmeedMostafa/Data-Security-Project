using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary

{

    public class PlayFair : ICryptographic_Technique<string, string>
    {
        public string Decrypt(string cipherText, string key)
        {
            string rf = key.ToLower();
            string cf = cipherText.ToLower();
            char[,] matrix = new char[5, 5];
            int x = 0, y = 0;
            Dictionary<char, int> char_Xpos = new Dictionary<char, int>();
            Dictionary<char, int> char_Ypos = new Dictionary<char, int>();
            bool i_and_j = false;

            void add_to_matrix(char c)
            {
                char_Xpos.Add(c, x);
                char_Ypos.Add(c, y);
                matrix[x, y] = c;
            }

           
            foreach (char c in rf)
            {
                if (!char_Xpos.ContainsKey(c))
                {
                    if (c == 'i' || c == 'j')
                    {
                        if (!i_and_j)
                        {
                            char_Xpos.Add('i', x);
                            char_Ypos.Add('i', y);
                            char_Xpos.Add('j', x);
                            char_Ypos.Add('j', y);
                            matrix[x, y] = c;
                            i_and_j = true;
                            y++;
                        }
                    }
                    else
                    {
                        add_to_matrix(c);
                        y++;
                    }
                    if (y == 5)
                    {
                        y = 0;
                        x++;
                    }
                }
            }

            char Char;
            for (int i = 0; i < 26; i++)
            {
                Char = (char)('a' + i);
                if (!char_Xpos.ContainsKey(Char))
                {
                    if (Char == 'i' || Char == 'j')
                    {
                        if (!i_and_j)
                        {
                            char_Xpos.Add('i', x);
                            char_Ypos.Add('i', y);
                            char_Xpos.Add('j', x);
                            char_Ypos.Add('j', y);
                            matrix[x, y] = Char;
                            i_and_j = true;
                            y++;
                        }
                    }
                    else
                    {
                        add_to_matrix(Char);
                        y++;
                    }
                    if (y == 5)
                    {
                        y = 0;
                        x++;
                    }
                }
            }

           
            List<char> pairs = new List<char>();
            for (int i = 0; i < cf.Length; i += 2)
            {
                pairs.Add(cf[i]);
                if (i + 1 < cf.Length)
                {
                    pairs.Add(cf[i + 1]);
                }
            }
            
            char[] result = new char[pairs.Count];
            for (int i = 0; i < pairs.Count; i += 2)
            {
                char firstChar = pairs[i];
                char secondChar = pairs[i + 1];

                if (char_Xpos[firstChar] == char_Xpos[secondChar])
                {
                    // Same row
                    result[i] = matrix[char_Xpos[firstChar], (char_Ypos[firstChar] - 1 + 5) % 5];
                    result[i + 1] = matrix[char_Xpos[secondChar], (char_Ypos[secondChar] - 1 + 5) % 5];
                }
                else if (char_Ypos[firstChar] == char_Ypos[secondChar])
                {
                    // Same column
                    result[i] = matrix[(char_Xpos[firstChar] - 1 + 5) % 5, char_Ypos[firstChar]];
                    result[i + 1] = matrix[(char_Xpos[secondChar] - 1 + 5) % 5, char_Ypos[secondChar]];
                }
                else
                {
                    // Rectangle
                    result[i] = matrix[char_Xpos[firstChar], char_Ypos[secondChar]];
                    result[i + 1] = matrix[char_Xpos[secondChar], char_Ypos[firstChar]];
                }
            }
            
            string decryptedText = new string(result).ToLower();
            if (decryptedText.EndsWith("x"))
            {
                decryptedText = decryptedText.Substring(0, decryptedText.Length - 1);
            }
            List<char> dt = decryptedText.ToList();
            
            for (int i = 1; i < dt.Count; i+=2)
            {
                if (dt[i] == 'x' )
                {
                    if (dt[i - 1] == dt[i + 1] )
                    {
                        dt[i] = '#';
                    }

                }

            }
            char[] res = new char[dt.Count];
            int j = 0;
            for (int i = 0; i < dt.Count; i++)
            {
                if (dt[i] != '#')
                {
                    res[j] = dt[i];
                    j++;
                }
            }
           
            return new string(res).ToUpper(); ;
        }

        public string Encrypt(string plainText, string key)
        {
            string rf = key.ToLower();
            char[,] matrix = new char[5, 5];
            int x = 0, y = 0;
            Dictionary<char, int> char_Xpos = new Dictionary<char, int>();
            Dictionary<char, int> char_Ypos = new Dictionary<char, int>();
            bool i_and_j = false;
            void add_to_matrix(char c)
            {
                Console.WriteLine(c);
                char_Xpos.Add(c, x);
                char_Ypos.Add(c, y);
                matrix[x, y] = c;

            }
            List<char> pairs = new List<char>();
            for (int i = 0; i < plainText.Length; i += 0)
            {
                Console.WriteLine(i);
                if (i == plainText.Length - 1)
                {
                    pairs.Add(plainText[i]);

                    break;
                }
                if (plainText[i] == plainText[i + 1])
                {
                    pairs.Add(plainText[i]);
                    pairs.Add('x');
                    i++;
                }
                else
                {
                    pairs.Add(plainText[i]);
                    pairs.Add(plainText[i + 1]);
                    i += 2;
                }

            }

            if (pairs.Count % 2 == 1)
            {
                pairs.Add('x');
            }
            foreach (char c in rf)
            {
                if (!char_Xpos.ContainsKey(c))
                {
                    if (c == 'i' || c == 'j')
                    {
                        if (!i_and_j)
                        {
                            char_Xpos.Add('i', x);
                            char_Ypos.Add('i', y);
                            char_Xpos.Add('j', x);
                            char_Ypos.Add('j', y);
                            matrix[x, y] = c;
                            
                            i_and_j = true;
                            y++;
                        }
                    }
                    else
                    {
                        add_to_matrix(c);
                        y++;
                    }
                    if (y == 5)
                    {
                        y = 0;
                        x++;
                    }
                }

            }
            char Char;
            for (int i = 0; i < 26; i++)
            {
                Char = (char)('a' + i);
                if (!char_Xpos.ContainsKey(Char))
                {
                    if (Char == 'i' || Char == 'j')
                    {
                        if (!i_and_j)
                        {
                            char_Xpos.Add('i', x);
                            char_Ypos.Add('i', y);
                            char_Xpos.Add('j', x);
                            char_Ypos.Add('j', y);
                            matrix[x, y] = Char;
                            
                            i_and_j = true;
                            y++;
                        }
                    }
                    else
                    {
                        add_to_matrix(Char);
                        y++;
                    }
                    if (y == 5)
                    {
                        y = 0;
                        x++;
                    }
                }
            }
            char[] result = new char[pairs.Count];
            for (int i = 0; i < pairs.Count; i += 2)
            {
                if (char_Xpos[pairs[i]] == char_Xpos[pairs[i + 1]])
                {
                    result[i] = matrix[char_Xpos[pairs[i]], (char_Ypos[pairs[i]] + 1) % 5];
                    result[i + 1] = matrix[char_Xpos[pairs[i + 1]], (char_Ypos[pairs[i + 1]] + 1) % 5];
                }
                else if (char_Ypos[pairs[i]] == char_Ypos[pairs[i + 1]])
                {
                    result[i] = matrix[(char_Xpos[pairs[i]] + 1) % 5, char_Ypos[pairs[i]]];
                    result[i + 1] = matrix[(char_Xpos[pairs[i + 1]] + 1) % 5, char_Ypos[pairs[i + 1]]];
                }
                else
                {
                    result[i] = matrix[char_Xpos[pairs[i]], char_Ypos[pairs[i + 1]]];
                    result[i + 1] = matrix[char_Xpos[pairs[i + 1]], char_Ypos[pairs[i]]];
                }
            }
            return new string(result).ToUpper();
        }
    }
}
