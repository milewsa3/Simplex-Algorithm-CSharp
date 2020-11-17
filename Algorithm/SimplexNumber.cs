using System;

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
            => new SimplexNumber(
                a.StandardValue * b.StandardValue,
                a.InfinityValue * b.InfinityValue);

        public static SimplexNumber operator /(SimplexNumber a, SimplexNumber b)
            => new SimplexNumber(
                a.StandardValue / b.StandardValue,
                a.InfinityValue / b.InfinityValue);

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
        public static explicit operator SimplexNumber(double dbl) => new SimplexNumber(dbl);

        public override string ToString()
        {
            return InfinityValue != 0 ? $"{InfinityValue}M + {StandardValue}" : $"{StandardValue}";
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