using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class PermutationIterator : IEnumerator<List<int>>
    {
        private int FLength;
        private IEnumerator<List<int>> FInnerEnumerator;
        private List<int> FCurrent;
        private int FPosition;

        public PermutationIterator(int aLength)
        {
            this.FLength = aLength;
            this.FPosition = aLength;

            if(aLength > 1)
            {
                this.FInnerEnumerator = new PermutationIterator(aLength - 1);
            }
            else if (aLength == 1)
            {
                var _list = new List<List<int>>();
                _list.Add(new List<int>());
                this.FInnerEnumerator = _list.GetEnumerator();
            }

        }

        public List<int> Current
        {
            get
            {
                return this.FCurrent;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return this.FCurrent;
            }
        }

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            if(this.FPosition > this.FLength - 1)
            {
                this.FPosition = 0;
                var _isEnd = !this.FInnerEnumerator.MoveNext();
                if (_isEnd)
                {
                    return false;
                }
            }

            this.FCurrent = new List<int>(this.FInnerEnumerator.Current);
            this.FCurrent.Insert(this.FPosition, this.FLength - 1);
            this.FPosition++;
            return true;
        }

        public void Reset()
        {
            if(this.FInnerEnumerator != null)
            {
                this.FInnerEnumerator.Reset();
            }

            this.FPosition = 0;
        }
    }
}
