// $Id$

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BumpVersion.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BumpVersion.Tests.Tasks
{
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	[TestClass]
	public class WriteToFileTests
	{
		[TestMethod]
		public void BumpExceptionTest()
		{
			using( ShimsContext.Create() )
			{
				System.IO.Fakes.ShimFile.WriteAllTextStringString = ( path, content ) =>
				{
					throw new IOException( "test exception" );
				};

				Dictionary<string, string> settings = new Dictionary<string, string>
				{
					{"files", "exception.txt"}
				};

				WriteToFile task = new WriteToFile( settings, new Dictionary<string, string>() );
				OperationResult result = task.Bump( new Version(), new Version( 1, 0 ) );

				Assert.IsFalse( result.IsSuccess );
				Assert.IsTrue( result.ToString( true ).Contains( "test exception" ) );
			}
		}

		[TestMethod]
		public void BumpTest()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>
			{
				{"files", "out1.txt;out2.txt"}
			};
			WriteToFile task = new WriteToFile( settings, new Dictionary<string, string>() );
			OperationResult result = task.Bump( new Version(), new Version( 1, 0 ) );

			Assert.IsTrue( result.IsSuccess );

			Assert.IsTrue( File.Exists( "out1.txt" ) );
			Assert.IsTrue( File.Exists( "out2.txt" ) );

			Assert.AreEqual( "1.0", File.ReadAllText( "out1.txt" ) );
			Assert.AreEqual( "1.0", File.ReadAllText( "out2.txt" ) );
		}

		[TestMethod]
		public void ValidateTest()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			WriteToFile task = new WriteToFile( settings, new Dictionary<string, string>() );
			OperationResult validation = task.Validate();

			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "No files given" ) );

			settings["files"] = string.Empty;
			task = new WriteToFile( settings, new Dictionary<string, string>() );
			validation = task.Validate();

			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "No files given" ) );

			settings["files"] = "out.txt";
			task = new WriteToFile( settings, new Dictionary<string, string>() );
			validation = task.Validate();

			Assert.IsTrue( validation.IsSuccess );
		}
	}
}