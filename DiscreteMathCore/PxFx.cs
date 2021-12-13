using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class PxFx<T, R> : RingBase<Polynom<T, R>>, IEnumerable<Polynom<T, R>> where R : RingBase<T>
    {
        private R FRing;
        private Polynom<T, R> FMode;

        public PxFx(R aRing, Polynom<T, R> aMode)
        {
            this.FRing = aRing;
            this.FMode = aMode;
        }

        private List<Polynom<T, R>> FValues;
        public List<Polynom<T, R>> Values
        {
            get
            {
                if(this.FValues == null)
                {
                    this.FValues = new List<Polynom<T, R>>();
                    foreach (var _item in this)
                        this.FValues.Add(_item);
                }

                return this.FValues;
            }
        }

        public override Polynom<T, R> One
        {
            get
            {
                return new Polynom<T, R>(this.FRing, 0);
            }
        }

        public override Polynom<T, R> Zero
        {
            get
            {
                return new Polynom<T, R>(this.FRing, -1);
            }
        }

        public override long SimpleSubfieldSize
        {
            get { return this.FRing.Size; }
        }

        public override bool Equals(Polynom<T, R> a, Polynom<T, R> b)
        {
            return a == b;
        }

        public override string GetTexString(Polynom<T, R> a)
        {
            return String.Format("[{0}]", a.TexString());
        }

        public override Polynom<T, R> Opposite(Polynom<T, R> a)
        {
            return -a;
        }

        public override Polynom<T, R> Prod(Polynom<T, R> a, Polynom<T, R> b)
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

        public override Polynom<T, R> InnerReverse(Polynom<T, R> a)
        {
            if (a == this.Zero)
                throw new DivideByZeroException(
                    String.Format("The polynom {0} isn't invertible on the ring {1}.", a, this));

            if(a.Equals(this.One))
            {
                return this.One;
            }

            Polynom<T, R> u, v = null;
            //var _aa = new Polynom<T, R>(a);
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

        public override Polynom<T, R> Sum(Polynom<T, R> a, Polynom<T, R> b)
        {
            return a + b;
        }

        public override string ToString()
        {
            return String.Format("{0}[x]/{1}", this.FRing, this.FMode);
        }

        public override Polynom<T, R> RightReverse(Polynom<T, R> a)
        {
            return this.Reverse(a);
        }

        public override Polynom<T, R> LeftReverse(Polynom<T, R> a)
        {
            return this.Reverse(a);
        }

        public override bool IsNaN(Polynom<T, R> a)
        {
            return false;
        }

        private List<Polynom<T, R>> FModeMultipliers;
        public override bool IsField
        {
            get
            {
                if (!this.FRing.IsField)
                    return false;

                if (this.FModeMultipliers == null)
                    this.FModeMultipliers = this.FMode.Factorisation();

                return this.FModeMultipliers.Count < 2;
            }
        }

        private long FSize;
        public override long Size
        {
            get
            {
                if(this.FSize == 0)
                {
                    this.FSize = (int)Math.Pow(this.FRing.Size, this.FMode.Degree);
                }

                return this.FSize;
            }
        }

        private FiniteRing FFiniteRing;
        public FiniteRing Ring
        {
            get
            {
                if (this.FFiniteRing == null)
                {
                    var _add = new long[this.Size, this.Size];
                    var _mult = new long[this.Size, this.Size];
                    var i = 0;
                    foreach (var _item1 in this.Values)
                    {
                        var j = 0;
                        foreach (var _item2 in this.Values)
                        {
                            _add[i, j] = this.Values.IndexOf(this.Sum(_item1, _item2));
                            _mult[i, j] = this.Values.IndexOf(this.Prod(_item1, _item2));
                            ++j;
                        }
                        ++i;
                    }

                    this.FFiniteRing = new FiniteRing(_add, _mult, this.SimpleSubfieldSize);
                }

                return this.FFiniteRing;
            }
        }

        public IEnumerator<Polynom<T, R>> GetEnumerator()
        {
            return new PxFxEnumerator(this.FRing, this.FMode.Degree);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new PxFxEnumerator(this.FRing, this.FMode.Degree);
        }

        public class PxFxEnumerator : IEnumerator<Polynom<T, R>>
        {
            public PxFxEnumerator(R aRing, long aModeDegree)
            {
                if (!(aRing is IEnumerable<T>))
                    throw new ArgumentException("Polynom's ring must be enumerable.");

                this.FRing = aRing;
                this.FEnumerator = new IEnumerator<T>[aModeDegree];
                for (var i = 0; i < aModeDegree; ++i)
                {
                    this.FEnumerator[i] = ((IEnumerable<T>)aRing).GetEnumerator();
                    if(i > 0)
                    this.FEnumerator[i].MoveNext();
                }

                //this.FEnumerator[0].Reset();
            }

            private R FRing;
            private IEnumerator<T>[] FEnumerator;
            private int FCurrentCoeff;

            public Polynom<T, R> Current
            {
                get
                {
                    return new Polynom<T, R>(this.FRing, this.FEnumerator.Select(x => x.Current));
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return new Polynom<T, R>(this.FRing, this.FEnumerator.Select(x => x.Current));
                }
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                while (this.FCurrentCoeff < this.FEnumerator.Length 
                    && !this.FEnumerator[this.FCurrentCoeff].MoveNext())
                {
                    this.FEnumerator[this.FCurrentCoeff].Reset();
                    this.FEnumerator[this.FCurrentCoeff].MoveNext();
                    this.FCurrentCoeff++;
                }

                if (this.FCurrentCoeff < this.FEnumerator.Length)
                {
                    this.FCurrentCoeff = 0;
                    return true;
                }
                else
                    return false;
            }

            public void Reset()
            {
                this.FCurrentCoeff = 0;
                for (var i = 0; i < this.FEnumerator.Length; ++i)
                {
                    this.FEnumerator[i].Reset();
                    if(i > 0)
                        this.FEnumerator[i].MoveNext();
                }
            }
        }
    }
}
