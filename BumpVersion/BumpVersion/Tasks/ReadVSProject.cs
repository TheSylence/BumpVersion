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
		public ReadVSProject( Dictionary<string, string> settings, Dictionary<string, string> variables )
			: base( settings, variables )
		{
		}

		public override OperationResult Bump( Version newVersion )
		{
			throw new NotImplementedException();
		}

		public override OperationResult Validate()
		{
			throw new NotImplementedException();
		}
	}
}