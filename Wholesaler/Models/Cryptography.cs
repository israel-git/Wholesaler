using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Wholesaler.Models
{
    public static class Cryptography
    {
        public static string Hash(string data, int salt)
        {
            return Hexify(HashBytes(data, salt));
        }

        public static byte[] HashBytes(string data, int salt)
        {
            if (data == null) throw new ArgumentNullException("data", "Cannot hash null.");
            var bytes = Encoding.UTF8.GetBytes(data).Concat(BitConverter.GetBytes(salt)).ToArray();
            using (var sha = new SHA256Cng())
                return sha.ComputeHash(bytes);
        }

        public static string Hexify(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public static string RandomGuidStr()
        {
            return Hexify(RandomBytes());
        }

        public static Guid RandomGuid()
        {
            return new Guid(RandomBytes());
        }

        public static byte[] RandomBytes()
        {
            var randomBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(randomBytes);
            return randomBytes;
        }

    }
}