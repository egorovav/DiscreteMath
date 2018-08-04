using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class FiniteRing : IRing<long>
    {
        private long[,] FAddTable;
        private long[,] FMultTable;

        public int Size
        {
            get { return this.FAddTable.GetLength(0); }
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

        private string CheckAddAcc()
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

        public string CheckAdd()
        {
            var _res = this.CheckZero();
            if (_res != null)
                return _res;

            _res = this.CheckAddAcc();
            if (_res != null)
                return _res;

            _res = this.CheckAddCom();
            if (_res != null)
                return _res;

            for (var i = 0; i < this.Size; ++i)
            {
                if (this.Opposite(i) < 0)
                    return String.Format("The -{0} is absent.", i);
            }

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

        public long One
        {
            get
            {
                return 1;
            }
        }

        public long Zero
        {
            get
            {
                return 0;
            }
        }

        public bool Equals(long a, long b)
        {
            return a == b;
        }

        public string GetTexString(long a)
        {
            throw new NotImplementedException();
        }

        public bool IsNaN(long a)
        {
            throw new NotImplementedException();
        }

        public long LeftReverse(long a)
        {
            for (var i = 1; i < this.Size; ++i)
            {
                if (this.FMultTable[a, i] == 1)
                    return i;
            }

            return -1;
        }

        public long Opposite(long a)
        {
            for(var i = 0; i < this.Size; ++i)
            {
                if (this.FAddTable[a, i] == 0)
                    return i;
            }

            return -1;
        }

        public long Prod(long a, long b)
        {
            return this.FMultTable[a, b];
        }

        public long Reverse(long a)
        {
            for (var i = 1; i < this.Size; ++i)
            {
                if (this.FMultTable[a, i] == 1)
                    return i;
            }

            return -1;
        }

        public long RightReverse(long a)
        {
            for (var i = 1; i < this.Size; ++i)
            {
                if (this.FMultTable[i, a] == 1)
                    return i;
            }

            return -1;
        }

        public long Sum(long a, long b)
        {
            return this.FAddTable[a, b];
        }
    }
}
