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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace BumpVersion.Tasks
{
	/// <summary>Task that reads files from a Visual Studio project and stores them in a variable</summary>
	internal class ReadVSProject : BumpTask
	{
		public ReadVSProject( Dictionary<string, string> settings, Dictionary<string, string> variables )
			: base( settings, variables )
		{
		}

		public override OperationResult Bump( Version oldVersion, Version newVersion )
		{
			// There is no actual bumping to do here.
			return new OperationResult();
		}

		public override Dictionary<string, string> GetVariables()
		{
			Dictionary<string, string> variables = new Dictionary<string, string>();

			string[] elementsToSearch = new[]
			{
				"Compile", "Page", "None"
			};

			string elements = GetValue( "elements" );
			if( elements != null )
			{
				elementsToSearch = elements.Split( ';' );
			}

			string projectFile = GetValue( "projectFile" );
			string outputVar = GetValue( "output" );
			if( projectFile != null && outputVar != null )
			{
				string basePath = Path.GetDirectoryName( projectFile );

				XmlDocument doc = new XmlDocument();
				doc.Load( projectFile );

				foreach( XmlElement itemGroup in doc.DocumentElement.GetElementsByTagName( "ItemGroup" ) )
				{
					foreach( string element in elementsToSearch )
					{
						foreach( XmlElement item in itemGroup.GetElementsByTagName( element ) )
						{
							Files.Add( Path.Combine( basePath, item.GetAttribute( "Include" ) ) );
						}
					}
				}

				variables.Add( outputVar, string.Join( ";", Files ) );
			}

			return variables;
		}

		public override OperationResult Validate()
		{
			OperationResult result = new OperationResult();
			string projectFile = GetValue( "projectFile" );
			if( projectFile == null )
			{
				result.AddError( "No project file specified" );
			}
			else if( !File.Exists( projectFile ) )
			{
				result.AddError( string.Format( "File '{0}' does not exist", projectFile ) );
			}

			if( GetValue( "output" ) == null )
			{
				result.AddError( "No output variable defined" );
			}

			if( Files.Count == 0 )
			{
				result.AddError( "No files found in Visual Studio project" );
			}

			return result;
		}

		private List<string> Files = new List<string>();
	}
}