//using MathNet.Numerics.LinearAlgebra;
//using MathNet.Numerics.LinearAlgebra.Complex;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher : ICryptographicTechnique<List<int>, List<int>>
    {

        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            int size = 2;

            int[,] plainmatrix = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int index = j * size + i;
                    plainmatrix[i, j] = (index < plainText.Count) ? plainText[index] : 0;
                }
            }

            int[,] ciphermatrx = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int index = j * size + i;
                    ciphermatrx[i, j] = (index < cipherText.Count) ? cipherText[index] : 0;
                }
            }

            for (int a = 0; a < 26; a++)
            {
                for (int b = 0; b < 26; b++)
                {
                    for (int c = 0; c < 26; c++)
                    {
                        for (int d = 0; d < 26; d++)
                        {
                            int[,] keyMatrix = { { a, b }, { c, d } };
                            int[,] resultMatrix = new int[2, 2];

                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    resultMatrix[i, j] = (keyMatrix[i, 0] * plainmatrix[0, j] + keyMatrix[i, 1] * plainmatrix[1, j]) % 26;
                                }
                            }

                            bool match = true;
                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    if (resultMatrix[i, j] % 26 != ciphermatrx[i, j] % 26)
                                    {
                                        match = false;
                                        break;
                                    }
                                }
                                if (!match) break;
                            }

                            if (match)
                            {
                                return new List<int> { a, b, c, d };
                            }
                        }
                    }
                }
            }
            throw new SecurityLibrary.InvalidAnlysisException();
        }

        private int[,] ConvertToMatrix(List<int> values, int size)
        {
            int[,] matrix = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int index = j * size + i;
                    matrix[i, j] = (index < values.Count) ? values[index] : 0;
                }
            }
            return matrix;
        }

       /* private int[,] MultiplyMatrices(int[,] key, int[,] plain)
        {
            int[,] result = new int[2, 2];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    result[i, j] = (key[i, 0] * plain[0, j] + key[i, 1] * plain[1, j]) % 26;
                }
            }
            return result;
        }

        private bool MatricesMatch(int[,] matrix1, int[,] matrix2)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (matrix1[i, j] % 26 != matrix2[i, j] % 26)
                        return false;
                }
            }
            return true;
        }*/
        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            int matrixsize = (int)Math.Sqrt(key.Count);
            int det = 0;
            int[][] keymatrix = new int[matrixsize][];
            List<int> result = new List<int>();
            for (int i = 0; i < matrixsize; i++)
            {
                keymatrix[i] = new int[matrixsize];
                for (int j = 0; j < matrixsize; j++)
                {
                    keymatrix[i][j] = key[i * matrixsize + j];
                }
            }
            if (matrixsize == 2)
            {
                det = (keymatrix[0][0] * keymatrix[1][1] - keymatrix[0][1] * keymatrix[1][0]);
                det = ((det % 26) + 26) % 26;
                int b = 26;
                int a = det;
                while (b != 0)
                {
                    int temp = b;
                    b = det % b;
                    det = temp;
                }
                if (det != 1)
                {
                    throw new ArgumentException("Invalid key: The key matrix is not invertible modulo 26 Ya7addyyyyy.");
                }
                det = a;
                int invdet = 0;
                for (int i = 1; i < 26; i++)
                {
                    if ((det * i) % 26 == 1)
                    {
                        invdet = i;
                        break;
                    }
                }

                int[][] inversematrix = new int[matrixsize][];
                for (int i = 0; i < matrixsize; i++)
                {
                    inversematrix[i] = new int[matrixsize];
                }
                inversematrix[0][0] = keymatrix[1][1];
                inversematrix[0][1] = -keymatrix[0][1];
                inversematrix[1][0] = -keymatrix[1][0];
                inversematrix[1][1] = keymatrix[0][0];

                int[][] finalKeymatrix = new int[matrixsize][];
                for (int i = 0; i < matrixsize; i++)
                {
                    finalKeymatrix[i] = new int[matrixsize];
                    for (int j = 0; j < matrixsize; j++)
                    {
                        finalKeymatrix[i][j] = ((inversematrix[i][j] * invdet) % 26 + 26) % 26;
                    }
                }

                int[] res = new int[matrixsize];
                int o = 0;
                while (o < cipherText.Count())
                {
                    for (int i = 0; i < matrixsize; i++)
                    {
                        res[i] = cipherText[o];
                        o++;
                    }
                    for (int j = 0; j < matrixsize; j++)
                    {
                        int x = 0;
                        for (int k = 0; k < matrixsize; k++)
                        {
                            x += res[k] * finalKeymatrix[j][k];
                        }
                        result.Add((x % 26 + 26) % 26);
                    }
                }

            }
            else
            {
                det = (keymatrix[0][0] * (keymatrix[1][1] * keymatrix[2][2] - keymatrix[1][2] * keymatrix[2][1]))
                    - (keymatrix[0][1] * ((keymatrix[1][0] * keymatrix[2][2] - keymatrix[1][2] * keymatrix[2][0])))
                    + (keymatrix[0][2] * ((keymatrix[1][0] * keymatrix[2][1] - keymatrix[1][1] * keymatrix[2][0])));
                det = ((det % 26) + 26) % 26;
                int b = 26;
                int a = det;

                int invdet = 0;
                for (int i = 1; i < 26; i++)
                {
                    if ((det * i) % 26 == 1)
                    {
                        invdet = i;
                        break;
                    }
                }
                int[][] inverseMatrix = new int[matrixsize][];
                for (int i = 0; i < matrixsize; i++)
                {
                    inverseMatrix[i] = new int[matrixsize];
                }

                int[][] transpose = new int[matrixsize][];
                for (int i = 0; i < matrixsize; i++)
                {
                    transpose[i] = new int[matrixsize];
                }
                transpose[0][0] = (keymatrix[1][1] * keymatrix[2][2] - keymatrix[1][2] * keymatrix[2][1]) % 26;
                transpose[0][1] = -(keymatrix[0][1] * keymatrix[2][2] - keymatrix[0][2] * keymatrix[2][1]) % 26;
                transpose[0][2] = (keymatrix[0][1] * keymatrix[1][2] - keymatrix[0][2] * keymatrix[1][1]) % 26;

                transpose[1][0] = -(keymatrix[1][0] * keymatrix[2][2] - keymatrix[1][2] * keymatrix[2][0]) % 26;
                transpose[1][1] = (keymatrix[0][0] * keymatrix[2][2] - keymatrix[0][2] * keymatrix[2][0]) % 26;
                transpose[1][2] = -(keymatrix[0][0] * keymatrix[1][2] - keymatrix[0][2] * keymatrix[1][0]) % 26;

                transpose[2][0] = (keymatrix[1][0] * keymatrix[2][1] - keymatrix[1][1] * keymatrix[2][0]) % 26;
                transpose[2][1] = -(keymatrix[0][0] * keymatrix[2][1] - keymatrix[0][1] * keymatrix[2][0]) % 26;
                transpose[2][2] = (keymatrix[0][0] * keymatrix[1][1] - keymatrix[0][1] * keymatrix[1][0]) % 26;

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        transpose[i][j] = (((transpose[i][j] * invdet) % 26) + 26) % 26;
                    }
                }



                int[] res = new int[matrixsize];
                int o = 0;
                while (o < cipherText.Count())
                {
                    for (int i = 0; i < matrixsize; i++)
                    {
                        res[i] = cipherText[o];
                        o++;
                    }
                    for (int j = 0; j < matrixsize; j++)
                    {
                        int x = 0;
                        for (int k = 0; k < matrixsize; k++)
                        {
                            x += res[k] * transpose[j][k];
                        }
                        result.Add(x % 26);
                    }
                }



            }



            return result;
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            int matrixsize = (int)Math.Sqrt(key.Count);
            List<int> result = new List<int>();
            int[][] keymatrix = new int[matrixsize][];
            for (int i = 0; i < matrixsize; i++)
            {
                keymatrix[i] = new int[matrixsize];
                for (int j = 0; j < matrixsize; j++)
                {
                    keymatrix[i][j] = key[i * matrixsize + j];
                }
            }

            int[] res = new int[matrixsize];
            int o = 0;
            while (o < plainText.Count())
            {
                for (int i = 0; i < matrixsize; i++)
                {
                    res[i] = plainText[o];
                    o++;
                }
                for (int j = 0; j < matrixsize; j++)
                {
                    int x = 0;
                    for (int k = 0; k < matrixsize; k++)
                    {
                        x += res[k] * keymatrix[j][k];
                    }
                    result.Add(x % 26);
                }
            }

            return result;
        }

        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            int matrixsize = (int)Math.Sqrt(plainText.Count);
            int det = 0;
            int[,] plainTextmatrix = new int[matrixsize, matrixsize];
            int[,] cipherTextmatrix = new int[matrixsize, matrixsize];
            List<int> result = new List<int>();

            for (int i = 0; i < matrixsize; i++)
            {
                for (int j = 0; j < matrixsize; j++)
                {
                    plainTextmatrix[j, i] = plainText[i * matrixsize + j];
                    cipherTextmatrix[j, i] = cipherText[i * matrixsize + j];
                }
            }

            det = (plainTextmatrix[0, 0] * (plainTextmatrix[1, 1] * plainTextmatrix[2, 2] - plainTextmatrix[1, 2] * plainTextmatrix[2, 1]))
                  - (plainTextmatrix[0, 1] * ((plainTextmatrix[1, 0] * plainTextmatrix[2, 2] - plainTextmatrix[1, 2] * plainTextmatrix[2, 0])))
                  + (plainTextmatrix[0, 2] * ((plainTextmatrix[1, 0] * plainTextmatrix[2, 1] - plainTextmatrix[1, 1] * plainTextmatrix[2, 0])));
            det = ((det % 26) + 26) % 26;

            int invdet = 0;
            for (int i = 1; i < 26; i++)
            {
                if ((det * i) % 26 == 1)
                {
                    invdet = i;
                    break;
                }
            }

            if (invdet == 0)
            {
                throw new InvalidOperationException("Determinant has no modular inverse under mod 26, decryption is not possible.");
            }

            int[,] transpose = new int[matrixsize, matrixsize];

            transpose[0, 0] = (plainTextmatrix[1, 1] * plainTextmatrix[2, 2] - plainTextmatrix[1, 2] * plainTextmatrix[2, 1]) % 26;
            transpose[0, 1] = ((-(plainTextmatrix[0, 1] * plainTextmatrix[2, 2] - plainTextmatrix[0, 2] * plainTextmatrix[2, 1])) % 26 + 26) % 26;
            transpose[0, 2] = (plainTextmatrix[0, 1] * plainTextmatrix[1, 2] - plainTextmatrix[0, 2] * plainTextmatrix[1, 1]) % 26;

            transpose[1, 0] = ((-(plainTextmatrix[1, 0] * plainTextmatrix[2, 2] - plainTextmatrix[1, 2] * plainTextmatrix[2, 0])) % 26 + 26) % 26;
            transpose[1, 1] = (plainTextmatrix[0, 0] * plainTextmatrix[2, 2] - plainTextmatrix[0, 2] * plainTextmatrix[2, 0]) % 26;
            transpose[1, 2] = ((-(plainTextmatrix[0, 0] * plainTextmatrix[1, 2] - plainTextmatrix[0, 2] * plainTextmatrix[1, 0])) % 26 + 26) % 26;

            transpose[2, 0] = (plainTextmatrix[1, 0] * plainTextmatrix[2, 1] - plainTextmatrix[1, 1] * plainTextmatrix[2, 0]) % 26;
            transpose[2, 1] = ((-(plainTextmatrix[0, 0] * plainTextmatrix[2, 1] - plainTextmatrix[0, 1] * plainTextmatrix[2, 0])) % 26 + 26) % 26;
            transpose[2, 2] = (plainTextmatrix[0, 0] * plainTextmatrix[1, 1] - plainTextmatrix[0, 1] * plainTextmatrix[1, 0]) % 26;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    transpose[i, j] = (((transpose[i, j] * invdet) % 26) + 26) % 26;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < 3; k++)
                    {
                        sum = (sum + transpose[k, j] * cipherTextmatrix[i, k]) % 26;
                    }
                    result.Add(sum);
                }
            }

            return result;
        }


    }
}
