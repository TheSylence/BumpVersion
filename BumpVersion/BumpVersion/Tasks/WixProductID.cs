// Copyright (c) 2014 Matthias Specht
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BumpVersion.Tasks
{
	internal class WixNamespaceManager : XmlNamespaceManager
	{
		public const string Namespace = "http://schemas.microsoft.com/wix/2006/wi";
		public const string Prefix = "w";

		public WixNamespaceManager( XmlNameTable nameTable )
			: base( nameTable )
		{
			AddNamespace( Prefix, Namespace );
		}
	}

	/// <summary>
	/// Task that generates a new Product-ID inside a WiX-Project.
	/// This is useful for generating an installer that can update previous installations.
	/// </summary>
	internal class WixProductID : BumpTask
	{
		private const string FileKey = "wixFile";

		public WixProductID( Dictionary<string, string> settings, Dictionary<string, string> variables )
			: base( settings, variables )
		{
		}

		public override OperationResult Bump( Version oldVersion, Version newVersion )
		{
			OperationResult result = new OperationResult();
			string wixFile = GetValue( FileKey );

			XmlDocument doc = new XmlDocument();
			WixNamespaceManager namespaceManager = new WixNamespaceManager( doc.NameTable );

			doc.Load( wixFile );

			XmlNode node = doc.SelectSingleNode( "w:Wix/w:Product", namespaceManager );
			try
			{
				node.Attributes.GetNamedItem( "Id" ).Value = Guid.NewGuid().ToString();

				doc.PreserveWhitespace = true;
				doc.Save( wixFile );
			}
			catch( Exception ex )
			{
				result.AddError( ex.ToString() );
			}

			return result;
		}

		public override OperationResult Validate()
		{
			OperationResult result = new OperationResult();

			string file = GetValue( FileKey );
			if( string.IsNullOrWhiteSpace( file ) )
			{
				result.AddError( "No files given" );
			}
			else if( !File.Exists( file ) )
			{
				result.AddError( string.Format( "WiX file '{0}' does not exist", file ) );
			}

			return result;
		}
	}
}