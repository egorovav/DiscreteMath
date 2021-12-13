using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class FiniteRing : RingBase<long>, IEnumerable<long>
    {
        private long[,] FAddTable;
        private long[,] FMultTable;
        private List<long> FValues = new List<long>();

        public override long Size
        {
            get { return this.FValues.Count; }
        }

        public Matrix<long, ZnRing> AddTable
        {
            get
            {
                return new Matrix<long, ZnRing>(new ZnRing(this.Size), this.FAddTable);
            }
        }

        public Matrix<long, ZnRing> MultTable
        {
            get
            {
                return new Matrix<long, ZnRing>(new ZnRing(this.Size), this.FMultTable);
            }
        }

        public FiniteRing(long[,] aAddOpp, long[,] aMultOpp)
        {
            var _size = aAddOpp.GetLength(0);
            if (aAddOpp.GetLength(1) != _size || aMultOpp.GetLength(0) != _size || aMultOpp.GetLength(1) != _size)
                throw new ArgumentException();

            this.FAddTable = aAddOpp;
            this.FMultTable = aMultOpp;

            for (var i = 0; i < _size; ++i)
                this.FValues.Add(i);
        }

        public FiniteRing(long[,] aAddOpp, long[,] aMultOpp, long aSimpleSubfieldSize)
            : this(aAddOpp, aMultOpp)
        {
            this.FSimpleSubfieldSize = aSimpleSubfieldSize;
        }

        private bool CheckZeroB()
        {
            for (long i = 0; i < this.Size; ++i)
            {
                var _0i = this.FAddTable[0, i];
                if (!this.Equals(_0i, i))
                    return false;

                var _i0 = this.FAddTable[i, 0];
                if (!this.Equals(_i0, i))
                    return false;
            }

            return true;
        }

        private string CheckZero()
        {
            for(long i = 0; i < this.Size; ++i)
            {
                var _0i = this.FAddTable[0, i];
                if (!this.Equals(_0i, i))
                    return String.Format("0 + {0} = {1}", i, _0i);

                var _i0 = this.FAddTable[i, 0];
                if (!this.Equals(_i0, i))
                    return String.Format("{0} + 0 = {1}", i, _i0);
            }

            return null;
        }

        public string CheckOne()
        {
            for(var i = 0; i < this.Size; ++i)
            {
                var _1i = this.FMultTable[1, i];
                if (!this.Equals(_1i, i))
                    return String.Format("1 * {0} = {1}", i, _1i);

                var _i1 = this.FMultTable[i, 1];
                if (!this.Equals(_i1, i))
                    return String.Format("{0} * 1 = {1}", i, _i1);
            }

            return null;
        }

        public bool CheckAddAccB()
        {
            for (var i = 0; i < this.Size; ++i)
                for (var j = 0; j < this.Size; ++j)
                    for (var k = 0; k < this.Size; ++k)
                    {
                        var _ab = this.FAddTable[i, j];
                        var _ab_c = this.FAddTable[_ab, k];
                        var _bc = this.FAddTable[j, k];
                        var _a_bc = this.FAddTable[i, _bc];
                        if (!this.Equals(_ab_c, _a_bc))
                            return false;
                    }

            return true;
        }

        public string CheckAddAcc()
        {
            for (var i = 0; i < this.Size; ++i)
                for (var j = 0; j < this.Size; ++j)
                    for(var k = 0; k < this.Size; ++k)
                    {
                        var _ab = this.FAddTable[i, j];
                        var _ab_c = this.FAddTable[_ab, k];
                        var _bc = this.FAddTable[j, k];
                        var _a_bc = this.FAddTable[i, _bc];
                        if (!this.Equals(_ab_c, _a_bc))
                            return String.Format(
                                "({0} + {1}) + {2} = {3} \n {0} + ({1} + {2}) = {4}", i, j, k, _ab_c, _a_bc);
                    }             

            return null;
        }

        private string CheckMultAcc()
        {
            for (var i = 0; i < this.Size; ++i)
                for (var j = 0; j < this.Size; ++j)
                    for (var k = 0; k < this.Size; ++k)
                    {
                        var _ab = this.FMultTable[i, j];
                        var _ab_c = this.FMultTable[_ab, k];
                        var _bc = this.FMultTable[j, k];
                        var _a_bc = this.FMultTable[i, _bc];
                        if (!this.Equals(_ab_c, _a_bc))
                            return String.Format(
                                "({0} * {1}) * {2} = {3} \n {0} * ({1} * {2}) = {4}", i, j, k, _ab_c, _a_bc);
                    }

            return null;
        }

        private string CheckAddCom()
        {
            for (var i = 0; i < this.Size; ++i)
                for (var j = 0; j < this.Size; ++j)
                    {
                        var _ab = this.FAddTable[i, j];
                        var _ba = this.FAddTable[j, i];
                        if (!this.Equals(_ab, _ba))
                            return String.Format("{0} + {1} = {2} \n {1} + {0} = {3}", i, j, _ab, _ba);
                    }

            return null;
        }

        public string CheckMultCom()
        {
            for (var i = 0; i < this.Size; ++i)
                for (var j = 0; j < this.Size; ++j)
                {
                    var _ab = this.FMultTable[i, j];
                    var _ba = this.FMultTable[j, i];
                    if (!this.Equals(_ab, _ba))
                        return String.Format("{0} * {1} = {2} \n {1} * {0} = {3}", i, j, _ab, _ba);
                }

            return null;
        }

        private string CheckDistrib()
        {
            for (var i = 0; i < this.Size; ++i)
                for (var j = 0; j < this.Size; ++j)
                    for (var k = 0; k < this.Size; ++k)
                    {
                        var _ab = this.FMultTable[i, j];
                        var _ac = this.FMultTable[i, k];
                        var _b_c = this.FAddTable[j, k];
                        var _left = this.FAddTable[_ab, _ac];
                        var _right = this.FMultTable[i, _b_c];
                        if (!this.Equals(_left, _right))
                            return String.Format(
                                "{0} * {1} + {0} * {2} = {3} \n {0} * ({1} + {2}) = {4}", i, j, k, _left, _right);
                    }

            return null;
        }

        public bool CheckAddAsGroupB()
        {
            // var _res = this.CheckZeroB();
            // if (!_res)
            //    return false;

            var _res = this.CheckAddAccB();
            if (!_res)
                return false;

            for (var i = 0; i < this.Size; ++i)
            {
                if (this.Opposite(i) < 0)
                    return false;
            }

            return true;
        }

        public string CheckAddAsGroup()
        {
            var _res = this.CheckZero();
            if (_res != null)
                return _res;

            _res = this.CheckAddAcc();
            if (_res != null)
                return _res;

            for (var i = 0; i < this.Size; ++i)
            {
                if (this.Opposite(i) < 0)
                    return String.Format("The -{0} is absent.", i);
            }

            return null;
        }

        public string CheckAdd()
        {
            var _res = CheckAddAsGroup();

            if (_res != null)
                return _res;

            _res = this.CheckAddCom();

            if (_res != null)
                return _res;

            return null;
        }

        public string CheckReverse()
        {
            for(var i = 1; i < this.Size; ++i)
            {
                if (this.Reverse(i) < 0)
                    return String.Format("The {0}^-1 is absent.", i);
            }

            return null;
        }

        public string CheckMult()
        {
            var _res = this.CheckMultAcc();
            if (_res != null)
                return _res;


            _res = this.CheckDistrib();
            if (_res != null)
                return _res;

            return null;
        }

        public string CheckRing()
        {
            var _res = this.CheckAdd();
            if (_res != null)
                return _res;

            _res = this.CheckMult();
            if (_res != null)
                return _res;

            return null;
        }

        public string CheckField()
        {
            var _res = CheckRing();
            if (_res != null)
                return _res;

            _res = CheckMultCom();
            if (_res != null)
                return _res;

            _res = CheckOne();
            if (_res != null)
                return _res;

            _res = CheckReverse();
            if (_res != null)
                return _res;

            return null;
        }

        public override long One
        {
            get
            {
                return 1;
            }
        }

        public override long Zero
        {
            get
            {
                return 0;
            }
        }

        public override bool Equals(long a, long b)
        {
            return a == b;
        }

        public override string GetTexString(long a)
        {
            return a.ToString();
        }

        public override bool IsNaN(long a)
        {
            throw new NotImplementedException();
        }

        public override long LeftReverse(long a)
        {
            for (var i = 1; i < this.Size; ++i)
            {
                if (this.FMultTable[a, i] == 1)
                    return i;
            }

            return -1;
        }

        public override long Opposite(long a)
        {
            for(var i = 0; i < this.Size; ++i)
            {
                if (this.FAddTable[a, i] == 0)
                    return i;
            }

            return -1;
        }

        public override long Prod(long a, long b)
        {
            return this.FMultTable[a, b];
        }

        public override long InnerReverse(long a)
        {
            for (var i = 1; i < this.Size; ++i)
            {
                if (this.FMultTable[a, i] == 1)
                    return i;
            }

            return -1;
        }

        public override long RightReverse(long a)
        {
            for (var i = 1; i < this.Size; ++i)
            {
                if (this.FMultTable[i, a] == 1)
                    return i;
            }

            return -1;
        }

        public override long Sum(long a, long b)
        {
            return this.FAddTable[a, b];
        }

        private long FSimpleSubfieldSize = 0;
        public override long SimpleSubfieldSize
        {
            get { return this.FSimpleSubfieldSize; }
        }

        public IEnumerator<long> GetEnumerator()
        {
            return this.FValues.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.FValues.GetEnumerator();
        }

        public override string ToString()
        {
            var _sb = new StringBuilder();

            _sb.AppendLine(this.AddTable.ToString());
            _sb.AppendLine();
            _sb.AppendLine(this.MultTable.ToString());

            return _sb.ToString();
        }

        public static FiniteRing GF4
        {
            get
            {
                var _add = new long[,]
                {
                    { 0, 1, 2, 3 },
                    { 1, 0, 3, 2 },
                    { 2, 3, 0, 1 },
                    { 3, 2, 1, 0 }
                };

                var _mult = new long[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 1, 2, 3 },
                    { 0, 2, 3, 1 },
                    { 0, 3, 1, 2 }
                };

                return new FiniteRing(_add, _mult);
            }
        }
    }
}
