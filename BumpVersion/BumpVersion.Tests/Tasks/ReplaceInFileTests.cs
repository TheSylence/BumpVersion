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
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
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
		public void BumpDefaultTest()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Dictionary<string, string> variables = new Dictionary<string, string>();
			OperationResult result;
			settings.Add( "files", "replace.default.txt;replace.default2.txt" );

			File.WriteAllText( "replace.default.txt", "This is a test version of 0.1 :)" );
			File.WriteAllText( "replace.default2.txt", "Version 0.1 is replaced. However, Version 1.2 , 0.3 or even 0.1.1 not" );

			ReplaceInFile task = new ReplaceInFile( settings, variables );

			result = task.Bump( new Version( 0, 1 ), new Version( 0, 2 ) );

			Assert.IsTrue( result.IsSuccess );

			string content = File.ReadAllText( "replace.default.txt" );
			Assert.AreEqual( "This is a test version of 0.2 :)", content );

			content = File.ReadAllText( "replace.default2.txt" );
			Assert.AreEqual( "Version 0.2 is replaced. However, Version 1.2 , 0.3 or even 0.1.1 not", content );
		}

		[TestMethod]
		public void BumpCustomPatternTest()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Dictionary<string, string> variables = new Dictionary<string, string>();
			OperationResult result;
			settings.Add( "files", "replace.custom.txt" );
			settings.Add( "search", "Version {0}" );
			settings.Add( "replace", "Version {0}" );

			File.WriteAllText( "replace.custom.txt", "Version 0.1 will be replaced, 0.1 not" );

			ReplaceInFile task = new ReplaceInFile( settings, variables );
			result = task.Bump( new Version( 0, 1 ), new Version( 0, 2 ) );

			string content = File.ReadAllText( "replace.custom.txt" );
			Assert.AreEqual( "Version 0.2 will be replaced, 0.1 not", content );
		}

		[TestMethod]
		public void BumpExceptionTest()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Dictionary<string, string> variables = new Dictionary<string, string>();

			settings.Add( "files", "non.existing" );

			ReplaceInFile task = new ReplaceInFile( settings, variables );

			OperationResult result = task.Bump( new Version( 0, 1 ), new Version( 0, 2 ) );

			Assert.IsFalse( result.IsSuccess );
			Assert.IsTrue( result.Errors[0].StartsWith( "non.existing => System.IO.FileNotFoundException" ) );
		}
	}
}