// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
    internal static partial class SpanHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int BinarySearch<T, TComparable>(
            ReadOnlySpan<T> span, TComparable comparable)
            where TComparable : IComparable<T>, allows ref struct
        {
            if (comparable == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.comparable);

            return BinarySearch(ref MemoryMarshal.GetReference(span), span.Length, comparable);
        }

        public static int BinarySearch<T, TComparable>(
            ref T spanStart, int length, TComparable comparable)
            where TComparable : IComparable<T>, allows ref struct
        {
            int lo = 0;
            int hi = length - 1;
            // If length == 0, hi == -1, and loop will not be entered
            while (lo <= hi)
            {
                // PERF: `lo` or `hi` will never be negative inside the loop,
                //       so computing median using uints is safe since we know
                //       `length <= int.MaxValue`, and indices are >= 0
                //       and thus cannot overflow an uint.
                //       Saves one subtraction per loop compared to
                //       `int i = lo + ((hi - lo) >> 1);`
                int i = (int)(((uint)hi + (uint)lo) >> 1);

                int c = comparable.CompareTo(Unsafe.Add(ref spanStart, i));
                if (c == 0)
                {
                    return i;
                }
                else if (c > 0)
                {
                    lo = i + 1;
                }
                else
                {
                    hi = i - 1;
                }
            }
            // If none found, then a negative number that is the bitwise complement
            // of the index of the next element that is larger than or, if there is
            // no larger element, the bitwise complement of `length`, which
            // is `lo` at this point.
            return ~lo;
        }

        // Helper to allow sharing all code via IComparable<T> inlineable
        internal readonly ref struct ComparerComparable<T, TComparer> : IComparable<T>
            where TComparer : IComparer<T>, allows ref struct
        {
            private readonly T _value;
            private readonly TComparer _comparer;

            public ComparerComparable(T value, TComparer comparer)
            {
                _value = value;
                _comparer = comparer;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int CompareTo(T? other) => _comparer.Compare(_value, other);
        }
    }
}
