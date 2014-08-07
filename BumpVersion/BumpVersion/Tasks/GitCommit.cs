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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BumpVersion.Tasks
{
	/// <summary>
	/// Commits changes to a git repository.
	/// </summary>
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
			else
			{
				try
				{
					Process proc = Process.Start( inf );
					proc.WaitForExit();
				}
				catch( Exception ex )
				{
					result.AddError( string.Format( "Failed to execute git commit: {0}", ex ) );
				}
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