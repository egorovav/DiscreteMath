using DiscreteMathCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Approximation
{
    public class Approximator
    {
        public int Degree { get; set; }
        public double To { get; set; }
        public double From { get; set; }
        public Func<double, double> Func { get; set; }

        public Approximator(int aDegree, double aLeft, double aRight, Func<double, double> aFunc)
        {
            this.Degree = aDegree;
            this.From = aLeft;
            this.To = aRight;
            this.Func = aFunc;
        }

        public Approximator(Func<double, double> aFunc)
            : this(10, -1, 1, aFunc)
        {
            this.Func = aFunc;
        }

        public Polynom<double, Real> GetPolynom()
        {
            var _values = this.GetValues();
            var _polinom = GetPolinom(_values);
            return _polinom;
        }

        public Matrix<double, Real> FillMatrix(IEnumerable<Point> aValues)
        {
            var _real = new Real();
            var _rowCount = aValues.Count(x => !_real.IsNaN(x.Y));
            var _colCount = _rowCount + 1;
            var _count = 0;
            var _dValues = new double[_rowCount, _colCount];
            foreach (var _pair in aValues)
            {
                if (_real.IsNaN(_pair.Y))
                    continue;

                double p = 1;
                for (int i = 0; i < _rowCount; i++)
                {
                    _dValues[_count, i] = p;
                    p = _real.Prod(p, _pair.X);
                }
                _dValues[_count, _colCount - 1] = _pair.Y;
                _count++;
            }

            var _matrix = new Matrix<double, Real>(_real, _dValues);
            return _matrix;
        }

        public Polynom<double, Real> GetPolinom(IEnumerable<Point> aValues)
        {
            var _matrix = FillMatrix(aValues);
            var _solv = Matrix<double, Real>.SolveLs(_matrix);
            var _vals = _solv.Values;

            var _coeffs = new double[_solv.RowCount];
            for (var i = 0; i < _solv.RowCount; ++i)
                _coeffs[i] = _vals[i, 0];

            return new Polynom<double, Real>(new Real(), _coeffs);
        }

        public IEnumerable<Point> GetValues()
        {
            var _step = (this.To - this.From) / this.Degree;
            var _arg = this.From;
            for(int i = 0; i < this.Degree; ++i)
            {
                var _val = this.Func(_arg);
                yield return new Point(_arg, _val);
                _arg += _step;
            }
        }
    }
}
