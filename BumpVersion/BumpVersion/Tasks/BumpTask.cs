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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BumpVersion.Tasks
{
	/// <summary>
	/// Base class for a task that can be included in version bumping
	/// </summary>
	internal abstract class BumpTask
	{
		private Dictionary<string, string> Settings;
		private Dictionary<string, string> Variables;

		/// <summary>
		/// </summary>
		/// <param name="settings">A dictionary of settings the user has set for this task</param>
		/// <param name="variables">A dictionary of all available variables</param>
		public BumpTask( Dictionary<string, string> settings, Dictionary<string, string> variables )
		{
			Settings = settings;
			Variables = variables;
		}

		/// <summary>
		/// Called when a version should be bumped
		/// </summary>
		/// <param name="oldVersion">The current version</param>
		/// <param name="newVersion">The version to bump to</param>
		/// <returns>An instance of the <see cref="OperationResult"/> class containing all errors and warnings
		/// that occured during bumping</returns>
		public abstract OperationResult Bump( Version oldVersion, Version newVersion );

		/// <summary>
		/// Returns a list of variables this task offers for other tasks
		/// </summary>
		/// <returns>A dicationary of all variables and their values</returns>
		public virtual Dictionary<string, string> GetVariables()
		{
			return new Dictionary<string, string>();
		}

		/// <summary>
		/// Validates this task
		/// </summary>
		/// <returns>An instance of the <see cref="OperationResult"/> class containing the validation result</returns>
		public abstract OperationResult Validate();

		/// <summary>
		/// Searches for a file in all folders defined in the PATH environment variable.
		/// </summary>
		/// <param name="exe">The file to search for</param>
		/// <returns>The full path of the found file or an empty string if not found</returns>
		protected internal string GetFullPathFromEnvironment( string exe )
		{
			string enviromentPath = System.Environment.GetEnvironmentVariable( "PATH" );

			string[] paths = enviromentPath.Split( ';' );
			return paths.Select( x => Path.Combine( x, exe ) ).FirstOrDefault( x => File.Exists( x ) );
		}

		/// <summary>
		/// Reads the value of a setting key taking variables into account
		/// </summary>
		/// <param name="settingKey">The key to read</param>
		/// <returns>The effective value of the key or <c>null</c> if the key could not be found</returns>
		protected internal string GetValue( string settingKey )
		{
			string value;
			if( !Settings.TryGetValue( settingKey, out value ) )
			{
				return null;
			}

			if( value.StartsWith( "@" ) )
			{
				value = value.Substring( 1 );
				if( !Variables.TryGetValue( value, out value ) )
				{
					return null;
				}
			}

			return value;
		}
	}
}