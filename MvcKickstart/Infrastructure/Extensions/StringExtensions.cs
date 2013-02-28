using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ServiceStack.Text;

namespace MvcKickstart.Infrastructure.Extensions
{
	public static class StringExtensions
	{
		public static string ToMD5Hash(this string input)
		{
			var crypto = MD5.Create();
			var inputBytes = Encoding.UTF8.GetBytes(input);
			var hashBytes = crypto.ComputeHash(inputBytes);

			// Convert byte array to hex string
			var sb = new StringBuilder();
			foreach (var hashByte in hashBytes)
			{
				sb.Append(hashByte.ToString("X2"));
			}
			return sb.ToString();
		}
		public static string ToSHAHash(this string input)
		{
			var crypto = SHA256.Create();
			var inputBytes = Encoding.UTF8.GetBytes(input);
			var hashBytes = crypto.ComputeHash(inputBytes);

			// Convert byte array to hex string
			var sb = new StringBuilder();
			foreach (var hashByte in hashBytes)
			{
				sb.Append(hashByte.ToString("X2"));
			}
			return sb.ToString();
		}
		public static string ToSentence(this string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				return string.Empty;

			var newText = new StringBuilder(text.Length * 2);
			newText.Append(text[0]);
			for (var i = 1; i < text.Length; i++)
			{
				if (char.IsUpper(text[i]) && text[i - 1] != ' ')
					newText.Append(' ');
				newText.Append(text[i]);
			}
			return newText.ToString();
		}

		public static string ToJson<T>(this T obj)
		{
			var settings = new JsonSerializerSettings
				               {
					               ContractResolver = new CamelCasePropertyNamesContractResolver()
				               };
			return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, settings);
		}

        public static string ToSEOFriendlyName(this string s)
        {
            if (s == null) return null;
            s = s.ToLower();
            s = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }
            s = sb.ToString().Normalize(NormalizationForm.FormC);
            s = Regex.Replace(s, @"[^a-z0-9_\-]+", "-", RegexOptions.IgnoreCase);
            s = s.Trim(new char[] { '-' });

            return s;
        }

        public static string ToSEOFriendlyNameTitleCase(this string s)
        {
            if (s == null) return null;
            return s.ToSEOFriendlyName().ToTitleCase();
        }

        /// <summary>
        /// Get a substring of the first N characters.
        /// </summary>
        public static string Truncate(this string source, int length)
        {
            if (source == null) return null;

            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }
            return source;
        }
	}
}