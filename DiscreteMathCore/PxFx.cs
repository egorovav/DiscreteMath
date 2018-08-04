using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class PxFx<T, R> : IRing<Polynom<T, R>> where R : IRing<T>
    {
        private R FRing;
        private Polynom<T, R> FMode;

        public PxFx(R aRing, Polynom<T, R> aMode)
        {
            this.FRing = aRing;
            this.FMode = aMode;
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
            var _prod = a * b;
            if (_prod.Degree >= this.FMode.Degree)
            {
                var _rem = new List<Polynom<T, R>>();
                _prod.Div(this.FMode, _rem);
                _prod = _rem.Last();
            }

            return _prod;
        }

        public Polynom<T, R> Reverse(Polynom<T, R> a)
        {
            if (a == this.Zero)
                throw new DivideByZeroException(
                    String.Format("The polynom {0} isn't invertible on the ring {1}.", a, this));

            Polynom<T, R> u, v = null;
            var _gcd = Polynom<T, R>.GetGcdEx(a, this.FMode, out u, out v);
            if (!_gcd.Equals(this.One))
                throw new DivideByZeroException(
                    String.Format("The polynom {0} isn't invertible on the ring {1}.", a, this));

            if (u.Degree >= this.FMode.Degree)
            {
                List<Polynom<T, R>> _rem = new List<Polynom<T, R>>();
                u.Div(this.FMode, _rem);
                u = _rem.Last();
            }

            return u;
        }

        public Polynom<T, R> Sum(Polynom<T, R> a, Polynom<T, R> b)
        {
            return a + b;
        }

        public override string ToString()
        {
            return String.Format("{0}[x]/{1}", this.FRing, this.FMode);
        }

        public Polynom<T, R> RightReverse(Polynom<T, R> a)
        {
            return this.Reverse(a);
        }

        public Polynom<T, R> LeftReverse(Polynom<T, R> a)
        {
            return this.Reverse(a);
        }

        public bool IsNaN(Polynom<T, R> a)
        {
            return false;
        }
    }
}
