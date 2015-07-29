using System;
using System.Security.Cryptography;
using System.Text;

namespace ReactiveServices.Extensions
{
    public static class Extensions
    {
        public static StringExtender WithLength(this string str, int length)
        {
            return StringExtender.ForString(str).WithLength(length);
        }

        public static string GetMd5Hash(this string input, MD5 md5Hash)
        {

            // Convert the input string to a byte array and compute the hash. 
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            foreach (var b in data)
            {
                sBuilder.Append(b.ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        public static string Tail(this string input, int count)
        {
            if (input == null) throw new ArgumentNullException("input");
            if (input.Length < count) throw new ArgumentException("Input length should be greater than or equals the tail count!");
            return input.Substring(input.Length - count, count);
        }
    }
}
