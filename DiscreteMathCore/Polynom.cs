using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class Polynom<T, R> where R : RingBase<T>
    {
        private R FRing;
        private T[] FCoeffs;

        public Polynom(R aRing, IEnumerable<T> aCoeffs)
        {
            this.FRing = aRing;

            var _arr = aCoeffs.ToArray();

            var _coeffs = new List<T>();
            for(var i = _arr.Length - 1; i >= 0; --i)
            {
                if (_arr[i] == null)
                    _arr[i] = aRing.Zero;

                if(_arr[i].Equals(aRing.Zero) && _coeffs.Count == 0)
                {
                    continue;
                }

                _coeffs.Add(_arr[i]);
            }

            _coeffs.Reverse();
            this.FCoeffs = _coeffs.ToArray();
        }

        public Polynom(Polynom<T, R> aPolynom)
        {
            this.FRing = aPolynom.FRing;

            if (aPolynom.Degree >= 0)
            {
                this.FCoeffs = new T[aPolynom.Degree + 1];
                Array.Copy(aPolynom.FCoeffs, this.FCoeffs, aPolynom.Degree + 1);
            }
        }

        public Polynom(R aRing, int aDegree)
        {
            this.FRing = aRing;

            if (aDegree >= 0)
            {
                this.FCoeffs = Enumerable.Repeat<T>(aRing.Zero, aDegree + 1).ToArray();
                this.FCoeffs[aDegree] = aRing.One;
            }

        }

        public int Degree
        {
            get
            {
                if (this.FCoeffs == null)
                    return -1;
                return this.FCoeffs.Length - 1;
            }
        }

        public void Mult(T a)
        {
            //for (int i = 0; i <= this.Degree; ++i)
            //{
            //    this.FCoeffs[i] = this.FRing.Prod(this.FCoeffs[i], a);
            //}

            var _index = this.Degree;
            while (_index >= 0 && this.FRing.Equals(this.FCoeffs[_index], this.FRing.Zero))
            {
                _index--;
            }

            var _coeffs = new T[_index + 1];

            for (int i = 0; i <= _index; ++i)
            {
                _coeffs[i] = this.FRing.Prod(this.FCoeffs[i], a);
            }

            this.FCoeffs = _coeffs;
        }

        public static Polynom<T, R> operator +(Polynom<T, R> Ax, Polynom<T, R> Bx)
        {
            int _degree = -1;
            int _maxDegree = Math.Max(Ax.Degree, Bx.Degree);
            var _sum = new List<T>();
            for (int i = 0; i <= _maxDegree; ++i)
            {
                T ai = Ax.FRing.Zero;
                T bi = Bx.FRing.Zero;
                if (i <= Ax.Degree)
                    ai = Ax.FCoeffs[i];
                if (i <= Bx.Degree)
                    bi = Bx.FCoeffs[i];
                T si = Ax.FRing.Sum(ai, bi);
                if (!Ax.FRing.Equals(si, Ax.FRing.Zero))
                    _degree = i;
                _sum.Add(si);
            }

            var _coeffs = _sum.GetRange(0, _degree + 1);
            return new Polynom<T, R>(Ax.FRing, _coeffs);
        }

        public static Polynom<T, R> operator *(Polynom<T, R> Ax, Polynom<T, R> Bx)
        {
            if (Ax.Degree < 0 || Bx.Degree < 0)
                return new Polynom<T, R>(Ax.FRing, -1);

            int deg_prod = Ax.Degree + Bx.Degree;
            var _prod = new T[deg_prod + 1];

            for (int i = 0; i <= deg_prod; ++i)
            {
                T pi = Ax.FRing.Zero;
                for (int j = 0; j <= i; j++)
                {
                    T aj = j <= Ax.Degree ? Ax.FCoeffs[j] : Ax.FRing.Zero;
                    T bi_j = i - j <= Bx.Degree ? Bx.FCoeffs[i - j] : Bx.FRing.Zero;
                    pi = Ax.FRing.Sum(pi, Ax.FRing.Prod(aj, bi_j));
                }

                _prod[i] = pi;
            }
            return new Polynom<T, R>(Ax.FRing, _prod);
        }

        public static Polynom<T, R> operator -(Polynom<T, R> Ax, Polynom<T, R> Bx)
        {
            var _bx = new Polynom<T, R>(Bx);
            return Ax + (-_bx);
        }

        public static Polynom<T, R> operator -(Polynom<T, R> Ax)
        {
            var _minusOne = Ax.FRing.Opposite(Ax.FRing.One);
            var _opposite = new Polynom<T, R>(Ax);
            _opposite.Mult(_minusOne);
            return _opposite;
        }

        public static bool operator ==(Polynom<T, R> Ax, Polynom<T, R> Bx)
        {
            if ((object)Ax == null || (object)Bx == null)
            {
                return (object)Ax == null && (object)Bx == null;
            }

            return Ax.Equals(Bx);
        } 

        public static bool operator !=(Polynom<T, R> Ax, Polynom<T, R> Bx)
        {
            return !(Ax == Bx);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is Polynom<T, R>)
            {
                var p = (Polynom<T, R>)obj;
                if (this.Degree != p.Degree)
                    return false;
                for(int i = 0; i <= p.Degree; ++i)
                {
                    if (!this.FRing.Equals(this.FCoeffs[i], p.FCoeffs[i]))
                        return false;
                }
                return true;
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            var _res = this.FRing.Zero.GetHashCode();
            foreach (var c in this.FCoeffs)
                _res ^= c.GetHashCode();
            return _res;
        }

        private Polynom<T, R> Div(Polynom<T, R> aDenumerator, List<Polynom<T, R>> aRemainder, bool aIsRight)
        {
            if (aRemainder == null)
                aRemainder = new List<Polynom<T, R>>();

            var quot = new Polynom<T, R>(this.FRing, -1);
            if (this.Degree < aDenumerator.Degree)
            {
                aRemainder.Add(this);
                return quot;
            }

            int deg_quot = this.Degree - aDenumerator.Degree;

            var _rem = new Polynom<T, R>(this);

            var _minusOne = this.FRing.Opposite(this.FRing.One);
            var c = this.FRing.Reverse(aDenumerator.FCoeffs[aDenumerator.Degree]);
            while (_rem.Degree >= aDenumerator.Degree)
            {
                int p = _rem.Degree - aDenumerator.Degree;
                var d = new T[p + 1];
                if(aIsRight)
                    d[p] = this.FRing.Prod(_rem.FCoeffs[_rem.Degree], c);
                else
                    d[p] = this.FRing.Prod(c, _rem.FCoeffs[_rem.Degree]);
                var dp = new Polynom<T, R>(this.FRing, d);
                quot += dp;
                var dd = new Polynom<T, R>(this.FRing, -1);
                if (aIsRight)
                    dd = dp * aDenumerator;
                else
                    dd = aDenumerator * dp;

                aRemainder.Add(new Polynom<T, R>(dd));

                dd.Mult(_minusOne);
                _rem += dd;

                aRemainder.Add(_rem);
            }

            if(quot.Degree < deg_quot)
            {
                var _shift = new Polynom<T, R>(this.FRing, deg_quot - quot.Degree);
                quot = quot * _shift;
            }

            return quot;
        }

        public Polynom<T, R> Div(Polynom<T, R> aDenumerator, List<Polynom<T, R>> aRemainder)
        {
            return this.LeftDiv(aDenumerator, aRemainder);
        }

        public Polynom<T, R> LeftDiv(Polynom<T, R> aDenumerator, List<Polynom<T, R>> aRemainder)
        {
            return this.Div(aDenumerator, aRemainder, false);
        }

        public Polynom<T, R> RightDiv(Polynom<T, R> aDenumerator, List<Polynom<T, R>> aRemainder)
        {
            return this.Div(aDenumerator, aRemainder, true);
        }

        public string MakeDivTexString(
            Polynom<T, R> aDenumerator, Polynom<T, R> aQuot, List<Polynom<T, R>> aRemainder, string aVarName)
        {
            var _sb = new StringBuilder();

            _sb.AppendLine(@"\extrarowheight=2pt");
            _sb.AppendLine(@"\arraycolsep=0.05em");
            _sb.AppendFormat(@"\begin{{array}}{{");
            for (int i = 0; i <= this.Degree; ++i)
            {
                _sb.Append("r");
            }
            _sb.Append(@"@{\,}r|l}");
            _sb.AppendLine();
            _sb.Append(this.TexString(aVarName, true));
            _sb.Append(@"&&\,");
            _sb.Append(aDenumerator.TexString(aVarName, false));
            _sb.Append(@"\\");
            _sb.AppendLine();
            _sb.AppendFormat("\\cline{{{0}-{0}}}", this.Degree + 3);

            var _secondString = String.Format("{0}&&\\,{1}\\\\", 
                aRemainder[0].TexString(aVarName, true).TrimEnd('\\'), aQuot.TexString(aVarName, false));
            _sb.AppendLine(_secondString);

            for (var i = 1; i < aRemainder.Count; ++i)
            {
                if(i % 2 == 1)
                {
                    int s = this.Degree - aRemainder[i - 1].Degree + 1;
                    _sb.AppendLine(String.Format("\\cline{{{0}-{1}}}", s, s + aDenumerator.Degree));
                }

                int _shift = this.Degree - aRemainder[i].Degree + 1;
                if (aRemainder[i].Degree < 0)
                    _shift--;

                var _pref = new String('&', _shift - 1);
                _sb.AppendLine(String.Format("{0}{1}\\\\", _pref, aRemainder[i].TexString(aVarName, true)));
            }

            _sb.AppendLine(@"\end{array}");

            return _sb.ToString();
        }

        public override string ToString()
        {
            if (this.Degree < 0)
                return "0";

            var _str = String.Join<T>(", ", this.FCoeffs);
            if (this.FCoeffs == null)
                _str = this.FRing.Zero.ToString();
            return _str;
        }

        public string TexString(string aVar, bool isTabular)
        {
            if (this.Degree < 0)
                return "0";

            var _sb = new StringBuilder();
            for (int i = this.Degree; i >= 0; --i)
            {
                var c = this.FCoeffs[i];
                if (!this.FRing.Equals(c, this.FRing.Zero))
                {
                    var _cs = this.FRing.GetTexString(c);
                    if (i != this.Degree)
                    {
                        if (!_cs.StartsWith("-"))
                            _sb.Append("+");
                    }

                    if (i != 0)
                    {
                        if (this.FRing.Equals(c, this.FRing.One))
                            _cs = String.Empty;
                        else if (this.FRing.Size < 0 &&
                            this.FRing.Equals(c, this.FRing.Opposite(this.FRing.One)))
                            _cs = "-";
                    }

                    _sb.Append(_cs);
                    

                    if (i > 0)
                        _sb.Append(aVar);

                    if (i > 1)
                        _sb.AppendFormat("^{0}", i);
                }

                if (isTabular && i != 0)
                    _sb.Append("&");
                
            }
            return _sb.ToString();
        }

        public string TexString()
        {
            return TexString("x", false);
        }

        public static Polynom<T, R> GetGcd(Polynom<T, R> a, Polynom<T, R> b)
        {
            var _alocal = new Polynom<T, R>(a);

            var _zero = new Polynom<T, R>(_alocal.FRing, -1);
            var _one = new Polynom<T, R>(_alocal.FRing, 0);

            var gcd = _zero;

            if (_alocal.Degree < 0)
            {
                gcd = b;
            }
            else if (b.Degree < 0)
            {
                gcd = _alocal;
            }
            else
            {
                var r1 = b;

                var r2 = new List<Polynom<T, R>>();
                var _quot = _alocal.Div(b, r2);

                gcd = b;

                var q = _zero;

                var temp = new List<Polynom<T, R>>();
                var temp1 = _zero;

                while (r2.Last().Degree >= 0)
                {
                    gcd = r2.Last();
                    q = r1.Div(r2.Last(), temp);
                    //r1 = r2.Last();
                    r1 = gcd;
                    r2 = temp;
                }
            }

            var _major = gcd.FCoeffs[gcd.Degree];
            if (!_alocal.FRing.Equals(_major, _alocal.FRing.One))
            {
                var _major_1 = _alocal.FRing.Reverse(_major);
                gcd.Mult(_major_1);
            }

            return gcd;
        }

        public static Polynom<T, R> GetGcdEx(Polynom<T, R> a, Polynom<T, R> b,
           out Polynom<T, R> u, out Polynom<T, R> v)
        {
            var _alocal = new Polynom<T, R>(a);

            var _zero = new Polynom<T, R>(_alocal.FRing, -1);
            var _one = new Polynom<T, R>(_alocal.FRing, 0);

            var gcd = _zero;

            if (_alocal.Degree < 0)
            {
                u = _zero;
                v = (b.Degree < 0 ? _zero : _one);
                gcd = b;
            }
            else if (b.Degree < 0)
            {
                v = _zero;
                u = (_alocal.Degree < 0 ? _zero : _one);
                gcd = _alocal;
            }
            else
            {
                var r1 = b;

                var r2 = new List<Polynom<T, R>>();
                var _quot = _alocal.Div(b, r2);

                gcd = b;

                var u1 = _one;
                var u2 = _zero;
                var v1 = _one;
                var v2 = _one;
                var q = _zero;

                if (r2.Last().Degree < 0)
                    u1 = _zero;
                else
                    v1 = -_quot;

                var temp = new List<Polynom<T, R>>();
                var temp1 = _zero;

                while (r2.Last().Degree >= 0)
                {
                    gcd = r2.Last();
                    q = r1.Div(r2.Last(), temp);
                    //r1 = r2.Last();
                    r1 = gcd;
                    r2 = temp;
                    temp1 = u1;
                    u1 = u2 - u1 * q;
                    u2 = temp1;
                    temp1 = v1;
                    v1 = v2 - v1 * q;
                    v2 = temp1;
                }

                u = u2;
                v = v2;
            }

            var _major = gcd.FCoeffs[gcd.Degree];
            if (!_alocal.FRing.Equals(_major, _alocal.FRing.One))
            {
                var _major_1 = _alocal.FRing.Reverse(_major);
                gcd.Mult(_major_1);
            }

            var _sum = u * a + v * b;
            _major = _sum.FCoeffs[_sum.Degree];
            if(!_alocal.FRing.Equals(_major, _alocal.FRing.One))
            {
                var _major_1 = _alocal.FRing.Reverse(_major);
                u.Mult(_major_1);
                v.Mult(_major_1);
            }

            return gcd;
        }

        public T GetValue(T aArg)
        {
            var p = this.FRing.One;
            var _res = this.FRing.Zero;
            for (var i = 0; i <= this.Degree; ++i)
            {
                _res = this.FRing.Sum(_res, this.FRing.Prod(this.FCoeffs[i], p));
                p = this.FRing.Prod(p, aArg);
            }
            return _res;
        }

        public Polynom<Matrix<T, R>, MRing<T, R>>  GetMatrixPolynom(int aMatrixSize)
        {

                var _mring = new MRing<T, R>(this.FRing, aMatrixSize);
                var _mcoeffs = new Matrix<T, R>[this.FCoeffs.Length];
                for (var i = 0; i < this.FCoeffs.Length; ++i)
                {
                    _mcoeffs[i] = _mring.One;
                    _mcoeffs[i].Mult(this.FCoeffs[i]);
                }

                var _m_xi = new Polynom<Matrix<T, R>, MRing<T, R>>(_mring, _mcoeffs);
                return _m_xi;
        }

        public Polynom<T, R> Derivative
        {
            get
            {
                if (this.Degree == 0)
                    return new Rx<T, R>(this.FRing).Zero;

                var _coeffs = new List<T>();
                for(var i = this.Degree - 1; i >= 0; i--)
                {
                    var _coeff = this.FRing.Mult(this.FCoeffs[i + 1], i + 1);
                    if(_coeffs.Count == 0 && _coeff.Equals(this.FRing.Zero))
                    {
                        continue;
                    }
                    else
                    {
                        _coeffs.Add(_coeff);
                    }
                }
                _coeffs.Reverse();
                var _derivative = new Polynom<T, R>(this.FRing, _coeffs);
                return _derivative;
            }
        }

        public static List<Polynom<Polynom<long, ZnRing>, PxFx<long, ZnRing>>>
            FactorisationPxFx(Polynom<Polynom<long, ZnRing>, PxFx<long, ZnRing>> aPolynom)
        {
            var _finiteRing = aPolynom.FRing.Ring;
            var _coeffs = new long[aPolynom.Degree + 1];
            for(var i = 0; i <= aPolynom.Degree; i++)
            {
                _coeffs[i] = aPolynom.FRing.Values.IndexOf(aPolynom.FCoeffs[i]);
            }

            var _polynom = new Polynom<long, FiniteRing>(_finiteRing, _coeffs);
            var _res = _polynom.Factorisation();

            var _res1 = new List<Polynom<Polynom<long, ZnRing>, PxFx<long, ZnRing>>>();
            foreach(var _pol in _res)
            {
                var _cs = new Polynom<long, ZnRing>[_pol.Degree + 1];
                for(var i = 0; i <= _pol.Degree; i++)
                {
                    _cs[i] = aPolynom.FRing.Values[(int)_pol.FCoeffs[i]];
                }
                _res1.Add(new Polynom<Polynom<long, ZnRing>, PxFx<long, ZnRing>>(aPolynom.FRing, _cs));
            }

            return _res1;
        }

        private List<Polynom<T, R>> Factorisation(Polynom<T, R> aPolynom)
        {
            if(!(aPolynom.FRing is IEnumerable<T>))
            {
                throw new ArgumentException("Polinom's ring mast be enumerable.");
            }

            var p = (int)aPolynom.FRing.Size;
            var _res = new List<Polynom<T, R>>();
            if (aPolynom.Degree < 2)
                return _res;
            var _pring = new Rx<T, R>(aPolynom.FRing);
            var _derivative = aPolynom.Derivative;
            var _gcd = Polynom<T, R>.GetGcd(aPolynom, _derivative);

            if (_gcd.Degree > 0)
            {
                if (_derivative != _pring.Zero)
                {
                    _res.Add(_gcd);
                    _res.Add(aPolynom.Div(_gcd, null));
                }
                else
                {
                    p = (int)aPolynom.FRing.SimpleSubfieldSize;

                    var c = aPolynom.Degree / p;
                    var _pol = new Polynom<T, R>(aPolynom.FRing, c);
                    for (var i = 0; i <= c; i++)
                    {
                        _pol.FCoeffs[i] = aPolynom.FRing.Pow(
                            aPolynom.FCoeffs[p * i], this.FRing.Size / p);
                    }

                    for (var i = 0; i < p; i++)
                        _res.Add(new Polynom<T, R>(_pol));
                }

                return _res;
            }

            var _aij_vals = new T[aPolynom.Degree, aPolynom.Degree];
            for(var i = 1; i < aPolynom.Degree; i++)
            {
                var _pol = new Polynom<T, R>(aPolynom.FRing, i * p);
                _pol.FCoeffs[i] = aPolynom.FRing.Opposite(aPolynom.FRing.One);
                var _rem = new List<Polynom<T, R>>();
                _pol.Div(aPolynom, _rem);
                var _ai = _rem.Last();

                for (var j = 0; j <= _ai.Degree; j++)
                    _aij_vals[j, i - 1] = _ai.FCoeffs[j];
            }

            var _aij = new Matrix<T, R>(aPolynom.FRing, _aij_vals);

            var _matrix = Matrix<T, R>.SolveLs(_aij);
            var _solvation = new T[_matrix.RowCount + 1];
            _solvation[0] = aPolynom.FRing.Zero;
            bool IsResEmpty = true;
            var _matrixValues = _matrix.Values;
            for (var i = 0; i < _matrix.RowCount; i++)
            {
                if (_solvation[i + 1] == null)
                    _solvation[i + 1] = aPolynom.FRing.Zero;

                for(var j = 0; j < _matrix.ColumnCount; j++)
                    _solvation[i + 1] = aPolynom.FRing.Sum(_solvation[i + 1], _matrixValues[i, j]);

                IsResEmpty = _solvation[i + 1].Equals(aPolynom.FRing.Zero);
            }
            if (!IsResEmpty)
            {
                var _pp = new Polynom<T, R>(aPolynom.FRing, _solvation);

                var _gcd1 = _pring.One;
                var _enumerable = (IEnumerable<T>)this.FRing;

                foreach (var _item in _enumerable)
                {
                    var _ppp = new Polynom<T, R>(_pp);
                    _ppp.FCoeffs[0] = _ppp.FRing.Sum(_item, _ppp.FCoeffs[0]);
                    _gcd1 = Polynom<T, R>.GetGcd(aPolynom, _ppp);
                    if(!_gcd1.Equals(_pring.One))
                    {
                        _res.Add(_gcd1);
                        var _rem = new List<Polynom<T, R>>();
                        var _quot = aPolynom.Div(_gcd1, _rem);
                        _res.Add(_quot);
                        break;
                    }
                }
            }
            return _res;
        }

        public List<Polynom<T, R>> Factorisation()
        {
            var _res = new List<Polynom<T, R>>();

            var _stack = new Stack<Polynom<T, R>>();
            _stack.Push(this);

            while(_stack.Count > 0)
            {
                var _polynom = _stack.Pop();
                var _multipliers = this.Factorisation(_polynom);
                if (_multipliers.Count == 0)
                {
                    _res.Add(_polynom);
                }
                else
                {
                    foreach (var _mult in _multipliers)
                    {
                        _stack.Push(_mult);
                    }
                }
            }

            return _res;
        }
    }
}
