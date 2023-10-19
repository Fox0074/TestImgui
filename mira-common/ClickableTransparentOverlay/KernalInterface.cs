using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ClickableTransparentOverlay;

namespace Common
{
    public class KernalInterface
    {
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct _KERNEL_READ_REQUEST
        {
            public long ProcessId;
            public IntPtr Address;
            public IntPtr pBuffer;
            public long Size;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct _KERNEL_WRITE_REQUEST
        {
            public long ProcessId;
            public IntPtr Address;
            public IntPtr pBuffer;
            public long Size;
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFile(
        [MarshalAs(UnmanagedType.LPWStr)]
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        IntPtr lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        IntPtr hTemplateFile);

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            ref long InBuffer,
            int nInBufferSize,
            ref long OutBuffer,
            int nOutBufferSize,
            ref int pBytesReturned,
            [In] ref NativeOverlapped lpOverlapped);


        //TODO: Дублирование кода
        [DllImport("kernel32.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private unsafe static extern System.Boolean DeviceIoControl(
            IntPtr hDevice,
            long dwIoControlCode,
            ref _KERNEL_READ_REQUEST lpInBuffer,
            long nInBufferSize,
            ref _KERNEL_READ_REQUEST lpOutBuffer,
            long nOutBufferSize,
            ref long lpBytesReturned,
            [In] ref NativeOverlapped lpOverlapped);

        [DllImport("kernel32.dll", EntryPoint = "DeviceIoControl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private unsafe static extern System.Boolean DeviceIoControl(
            IntPtr hDevice,
            long dwIoControlCode,
            ref _KERNEL_WRITE_REQUEST lpInBuffer,
            long nInBufferSize,
            ref _KERNEL_WRITE_REQUEST lpOutBuffer,
            long nOutBufferSize,
            ref long lpBytesReturned,
            [In] ref NativeOverlapped lpOverlapped);

        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;

        private const uint FILE_SHARE_READ = 0x00000001;
        private const uint FILE_SHARE_WRITE = 0x00000002;

        private uint GET_CLIENT_ADDRESS = 0x731;
        private uint READ_REQUEST = 0x732;
        private uint WRITE_REQUEST = 0x733;
        private uint GET_MODULE_ADDRESS = 0x734;
        private uint AUTORIZATION_REQUEST = 0x735;

        public IntPtr hDriver;
        public long _processId;

        public KernalInterface(
            string registryPath,
            string driverFile,
            string mapperFile,
            uint getClientAddress,
            uint read,
            uint write,
            uint getModuleAddress,
            uint autorization)
        {
            GET_CLIENT_ADDRESS = getClientAddress;
            READ_REQUEST = read;
            WRITE_REQUEST = write;
            GET_MODULE_ADDRESS = getModuleAddress;
            AUTORIZATION_REQUEST = autorization;

            var lastValue = hDriver;
            hDriver = CreateFile(registryPath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, (IntPtr)0, 3, 0, (IntPtr)0);

            try
            {
                if (hDriver == IntPtr.Zero || hDriver == (IntPtr)(-1))
                {
                    new Process
                    {
                        StartInfo =
                         {
                             FileName = mapperFile,
                             WindowStyle = ProcessWindowStyle.Hidden,
                             Verb = T.GetString("yd3DAgEE7jRRzlli", "runas"),
                             Arguments = driverFile,
                             UseShellExecute = false
                         }
                    }.Start();

                    Thread.Sleep(500);

                    hDriver = CreateFile(registryPath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, (IntPtr)0, 3, 0, (IntPtr)0);
                }
            }
            catch(Exception ex)
            {
                Server.LogError(T.GetString("yd3DAgEE7jRRzlli", "Error: cant inject driver"), "");
                Functions.ShowMessage(T.GetString("yd3DAgEE7jRRzlli", "Ошибка: драйвер не найден"), T.GetString("yd3DAgEE7jRRzlli", "Error"));
                Environment.Exit(0);
            }

            if (hDriver == IntPtr.Zero || hDriver == (IntPtr)(-1))
            {
                Server.LogError(T.GetString("yd3DAgEE7jRRzlli", "Error driver handle not found"), "");
                Functions.ShowMessage(T.GetString("yd3DAgEE7jRRzlli", "Ошибка: драйвер не найден"), T.GetString("yd3DAgEE7jRRzlli", "Error"));
                Environment.Exit(0);
            }
        }

        private uint IoMethodRequest(uint value)
        {
            return (0x00000022 << 16) | (0 << 14) | (value << 2) | 0;
        }

        public IntPtr GetClientAddress(long pid)
        {
            if (hDriver == (IntPtr)(-1))
            {
                return IntPtr.Zero;
            }

            long address = pid;
            int bytes = 0;
            var test = new NativeOverlapped();

            if (DeviceIoControl(hDriver, IoMethodRequest(GET_CLIENT_ADDRESS), ref address, sizeof(long), ref address, sizeof(long), ref bytes, ref test))
            {
                return (IntPtr)address;
            }
            
            return IntPtr.Zero;
        }

        public long GetModuleAddress(long pid)
        {
            if (hDriver == (IntPtr)(-1))
            {
                return 0;
            }

            long address = pid;
            int bytes = 0;
            var test = new NativeOverlapped();

            if (DeviceIoControl(hDriver, IoMethodRequest(GET_MODULE_ADDRESS), ref address, sizeof(long), ref address, sizeof(long), ref bytes, ref test))
            {
                return address;
            }

            return 0;
        }

        public IntPtr ReadChainPtr(IntPtr pointer, int[] offsets)
        {
            try
            {
                IntPtr pointedto = pointer;
                foreach (int offset in offsets)
                {
                    IntPtr tmpPointed = (IntPtr)ReadVirtualMemory<long>(pointedto);
                    pointedto = IntPtr.Add(tmpPointed, offset);
                }

                return pointedto;
            }
            catch (Exception ex)
            {
                return IntPtr.Zero;
            }
        }
        public T ReadVirtualMemory<T>(IntPtr readAddress, int[] offsets)
        {
            return ReadVirtualMemory<T>(ReadChainPtr(readAddress, offsets));
        }

        public T ReadVirtualMemory<T>(IntPtr readAddress)
        {

            if (_processId == -1)
            {
                return default(T);
            }

            if (readAddress <= 0x1000)
                return default(T);

            _KERNEL_READ_REQUEST readRequest;
            var size = Marshal.SizeOf(typeof(T));
            IntPtr bufferPtr = Marshal.AllocHGlobal(size);
            var buffer = new byte[size];
            Marshal.Copy(bufferPtr, buffer, 0, size);

            if (hDriver == (IntPtr)(-1) || readAddress.ToInt64() > 0x7fffffffffff || readAddress.ToInt64() <= 0)
                return default(T);

            readRequest.ProcessId = _processId;
            readRequest.Address = readAddress;
            readRequest.pBuffer = bufferPtr;
            readRequest.Size = size;

            long bytes = 0;
            var test = new NativeOverlapped();

            if (DeviceIoControl(
                hDriver,
                IoMethodRequest(READ_REQUEST),
                ref readRequest,
                32,
                ref readRequest,
                32,
                ref bytes,
                ref test))
            {
     
                Marshal.Copy(bufferPtr, buffer,0, size);
                var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                var data = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
                handle.Free();
                Marshal.FreeHGlobal(bufferPtr);
                return data;
            }

            Marshal.FreeHGlobal(bufferPtr);
            return default(T);
        }

        public byte[] ReadVirtualMemoryBytes(IntPtr readAddress, int size)
        {

            if (_processId == -1)
            {
                return null;
            }


            if (readAddress <= 0x1000)
                return null;

            _KERNEL_READ_REQUEST readRequest;
            IntPtr bufferPtr = Marshal.AllocHGlobal(size);
            var buffer = new byte[size];
            Marshal.Copy(bufferPtr, buffer, 0, size);

            if (hDriver == (IntPtr)(-1) || readAddress.ToInt64() > 0x7fffffffffff || readAddress.ToInt64() <= 0)
                return null;

            readRequest.ProcessId = _processId;
            readRequest.Address = readAddress;
            readRequest.pBuffer = bufferPtr;
            readRequest.Size = size;

            long bytes = 0;
            var test = new NativeOverlapped();

            if (DeviceIoControl(
                hDriver,
                IoMethodRequest(READ_REQUEST),
                ref readRequest,
                32,
                ref readRequest,
                32,
                ref bytes,
                ref test))
            {

                Marshal.Copy(bufferPtr, buffer, 0, size);
                Marshal.FreeHGlobal(bufferPtr);
                return buffer;
            }

            Marshal.FreeHGlobal(bufferPtr);
            return null;
        }

        public string ReadVirtualMemoryString(IntPtr readAddress, int size, bool isAscii = false)
        {
            if (_processId == -1)
            {
                return T.GetString("yd3DAgEE7jRRzlli", "NULL");
            }

            if (size > 1000)
            {
                throw new Exception();
            }


            if (readAddress <= 0x1000)
                return T.GetString("yd3DAgEE7jRRzlli", "NULL");

            _KERNEL_READ_REQUEST readRequest;
            IntPtr ptr = Marshal.AllocHGlobal(size);
            var buffer = new byte[size];
            
            Marshal.Copy(ptr, buffer, 0, size);

            if (hDriver == (IntPtr)(-1) || readAddress.ToInt64() > 0x7fffffffffff || readAddress.ToInt64() <= 0)
            {
                Marshal.FreeHGlobal(ptr);
                return "NULL";
            }

            readRequest.ProcessId = _processId;
            readRequest.Address = readAddress;
            readRequest.pBuffer = ptr;
            readRequest.Size = size;

            long bytes = 0;
            var test = new NativeOverlapped();

            if (DeviceIoControl(
                hDriver,
                IoMethodRequest(READ_REQUEST),
                ref readRequest,
                32,
                ref readRequest,
                32,
                ref bytes,
                ref test))
            {

                Marshal.Copy(ptr, buffer, 0, size);

                if (buffer == null || buffer.Length == 0)
                    return T.GetString("yd3DAgEE7jRRzlli", "NULL");
                byte end = 152;
                var index = buffer.ToList().IndexOf(isAscii ? (byte)0 : end);
                var list = buffer.ToList();
                if (index >= 0)
                    list.RemoveRange(index, buffer.Length - index);
                var data = isAscii ?  Encoding.UTF8.GetString(list.ToArray()) : Encoding.Unicode.GetString(list.ToArray()).Replace("\0","");
                Marshal.FreeHGlobal(ptr);

                return data;
            }

            Marshal.FreeHGlobal(ptr);
            return T.GetString("yd3DAgEE7jRRzlli", "NULL");
        }

        public bool WriteVirtualMemory<T>(IntPtr writeAddress, float writeValue)
        {
            if (_processId == -1)
            {
                return false;
            }


            if (writeAddress <= 0x1000)
                return false;

            _KERNEL_WRITE_REQUEST writeRequest;
            var size = Marshal.SizeOf(typeof(T));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            var buffer = BitConverter.GetBytes(writeValue);
            Marshal.Copy(buffer, 0, ptr, size);

            if (hDriver == (IntPtr)(-1))
                return false;

            writeRequest.ProcessId = _processId;
            writeRequest.Address = writeAddress;
            writeRequest.pBuffer = ptr;
            writeRequest.Size = size;

            long bytes = 0;
            var test = new NativeOverlapped();

            if (DeviceIoControl(
                hDriver,
                IoMethodRequest(WRITE_REQUEST),
                ref writeRequest,
                32,
                ref writeRequest,
                32,
                ref bytes,
                ref test))
            {
                Marshal.FreeHGlobal(ptr);
                return true;
            }

            Marshal.FreeHGlobal(ptr);
            return false;
        }

        public bool WriteVirtualMemory(IntPtr writeAddress, byte[] writeValue)
        {
            if (_processId == -1)
            {
                return false;
            }

            if (writeAddress <= 0x1000)
                return false;

            _KERNEL_WRITE_REQUEST writeRequest;
            var size = writeValue.Length;
            IntPtr ptr = Marshal.AllocHGlobal(size);
            var buffer = writeValue;
            Marshal.Copy(buffer, 0, ptr, size);

            if (hDriver == (IntPtr)(-1))
                return false;

            writeRequest.ProcessId = _processId;
            writeRequest.Address = writeAddress;
            writeRequest.pBuffer = ptr;
            writeRequest.Size = size;

            long bytes = 0;
            var test = new NativeOverlapped();

            if (DeviceIoControl(
                hDriver,
                IoMethodRequest(WRITE_REQUEST),
                ref writeRequest,
                32,
                ref writeRequest,
                32,
                ref bytes,
                ref test))
            {
                Marshal.FreeHGlobal(ptr);
                return true;
            }

            Marshal.FreeHGlobal(ptr);
            return false;
        }
    }
}
