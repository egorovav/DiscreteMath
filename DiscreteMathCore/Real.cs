using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class Real : RingBase<double>
    {
        public override double One
        {
            get
            {
                return 1;
            }
        }

        public override double Zero
        {
            get
            {
                return 0;
            }
        }

        //private int Digits = 8;
        private double Epsilon = 0.00000000000001;

        public override bool Equals(double a, double b)
        {
            return Math.Abs(a - b) <= Epsilon;
        }

        public override string GetTexString(double a)
        {
            return a.ToString();
        }

        public override bool IsNaN(double a)
        {
            return Double.IsNaN(a);
        }

        public override double LeftReverse(double a)
        {
            //return Math.Round(1 / a, Digits);
            return 1 / a;
        }

        public override double Opposite(double a)
        {
            return -a;
        }

        public override double Prod(double a, double b)
        {
            //return Math.Round(a * b, Digits);

            return a * b;
        }

        public override double InnerReverse(double a)
        {
            //return Math.Round(1 / a, Digits);
            return 1 / a;
        }

        public override double RightReverse(double a)
        {
            //return Math.Round(1 / a, Digits);
            return 1 / a;
        }

        public override double Sum(double a, double b)
        {
            //return Math.Round(a + b, Digits);
            return a + b;
        }
    }
}
