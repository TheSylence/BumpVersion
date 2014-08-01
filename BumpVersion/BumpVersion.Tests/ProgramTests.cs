// $Id$

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BumpVersion.Tests
{
	[TestClass]
	public class ProgramTests
	{
		[TestMethod]
		public void FailedBumpTest()
		{
			File.WriteAllText( "bumpversion_fail.xml", TestData.SimpleFileContent );

			using( StringWriter sw = new StringWriter() )
			{
				Console.SetError( sw );

				using( ShimsContext.Create() )
				{
					System.IO.Fakes.ShimFile.WriteAllTextStringString = ( path, content ) =>
					{
						throw new IOException( "test exception" );
					};

					Program.Main( new string[] { "1.0", "bumpversion_fail.xml" } );
				}

				string actual = sw.ToString();
				Assert.IsTrue( actual.Contains( "Failed to bump version" ) );
				Assert.IsTrue( actual.Contains( "Errors: 1" ) );
				Assert.IsTrue( actual.Contains( "test exception" ) );
				Assert.AreEqual( Environment.ExitCode, -4 );
			}
		}

		[TestMethod]
		public void InvalidTest()
		{
			File.WriteAllText( "bumpversion_invalid.xml", TestData.EmptyFileContent );

			using( StringWriter sw = new StringWriter() )
			{
				Console.SetError( sw );

				Program.Main( new string[] { "1.0", "bumpversion_invalid.xml" } );

				string actual = sw.ToString();
				Assert.IsTrue( actual.Contains( "There are some errors in your project file" ) );
				Assert.IsTrue( actual.Contains( "Errors: 1" ) );
				Assert.IsTrue( actual.Contains( "No tasks in project" ) );
				Assert.AreEqual( Environment.ExitCode, -2 );
			}
		}

		[TestMethod]
		public void InvalidVersionTest()
		{
			using( StringWriter sw = new StringWriter() )
			{
				Console.SetError( sw );

				Program.Main( new string[] { "abc" } );

				Assert.AreEqual( "Version 'abc' is not a valid version" + Environment.NewLine, sw.ToString() );
				Assert.AreEqual( -3, Environment.ExitCode );
			}
		}

		[TestMethod]
		public void NonExistingProjectTest()
		{
			using( StringWriter sw = new StringWriter() )
			{
				Console.SetError( sw );

				File.Delete( "bumpversion.xml" );
				Program.Main( new string[] { "1.0" } );

				Assert.AreEqual( "The project file 'bumpversion.xml' could not be found" + Environment.NewLine, sw.ToString() );
				Assert.AreEqual( -1, Environment.ExitCode );
			}

			using( StringWriter sw = new StringWriter() )
			{
				Console.SetError( sw );

				Program.Main( new string[] { "1.0", "non.existing" } );

				Assert.AreEqual( "The project file 'non.existing' could not be found" + Environment.NewLine, sw.ToString() );
				Assert.AreEqual( -1, Environment.ExitCode );
			}
		}

		[TestMethod]
		public void SuccessfulBumpTest()
		{
			File.WriteAllText( "bumpversion_success.xml", TestData.SimpleFileContent );

			using( StringWriter sw = new StringWriter() )
			{
				Console.SetOut( sw );

				Program.Main( new string[] { "1.0", "bumpversion_success.xml" } );

				string actual = sw.ToString();
				Assert.IsTrue( actual.Contains( "Successfully bumped version to 1.0" ) );
				Assert.AreEqual( 0, Environment.ExitCode );
			}
		}

		[TestMethod]
		public void UsageTest()
		{
			string usage;
			using( StringWriter sw = new StringWriter() )
			{
				Console.SetOut( sw );

				Program.Main( new string[0] );

				usage = sw.ToString();
				Assert.IsTrue( usage.Contains( "BumpVersion" ) );
				Assert.IsTrue( usage.Contains( "Usage:" ) );
				Assert.IsTrue( usage.Contains( "VERSION" ) );
				Assert.IsTrue( usage.Contains( "PROJECT_FILE" ) );
			}

			using( StringWriter sw = new StringWriter() )
			{
				Console.SetOut( sw );

				Program.Main( new string[3] );

				Assert.AreEqual( usage, sw.ToString() );
			}
		}
	}
}