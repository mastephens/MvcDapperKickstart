using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace MvcKickstart.Infrastructure.Extensions
{
	/// <summary>
	/// Summary description for XmlDocumentExtensions
	/// </summary>
	public static class XmlDocumentExtensions
	{
		/// <summary>
		/// Gets the byte array for the specified document
		/// </summary>
		/// <param name="doc">The document</param>
		/// <returns></returns>
		public static byte[] ToByteArray(this XDocument doc)
		{
			using (var stream = new MemoryStream())
			{
				using (var writer = XmlWriter.Create(stream))
				{
					if (writer != null) doc.WriteTo(writer);
				}

				return stream.ToArray();
			}
		}
	}
}