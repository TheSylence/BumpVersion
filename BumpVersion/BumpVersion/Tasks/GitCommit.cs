// $Id$

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BumpVersion.Tasks
{
	internal class GitCommit : BumpTask
	{
		public GitCommit( Dictionary<string, string> settings, Dictionary<string, string> variables )
			: base( settings, variables )
		{
		}

		public override OperationResult Bump( Version oldVersion, Version newVersion )
		{
			string message = GetValue( "message" );
			if( message == null )
			{
				message = string.Format( "Bump version to {0}", newVersion );
			}
			else if( message.Contains( "{0}" ) )
			{
				message = string.Format( message, newVersion );
			}

			message = string.Format( "\"{0}\"", message );

			OperationResult result = new OperationResult();

			ProcessStartInfo inf = new ProcessStartInfo();
			inf.Arguments = string.Format( "commit -m {0}", message );
			inf.FileName = GetFullPathFromEnvironment( "git.exe" );
			if( string.IsNullOrEmpty( inf.FileName ) )
			{
				result.AddError( "git.exe not found in PATH" );
			}

			try
			{
				Process proc = Process.Start( inf );
				proc.WaitForExit();
			}
			catch( Exception ex )
			{
				result.AddError( string.Format( "Failed to execute git commit: {0}", ex ) );
			}

			return result;
		}

		public override OperationResult Validate()
		{
			OperationResult result = new OperationResult();
			string message = GetValue( "message" );
			if( message != null )
			{
				if( !message.Contains( "{0}" ) )
				{
					result.AddWarning( "Message contains no placeholder for version" );
				}
			}

			return result;
		}
	}
}