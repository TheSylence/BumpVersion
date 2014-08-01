// $Id$

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
		protected Dictionary<string, string> Settings;

		/// <summary>
		/// </summary>
		/// <param name="settings">A dictionary of settings the user has set for this task</param>
		public BumpTask( Dictionary<string, string> settings )
		{
			Settings = settings;
		}

		/// <summary>
		/// Called when a version should be bumped
		/// </summary>
		/// <param name="newVersion">The version to bump to</param>
		/// <returns>An instance of the <see cref="OperationResult"/> class containing all errors and warnings
		/// that occured during bumping</returns>
		public abstract OperationResult Bump( Version newVersion );

		/// <summary>
		/// Validates this task
		/// </summary>
		/// <returns>An instance of the <see cref="OperationResult"/> class containing the validation result</returns>
		public abstract OperationResult Validate();
	}
}