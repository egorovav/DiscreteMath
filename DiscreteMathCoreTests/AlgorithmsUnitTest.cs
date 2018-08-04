using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DiscreteMathCore;
using System.Collections.Generic;

namespace DiscreteMathCoreTests
{
    [TestClass]
    public class AlgorithmsUnitTest
    {
        [TestMethod]
        public void GetGcd_Test()
        {
            var _input = new List<Tuple<int, int, int>>();
            _input.Add(new Tuple<int, int, int>(0, 0, 0));
            _input.Add(new Tuple<int, int, int>(0, 1, 1));
            _input.Add(new Tuple<int, int, int>(3, 3, 3));
            _input.Add(new Tuple<int, int, int>(2, 4, 2));
            _input.Add(new Tuple<int, int, int>(33, 21, 3));
            _input.Add(new Tuple<int, int, int>(33, 25, 1));
            _input.Add(new Tuple<int, int, int>(7, 5, 1));

            var _count = _input.Count;
            for(int i = 0; i < _count; ++i)
            {
                var t = _input[i];
                _input.Add(
                    new Tuple<int, int, int>(t.Item2, t.Item1, t.Item3));
            }

            _count = _input.Count;
            for(int i = 0; i < _count; ++i)
            {
                var t = _input[i];
                _input.Add(new Tuple<int, int, int>(-t.Item1, t.Item2, t.Item3));
                _input.Add(new Tuple<int, int, int>(t.Item1, -t.Item2, t.Item3));
                _input.Add(new Tuple<int, int, int>(-t.Item1, -t.Item2, t.Item3));
            }

            foreach (var t in _input)
            {
                long gcd = Algorithms.GetGcd(t.Item1, t.Item2);
                Assert.AreEqual<long>(t.Item3, gcd,
                    String.Format("Input: {0}, {1}", t.Item1, t.Item2));
            }

            foreach(var t in _input)
            {
                int gcd = Algorithms.get_gcd(t.Item1, t.Item2);
                Assert.AreEqual<int>(t.Item3, gcd, 
                    String.Format("Input: {0}, {1}", t.Item1, t.Item2));
            }

        } 

        [TestMethod]
        public void GetGcdEx_Test()
        {
            var _input = new List<Tuple<int, int, int>>();
            _input.Add(new Tuple<int, int, int>(0, 0, 0));
            _input.Add(new Tuple<int, int, int>(0, 1, 1));
            _input.Add(new Tuple<int, int, int>(3, 3, 3));
            _input.Add(new Tuple<int, int, int>(2, 4, 2));
            _input.Add(new Tuple<int, int, int>(33, 21, 3));
            _input.Add(new Tuple<int, int, int>(33, 25, 1));
            _input.Add(new Tuple<int, int, int>(7, 5, 1));

            var _count = _input.Count;
            for (int i = 0; i < _count; ++i)
            {
                var t = _input[i];
                _input.Add(new Tuple<int, int, int>(t.Item2, t.Item1, t.Item3));
            }

            _count = _input.Count;
            for (int i = 0; i < _count; ++i)
            {
                var t = _input[i];
                _input.Add(new Tuple<int, int, int>(-t.Item1, t.Item2, t.Item3));
                _input.Add(new Tuple<int, int, int>(t.Item1, -t.Item2, t.Item3));
                _input.Add(new Tuple<int, int, int>(-t.Item1, -t.Item2, t.Item3));
            }

            foreach (var t in _input)
            {
                long u, v;
                long gcd = Algorithms.GetGcdEx(t.Item1, t.Item2, out u, out v);
                Assert.AreEqual<long>(t.Item3, gcd, String.Format("Input: {0}, {1}", t.Item1, t.Item2));
                Assert.AreEqual<long>(t.Item3, t.Item1 * u + t.Item2 * v,
                    String.Format("Input: {0}, {1}; u, v: {2}, {3}", t.Item1, t.Item2, u, v));
            }
        }

        [TestMethod]
        public void GetGcdPolynom_Test()
        {
            var _rational = new Rational();
            var _input = new List<Tuple<Polynom<Q, Rational>, Polynom<Q, Rational>, Polynom<Q, Rational>>>();
            var _p11 = new Polynom<Q, Rational>(_rational, new Q[] { -1, 0, 0, 1 });
            var _p12 = new Polynom<Q, Rational>(_rational, new Q[] { -1, 0, 1 });
            var _gcd1 = new Polynom<Q, Rational>(_rational, new Q[] { -1, 1 });
            _input.Add(new Tuple<Polynom<Q, Rational>, Polynom<Q, Rational>, Polynom<Q, Rational>>(_p11, _p12, _gcd1));

            foreach (var t in _input)
            {
                var _gcd = Algorithms.GetGcd<Q, Rational>(t.Item1, t.Item2);
                Assert.AreEqual<Polynom<Q, Rational>>(t.Item3, _gcd, 
                    String.Format("Input: {0}, {1}", t.Item1, t.Item2));
            }
        }
    }
}
