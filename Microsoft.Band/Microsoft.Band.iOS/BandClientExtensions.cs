﻿using System;
using System.Threading.Tasks;

namespace Microsoft.Band
{
	public static class BandClientExtensions
	{
		public static Task<string> GetFirmwareVersionTaskAsync (this BandClient client)
		{
			var tcs = new TaskCompletionSource<string> ();
			client.GetFirmwareVersionAsync (tcs.AttachCompletionHandler ());
			return tcs.Task;
		}

		public static Task<string> GetHardwareVersionTaskAsyc (this BandClient client)
		{
			var tcs = new TaskCompletionSource<string> ();
			client.GetHardwareVersionAsyc (tcs.AttachCompletionHandler ());
			return tcs.Task;
		}
	}
}
