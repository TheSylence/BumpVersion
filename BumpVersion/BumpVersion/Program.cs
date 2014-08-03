using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BumpVersion
{
	internal class Program
	{
		internal static void Main( string[] args )
		{
			Console.Error.NewLine = Environment.NewLine;

			if( args.Length < 1 || args.Length > 2 )
			{
				PrintUsage();
				Environment.ExitCode = 0;
				return;
			}

			// Parse given version
			Version newVersion;
			if( !Version.TryParse( args[0], out newVersion ) )
			{
				Console.Error.WriteLine( "Version '{0}' is not a valid version", args[0] );
				Environment.ExitCode = -3;
				return;
			}

			// Set project file to load
			string projectFile = "bumpversion.xml";
			if( args.Length == 2 )
			{
				projectFile = args[1];
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

			// TODO: Check that new and old version are different

			// Validate project
			OperationResult validationResult = bumper.Vaildate();
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

		private static void PrintUsage()
		{
			Console.WriteLine( "BumpVersion {0} by Matthias Specht", Assembly.GetExecutingAssembly().GetName().Version );
			Console.WriteLine( "Usage: {0} VERSION [PROJECT_FILE=bumpversion.xml]", Environment.GetCommandLineArgs()[0] );
			Console.WriteLine();
			Console.WriteLine( "VERSION:      The version to bump to" );
			Console.WriteLine( "PROJECT_FILE: The project file to load. Defaults to 'bumpversion.xml'" );
		}
	}
}