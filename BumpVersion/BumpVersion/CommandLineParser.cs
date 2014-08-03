// $Id$

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BumpVersion
{
	internal class CommandLineParser
	{
		public readonly bool IsValid;
		public readonly string ProjectFile;
		public readonly string Version;

		public CommandLineParser( string[] args )
		{
			if( args.Length < 1 || args.Length > 2 )
			{
				return;
			}

			IsValid = true;
			Version = args[0];
			if( args.Length == 2 )
			{
				ProjectFile = args[1];
			}
		}

		public void PrintUsage()
		{
			Console.WriteLine( "BumpVersion {0} by Matthias Specht", Assembly.GetExecutingAssembly().GetName().Version );
			Console.WriteLine( "Usage: {0} VERSION [PROJECT_FILE=bumpversion.xml]", Environment.GetCommandLineArgs()[0] );
			Console.WriteLine();
			Console.WriteLine( "VERSION:      The version to bump to" );
			Console.WriteLine( "PROJECT_FILE: The project file to load. Defaults to 'bumpversion.xml'" );
		}
	}
}