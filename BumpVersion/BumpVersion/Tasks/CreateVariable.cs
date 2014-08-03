// $Id$

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BumpVersion.Tasks
{
	/// <summary>
	/// Tasks that creates a variable. This is useful if you want to use the same value in multiple places in your
	/// project and want to keep the value in one central place.
	/// </summary>
	internal class CreateVariable : BumpTask
	{
		public CreateVariable( Dictionary<string, string> settings, Dictionary<string, string> variables )
			: base( settings, variables )
		{
		}

		public override OperationResult Bump( Version oldVersion, Version newVersion )
		{
			return new OperationResult();
		}

		public override Dictionary<string, string> GetVariables()
		{
			Dictionary<string, string> variables = new Dictionary<string, string>();
			variables.Add( GetValue( "name" ), GetValue( "value" ) );
			return variables;
		}

		public override OperationResult Validate()
		{
			OperationResult result = new OperationResult();
			if( GetValue( "name" ) == null )
			{
				result.AddError( "No name given" );
			}
			if( GetValue( "value" ) == null )
			{
				result.AddError( "No value given" );
			}

			return result;
		}
	}
}