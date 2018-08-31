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
            //var _pm = new Rx<Matrix<Q, Rational>, MRing<Q, Rational>>(_mring);

            var _a_vals = new Q[,]
                {
                    { 4, 8, 3, 6 },
                    { -1, -2, -1, -2 },
                    { 3, 6, 4, 8 },
                    { -2, -4, -2, -4 }
                };

            var A = new Matrix<Q, Rational>(_rational, _a_vals);


            var _ax_vals = new Polynom<Q, Rational>[_msize, _msize];
            var _ex_vals = new Polynom<Q, Rational>[_msize, _msize];
            for(var i = 0; i < _msize; ++i)
                for(var j = 0; j < _msize; ++j)
                {
                    _ax_vals[i, j] = _pring.GetPolynomByElement(_a_vals[i, j]);
                    if (i == j)
                        _ex_vals[i, j] = new Polynom<Q, Rational>(_rational, new Q[] { 0, 1 });
                    else
                        _ex_vals[i, j] = _pring.GetPolynomByElement(0);
                }

            var _Ax = new Matrix<Polynom<Q, Rational>, Rx<Q, Rational>>(_pring, _ax_vals);
            var _EX = new Matrix<Polynom<Q, Rational>, Rx<Q, Rational>>(_pring, _ex_vals);
            var _xi = (_EX + _mp.Opposite(_Ax)).Determinant;

            var _xi_coeffs = _xi.Coeffs;

            var _mcoeffs = new Matrix<Q, Rational>[_xi_coeffs.Length];
            for (var i = 0; i < _xi_coeffs.Length; ++i)
            {
                _mcoeffs[i] = _mring.One;
                _mcoeffs[i].Mult(_xi_coeffs[i]);
            }

            var _m_xi = new Polynom<Matrix<Q, Rational>, MRing<Q, Rational>>(_mring, _mcoeffs);
            var _res = _m_xi.GetValue(A);

            Assert.AreEqual(_res, _mring.Zero);
        }


    }
}
