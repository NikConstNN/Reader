using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;

namespace Common.Encrypting
{
    /// <summary>
    /// Шифрование/дешифрование string или byte[] по ключевому слову
    /// </summary>
    public class CryptPassword
    {
        /// <summary>
        /// Шифрование по ключевому слову
        /// </summary>
        /// <param name="data">Данные</param>
        /// <param name="password">Ключевое слово</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, string password)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            ICryptoTransform ct = sa.CreateEncryptor(
                (new PasswordDeriveBytes(password, null)).GetBytes(16),
                new byte[16]);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            try
            {
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
            finally
            {
                ms.Close();
                cs.Close();
                cs.Clear();
                sa.Clear();
                ms.Dispose();
                cs.Dispose();
                ct.Dispose();
            }
        }
        /// <summary>
        /// Шифрование по ключевому слову
        /// </summary>
        /// <param name="data">Данные</param>
        /// <param name="password">Ключевое слово</param>
        /// <returns></returns>
        public static string Encrypt(string data, string password)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(data), password));
        }
        /// <summary>
        /// Дешифрование по ключевому слову
        /// </summary>
        /// <param name="data">Данные</param>
        /// <param name="password">Ключевое слово</param>
        /// <returns></returns>
        static public byte[] Decrypt(byte[] data, string password)
        {
            CryptoStream cs = InternalDecrypt(data, password);
            BinaryReader br = new BinaryReader(cs);
            try
            {
                return br.ReadBytes((int)br.BaseStream.Length);
            }
            finally
            {
                br.Close();
                cs.Close();
                cs.Clear();
                cs.Dispose();
            }
        }
        /// <summary>
        /// Дешифрование по ключевому слову
        /// </summary>
        /// <param name="data">Данные</param>
        /// <param name="password">Ключевое слово</param>
        /// <returns></returns>
        static public string Decrypt(string data, string password)
        {
            CryptoStream cs = InternalDecrypt(Convert.FromBase64String(data), password);
            StreamReader sr = new StreamReader(cs);
            try
            {
                return sr.ReadToEnd();
            }
            finally
            {
                sr.Close();
                cs.Close();
                cs.Clear();
                sr.Dispose();
                cs.Dispose();
            }
        }

        static CryptoStream InternalDecrypt(byte[] data, string password)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            ICryptoTransform ct = sa.CreateDecryptor(
                (new PasswordDeriveBytes(password, null)).GetBytes(16),
                new byte[16]);
            MemoryStream ms = new MemoryStream(data);
            return new CryptoStream(ms, ct, CryptoStreamMode.Read);
        }
    }
}