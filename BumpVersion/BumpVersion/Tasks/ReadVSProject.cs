// $Id$

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

			string projectFile = GetValue( "projectFile" );
			if( projectFile != null )
			{
				// TODO: Parse project file
			}

			return variables;
		}

		public override OperationResult Validate()
		{
			OperationResult result = new OperationResult();
			if( GetValue( "projectFile" ) == null )
			{
				result.AddError( "No project file specified" );
			}

			if( Files.Count == 0 )
			{
				result.AddError( "No files found in Visual Studio project" );
			}

			return result;
		}
	}
}