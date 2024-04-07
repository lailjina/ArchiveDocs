using Arctium.Cryptography.HashFunctions.Hashes;
using Force.Crc32;
using Murmur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ArchiveDocs
{
    public class HashCodeBench
    {
        public static byte[] GetFakeFileBytes()
        {
            // Пример массива байтов в виде строки шестнадцатеричных значений
            string hexBytes = "AABBCCDDEEFF00112233445566778899";

            // Преобразование строки шестнадцатеричных значений в массив байтов
            byte[] fileBytes = new byte[hexBytes.Length / 2];
            for (int i = 0; i < hexBytes.Length; i += 2)
            {
                fileBytes[i / 2] = Convert.ToByte(hexBytes.Substring(i, 2), 16);
            }

            return fileBytes;
        }

        [Benchmark]
        public string GetCRC32Hash()
        {
            var crc32Algorithm = new Crc32Algorithm();

            byte[] crc32Bytes = crc32Algorithm.ComputeHash(GetFakeFileBytes());
            string hash = BitConverter.ToString(crc32Bytes).Replace("-", "").ToLower();

            return hash;
        }

        [Benchmark]
        public string GetMD5Hash()
        {
            using (var md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(GetFakeFileBytes());
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hash;
            }
        }

        [Benchmark]
        public string GetStreebogHash()
        {
            Streebog_256 streebog = new Streebog_256();
            streebog.HashBytes(GetFakeFileBytes());
            byte[] hashBytes = streebog.HashFinal();
            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hash;
        }


        [Benchmark]
        public string GetMurmurHash()
        {

            HashAlgorithm murmur128 = MurmurHash.Create128(managed: false); // returns a 128-bit algorithm using "unsafe" code with default seed
            byte[] hash = murmur128.ComputeHash(GetFakeFileBytes());

            // you can also use a seed to affect the hash
            //HashAlgorithm seeded128 = MurmurHash.Create128(seed: 3475832); // returns a managed 128-bit algorithm with seed
            //byte[] seedResult = murmur128.ComputeHash(GetFakeFileBytes());

            string hashStr = BitConverter.ToString(hash).Replace("-", "").ToLower();

            return hashStr;
        }
    }
}
