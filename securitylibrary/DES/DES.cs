﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class DES : CryptographicTechnique
    {
        // Permutation tables for DES steps
        private static readonly int[] IP = {
        58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4,
        62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8,
        57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3,
        61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7
    };

        private static readonly int[] PC1 = {
        57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18,
        10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36,
        63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22,
        14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4
    };

        private static readonly int[] PC2 = {
        14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10,
        23, 19, 12, 4, 26, 8, 16, 7, 27, 20, 13, 2,
        41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48,
        44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32
    };

        private static readonly int[] E = {
        32, 1, 2, 3, 4, 5, 4, 5, 6, 7, 8, 9, 8, 9, 10, 11,
        12, 13, 12, 13, 14, 15, 16, 17, 16, 17, 18, 19, 20, 21, 20, 21,
        22, 23, 24, 25, 24, 25, 26, 27, 28, 29, 28, 29, 30, 31, 32, 1
    };

        private static readonly int[] P = {
        16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26,
        5, 18, 31, 10, 2, 8, 24, 14, 32, 27, 3, 9,
        19, 13, 30, 6, 22, 11, 4, 25
    };

        private static readonly int[] FP = {
        40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31,
        38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29,
        36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27,
        34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25
    };

        private static readonly int[] ShiftBits = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };

        // S-Boxes: Lookup tables for substitution in each round
        private static readonly int[,,] SBox = {
        {
            {14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7},
            {0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8},
            {4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0},
            {15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13}
        },
        {
            {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10},
            {3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5},
            {0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15},
            {13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9}
        },
        {
            {10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8},
            {13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1},
            {13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7},
            {1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12}
        },
        {
            {7, 13, 14, 2, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15},
            {13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9},
            {10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4},
            {3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14}
        },
        {
            {2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9},
            {14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6},
            {4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14},
            {11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3}
        },
        {
            {12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11},
            {10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8},
            {9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6},
            {4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13}
        },
        {
            {4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1},
            {13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6},
            {1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2},
            {6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12}
        },
        {
            {13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7},
            {1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2},
            {7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8},
            {2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11}
        }
    };

        // Dictionary to convert hex digits to 4-bit binary
        private static readonly Dictionary<char, string> HexToBinMap = new Dictionary<char, string> {
        {'0', "0000"}, {'1', "0001"}, {'2', "0010"}, {'3', "0011"},
        {'4', "0100"}, {'5', "0101"}, {'6', "0110"}, {'7', "0111"},
        {'8', "1000"}, {'9', "1001"}, {'A', "1010"}, {'B', "1011"},
        {'C', "1100"}, {'D', "1101"}, {'E', "1110"}, {'F', "1111"}
    };

        // Dictionary to convert 4-bit binary to hex digits
        private static readonly Dictionary<string, char> BinToHexMap = new Dictionary<string, char> {
        {"0000", '0'}, {"0001", '1'}, {"0010", '2'}, {"0011", '3'},
        {"0100", '4'}, {"0101", '5'}, {"0110", '6'}, {"0111", '7'},
        {"1000", '8'}, {"1001", '9'}, {"1010", 'A'}, {"1011", 'B'},
        {"1100", 'C'}, {"1101", 'D'}, {"1110", 'E'}, {"1111", 'F'}
    };

        // Convert hex string (starting with "0x") to binary string
        private static string HexToBin(string hex) => string.Concat(hex.Skip(2).Select(c => HexToBinMap[char.ToUpper(c)]));

        // Convert binary string to hex string with "0x" prefix
        private static string BinToHex(string binary)
        {
            string hex = "0x";
            for (int i = 0; i < binary.Length; i += 4)
                hex += BinToHexMap[binary.Substring(i, 4)];
            return hex;
        }

        // Convert binary string to decimal number
        private static int BinToDec(string binary) => Convert.ToInt32(binary, 2);

        // Convert decimal to binary string with padding (default 4 bits)
        private static string DecToBin(int dec, int padding = 4) =>
            Convert.ToString(dec, 2).PadLeft(padding, '0');

        // Permute input bits according to a table
        private static string Permute(string input, int[] table) =>
            string.Concat(table.Select(i => input[i - 1]));

        // Shift a binary string left by shiftCount bits
        private static string LeftShift(string key, int shiftCount) =>
            key.Substring(shiftCount) + key.Substring(0, shiftCount);

        // XOR two binary strings (same length)
        private static string Xor(string a, string b) =>
            string.Concat(a.Zip(b, (x, y) => x == y ? '0' : '1'));

        // Apply S-Boxes to a 48-bit input, producing a 32-bit output
        private static string ApplySBoxes(string input)
        {
            string output = "";
            // Process 8 blocks of 6 bits each
            for (int i = 0; i < 8; i++)
            {
                string block = input.Substring(i * 6, 6);
                // Get row from first and last bits
                int row = BinToDec(block[0].ToString() + block[5]);
                // Get column from middle 4 bits
                int col = BinToDec(block.Substring(1, 4));
                // Lookup S-Box value and convert to 4-bit binary
                output += DecToBin(SBox[i, row, col]);
            }
            return output;
        }

        // Generate 16 round keys from the input key
        private static string[] GenerateRoundKeys(string key)
        {
            // Permute 64-bit key to 56 bits using PC1
            string permutedKey = Permute(key, PC1);
            string[] roundKeys = new string[16];
            // Split into two 28-bit halves: C (left) and D (right)
            string C = permutedKey.Substring(0, 28);
            string D = permutedKey.Substring(28);

            // Generate 16 round keys
            for (int i = 0; i < 16; i++)
            {
                // Shift left by specified bits
                C = LeftShift(C, ShiftBits[i]);
                D = LeftShift(D, ShiftBits[i]);
                // Combine and permute to 48-bit key using PC2
                roundKeys[i] = Permute(C + D, PC2);
            }
            return roundKeys;
        }

        // Process a 64-bit block through 16 DES rounds
        private static string ProcessBlock(string block, string[] roundKeys)
        {
            // Initial permutation
            block = Permute(block, IP);
            // Split into 32-bit left and right halves
            string left = block.Substring(0, 32);
            string right = block.Substring(32);

            // 16 rounds of Feistel network
            for (int i = 0; i < 16; i++)
            {
                // Expand right half to 48 bits
                string expanded = Permute(right, E);
                // XOR with round key
                string xored = Xor(expanded, roundKeys[i]);
                // Apply S-Boxes to get 32-bit output
                string substituted = ApplySBoxes(xored);
                // Permute result
                string permuted = Permute(substituted, P);
                // XOR with left half to get new right half
                string newLeft = Xor(left, permuted);

                // Swap: left becomes right, right becomes newLeft
                left = right;
                right = newLeft;
            }

            // Final permutation (swap and combine)
            return Permute(right + left, FP);
        }

        // Encrypt plaintext using DES
        public override string Encrypt(string plainText, string key)
        {
            // Generate 16 round keys from binary key
            string[] roundKeys = GenerateRoundKeys(HexToBin(key));
            // Process 64-bit plaintext block
            string cipherText = ProcessBlock(HexToBin(plainText), roundKeys);
            // Convert result to hex
            return BinToHex(cipherText);
        }

        // Decrypt ciphertext using DES
        public override string Decrypt(string cipherText, string key)
        {
            // Generate 16 round keys from binary key
            string[] roundKeys = GenerateRoundKeys(HexToBin(key));
            // Reverse round keys for decryption
            Array.Reverse(roundKeys);
            // Process 64-bit ciphertext block
            string plainText = ProcessBlock(HexToBin(cipherText), roundKeys);
            // Convert result to hex
            return BinToHex(plainText);
        }
    }
}