using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Extensions
{
    public class FileExtension
    {
        #region 加密解密文件
        private static byte[] DESKeys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };//密钥
        private static byte[] DesKey = { 0xFE, 0xDC, 0xBA, 0x09, 0x87, 0x65, 0x43, 0x21 };
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="inName">需加密文件路径(包含后缀)</param>
        /// <param name="outName">加密后文件路径(包含后缀)</param>
        private static bool EncryptFile(string inPath, string outPath)
        {
            try
            {
                FileStream fin = new FileStream(inPath, FileMode.Open, FileAccess.Read);
                FileStream fout = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.Write);
                fout.SetLength(0);
                byte[] bin = new byte[100];
                long rdlen = 0;
                long totlen = fin.Length;
                int len;

                DES des = new DESCryptoServiceProvider();
                CryptoStream encStream = new CryptoStream(fout, des.CreateEncryptor(DESKeys, DesKey), CryptoStreamMode.Write);

                //Read from the input file, then encrypt and write to the output file.
                while (rdlen < totlen)
                {
                    len = fin.Read(bin, 0, 100);
                    encStream.Write(bin, 0, len);
                    rdlen = rdlen + len;
                }
                encStream.Close();
                fout.Close();
                fin.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="inName">需解密文件路径(包含后缀)</param>
        /// <param name="outName">解密后文件路径(包含后缀)</param>
        private static bool DecryptData(string inPath, string outPath)
        {
            try
            {
                FileStream fin = new FileStream(inPath, FileMode.Open, FileAccess.Read);
                FileStream fout = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.Write);
                fout.SetLength(0);
                byte[] bin = new byte[100];
                long rdlen = 0;
                long totlen = fin.Length;
                int len;
                DES des = new DESCryptoServiceProvider();
                CryptoStream encStream = new CryptoStream(fout, des.CreateDecryptor(DESKeys, DESKeys), CryptoStreamMode.Write);
                while (rdlen < totlen)
                {
                    len = fin.Read(bin, 0, 100);
                    encStream.Write(bin, 0, len);
                    rdlen = rdlen + len;
                }
                encStream.Close();
                fout.Close();
                fin.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        #endregion 
    }
}
