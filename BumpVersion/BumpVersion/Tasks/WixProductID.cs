// $Id$

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
			}
			catch( Exception ex )
			{
				result.AddError( ex.ToString() );
			}

			doc.PreserveWhitespace = true;
			doc.Save( wixFile );

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