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

namespace ArchiveDocs
{
    public class HashCode
    {
        public static string GetCRC32Hash(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Файл не найден", filePath);
            }

            byte[] fileBytes = File.ReadAllBytes(filePath);
            var crc32Algorithm = new Crc32Algorithm();

            byte[] crc32Bytes = crc32Algorithm.ComputeHash(fileBytes);
            string hash = BitConverter.ToString(crc32Bytes).Replace("-", "").ToLower();

            return hash;
        }

        public static string GetMD5Hash(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Файл не найден", filePath);
            }

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = md5.ComputeHash(stream);
                    string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                    return hash;
                }
            }
        }

        public static string GetStreebogHash(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Файл не найден", filePath);
            }

            byte[] fileBytes = File.ReadAllBytes(filePath);
            Streebog_256 streebog = new Streebog_256();
            streebog.HashBytes(fileBytes);
            byte[] hashBytes = streebog.HashFinal();
            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hash;
        }


        public static string GetMurmurHash(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Файл не найден", filePath);
            }

            byte[] fileBytes = File.ReadAllBytes(filePath);

            HashAlgorithm murmur128 = MurmurHash.Create128(managed: false); // returns a 128-bit algorithm using "unsafe" code with default seed
            byte[] hash = murmur128.ComputeHash(fileBytes);

            // you can also use a seed to affect the hash
            HashAlgorithm seeded128 = MurmurHash.Create128(seed: 3475832); // returns a managed 128-bit algorithm with seed
            byte[] seedResult = murmur128.ComputeHash(fileBytes);

            string hashStr = BitConverter.ToString(seedResult).Replace("-", "").ToLower();

            return hashStr;
        }
    }
}
