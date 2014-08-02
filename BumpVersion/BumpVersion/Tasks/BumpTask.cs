﻿// $Id$

using System;
using System.Collections.Generic;
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
		/// <param name="newVersion">The version to bump to</param>
		/// <returns>An instance of the <see cref="OperationResult"/> class containing all errors and warnings
		/// that occured during bumping</returns>
		public abstract OperationResult Bump( Version newVersion );

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
		/// Reads the value of a setting key taking variables into account
		/// </summary>
		/// <param name="settingKey">The key to read</param>
		/// <returns>The effective value of the key or <c>null</c> if the key could not be found</returns>
		protected string GetValue( string settingKey )
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