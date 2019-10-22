using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace bayoen.library.General.Memories
{
    /// <summary>
    /// ProcessMemory (edited by SemiR4in)
    /// </summary>
    public class ProcessMemory
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint dwSize, uint lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, uint lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool GetExitCodeProcess(IntPtr hObject, out uint lpExitCode);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject); // unused?

        [DllImport("kernel32.dll")]
        private static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect); // unused?

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll")]
        private static extern uint SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        private static extern int ResumeThread(IntPtr hThread);

        public IntPtr GetBaseAddress()
        {
            //IntPtr baseAddress = (IntPtr)0;
            //ProcessModule processModule = this.mainProcess[0].MainModule;
            //baseAddress = processModule.BaseAddress;
            //return baseAddress;
            try
            {
                if (this.mainProcess.Length == 0) return IntPtr.Zero;
                return this.mainProcess[0].MainModule.BaseAddress;
            }
            catch
            {
                return IntPtr.Zero;
            }
            
        }

        public string ProcessName;
        public Process Process;
        private Process[] mainProcess;
        private IntPtr processHandle = IntPtr.Zero;

        public ProcessMemory(string param)
        {
            this.ProcessName = param;
        }

        public bool CheckProcess()
        {
            if (this.ProcessName == null) return false;

            bool success = GetExitCodeProcess(this.processHandle, out uint code);

            if (success && code != 259)
            {
                ProcessMemory.CloseHandle(this.processHandle);
                this.processHandle = IntPtr.Zero;
            }

            if (this.processHandle == IntPtr.Zero)
            {
                this.mainProcess = Process.GetProcessesByName(this.ProcessName);
                if (this.mainProcess.Length == 0) return false;

                this.Process = this.mainProcess[0];

                this.processHandle = OpenProcess(0x001F0FFF, false, this.mainProcess[0].Id);
                if (this.processHandle == IntPtr.Zero) return false;
            }

            return true;
        }

        public void Suspend()
        {
            foreach (ProcessThread pT in this.mainProcess[0].Threads)
            {
                IntPtr pOpenThread = OpenThread(0x02 /* suspend/resume */, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                ProcessMemory.SuspendThread(pOpenThread);
                ProcessMemory.CloseHandle(pOpenThread);
            }
        }

        public void Resume()
        {
            foreach (ProcessThread pT in this.mainProcess[0].Threads)
            {
                IntPtr pOpenThread = OpenThread(0x02 /* suspend/resume */, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                var suspendCount = 0;
                do
                {
                    suspendCount = ResumeThread(pOpenThread);
                } while (suspendCount > 0);

                ProcessMemory.CloseHandle(pOpenThread);
            }
        }

        public IntPtr Traverse(IntPtr pOffset, long[] offsets)
        {
            if (pOffset == IntPtr.Zero) return IntPtr.Zero;

            foreach (long offset in offsets)
            {
                pOffset = (IntPtr)(ReadInt64(pOffset) + offset);
                if (pOffset == IntPtr.Zero) return IntPtr.Zero;
            }

            return pOffset;
        }

        public byte[] ReadByteArray(IntPtr pOffset, uint pSize)
        {
            if (CheckProcess())
            {
                VirtualProtectEx(this.processHandle, pOffset, (UIntPtr)pSize, 0x04 /* rw */, out uint flNewProtect);

                byte[] array = new byte[pSize];
                ReadProcessMemory(this.processHandle, pOffset, array, pSize, 0u);

                VirtualProtectEx(this.processHandle, pOffset, (UIntPtr)pSize, flNewProtect, out flNewProtect);
                //ProcessMemory.CloseHandle(this.processHandle);
                return array;

            }
            else return new byte[1];
        }
        public byte[] ReadByteArray(IntPtr pOffset, uint pSize, params long[] offsets) => ReadByteArray(Traverse(pOffset, offsets), pSize);

        public string ReadStringUnicode(IntPtr pOffset, uint pSize) => CheckProcess()
            ? Encoding.Unicode.GetString(ReadByteArray(pOffset, pSize), 0, (int)pSize)
            : "";
        public string ReadStringUnicode(IntPtr pOffset, uint pSize, params long[] offsets) => ReadStringUnicode(Traverse(pOffset, offsets), pSize);

        public string ReadStringASCII(IntPtr pOffset, uint pSize) => CheckProcess()
            ? Encoding.ASCII.GetString(ReadByteArray(pOffset, pSize), 0, (int)pSize)
            : "";
        public string ReadStringASCII(IntPtr pOffset, uint pSize, params long[] offsets) => ReadStringASCII(Traverse(pOffset, offsets), pSize);

        public char ReadChar(IntPtr pOffset) => CheckProcess()
            ? BitConverter.ToChar(ReadByteArray(pOffset, 0x01), 0)
            : ' ';
        public char ReadChar(IntPtr pOffset, params long[] offsets) => ReadChar(Traverse(pOffset, offsets));

        public bool ReadBoolean(IntPtr pOffset) => CheckProcess()
            ? BitConverter.ToBoolean(ReadByteArray(pOffset, 0x01), 0)
            : false;
        public bool ReadBoolean(IntPtr pOffset, params long[] offsets) => ReadBoolean(Traverse(pOffset, offsets));

        public byte ReadByte(IntPtr pOffset) => CheckProcess()
            ? ReadByteArray(pOffset, 0x01)[0]
            : (byte)0;
        public byte ReadByte(IntPtr pOffset, params long[] offsets) => ReadByte(Traverse(pOffset, offsets));

        public short ReadInt16(IntPtr pOffset) => CheckProcess()
            ? BitConverter.ToInt16(ReadByteArray(pOffset, 0x02), 0)
            : (short)0;
        public short ReadInt16(IntPtr pOffset, params long[] offsets) => ReadInt16(Traverse(pOffset, offsets));

        public int ReadInt32(IntPtr pOffset) => CheckProcess()
            ? BitConverter.ToInt32(ReadByteArray(pOffset, 4u), 0)
            : 0;
        public int ReadInt32(IntPtr pOffset, params long[] offsets) => ReadInt32(Traverse(pOffset, offsets));

        public long ReadInt64(IntPtr pOffset) => CheckProcess()
            ? BitConverter.ToInt64(ReadByteArray(pOffset, 8u), 0)
            : 0;
        public long ReadInt64(IntPtr pOffset, params long[] offsets) => ReadInt64(Traverse(pOffset, offsets));

        public ushort ReadUInt16(IntPtr pOffset) => CheckProcess()
            ? BitConverter.ToUInt16(ReadByteArray(pOffset, 0x02), 0)
            : (ushort)0;
        public ushort ReadUInt16(IntPtr pOffset, params long[] offsets) => ReadUInt16(Traverse(pOffset, offsets));

        public uint ReadUInt32(IntPtr pOffset) => CheckProcess()
            ? BitConverter.ToUInt32(ReadByteArray(pOffset, 4u), 0)
            : 0;
        public uint ReadUInt32(IntPtr pOffset, params long[] offsets) => ReadUInt32(Traverse(pOffset, offsets));

        public ulong ReadUInt64(IntPtr pOffset) => CheckProcess()
            ? BitConverter.ToUInt64(ReadByteArray(pOffset, 8u), 0)
            : 0;
        public ulong ReadUInt64(IntPtr pOffset, params long[] offsets) => ReadUInt64(Traverse(pOffset, offsets));

        public float ReadFloat(IntPtr pOffset) => CheckProcess()
            ? BitConverter.ToSingle(ReadByteArray(pOffset, 8u), 0)
            : 0f;
        public float ReadFloat(IntPtr pOffset, params long[] offsets) => ReadFloat(Traverse(pOffset, offsets));

        public double ReadDouble(IntPtr pOffset) => CheckProcess()
            ? BitConverter.ToDouble(ReadByteArray(pOffset, 8u), 0)
            : 0.0;
        public double ReadDouble(IntPtr pOffset, params long[] offsets) => ReadDouble(Traverse(pOffset, offsets));

        public bool WriteByteArray(IntPtr pOffset, byte[] pBytes)
        {
            if (CheckProcess())
            {
                VirtualProtectEx(this.processHandle, pOffset, (UIntPtr)pBytes.Length, 0x04 /* rw */, out uint flNewProtect);

                bool flag = WriteProcessMemory(this.processHandle, pOffset, pBytes, (uint)pBytes.Length, 0u);

                VirtualProtectEx(this.processHandle, pOffset, (UIntPtr)pBytes.Length, flNewProtect, out flNewProtect);
                return flag;

            }
            else return false;
        }

        public bool WriteByte(IntPtr pOffset, byte pData) => WriteByteArray(pOffset, BitConverter.GetBytes(pData));
        public bool WriteByte(IntPtr pOffset, byte pData, params long[] offsets) => WriteByte(Traverse(pOffset, offsets), pData);

        public bool WriteInt32(IntPtr pOffset, int pData) => WriteByteArray(pOffset, BitConverter.GetBytes(pData));
        public bool WriteInt32(IntPtr pOffset, byte pData, params long[] offsets) => WriteInt32(Traverse(pOffset, offsets), pData);
    }
}
