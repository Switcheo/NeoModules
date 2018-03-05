using System;
using System.Collections.Generic;
using System.Text;

namespace NeoModules.NVM
{
    public static class Helper
    {
		public static ScriptBuilder Emit(this ScriptBuilder sb, params OpCode[] ops)
		{
			foreach (OpCode op in ops)
				sb.Emit(op);
			return sb;
		}

		//public static ScriptBuilder EmitAppCall(this ScriptBuilder sb, UInt160 scriptHash, bool useTailCall = false)
		//{
		//	return sb.EmitAppCall(scriptHash.ToArray(), useTailCall);
		//}
    }
}
