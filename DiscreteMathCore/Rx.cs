using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class Rx<T, R> : IRing<Polynom<T, R>> where R : IRing<T>
    {
        private R FRing;

        public Rx(R aRing)
        {
            this.FRing = aRing;
        }

        public Polynom<T, R> One
        {
            get
            {
                return new Polynom<T, R>(this.FRing, 0);
            }
        }

        public Polynom<T, R> Zero
        {
            get
            {
                return new Polynom<T, R>(this.FRing, -1);
            }
        }

        public bool Equals(Polynom<T, R> a, Polynom<T, R> b)
        {
            return a == b;
        }

        public string GetTexString(Polynom<T, R> a)
        {
            return String.Format("[{0}]", a.TexString());
        }

        public Polynom<T, R> Opposite(Polynom<T, R> a)
        {
            return -a;
        }

        public Polynom<T, R> Prod(Polynom<T, R> a, Polynom<T, R> b)
        {
            return a * b;
        }

        public Polynom<T, R> Reverse(Polynom<T, R> a)
        {
            if (a.Degree != 0)
                throw new DivideByZeroException(
                    String.Format("The polynom {0} isn't invertible on the ring {1}.", a, this));

            var _reverse = this.FRing.Reverse(a.GetValue(this.FRing.Zero));
            return new Polynom<T, R>(this.FRing, new T[] { _reverse });
        }

        public Polynom<T, R> Sum(Polynom<T, R> a, Polynom<T, R> b)
        {
            return a + b;
        }

        public override string ToString()
        {
            return String.Format("{0}[x]", this.FRing);
        }

        public Polynom<T, R> GetPolynomByElement(T aVal)
        {
            return new Polynom<T, R>(this.FRing, new T[] { aVal });
        }

        public Polynom<T, R> RightReverse(Polynom<T, R> a)
        {

            if (a.Degree != 0)
                throw new DivideByZeroException(
                    String.Format("The polynom {0} isn't invertible on the ring {1}.", a, this));

            var _reverse = this.FRing.RightReverse(a.GetValue(this.FRing.Zero));
            return new Polynom<T, R>(this.FRing, new T[] { _reverse });
        }


        public Polynom<T, R> LeftReverse(Polynom<T, R> a)
        {

            if (a.Degree != 0)
                throw new DivideByZeroException(
                    String.Format("The polynom {0} isn't invertible on the ring {1}.", a, this));

            var _reverse = this.FRing.LeftReverse(a.GetValue(this.FRing.Zero));
            return new Polynom<T, R>(this.FRing, new T[] { _reverse });
        }

        public bool IsNaN(Polynom<T, R> a)
        {
            return false;
        }
    }
}
