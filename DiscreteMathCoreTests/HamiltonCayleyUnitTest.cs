using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DiscreteMathCore;

namespace DiscreteMathCoreTests
{
    [TestClass]
    public class HamiltonCayleyUnitTest
    {
        [TestMethod]
        public void HamiltonCayley_Test()
        {
            var _msize = 4;
            var _rational = new Rational();
            var _mring = new MRing<Q, Rational>(_rational, _msize);
            var _pring = new Rx<Q, Rational>(_rational);
            var _mp = new MRing<Polynom<Q, Rational>, Rx<Q, Rational>>(_pring, _msize);

            var _a_vals = new Q[,]
                {
                    { 4, 8, 3, 6 },
                    { -1, -2, -1, -2 },
                    { 3, 6, 4, 8 },
                    { -2, -4, -2, -4 }
                };

            var A = new Matrix<Q, Rational>(_rational, _a_vals);

            var _xi = A.CharPolynom;
            var _m_xi = _xi.GetMatrixPolynom(_msize);
            var _res1 = _m_xi.GetValue(A);
            Assert.AreEqual(_res1, _mring.Zero, String.Format("Input: {0}", _res1));

            _msize = 5;
            var _ring = new ZnRing(7);
            var _mring1 = new MRing<long, ZnRing>(_ring, _msize);

            var _a_vals1 = new long[,]
            {
                { 1, 0, 1, 0, 0 },
                { 2, 1, 4, 1, 0 },
                { 4, 4, 2, 1, 1 },
                { 1, 5, 1, 6, 5 },
                { 2, 4, 4, 4, 5 }
            };

            var B = new Matrix<long, ZnRing>(_ring, _a_vals1);
            var _xi1 = B.CharPolynom;
            var _m_xi1 = _xi1.GetMatrixPolynom(_msize);
            var _res2 = _m_xi1.GetValue(B);

            Assert.AreEqual(_res2, _mring1.Zero, String.Format("Input: {0}", _res2));
        }


    }
}
