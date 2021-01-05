using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace GeckoLoader
{
	public class GameProcess
	{
		public bool IsConnected { get { return _isconnected; } }
		internal bool _isconnected;

		internal static IntPtr proc_p;

		public GameProcess(Process process)
		{
			_isconnected = false;

			if (process == null)
				return;

			proc_p = OpenProcess(2035711L, false, process.Id);

			_isconnected = true;
		}

		internal void WriteMemory(ulong addr, byte[] value)
		{
			long written = 0;

			WriteProcessMemory(proc_p, addr, value, (UIntPtr)value.Length, ref written);
		}

		internal byte[] ReadMemory(ulong addr, int length)
		{
			long written = 0;
			byte[] result = new byte[length];

			ReadProcessMemory(proc_p, addr, result, (UIntPtr)length, ref written);

			if (written > length)
				written = 0;

			byte[] output = new byte[written];
			Array.Copy(result, output, written);

			return output;
		}

		public byte[] ReadBytes(ulong offset, int count)
		{
			byte[] blah = ReadMemory(offset, count);
			return blah;
		}

		public void Write(ulong offset, byte[] value)
		{
			WriteMemory(offset, value);
		}

		public long Allocate(int length)
		{
			IntPtr lpAddress = VirtualAllocEx(proc_p, (IntPtr)null, (IntPtr)length, 0x3000, 0X40);

			return (long)lpAddress;
		}

		public void Write(ulong offset, string value)
		{
			byte[] blah = new byte[value.Length];

			byte[] temp = Encoding.ASCII.GetBytes(value);
			Array.Copy(temp, blah, temp.Length);

			Write(offset, blah);
		}

		public void Write(ulong offset, uint value)
		{
			Write(offset, UIntToArray(value));
		}


		public static byte[] UIntToArray(uint input)
		{
			byte[] valArray = BitConverter.GetBytes(input);
			return valArray;
		}

		#region blah

		[DllImport("kernel32.dll")]
		internal static extern bool ReadProcessMemory(
		  IntPtr hProcess,
		  ulong lpBaseAddress,
		  byte[] lpBuffer,
		  UIntPtr nSize,
		  ref long lpNumberOfBytesWritten);

		[DllImport("kernel32.dll")]
		internal static extern bool WriteProcessMemory(
		  IntPtr hProcess,
		  ulong lpBaseAddress,
		  byte[] lpBuffer,
		  UIntPtr nSize,
		  ref long lpNumberOfBytesWritten);

		[DllImport("kernel32.dll")]
		internal static extern IntPtr OpenProcess(
		  long dwDesiredAccess,
		  bool bInheritHandle,
		  long dwProcessId);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);

		#endregion
	}
}
