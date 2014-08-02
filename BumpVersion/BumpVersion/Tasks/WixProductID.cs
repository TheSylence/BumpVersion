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
	/// <summary>
	/// Task that generates a new Product-ID inside a WiX-Project.
	/// This is useful for generating an installer that can update previous installations.
	/// </summary>
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

	internal class WixProductID : BumpTask
	{
		private const string FileKey = "wixFile";

		public WixProductID( Dictionary<string, string> settings )
			: base( settings )
		{
		}

		public override OperationResult Bump( Version newVersion )
		{
			OperationResult result = new OperationResult();
			string wixFile = Settings[FileKey];

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

			string file;
			if( !Settings.TryGetValue( FileKey, out file ) )
			{
				result.AddError( "No files given" );
			}
			else if( string.IsNullOrWhiteSpace( file ) )
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