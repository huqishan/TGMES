using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Extensions
{
    public static class ArrayExtension
    {
        public static short[] ByteToShort(this byte[] arr, bool isConvertData = false)
        {
            short[] result = new short[arr.Length];
            if (!isConvertData)
                result = arr.Select(item => (short)item).ToArray();
            else
            {
                //两个byte合成一个short
                result = new short[Convert.ToInt32(Math.Round((double)arr.Length / 2, MidpointRounding.AwayFromZero))];
                IntPtr p = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
                Marshal.Copy(p, result, 0, result.Length);
            }
            return result;
        }
        public static short[] ShortToBinary(this short[] arr)
        {
            int cursor = 0;
            short[] binaryArr = new short[arr.Length * 16];
            for (int i = 0; i < arr.Length; i++)
            {
                string str = Convert.ToString(arr[i], 2).PadLeft(16, '0').ReverseExtension();
                for (int j = 0; j < str.Length; j++)
                {
                    binaryArr[cursor] = Convert.ToInt16(str[j].ToString());
                    cursor++;
                }
            }
            return binaryArr;
        }
        public static byte[] ShortToByte(this short[] arr, bool isConvertData = false)
        {
            byte[] result = new byte[arr.Length];
            if (!isConvertData)
                result = arr.Select(item => (byte)item).ToArray();
            else
            {
                //一个short拆分为两个byte
                result = new byte[arr.Length * Marshal.SizeOf(arr[0])];
                IntPtr p = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
                Marshal.Copy(p, result, 0, result.Length);
            }
            return result;
        }
        public static string AsciiToString(this short[] arr)
        {
            ASCIIEncoding ASCIITochar = new ASCIIEncoding();
            char[] ascii = ASCIITochar.GetChars(arr.ShortToByte());      // 将接收字节解码为ASCII字符数组
            return string.Join("", ascii);
        }
        public static int[] ShortToInt(this short[] arr, bool isConvertData = false)
        {
            int[] result = new int[arr.Length];
            if (!isConvertData)
                result = arr.Select(item => (int)item).ToArray();
            else
            {
                //两个byte合成一个short
                result = new int[Convert.ToInt32(Math.Round((double)arr.Length / 2, MidpointRounding.AwayFromZero))];
                IntPtr p = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
                Marshal.Copy(p, result, 0, result.Length);
            }
            return result;
        }
        public static int ShortToInt(short highData, short lowData2)
        {
            uint ret = 0;
            ret = (uint)(highData & 0xFFFF) | (uint)(lowData2 << 16);
            return (int)ret;
        }
    }
}
