using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteMathCore
{
    public interface IRing<T>
    {
        T Zero { get; }
        T One { get; }

        T Sum(T a, T b);
        T Prod(T a, T b);

        T Opposite(T a);
        T Reverse(T a);
        T RightReverse(T a);
        T LeftReverse(T a);

        bool Equals(T a, T b);

        string GetTexString(T a);

        bool IsNaN(T a);
    }
}
