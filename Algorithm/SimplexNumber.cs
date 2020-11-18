using System;
using System.Text;

namespace Algorithm
{
    public struct SimplexNumber
    {
        public double StandardValue { get; set; }
        public int InfinityValue { get; set; }

        public SimplexNumber(double standardValue = 0.0, int infinityValue = 0)
        {
            StandardValue = standardValue;
            InfinityValue = infinityValue;
        }

        public static SimplexNumber operator +(SimplexNumber a, SimplexNumber b)
            => new SimplexNumber(
                a.StandardValue + b.StandardValue,
                a.InfinityValue + b.InfinityValue);

        public static SimplexNumber operator -(SimplexNumber a, SimplexNumber b)
            => new SimplexNumber(
                a.StandardValue - b.StandardValue,
                a.InfinityValue - b.InfinityValue);

        public static SimplexNumber operator *(SimplexNumber a, SimplexNumber b)
        {
            bool aHasInf = a.InfinityValue != 0;
            bool bHasInf = b.InfinityValue != 0;

            if (aHasInf && !bHasInf)
            {
                return new SimplexNumber(a.StandardValue * b.StandardValue,
                    a.InfinityValue * (int)b.StandardValue);
            }
            if (!aHasInf && bHasInf)
            {
                return new SimplexNumber(b.StandardValue * a.StandardValue,
                    b.InfinityValue * (int)a.StandardValue);
            }

            return new SimplexNumber(a.StandardValue * b.StandardValue,
                a.InfinityValue * b.InfinityValue);
        }

        public static SimplexNumber operator /(SimplexNumber a, SimplexNumber b)
        {
            double stdVal = b.StandardValue == 0 ? 0 : a.StandardValue / b.StandardValue;
            int infVal = b.InfinityValue == 0 ? 0 : a.InfinityValue / b.InfinityValue;

            return new SimplexNumber(stdVal, infVal);
        }

        public static bool operator ==(SimplexNumber a, SimplexNumber b)
            => (a.StandardValue.CompareTo(b.StandardValue) == 0 && a.InfinityValue.CompareTo(b.InfinityValue) == 0);

        public static bool operator !=(SimplexNumber a, SimplexNumber b)
            => (a.StandardValue.CompareTo(b.StandardValue) != 0 || a.InfinityValue.CompareTo(b.InfinityValue) != 0);

        public static bool operator >(SimplexNumber a, SimplexNumber b)
        {
            return a.InfinityValue.CompareTo(b.InfinityValue) != 0
                ? a.InfinityValue.CompareTo(b.InfinityValue) > 0
                : a.StandardValue.CompareTo(b.StandardValue) > 0;
        }

        public static bool operator <(SimplexNumber a, SimplexNumber b)
        {
            return a.InfinityValue.CompareTo(b.InfinityValue) != 0
                ? a.InfinityValue.CompareTo(b.InfinityValue) < 0
                : a.StandardValue.CompareTo(b.StandardValue) < 0;
        }

        public static bool operator >=(SimplexNumber a, SimplexNumber b)
        {
            if (a == b)
                return true;

            return a.InfinityValue.CompareTo(b.InfinityValue) != 0
                ? a.InfinityValue.CompareTo(b.InfinityValue) > 0
                : a.StandardValue.CompareTo(b.StandardValue) > 0;
        }

        public static bool operator <=(SimplexNumber a, SimplexNumber b)
        {
            if (a == b)
                return true;

            return a.InfinityValue.CompareTo(b.InfinityValue) != 0
                ? a.InfinityValue.CompareTo(b.InfinityValue) < 0
                : a.StandardValue.CompareTo(b.StandardValue) < 0;
        }

        public static implicit operator double(SimplexNumber num) => num.StandardValue;

        public static explicit operator SimplexNumber(double dbl) => 
            double.IsInfinity(dbl)
            ? (double.IsPositiveInfinity(dbl) ? new SimplexNumber(0, 1) : new SimplexNumber(0, -1))
            : new SimplexNumber(dbl);

        public override string ToString()
        {
            bool hasStd = StandardValue.CompareTo(0) != 0;
            bool hasInf = InfinityValue != 0;

            if (hasInf && hasStd)
                return $"{InfinityValue}M + {StandardValue}";
            if (hasInf)
                return $"{InfinityValue}M";
            

            return $"{StandardValue}";
        }
        
        public bool Equals(SimplexNumber other)
        {
            return StandardValue.Equals(other.StandardValue) && InfinityValue == other.InfinityValue;
        }

        public override bool Equals(object obj)
        {
            return obj is SimplexNumber other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StandardValue, InfinityValue);
        }
    }
}