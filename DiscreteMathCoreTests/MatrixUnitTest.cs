using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using DiscreteMathCore;

namespace DiscreteMathCoreTests
{
    [TestClass]
    public class MatrixUnitTest
    {
        [TestMethod]
        public void MatrixMult_Test()
        {
            var _GF2 = new ZnRing(2);
            var _m1 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 1, 1 }, { 0, 0 } });
            var _m2 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 0, 0 }, { 1, 1 } });

            var _input = new List<Tuple<Matrix<long, ZnRing>, Matrix<long, ZnRing>, Matrix<long, ZnRing>>>();
            _input.Add(new Tuple<Matrix<long, ZnRing>, Matrix<long, ZnRing>, Matrix<long, ZnRing>>(_m1, _m2, _m1));
            _input.Add(new Tuple<Matrix<long, ZnRing>, Matrix<long, ZnRing>, Matrix<long, ZnRing>>(_m2, _m1, _m2));

            //var _mring = new MRing<int, ZnRing>(_GF2, 2);
            foreach (var t in _input)
            {
                var _prod = t.Item1 * t.Item2;
                Assert.AreEqual(t.Item3, _prod,
                    String.Format("Input: {0}, {1}", t.Item1, t.Item2));
            }
        }

        [TestMethod]
        public void GetDeterminant_Test()
        {
            var _input = new List<Tuple<Matrix<Q, Rational>, Q>>();
            var _rational = new Rational();
            var _m1 = new Matrix<Q, Rational>(_rational, new Q[,] { { 0 } });
            _input.Add(new Tuple<Matrix<Q, Rational>, Q>(_m1, 0));
            var _m2 = new Matrix<Q, Rational>(_rational, new Q[,] { { 0, 0 }, { 0, 0 } });
            _input.Add(new Tuple<Matrix<Q, Rational>,Q>(_m2, 0));
            var _m3 = new Matrix<Q, Rational>(_rational, new Q[,] { { 1, 0 }, { 0, 0 } });
            _input.Add(new Tuple<Matrix<Q, Rational>, Q>(_m3, 0));
            var _m4 = new Matrix<Q, Rational>(_rational, new Q[,] { { 1, 0 }, { 0, 1 } });
            _input.Add(new Tuple<Matrix<Q, Rational>, Q>(_m4, 1));
            var _m5 = new Matrix<Q, Rational>(_rational, new Q[,] { { 0, 1 }, { 1, 0 } });
            _input.Add(new Tuple<Matrix<Q, Rational>, Q>(_m5, -1));
            var _m6 = new Matrix<Q, Rational>(_rational, 
                new Q[,]
                {
                    { 1, -1, 1, -2 },
                    { 1, 3, -1, 3 },
                    { -1, -1, 4, 3 },
                    { -3, 0, -8, -13 }
                });
            _input.Add(new Tuple<Matrix<Q, Rational>, Q>(_m6, -153));

            foreach(var t in _input)
            {
                Assert.AreEqual(t.Item2, t.Item1.Determinant, String.Format("Input: {0}", t.Item1));
            }
        }

        [TestMethod]
        public void MatrixReverse_Test()
        {
            var _rational = new Rational();
            var _mqring = new MRing<Q, Rational>(_rational, 5);
            var _real = new Real();
            var _mrring = new MRing<double, Real>(_real, 5);

            var _GF2 = new ZnRing(2);
            var _m1 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 0, 1 }, { 1, 0 } });
            var _m2 = new Matrix<long, ZnRing>(_GF2, new long[,] { { 0, 0 }, { 1, 1 } });
            var _m3 = new Matrix<Q, Rational>(_rational, 
                new Q[,] 
                {
                    { 1, 2, 3, 4, 1 },
                    { -3, 2, -5, 13, 2 },
                    { 1, -2, 10, 4, 3 },
                    { -2, 9, -8, 25, 4 },
                    { 1, 2, 3, 4, 5 }
                });

            var _m4 = new Matrix<double, Real>(_real,
                new double[,]
                {
                    { 1, 2, 3, 4, 1 },
                    { -3, 2, -5, 13, 2 },
                    { 1, -2, 10, 4, 3 },
                    { -2, 9, -8, 25, 4 },
                    { 1, 2, 3, 4, 5 }
                });

            Assert.AreEqual(_m1, _m1.Reverse, String.Format("Input: {0}", _m1));
            Assert.AreEqual(_mqring.One, _m3.Reverse * _m3, String.Format("Inpunt: {0}", _m3));
            Assert.AreEqual(_mrring.One, _m4.Reverse * _m4, String.Format("Inpunt: {0}", _m4));

            bool _isNotReversed = false;
            try
            {
                _m2 = _m2.Reverse;
            }
            catch (DivideByZeroException)
            {
                _isNotReversed = true;
            }

            Assert.IsTrue(_isNotReversed, String.Format("Input {0}:", _m2));
        }

        [TestMethod]
        public void SolveLs_TestQ()
        {
            var _rational = new Rational();
            var _input = new List<Tuple<Matrix<Q, Rational>, Matrix<Q, Rational>>>();

            var _mls1 = new Matrix<Q, Rational>(_rational, 
                new Q[,] 
                { 
                    { 1, 5, 3, 1 }, 
                    { -3, -1, -4, 2 }, 
                    { 2, 3, 1, 3 }
                });

            var _mls1s = new Matrix<Q, Rational>(_rational, 
                new Q[,] { { new Q(32, 35) }, { new Q(6, 7) }, { new Q(-7, 5) } });

            var _mls2 = new Matrix<Q, Rational>(_rational,
                new Q[,] 
                { 
                    { 12, 9, 3, 10, 13 }, 
                    { 4, 3, 1, 2, 3 }, 
                    { 8, 6, 2, 5, 7 }
                });

            var _mls2s = new Matrix<Q, Rational>(_rational,
                new Q[,]
                {
                    { new Q(-3, 4), new Q(-1, 4), new Q(1, 4) },
                    { 1, 0, 0 },
                    { 0, 1, 0 },
                    { 0, 0, 1 }
                });

            var _mls3 = new Matrix<Q, Rational>(_rational,
                new Q[,]
                    {
                        { -9, 10, 3, 7, 7 },
                        { -4, 7, 1, 3, 5 },
                        { 7, 5, -4, -6, 3 }
                    });

            _input.Add(new Tuple<Matrix<Q, Rational>, Matrix<Q, Rational>>(_mls1, _mls1s));
            _input.Add(new Tuple<Matrix<Q, Rational>, Matrix<Q, Rational>>(_mls2, _mls2s));
            _input.Add(new Tuple<Matrix<Q, Rational>, Matrix<Q, Rational>>(_mls3, null));

            foreach(var t in _input)
            {
                var _solvation = Matrix<Q, Rational>.SolveLs(t.Item1);
                Assert.AreEqual(t.Item2, _solvation);
            }
        }

        [TestMethod]
        public void SolveLs_TestR()
        {
            var _real = new Real();
            var _input = new List<Tuple<Matrix<double, Real>, Matrix<double, Real>>>();

            var _mls1 = new Matrix<double, Real>(_real,
                new double[,]
                {
                    { 1, 5, 3, 1 },
                    { -3, -1, -4, 2 },
                    { 2, 3, 1, 3 }
                });

            var _mls1s = new Matrix<double, Real>(_real,
                new double[,] { { new Q(32, 35).Real }, { new Q(6, 7).Real }, { new Q(-7, 5).Real } });

            var _mls2 = new Matrix<double, Real>(_real,
                new double[,]
                {
                    { 12, 9, 3, 10, 13 },
                    { 4, 3, 1, 2, 3 },
                    { 8, 6, 2, 5, 7 }
                });

            var _mls2s = new Matrix<double, Real>(_real,
                new double[,]
                {
                    { new Q(-3, 4).Real, new Q(-1, 4).Real, new Q(1, 4).Real },
                    { 1, 0, 0 },
                    { 0, 1, 0 },
                    { 0, 0, 1 }
                });

            var _mls3 = new Matrix<double, Real>(_real,
                new double[,]
                    {
                        { -9, 10, 3, 7, 7 },
                        { -4, 7, 1, 3, 5 },
                        { 7, 5, -4, -6, 3 }
                    });

            _input.Add(new Tuple<Matrix<double, Real>, Matrix<double, Real>>(_mls1, _mls1s));
            _input.Add(new Tuple<Matrix<double, Real>, Matrix<double, Real>>(_mls2, _mls2s));
            _input.Add(new Tuple<Matrix<double, Real>, Matrix<double, Real>>(_mls3, null));

            foreach (var t in _input)
            {
                var _solvation = Matrix<double, Real>.SolveLs(t.Item1);
                Assert.AreEqual(t.Item2, _solvation);
            }
        }
    }
}
