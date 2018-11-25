using System;
using System.IO;
using System.Numerics;
using System.Text;

namespace NeoModules.Core.NVM
{
    public class ScriptBuilder : IDisposable
    {
        private readonly MemoryStream _ms = new MemoryStream();
        private readonly BinaryWriter _writer;

        public ScriptBuilder()
        {
            _writer = new BinaryWriter(_ms);
        }

        public int Offset => (int) _ms.Position;

        public void Dispose()
        {
            _writer.Dispose();
            _ms.Dispose();
        }

        public ScriptBuilder Emit(OpCode op, byte[] arg = null)
        {
            _writer.Write((byte) op);
            if (arg != null)
                _writer.Write(arg);
            return this;
        }

        public ScriptBuilder EmitAppCall(byte[] scriptHash, bool useTailCall = false)
        {
            if (scriptHash.Length != 20)
                throw new ArgumentException();
            return Emit(useTailCall ? OpCode.TAILCALL : OpCode.APPCALL, scriptHash);
        }

        public ScriptBuilder EmitPush(BigInteger number)
        {
            if (number == -1) return Emit(OpCode.PUSHM1);
            if (number == 0) return Emit(OpCode.PUSH0);
            if (number > 0 && number <= 16) return Emit(OpCode.PUSH1 - 1 + (byte) number);
            return EmitPush(number.ToByteArray());
        }

        public ScriptBuilder EmitPush(bool data)
        {
            return Emit(data ? OpCode.PUSHT : OpCode.PUSHF);
        }

        public ScriptBuilder EmitPush(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException();
            if (data.Length <= (int) OpCode.PUSHBYTES75)
            {
                _writer.Write((byte) data.Length);
                _writer.Write(data);
            }
            else if (data.Length < 0x100)
            {
                Emit(OpCode.PUSHDATA1);
                _writer.Write((byte) data.Length);
                _writer.Write(data);
            }
            else if (data.Length < 0x10000)
            {
                Emit(OpCode.PUSHDATA2);
                _writer.Write((ushort) data.Length);
                _writer.Write(data);
            }
            else // if (data.Length < 0x100000000L)
            {
                Emit(OpCode.PUSHDATA4);
                _writer.Write(data.Length);
                _writer.Write(data);
            }
            return this;
        }

        public ScriptBuilder EmitPush(string data)
        {
            return EmitPush(Encoding.UTF8.GetBytes(data));
        }

        public ScriptBuilder EmitSysCall(string api)
        {
            if (api == null)
                throw new ArgumentNullException();
            byte[] apiBytes = Encoding.ASCII.GetBytes(api);
            if (apiBytes.Length == 0 || apiBytes.Length > 252)
                throw new ArgumentException();
            byte[] arg = new byte[apiBytes.Length + 1];
            arg[0] = (byte)apiBytes.Length;
            Buffer.BlockCopy(apiBytes, 0, arg, 1, apiBytes.Length);
            return Emit(OpCode.SYSCALL, arg);
        }

        public byte[] ToArray()
        {
            _writer.Flush();
            return _ms.ToArray();
        }
    }
}