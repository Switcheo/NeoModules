using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using NeoModules.Core;
using NeoModules.Core.NVM;

namespace NeoModules.NEP6.Helpers
{
    public static class Extensions
    {
        public static void Write<T>(this BinaryWriter writer, T[] value) where T : ISerializable
        {
            writer.WriteVarInt(value.Length);
            for (var i = 0; i < value.Length; i++) value[i].Serialize(writer);
        }

        public static void Write(this BinaryWriter writer, ISerializable value)
        {
            value.Serialize(writer);
        }

        public static Fixed8 Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Fixed8> selector)
        {
            return source.Select(selector).Sum();
        }

        public static Fixed8 Sum(this IEnumerable<Fixed8> source)
        {
            long sum = 0;
            checked
            {
                foreach (var item in source) sum += item.value;
            }

            return new Fixed8(sum);
        }

        public static int GetVarSize<T>(this T[] value)
        {
            int valueSize;
            var t = typeof(T);
            if (typeof(ISerializable).IsAssignableFrom(t))
            {
                valueSize = value.OfType<ISerializable>().Sum(p => p.Size);
            }
            else if (t.GetTypeInfo().IsEnum)
            {
                int elementSize;
                var u = t.GetTypeInfo().GetEnumUnderlyingType();
                if (u == typeof(sbyte) || u == typeof(byte))
                    elementSize = 1;
                else if (u == typeof(short) || u == typeof(ushort))
                    elementSize = 2;
                else if (u == typeof(int) || u == typeof(uint))
                    elementSize = 4;
                else //if (u == typeof(long) || u == typeof(ulong))
                    elementSize = 8;
                valueSize = value.Length * elementSize;
            }
            else
            {
                valueSize = value.Length * Marshal.SizeOf<T>();
            }

            return GetVarSize(value.Length) + valueSize;
        }

        public static int GetVarSize(int value)
        {
            if (value < 0xFD)
                return sizeof(byte);
            if (value <= 0xFFFF)
                return sizeof(byte) + sizeof(ushort);
            return sizeof(byte) + sizeof(uint);
        }
    }
}