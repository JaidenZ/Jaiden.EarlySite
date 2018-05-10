namespace EarlySite.Core.Serialization
{
    using EarlySite.Core.Utils;
    using System;
    using System.Runtime.InteropServices;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// 定长格式化
    /// </summary>
    public unsafe class FixedLengthFormatter
    {
        public static int SizeOf(object graph)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }
            return Marshal.SizeOf(graph);
        }

        public static int SizeOf<T>()
        {
            return Marshal.SizeOf(typeof(T)); 
        }

        public static byte[] Serialize(object graph)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }
            byte[] buffer = new byte[FixedLengthFormatter.SizeOf(graph)];
            Marshal.StructureToPtr(graph, Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0), true);
            return buffer;
        }

        public static void Deserialize(IntPtr ptr, long ofs, object graph)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }
            Marshal.PtrToStructure(unchecked((IntPtr)((long)ptr + ofs)), graph);
        }

        public static void Deserialize(IntPtr ptr, object graph)
        {
            FixedLengthFormatter.Deserialize(ptr, 0, graph);
        }

        public static void Deserialize(byte[] buffer, long ofs, object graph)
        {
            if (ListUtils.IsNullOrEmpty<byte>(buffer))
            {
                throw new ArgumentException();
            }
            FixedLengthFormatter.Deserialize(Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0), ofs, graph);
        }

        public static void Deserialize(byte[] buffer, object graph)
        {
            FixedLengthFormatter.Deserialize(buffer, 0, graph);
        }

        public static T Deserialize<T>(byte[] buffer, long ofs)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("graph");
            }
            fixed (byte* pinned = buffer)
            {
                return (T)Marshal.PtrToStructure((IntPtr)(pinned + ofs), typeof(T));
            }
        }

        public static T Deserialize<T>(byte[] buffer)
        {
            return FixedLengthFormatter.Deserialize<T>(buffer, 0);
        }
    }
}
