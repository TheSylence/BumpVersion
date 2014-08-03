// $Id$

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BumpVersion.Tasks
{
	/// <summary>
	/// Task that writes the given version to one or more files
	/// </summary>
	internal class WriteToFile : BumpTask
	{
		private const string FileKey = "files";

		public WriteToFile( Dictionary<string, string> settings, Dictionary<string, string> variables )
			: base( settings, variables )
		{
		}

		public override OperationResult Bump( Version oldVersion, Version newVersion )
		{
			OperationResult results = new OperationResult();
			string[] files = GetValue( FileKey ).Split( ';' );

			foreach( string fileName in files )
			{
				try
				{
					File.WriteAllText( fileName, newVersion.ToString() );
				}
				catch( IOException ex )
				{
					results.AddError( string.Format( "{0} => {1}", fileName, ex ) );
				}
			}

			return results;
		}

		public override OperationResult Validate()
		{
			OperationResult result = new OperationResult();

			string files = GetValue( FileKey );
			if( string.IsNullOrWhiteSpace( files ) )
			{
				result.AddError( "No files given" );
			}

			return result;
		}
	}
}