using DiscreteMathCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolynomR = DiscreteMathCore.Polynom<DiscreteMathCore.Q, DiscreteMathCore.Rational>;
using DivInputR = System.Tuple<DiscreteMathCore.Polynom<DiscreteMathCore.Q, DiscreteMathCore.Rational>, 
    DiscreteMathCore.Polynom<DiscreteMathCore.Q, DiscreteMathCore.Rational>,
    DiscreteMathCore.Polynom<DiscreteMathCore.Q, DiscreteMathCore.Rational>,
    DiscreteMathCore.Polynom<DiscreteMathCore.Q, DiscreteMathCore.Rational>>;

using DivInputZn = System.Tuple<DiscreteMathCore.Polynom<long, DiscreteMathCore.ZnRing>,
    DiscreteMathCore.Polynom<long, DiscreteMathCore.ZnRing>,
    DiscreteMathCore.Polynom<long, DiscreteMathCore.ZnRing>,
    DiscreteMathCore.Polynom<long, DiscreteMathCore.ZnRing>>;

using PolynomM = DiscreteMathCore.Polynom<DiscreteMathCore.Matrix<long, DiscreteMathCore.ZnRing>,
    DiscreteMathCore.MRing<long, DiscreteMathCore.ZnRing>>;

using PolynomPxFx = DiscreteMathCore.Polynom<
        DiscreteMathCore.Polynom<long, DiscreteMathCore.ZnRing>, 
        DiscreteMathCore.PxFx<long, DiscreteMathCore.ZnRing>
    >;

namespace DiscreteMathCoreTests
{
    [TestClass]
    public class PolynomUnitTest
    {
        [TestMethod]
        public void PolynomMult_Test()
        {
            var _rational = new Rational();
            var _input = new List<Tuple<PolynomR, PolynomR, PolynomR>>();
            var _p11 = new PolynomR(_rational, new Q[] { 1, 1 });
            var _p12 = new PolynomR(_rational, new Q[] { -1, 1 });
            var _res1 = new PolynomR(_rational, new Q[] { -1, 0, 1 });
            _input.Add(new Tuple<PolynomR, PolynomR, PolynomR>(_p11, _p12, _res1));

            foreach (var t in _input)
            {
                Assert.AreEqual(t.Item3, t.Item1 * t.Item2, String.Format("Input: {0}, {1}", t.Item1, t.Item2));
            }

            var _GF2 = new ZnRing(2);
            var _input2 = new List<Tuple<Polynom<long, ZnRing>, Polynom<long, ZnRing>, Polynom<long, ZnRing>>>();
            var _p21 = new Polynom<long, ZnRing>(_GF2, new long[] { 1, 1 });
            var _p22 = new Polynom<long, ZnRing>(_GF2, new long[] { 1, 1 });
            var _res2 = new Polynom<long, ZnRing>(_GF2, new long[] { 1, 0, 1 });
            _input2.Add(new Tuple<Polynom<long, ZnRing>, Polynom<long, ZnRing>, Polynom<long, ZnRing>>(_p21, _p22, _res2));

            foreach (var t in _input2)
            {
                Assert.AreEqual(t.Item3, t.Item1 * t.Item2, String.Format("Input: {0}, {1}", t.Item1, t.Item2));
            }
        }

        [TestMethod]
        public void PolynomDiv_Test()
        {
            var _rational = new Rational();
            var _input = new List<DivInputR>();

            var _num1 = new PolynomR(_rational, new Q[] { 1, 1, 2 });
            var _denum1 = new PolynomR(_rational, new Q[] { 1, 2 });
            var _quot1 = new PolynomR(_rational, new Q[] { 0, 1 });
            var _rem1 = new PolynomR(_rational, new Q[] { 1 });
            _input.Add(new DivInputR(_num1, _denum1, _quot1, _rem1));

            var _num3 = new PolynomR(_rational, new Q[] { 4, 4, 5, -2, -1, -2, 1 });
            var _denum3 = new PolynomR(_rational, new Q[] { -2, 1 });
            var _quot3 = new PolynomR(_rational, new Q[] { -2, -3, -4, -1, 0, 1 });
            var _rem3 = new PolynomR(_rational, -1);
            _input.Add(new DivInputR(_num3, _denum3, _quot3, _rem3));

            var _num4 = new PolynomR(_rational, new Q[] { 2, -1, -3 });
            var _denum4 = new PolynomR(_rational, new Q[] { new Q(-11, 9), new Q(16, 9) });
            var _quot4 = new PolynomR(_rational, new Q[] { new Q(-441, 256), new Q(-27, 16) });
            var _rem4 = new PolynomR(_rational, new Q[] { new Q(-27, 256) });
            _input.Add(new DivInputR(_num4, _denum4, _quot4, _rem4));

            foreach (var t in _input)
            {
                List<PolynomR> _rem = new List<PolynomR>();
                var _quot = t.Item1.Div(t.Item2, _rem);
                var _res = _quot * t.Item2 + _rem[_rem.Count - 1];
                var _inputData = String.Format("Input num=({0}); denum=({1})", t.Item1, t.Item2);
                Assert.AreEqual(t.Item3, _quot, _inputData);
                Assert.AreEqual(t.Item4, _rem.Last(), _inputData);
                Assert.AreEqual(t.Item1, _res, _inputData);
            }

            var _GF2 = new ZnRing(2);
            var _input2 = new List<DivInputZn>();

            var _num2 = new Polynom<long, ZnRing>(_GF2, new long[] { 1, 1, 0, 0, 0, 1 });
            var _denum2 = new Polynom<long, ZnRing>(_GF2, new long[] { 1, 0, 0, 1, 1 });
            var _quot2 = new Polynom<long, ZnRing>(_GF2, new long[] { 1, 1 });
            var _rem2 = new Polynom<long, ZnRing>(_GF2, new long[] { 0, 0, 0, 1 });
            _input2.Add(new DivInputZn(_num2, _denum2, _quot2, _rem2));

            foreach (var t in _input2)
            {
                List<Polynom<long, ZnRing>> _rem = new List<Polynom<long, ZnRing>>();
                var _quot = t.Item1.Div(t.Item2, _rem);
                var _res = _quot * t.Item2 + _rem.Last();
                var _inputData = String.Format("Input num=({0}); denum=({1})", t.Item1, t.Item2);
                Assert.AreEqual(t.Item3, _quot, _inputData);
                Assert.AreEqual(t.Item4, _rem.Last(), _inputData);
                Assert.AreEqual(t.Item1, _res, _inputData);
            }
        }

        [TestMethod]
        public void PolynomDivOnRing_Test()
        {
            var _GF2 = new ZnRing(2);
            var _mring = new MRing<long, ZnRing>(_GF2, 2);

            var _num10 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 0, 1 }, { 1, 0 } });
            var _num11 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 0, 1 }, { 0, 0 } });
            var _num12 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 0, 0 }, { 1, 1 } });
            var _num13 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 1, 1 }, { 0, 0 } });
            var _num1 = new PolynomM(_mring, new Matrix<long, ZnRing>[] { _num10, _num11, _num12, _num13 });

            var _denum10 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 0, 0 }, { 1, 0 } });
            var _denum11 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 1, 0 }, { 0, 0 } });
            var _denum12 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 1, 1 }, { 1, 0 } });
            var _denum1 = new PolynomM(_mring, new Matrix<long, ZnRing>[] { _denum10, _denum11, _denum12 });

            var _lquot10 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 1, 1 }, { 1, 1 } });
            var _lquot11 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 0, 0 }, { 1, 1 } });
            var _lquot1 = new PolynomM(_mring, new Matrix<long, ZnRing>[] { _lquot10, _lquot11 });

            var _lrem10 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 0, 1 }, { 0, 1 } });
            var _lrem11 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 1, 0 }, { 0, 0 } });
            var _lrem1 = new PolynomM(_mring, new Matrix<long, ZnRing>[] { _lrem10, _lrem11 });

            var _rem = new List<PolynomM>();
            var _quot = _num1.LeftDiv(_denum1, _rem);
            var _res = _denum1 * _quot + _rem.Last();
            var _inputData = String.Format("Input num={0}; denum={1}", _num1, _denum1);

            Assert.AreEqual(_lquot1, _quot, String.Format("Left quot: {0}", _inputData));
            Assert.AreEqual(_lrem1, _rem.Last(), String.Format("Left rem: {0}", _inputData));
            Assert.AreEqual(_num1, _res, String.Format("Left res: {0}", _inputData));

            var _rquot10 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 0, 1 }, { 1, 0 } });
            var _rquot11 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 1, 0 }, { 0, 0 } });
            var _rquot1 = new PolynomM(_mring, new Matrix<long, ZnRing>[] { _rquot10, _rquot11 });

            var _rrem10 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 1, 1 }, { 1, 0 } });
            var _rrem11 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 0, 1 }, { 1, 0 } });
            var _rrem1 = new PolynomM(_mring, new Matrix<long, ZnRing>[] { _rrem10, _rrem11 });

            _rem = new List<PolynomM>();
            _quot = _num1.RightDiv(_denum1, _rem);
            _res = _quot * _denum1 + _rem.Last();

            Assert.AreEqual(_rquot1, _quot, String.Format("Right quot: {0}", _inputData));
            Assert.AreEqual(_rrem1, _rem.Last(), String.Format("Right rem: {0}", _inputData));
            Assert.AreEqual(_num1, _res, String.Format("Right res: {0}", _inputData));
        }

        [TestMethod]
        public void PolynomGcdEx_Test()
        {
            var _rational = new Rational();
            var _input = new List<Tuple<PolynomR, PolynomR>>();
            var _p11 = new PolynomR(_rational, new Q[] { -5, 8, -3, -4, 2, 0, 1 });
            var _p12 = new PolynomR(_rational, new Q[] { 1, -1, 1, 0, 0, 1 });
            _input.Add(new Tuple<PolynomR, PolynomR>(_p11, _p12));

            var _p21 = new PolynomR(_rational, new Q[] { 1, 1, 0, 1, 0, 1 });
            var _p22 = new PolynomR(_rational, new Q[] { 1, 0, 0, 0, 1 });
            _input.Add(new Tuple<PolynomR, PolynomR>(_p21, _p22));

            foreach (var t in _input)
            {
                PolynomR a, b;
                var _gcd = PolynomR.GetGcdEx(t.Item1, t.Item2, out a, out b);
                var _sum = t.Item1 * a + t.Item2 * b;
                Assert.AreEqual(_gcd, _sum, String.Format("Input: ({0}); ({1})", t.Item1, t.Item2));
            }

            var _GF2 = new ZnRing(2);
            var _p31 = new Polynom<long, ZnRing>(_GF2, new long[] { 1, 1, 0, 0, 0, 1 });
            var _p32 = new Polynom<long, ZnRing>(_GF2, new long[] { 1, 0, 0, 1, 1 });

            Polynom<long, ZnRing> a3, b3;
            var _gcd3 = Polynom<long, ZnRing>.GetGcdEx(_p31, _p32, out a3, out b3);
            Assert.AreEqual(_gcd3, _p31 * a3 + _p32 * b3, String.Format("Input: ({0}); ({1})", _p31, _p32));
        }


        [TestMethod]
        public void PolynomGcd_Test()
        {
            var _rational = new Rational();
            var _input = new List<Tuple<PolynomR, PolynomR, PolynomR>>();
            var _p11 = new PolynomR(_rational, new Q[] { -5, 8, -3, -4, 2, 0, 1 });
            var _p12 = new PolynomR(_rational, new Q[] { 1, -1, 1, 0, 0, 1 });
            var _gcd1 = new PolynomR(_rational, new Q[] { 1, -1, 0, 1 });
            _input.Add(new Tuple<PolynomR, PolynomR, PolynomR>(_p11, _p12, _gcd1));

            var _p21 = new PolynomR(_rational, new Q[] { 1, 1, 0, 1, 0, 1 });
            var _p22 = new PolynomR(_rational, new Q[] { 1, 0, 0, 0, 1 });
            var _gcd2 = new PolynomR(_rational, new Q[] { 1 });
            _input.Add(new Tuple<PolynomR, PolynomR, PolynomR>(_p21, _p22, _gcd2));

            foreach (var t in _input)
            {
                Assert.AreEqual(t.Item3, PolynomR.GetGcd(t.Item1, t.Item2),
                    String.Format("Input: ({0}); ({1})", t.Item1, t.Item2));
            }

            var _GF2 = new ZnRing(2);
            var _p31 = new Polynom<long, ZnRing>(_GF2, new long[] { 1, 1, 0, 0, 0, 1 });
            var _p32 = new Polynom<long, ZnRing>(_GF2, new long[] { 1, 0, 0, 1, 1 });
            var _gcd3 = new Polynom<long, ZnRing>(_GF2, new long[] { 1 });

            Assert.AreEqual(_gcd3, Polynom<long, ZnRing>.GetGcd(_p31, _p32),
                String.Format("Input: ({0}); ({1})", _p31, _p32));
        }

        [TestMethod]
        public void PolynomGcdPxFx_Test()
        {
            var _GF2 = new ZnRing(2);
            var _mod = new Polynom<long, ZnRing>(_GF2, new long[] { 1, 1, 1 });
            var _ring = new PxFx<long, ZnRing>(_GF2, _mod);
            var _input = new List<Tuple<PolynomPxFx, PolynomPxFx, PolynomPxFx>>();

            var _p110 = new Polynom<long, ZnRing>(_GF2, new long[] { 1, 1 });
            var _p111 = new Polynom<long, ZnRing>(_GF2, new long[] { 0, 1 });
            var _p11 = new PolynomPxFx(_ring, new Polynom<long, ZnRing>[] { _p110, _p111 });

            var _p120 = new Polynom<long, ZnRing>(_GF2, new long[] { 1, 1 });
            var _p121 = new Polynom<long, ZnRing>(_GF2, new long[] { 0, 1 });
            var _p12 = new PolynomPxFx(_ring, new Polynom<long, ZnRing>[] { _p110, _p111 });
            var _gcd1 = new PolynomPxFx(_ring, new Polynom<long, ZnRing>[] { _p110, _p111 });
            _input.Add(new Tuple<PolynomPxFx, PolynomPxFx, PolynomPxFx>(_p11, _p12, _gcd1));

            var _p210 = new Polynom<long, ZnRing>(_GF2, new long[] { 1, 1 });
            var _p211 = new Polynom<long, ZnRing>(_GF2, new long[] { 0, 1 });
            var _p21 = new PolynomPxFx(_ring, new Polynom<long, ZnRing>[] { _p210, _p211 });

            var _p22 = _p21 * _p21;
            var _gcd2 = new PolynomPxFx(_ring, new Polynom<long, ZnRing>[] { _p210, _p211 });
            _input.Add(new Tuple<PolynomPxFx, PolynomPxFx, PolynomPxFx>(_p21, _p22, _gcd2));
        }

        [TestMethod]
        public void PolynomDerivative_Test()
        {
            var _rational = new Rational();
            var _input = new List<Tuple<PolynomR, PolynomR>>();
            var _p1 = new PolynomR(_rational, new Q[] { 1, 0, 1 });
            var _p2 = new PolynomR(_rational, new Q[] { 0, 2 });
            _input.Add(new Tuple<PolynomR, PolynomR>(_p1.Derivative, _p2));

            var _p11 = new PolynomR(_rational, new Q[] { 1, 0, 0, 1 });
            var _p21 = new PolynomR(_rational, new Q[] { 0, 0, 3 });
            _input.Add(new Tuple<PolynomR, PolynomR>(_p11.Derivative, _p21));

            var _p = new PolynomR(_rational, new Q[] { 1 });
            _input.Add(new Tuple<PolynomR, PolynomR>(_p.Derivative, new Rx<Q, Rational>(_rational).Zero));

            foreach (var t in _input)
            {
                Assert.AreEqual(t.Item1, t.Item2, String.Format("Input: ({0}); ({1})", t.Item1, t.Item2));
            }
        }

        [TestMethod]
        public void Factorisation_Test()
        {
            var _ring = new ZnRing(5);
            var _mod = new Polynom<long, ZnRing>(_ring, 5);
            var _pfx = new PxFx<long, ZnRing>(_ring, _mod);

            var _input = new List<Tuple<Polynom<long, ZnRing>, Polynom<long, ZnRing>>>();

            foreach (var _pol in _pfx)
            {
                var _mults = _pol.Factorisation();
                var _prod = new Polynom<long, ZnRing>(_ring, new long[] { 1 });
                foreach (var _pp in _mults)
                    _prod = _pfx.Prod(_prod, _pp);

                _input.Add(new Tuple<Polynom<long, ZnRing>, Polynom<long, ZnRing>>(_pol, _prod));
            }

            foreach (var t in _input)
            {
                Assert.AreEqual(t.Item1, t.Item2, String.Format("Input: {0}", t.Item1));
            }
        }

        [TestMethod]
        public void FactorisationPxFx_Test()
        {
            var p = 3;
            var _GF3 = new ZnRing(p);
            var _ring = new PxFx<long, ZnRing>(
                _GF3, new Polynom<long, ZnRing>(_GF3, new long[] { 2, 2, 1 }));
            var _mod = new Polynom<Polynom<long, ZnRing>, PxFx<long, ZnRing>>(_ring,
                new Polynom<long, ZnRing>[] {
                    _ring.One, _ring.One, _ring.Zero, _ring.One,
                    new Polynom<long, ZnRing>(_GF3, new long[] { 0, 1 })
                });
            var _pfx = new PxFx<Polynom<long, ZnRing>, PxFx<long, ZnRing>>(_ring, _mod);

            foreach (var _pol in _pfx)
            {
                //var _mults = _pol.Factorisation();
                var _mults = Polynom<long, ZnRing>.FactorisationPxFx(_pol);
                var _prod = new Polynom<Polynom<long, ZnRing>, PxFx<long, ZnRing>>(
                    _ring, new Polynom<long, ZnRing>[] { _ring.One });
                foreach (var _mult in _mults)
                {
                    _prod *= _mult;
                }

                if (!_pol.Equals(_prod))
                    Assert.AreEqual(_pol, _prod, String.Format("Input: {0}", _pol.TexString("y", false)));
            }
        }
    }
}
