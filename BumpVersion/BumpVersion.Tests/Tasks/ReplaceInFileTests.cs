// $Id$

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BumpVersion.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BumpVersion.Tests.Tasks
{
	[TestClass]
	public class ReplaceInFileTests
	{
		[TestMethod]
		public void ValidateTest()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Dictionary<string, string> variables = new Dictionary<string, string>();
			ReplaceInFile task = new ReplaceInFile( settings, variables );
			OperationResult result = task.Validate();

			Assert.IsFalse( result.IsSuccess );
			Assert.IsTrue( result.Errors.Contains( "No files given" ) );

			settings.Add( "files", "non.existing" );

			task = new ReplaceInFile( settings, variables );
			result = task.Validate();

			Assert.IsFalse( result.IsSuccess );
			Assert.IsTrue( result.Errors.Contains( "File 'non.existing' does not exist" ) );

			File.WriteAllText( "test.txt", "" );
			settings["files"] = "test.txt";

			task = new ReplaceInFile( settings, variables );
			result = task.Validate();
			Assert.IsTrue( result.IsSuccess );
		}

		[TestMethod]
		public void BumpTest()
		{
			Assert.Inconclusive( "TODO: Implement" );
		}

		[TestMethod]
		public void BumpExceptionTest()
		{
			Assert.Inconclusive( "TODO: Implement" );
		}
	}
}