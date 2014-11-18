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
using System.Linq;

namespace BumpVersion.Tasks
{
	/// <summary>
	/// Tasks that creates a variable. This is useful if you want to use the same value in multiple
	/// places in your project and want to keep the value in one central place.
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