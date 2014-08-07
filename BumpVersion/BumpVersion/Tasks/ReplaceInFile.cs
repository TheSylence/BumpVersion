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
	/// <summary>
	/// Task that replaces all occurrences of a token in files.
	/// </summary>
	internal class ReplaceInFile : BumpTask
	{
		private const string FileKey = "files";
		private const string SearchKey = "search";
		private const string ReplacementKey = "replace";

		public ReplaceInFile( Dictionary<string, string> settings, Dictionary<string, string> variables )
			: base( settings, variables )
		{
		}

		public override OperationResult Bump( Version oldVersion, Version newVersion )
		{
			OperationResult result = new OperationResult();

			string versionPattern = Regex.Escape( oldVersion.ToString() );
			//string replacement = newVersion.ToString();
			string replacement = GetValue( ReplacementKey );
			if( replacement == null )
			{
				replacement = string.Format( "{0}$2", newVersion );
			}
			else
			{
				replacement = string.Format( replacement, newVersion );
			}

			string pattern = GetValue( SearchKey );
			if( pattern == null )
			{
				pattern = string.Format( "\\b({0})\\b([^\\.]){{1}}", versionPattern );
			}
			else
			{
				pattern = string.Format( pattern, versionPattern );
			}
			Regex regex = new Regex( pattern );

			string[] files = GetValue( FileKey ).Split( ';' );
			foreach( string file in files )
			{
				try
				{
					string content = File.ReadAllText( file );

					content = regex.Replace( content, replacement );

					File.WriteAllText( file, content );
				}
				catch( IOException ex )
				{
					result.AddError( string.Format( "{0} => {1}", file, ex ) );
				}
			}

			return result;
		}

		public override OperationResult Validate()
		{
			OperationResult result = new OperationResult();

			string files = GetValue( FileKey );
			if( string.IsNullOrWhiteSpace( files ) )
			{
				result.AddError( "No files given" );
			}
			else
			{
				foreach( string file in files.Split( ';' ) )
				{
					if( !File.Exists( file ) )
					{
						result.AddError( string.Format( "File '{0}' does not exist", file ) );
					}
				}
			}

			return result;
		}
	}
}