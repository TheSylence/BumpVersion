// $Id$

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BumpVersion.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BumpVersion.Tests
{
	[TestClass]
	public class GitCommitTests
	{
		[TestMethod]
		public void BumpErrorTest()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Dictionary<string, string> variables = new Dictionary<string, string>();
			GitCommit task = new GitCommit( settings, variables );

			using( ShimsContext.Create() )
			{
				System.Fakes.ShimEnvironment.GetEnvironmentVariableString = ( v ) =>
				{
					return string.Empty;
				};

				OperationResult result = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
				Assert.IsFalse( result.IsSuccess );
				Assert.IsTrue( result.Errors.Contains( "git.exe not found in PATH" ) );

				System.Diagnostics.Fakes.ShimProcess.StartProcessStartInfo = ( inf ) =>
				{
					throw new Exception( "test exception" );
				};

				result = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
				Assert.IsFalse( result.IsSuccess );
				Assert.IsTrue( result.Errors.Any( e => e.StartsWith( "Failed to execute git commit: System.Exception: test exception" ) ) );
			}
		}

		[TestMethod]
		public void BumpTest()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Dictionary<string, string> variables = new Dictionary<string, string>();
			GitCommit task = new GitCommit( settings, variables );

			bool checkResult = task.GetFullPathFromEnvironment( "git.exe" ) != null;

			using( ShimsContext.Create() )
			{
				string args = null;
				System.Diagnostics.Fakes.ShimProcess.StartProcessStartInfo = ( inf ) =>
				{
					args = inf.Arguments;
					return new System.Diagnostics.Fakes.StubProcess();
				};

				bool waited = false;
				System.Diagnostics.Fakes.ShimProcess.AllInstances.WaitForExit = ( p ) =>
				{
					waited = true;
				};

				OperationResult result = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
				if( checkResult )
				{
					Assert.IsTrue( result.IsSuccess );
				}
				Assert.IsTrue( waited );
				Assert.AreEqual( "commit -m \"Bump version to 1.1\"", args );

				waited = false;
				settings.Add( "message", "Test {0}" );
				result = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
				if( checkResult )
				{
					Assert.IsTrue( result.IsSuccess );
				}
				Assert.IsTrue( waited );
				Assert.AreEqual( "commit -m \"Test 1.1\"", args );

				waited = false;
				settings["message"] = "Test";
				result = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
				if( checkResult )
				{
					Assert.IsTrue( result.IsSuccess );
				}
				Assert.IsTrue( waited );
				Assert.AreEqual( "commit -m \"Test\"", args );
			}

			if( !checkResult )
			{
				Assert.Inconclusive( "git.exe was not found in PATH. Everything else worked, though" );
			}
		}

		[TestMethod]
		public void ValidateTest()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Dictionary<string, string> variables = new Dictionary<string, string>();
			settings.Add( "message", "Test" );

			GitCommit task = new GitCommit( settings, variables );
			OperationResult validation = task.Validate();

			Assert.IsTrue( validation.Warnings.Contains( "Message contains no placeholder for version" ) );
		}
	}
}