using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="baseN"></param>
        /// <returns>Mul inverse, -1 if no inv</returns>
        // Find the multiplicative inverse of number modulo baseN (i.e., x where number * x ≡ 1 mod baseN)
        public int GetMultiplicativeInverse(int number, int baseN)
        {
            // Variables to store coefficients x and y for the equation: number * x + baseN * y = gcd
            int x = 0, y = 0;
            // Call Extended_Euclid to find GCD and coefficients x, y
            int gcd = Extended_Euclid(number, baseN, ref x, ref y);
            // If GCD is not 1, no multiplicative inverse exists, so return -1
            if (gcd != 1) return -1;
            // Return x (the inverse), ensuring it's positive and within modulo baseN
            else return (x % baseN + baseN) % baseN;
        }

        // Recursive method to compute GCD and coefficients x, y for equation: a * x + b * y = gcd
        public int Extended_Euclid(int a, int b, ref int x_prev, ref int y_prev)
        {
            // Base case: if b is 0, GCD is a, set x = 1, y = 0
            if (b == 0)
            {
                x_prev = 1;
                y_prev = 0;
                return a;
            }
            // Recursive call: compute GCD for b and a % b, update x_prev and y_prev
            int gcd = Extended_Euclid(b, a % b, ref y_prev, ref x_prev);
            // Update y_prev using the formula: y_prev = previous y - (a/b) * previous x
            y_prev -= a / b * x_prev;
            // Return the GCD
            return gcd;
        }
    }
}