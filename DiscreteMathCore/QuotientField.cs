using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public class QuotientField<T> : RingBase<Tuple<T, T>>
    {
        RingBase<T> innerRing;
        List<Tuple<T, T>> items = new List<Tuple<T, T>>();

        public QuotientField(RingBase<T> innerRing) {

            this.innerRing = innerRing;

            if(innerRing is IEnumerable<T>)
            {
                IEnumerator<T> numEnum = ((IEnumerable<T>)innerRing).GetEnumerator();
                IEnumerator<T> denumEnum = ((IEnumerable<T>)innerRing).GetEnumerator();
                while (numEnum.MoveNext())
                {
                    while(denumEnum.MoveNext())
                    {
                        if (denumEnum.Current.Equals(innerRing.Zero))
                        {
                            continue;
                        }

                        Tuple<T, T> current = new Tuple<T, T>(numEnum.Current, denumEnum.Current);
                        Tuple<T, T> duplicate = items.Find(x => Equals(x, current));
                        if(duplicate == null)
                        {
                            items.Add(current);
                        }
                    }
                    denumEnum.Reset();
                }
            }
        }

        public List<Tuple<T, T>> GetItems() {
            return items;
        }

        public override Tuple<T, T> One => new Tuple<T, T>(innerRing.One, innerRing.One);

        public override Tuple<T, T> Zero => new Tuple<T, T>(innerRing.Zero, innerRing.One);

        public override bool Equals(Tuple<T, T> a, Tuple<T, T> b)
        {
            //return a.Equals(b);

            return innerRing.Equals(
                    innerRing.Prod(a.Item1, innerRing.Opposite(a.Item2)),
                    innerRing.Prod(b.Item1, innerRing.Opposite(b.Item2))
                );
        }

        public override string GetTexString(Tuple<T, T> a)
        {
            return String.Format("\\frac{{{0}}}{{{1}}}", a.Item1, a.Item2);
        }

        public override Tuple<T, T> InnerReverse(Tuple<T, T> a)
        {
            return new Tuple<T, T>(a.Item2, a.Item1);
        }

        public override bool IsNaN(Tuple<T, T> a)
        {
            return innerRing.IsNaN(a.Item1) || innerRing.IsNaN(a.Item2);
        }

        public override Tuple<T, T> LeftReverse(Tuple<T, T> a)
        {
            return Reverse(a);
        }

        public override Tuple<T, T> Opposite(Tuple<T, T> a)
        {
            return new Tuple<T, T>(innerRing.Opposite(a.Item1), a.Item2);
        }

        public override Tuple<T, T> Prod(Tuple<T, T> a, Tuple<T, T> b)
        {
            return new Tuple<T, T>(innerRing.Prod(a.Item1, b.Item1), innerRing.Prod(a.Item2, b.Item2));
        }

        public override Tuple<T, T> RightReverse(Tuple<T, T> a)
        {
            return Reverse(a);
        }

        public override Tuple<T, T> Sum(Tuple<T, T> a, Tuple<T, T> b)
        {
            T numerator = innerRing.Sum(innerRing.Prod(a.Item1, b.Item2), innerRing.Prod(b.Item1, a.Item2));
            T denumerator = innerRing.Prod(a.Item2, b.Item2);
            return new Tuple<T, T>(numerator, denumerator);
        }      
    }
}
