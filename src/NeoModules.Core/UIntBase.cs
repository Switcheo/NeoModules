﻿using System;
using System.IO;
using System.Linq;

namespace NeoModules.Core
{
    /// Taken from neo-project https://github.com/neo-project/neo/blob/master/neo/UIntBase.cs
    public abstract class UIntBase : IEquatable<UIntBase>, ISerializable
    {
        private readonly byte[] _dataBytes;

        protected UIntBase(int bytes, byte[] value)
        {
            if (value == null)
            {
                _dataBytes = new byte[bytes];
                return;
            }
            if (value.Length != bytes)
                throw new ArgumentException();
            _dataBytes = value;
        }

        public bool Equals(UIntBase other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (_dataBytes.Length != other._dataBytes.Length)
                return false;
            return _dataBytes.SequenceEqual(other._dataBytes);
        }

        public int Size => _dataBytes.Length;

        void ISerializable.Deserialize(BinaryReader reader)
        {
            reader.Read(_dataBytes, 0, _dataBytes.Length);
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write(_dataBytes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (!(obj is UIntBase))
                return false;
            return Equals((UIntBase) obj);
        }

        public override int GetHashCode()
        {
            return _dataBytes.ToInt32(0);
        }

        public static UIntBase Parse(string s)
        {
            if (s.Length == 40 || s.Length == 42)
                return UInt160.Parse(s);
            //else if (s.Length == 64 || s.Length == 66)
            //	return UInt256.Parse(s);
            throw new FormatException();
        }

        public byte[] ToArray()
        {
            return _dataBytes;
        }

        /// <summary>
        ///     转为16进制字符串
        /// </summary>
        /// <returns>返回16进制字符串</returns>
        public override string ToString()
        {
            return "0x" + _dataBytes.Reverse().ToHexString();
        }

        public static bool TryParse<T>(string s, out T result) where T : UIntBase
        {
            int size;
            if (typeof(T) == typeof(UInt160))
                size = 20;
            else if (typeof(T) == typeof(UInt256))
                size = 32;
            else if (s.Length == 40 || s.Length == 42)
                size = 20;
            else if (s.Length == 64 || s.Length == 66)
                size = 32;
            else
                size = 0;
            if (size == 20)
            {
                if (UInt160.TryParse(s, out UInt160 r))
                {
                    result = (T)(UIntBase)r;
                    return true;
                }
            }
            else if (size == 32)
            {
                if (UInt256.TryParse(s, out UInt256 r))
                {
                    result = (T)(UIntBase)r;
                    return true;
                }
            }
            result = null;
            return false;
        }

        public static bool operator ==(UIntBase left, UIntBase right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(UIntBase left, UIntBase right)
        {
            return !(left == right);
        }
    }
}