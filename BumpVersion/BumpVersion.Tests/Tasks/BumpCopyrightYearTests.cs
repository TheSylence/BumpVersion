using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BumpVersion.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BumpVersion.Tests.Tasks
{
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	[TestClass]
	public class BumpCopyrightYearTests
	{
		[TestMethod]
		public void ValidateTest()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Dictionary<string, string> variables = new Dictionary<string, string>();
			BumpCopyrightYear task = new BumpCopyrightYear( settings, variables );
			OperationResult validation = task.Validate();

			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "No files given" ) );

			settings.Add( "files", "bumpyear.txt" );
			task = new BumpCopyrightYear( settings, variables );
			validation = task.Validate();

			Assert.IsTrue( validation.IsSuccess );

			settings.Add( "range", "true" );
			task = new BumpCopyrightYear( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings[ "range"] = "true";
			task = new BumpCopyrightYear( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings["range"] = "false";
			task = new BumpCopyrightYear( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings["range"] = "123";
			task = new BumpCopyrightYear( settings, variables );
			validation = task.Validate();
			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "Failed to parse 123 as boolean" ) );

			settings.Remove( "range" );
			settings["lines"] = "1";
			task = new BumpCopyrightYear( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings["lines"] = "1-10";
			task = new BumpCopyrightYear( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings["lines"] = "1;5;12";
			task = new BumpCopyrightYear( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings["lines"] = "1-";
			task = new BumpCopyrightYear( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings["lines"] = "-10";
			task = new BumpCopyrightYear( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings["lines"] = "13;";
			task = new BumpCopyrightYear( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings["lines"] = ";13";
			task = new BumpCopyrightYear( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings["lines"] = "a";
			task = new BumpCopyrightYear( settings, variables );
			validation = task.Validate();
			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "Invalid line: a" ) );

			settings["lines"] = "a;b";
			task = new BumpCopyrightYear( settings, variables );
			validation = task.Validate();
			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "Invalid line: a" ) );
			Assert.IsTrue( validation.Errors.Contains( "Invalid line: b" ) );
		}

		[TestMethod]
		public void BumpTest()
		{
			Assert.Inconclusive();
		}
	}
}
