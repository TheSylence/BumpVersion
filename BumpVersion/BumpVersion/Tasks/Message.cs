using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BumpVersion.Tasks
{
	/// <summary>Task to write a message to stdout or stderr</summary>
	internal class Message : BumpTask
	{
		public Message( Dictionary<string, string> settings, Dictionary<string, string> variables )
			: base( settings, variables )
		{
		}

		public override OperationResult Bump( Version oldVersion, Version newVersion )
		{
			OperationResult result = new OperationResult();
			string text = GetValue( TextKey );
			bool stderr = false;

			string strErr = GetValue( StdErrKey );
			if( !string.IsNullOrWhiteSpace( strErr ) )
			{
				if( !bool.TryParse( strErr, out stderr ) )
				{
					stderr = false;
				}
			}

			try
			{
				TextWriter writer = stderr ? Console.Error : Console.Out;
				writer.WriteLine( text );
			}
			catch( Exception ex )
			{
				result.AddError( "Failed to write message: " + ex.ToString() );
			}

			return result;
		}

		public override OperationResult Validate()
		{
			OperationResult result = new OperationResult();

			string text = GetValue( TextKey );
			if( string.IsNullOrEmpty( text ) )
			{
				result.AddError( "No text given" );
			}

			string stderr = GetValue( StdErrKey );
			if( !string.IsNullOrWhiteSpace( stderr ) )
			{
				bool tmp;
				if( !bool.TryParse( stderr, out tmp ) )
				{
					result.AddWarning( string.Format( "{0} is not a valid bool. Falling back to stdout", stderr ) );
				}
			}

			return result;
		}

		private const string StdErrKey = "stderr";
		private const string TextKey = "text";
	}
}