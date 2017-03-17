using System;
using ObjCRuntime;

namespace com.atombooster.roomplus
{
	public static class EnvironmentDetector
	{
		static public bool InSimulator()
		{
			return Runtime.Arch == Arch.SIMULATOR;
		}
	}
}
