using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NeoModules.Core;

namespace NeoModules.NVM
{
    public static class Helper
    {
        public static ScriptBuilder EmitAppCall(this ScriptBuilder sb, UInt160 scriptHash, bool useTailCall = false)
        {
            return sb.EmitAppCall(scriptHash.ToArray(), useTailCall);
        }

        public static ScriptBuilder EmitAppCall(this ScriptBuilder sb, UInt160 scriptHash, string operation,
            params object[] args)
        {
            for (var i = args.Length - 1; i >= 0; i--)
                sb.EmitPush(args[i]);
            sb.EmitPush(args.Length);
            sb.Emit(OpCode.PACK);
            sb.EmitPush(operation);
            sb.EmitAppCall(scriptHash);
            return sb;
        }

        public static ScriptBuilder EmitPush(this ScriptBuilder sb, object obj)
        {
            switch (obj)
            {
                case bool data:
                    sb.EmitPush(data);
                    break;
                case byte[] data:
                    sb.EmitPush(data);
                    break;
                case string data:
                    sb.EmitPush(data);
                    break;
                case BigInteger data:
                    sb.EmitPush(data);
                    break;
                case ISerializable data:
                    sb.EmitPush(data);
                    break;
                case sbyte data:
                    sb.EmitPush(data);
                    break;
                case byte data:
                    sb.EmitPush(data);
                    break;
                case short data:
                    sb.EmitPush(data);
                    break;
                case ushort data:
                    sb.EmitPush(data);
                    break;
                case int data:
                    sb.EmitPush(data);
                    break;
                case uint data:
                    sb.EmitPush(data);
                    break;
                case long data:
                    sb.EmitPush(data);
                    break;
                case ulong data:
                    sb.EmitPush(data);
                    break;
                case Enum data:
                    sb.EmitPush(BigInteger.Parse(data.ToString("d")));
                    break;
                default:
                    throw new ArgumentException();
            }

            return sb;
        }

        public static void EmitObject(ScriptBuilder sb, object item)
        {
            switch (item)
            {
                case IEnumerable<byte> _:
                {
                    var arr = ((IEnumerable<byte>) item).ToArray();

                    sb.EmitPush(arr);
                    break;
                }
                case IEnumerable<object> _:
                {
                    var arr = ((IEnumerable<object>) item).ToArray();

                    for (var index = arr.Length - 1; index >= 0; index--) EmitObject(sb, arr[index]);

                    sb.EmitPush(arr.Length);
                    sb.Emit(OpCode.PACK);
                    break;
                }
                case null:
                    sb.EmitPush("");
                    break;
                case string _:
                    sb.EmitPush((string) item);
                    break;
                case bool _:
                    sb.EmitPush((bool) item);
                    break;
                case BigInteger _:
                    sb.EmitPush((BigInteger) item);
                    break;
                default:
                    throw new Exception("Unsupported contract parameter: " + item);
            }
        }
    }
}