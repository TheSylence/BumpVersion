// Copyright (c) 2014 Matthias Specht
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.IO;
using System.Linq;

namespace BumpVersion
{
	internal class Program
	{
		internal static void Main( string[] args )
		{
			Console.Error.NewLine = Environment.NewLine;

			CommandLineParser cmdParser = new CommandLineParser( args );
			if( !cmdParser.IsValid )
			{
				cmdParser.PrintUsage();
				Environment.ExitCode = 0;
				return;
			}

			// Parse given version
			Version newVersion;
			if( !Version.TryParse( cmdParser.Version, out newVersion ) )
			{
				Console.Error.WriteLine( "Version '{0}' is not a valid version", cmdParser.Version );
				Environment.ExitCode = -3;
				return;
			}

			// Set project file to load
			string projectFile = "bumpversion.xml";
			if( cmdParser.ProjectFile != null )
			{
				projectFile = cmdParser.ProjectFile;
			}

			// Load project
			Bumper bumper;
			try
			{
				bumper = new Bumper( projectFile );
			}
			catch( FileNotFoundException )
			{
				Console.Error.WriteLine( "The project file '{0}' could not be found", projectFile );
				Environment.ExitCode = -1;
				return;
			}

			// Validate project
			OperationResult validationResult = bumper.Vaildate( newVersion );
			if( !validationResult.IsSuccess )
			{
				Console.Error.WriteLine( "There are some errors in your project file" );
				Console.Error.WriteLine( validationResult.ToString() );
				Environment.ExitCode = -2;
				return;
			}

			// Do the actual bumping
			OperationResult bumpResult = bumper.Bump( newVersion );
			if( !bumpResult.IsSuccess )
			{
				Console.Error.WriteLine( "Failed to bump version:" );
				Console.Error.WriteLine( bumpResult.ToString( true, true ) );
				Environment.ExitCode = -4;
				return;
			}

			// Write new version back to project file
			bumper.SaveCurrentVersion( projectFile, newVersion );

			Console.WriteLine( "Successfully bumped version to {0}", newVersion );
			Environment.ExitCode = 0;
		}
	}
}