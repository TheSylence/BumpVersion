// $Id$

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BumpVersion.Tasks
{
#if false
	/// <summary>
	/// Task that replaces version strings in given files.
	/// </summary>
	internal class ReplaceInFile : BumpTask
	{
		private const string FileKey = "files";
		private Regex Pattern;

		public ReplaceInFile( Dictionary<string, string> settings, Dictionary<string, string> variables )
			: base( settings, variables )
		{
			Pattern = new Regex( @"(\d+\.\d+((\.\d+)?\.\d+)?)" );
		}

		public override OperationResult Bump( Version oldVersion, Version newVersion )
		{
			OperationResult results = new OperationResult();
			string[] files = GetValue( FileKey ).Split( ';' );

			foreach( string fileName in files )
			{
				try
				{
					string content = File.ReadAllText( fileName );

					Pattern.Replace( content, newVersion.ToString() );

					File.WriteAllText( fileName, content );
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
#endif
}