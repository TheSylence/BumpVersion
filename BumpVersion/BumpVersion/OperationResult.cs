// $Id$

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BumpVersion
{
	/// <summary>
	/// Class that collects error and warning messages
	/// </summary>
	public class OperationResult
	{
		internal List<string> Errors = new List<string>();
		internal List<string> Warnings = new List<string>();

		/// <summary>
		/// Get a value indicating whether this result defines a success (no errors)
		/// </summary>
		public bool IsSuccess
		{
			get
			{
				return Errors.Count == 0;
			}
		}

		/// <summary>
		/// Adds an error to this result
		/// </summary>
		/// <param name="message">The message to add</param>
		public void AddError( string message )
		{
			Errors.Add( message );
		}

		/// <summary>
		/// Adds a warning to this result
		/// </summary>
		/// <param name="message">The message to add</param>
		public void AddWarning( string message )
		{
			Warnings.Add( message );
		}

		/// <summary>
		/// Merges this result with another one
		/// </summary>
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
	}
}