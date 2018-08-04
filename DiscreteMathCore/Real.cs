using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class Real : IRing<double>
    {
        public double One
        {
            get
            {
                return 1;
            }
        }

        public double Zero
        {
            get
            {
                return 0;
            }
        }

        //private int Digits = 8;
        private double Epsilon = 0.00000000000001;

        public bool Equals(double a, double b)
        {
            return Math.Abs(a - b) <= Epsilon;
        }

        public string GetTexString(double a)
        {
            return a.ToString();
        }

        public bool IsNaN(double a)
        {
            return Double.IsNaN(a);
        }

        public double LeftReverse(double a)
        {
            //return Math.Round(1 / a, Digits);
            return 1 / a;
        }

        public double Opposite(double a)
        {
            return -a;
        }

        public double Prod(double a, double b)
        {
            //return Math.Round(a * b, Digits);

            return a * b;
        }

        public double Reverse(double a)
        {
            //return Math.Round(1 / a, Digits);
            return 1 / a;
        }

        public double RightReverse(double a)
        {
            //return Math.Round(1 / a, Digits);
            return 1 / a;
        }

        public double Sum(double a, double b)
        {
            //return Math.Round(a + b, Digits);
            return a + b;
        }
    }
}
