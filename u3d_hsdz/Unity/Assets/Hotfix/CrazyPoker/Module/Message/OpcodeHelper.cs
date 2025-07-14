using System.Collections.Generic;

namespace ETHotfix
{
	public static class OpcodeHelper
	{
		private static readonly HashSet<ushort> ignoreDebugLogMessageSet = new HashSet<ushort>
		{
				HotfixOpcode.REQ_SYNC_SIGNAL_RESOURCE,
				HotfixOpcode.REQ_SYNC_SIGNAL_GAME,
		};

		private static readonly HashSet<ushort> ignoreShowJuhuaSet = new HashSet<ushort>()
		{
				HotfixOpcode.REQ_SYNC_SIGNAL_RESOURCE,
				HotfixOpcode.REQ_SYNC_SIGNAL_GAME,
		};

		public static bool IsNeedDebugLogMessage(ushort opcode)
        {
			if (ignoreDebugLogMessageSet.Contains(opcode))
			{
				return false;
			}

			return true;
		}


		public static bool IsNeedShowJuhua(ushort opcode)
		{
			if (ignoreShowJuhuaSet.Contains(opcode))
			{
				return false;
			}
			return true;
		}
	}
}