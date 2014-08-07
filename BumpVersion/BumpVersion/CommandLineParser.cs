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