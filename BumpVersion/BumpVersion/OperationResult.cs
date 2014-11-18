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
using System.Text;

namespace BumpVersion
{
	/// <summary>Class that collects error and warning messages</summary>
	public class OperationResult
	{
		/// <summary>Adds an error to this result</summary>
		/// <param name="message">The message to add</param>
		public void AddError( string message )
		{
			Errors.Add( message );
		}

		/// <summary>Adds a warning to this result</summary>
		/// <param name="message">The message to add</param>
		public void AddWarning( string message )
		{
			Warnings.Add( message );
		}

		/// <summary>Merges this result with another one</summary>
		/// <param name="other">The result to merge</param>
		public void Merge( OperationResult other )
		{
			foreach( string warning in other.Warnings )
			{
				AddWarning( warning );
			}

			foreach( string error in other.Errors )
			{
				AddError( error );
			}
		}

		/// <summary>
		/// Generates a list of all warnings and/or errors that are contained in this result.
		/// </summary>
		/// <param name="errors">Flag indicating whether to include errors or not</param>
		/// <param name="warnings">Flag indicating whether to include warnings or not</param>
		/// <returns>A string containing all the desired messages</returns>
		public string ToString( bool errors = true, bool warnings = true )
		{
			StringBuilder sb = new StringBuilder();

			if( errors )
			{
				sb.AppendFormat( "Errors: {0}", Errors.Count );
				sb.AppendLine();
				sb.Append( string.Join( Environment.NewLine, Errors ) );
				sb.AppendLine();
			}

			if( warnings )
			{
				sb.AppendFormat( "Warnings: {0}", Warnings.Count );
				sb.AppendLine();
				sb.Append( string.Join( Environment.NewLine, Warnings ) );
				sb.AppendLine();
			}

			return sb.ToString();
		}

		/// <summary>Get a value indicating whether this result defines a success (no errors)</summary>
		public bool IsSuccess
		{
			get
			{
				return Errors.Count == 0;
			}
		}

		internal List<string> Errors = new List<string>();
		internal List<string> Warnings = new List<string>();
	}
}