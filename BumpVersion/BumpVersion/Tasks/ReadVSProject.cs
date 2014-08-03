// $Id$

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BumpVersion.Tasks
{
	/// <summary>
	/// Task that reads files from a Visual Studio project and stores them in a variable
	/// </summary>
	internal class ReadVSProject : BumpTask
	{
		private List<string> Files = new List<string>();

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
	}
}