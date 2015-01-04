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
	public class BumpCopyrightYearTests
	{
		[TestMethod]
		public void BumpTest()
		{
			const string fileName = "yearTest.txt";
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Dictionary<string, string> variables = new Dictionary<string, string>();
			OperationResult result;

			settings.Add( "files", fileName );

			using( ShimsContext.Create() )
			{
				System.Fakes.ShimDateTime.NowGet = () => new DateTime( 2012, 5, 12 );

				BumpCopyrightYear task = new BumpCopyrightYear( settings, variables );

				File.WriteAllText( fileName, "Copyright 2010" );
				result = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
				Assert.IsTrue( result.IsSuccess );
				Assert.AreEqual( "Copyright 2010-2012" + Environment.NewLine, File.ReadAllText( fileName ) );

				File.WriteAllText( fileName, "Copyright (c) 2009-2011" );
				result = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
				Assert.IsTrue( result.IsSuccess );
				Assert.AreEqual( "Copyright (c) 2009-2012" + Environment.NewLine, File.ReadAllText( fileName ) );

				File.WriteAllText( fileName, "Copyright 2009,2011 Author" );
				result = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
				Assert.IsTrue( result.IsSuccess );
				// TODO: This doesn't feel like correct behavior. Need to get some sleep and figure out if this is correct
				Assert.AreEqual( "Copyright 2009-2012 Author" + Environment.NewLine, File.ReadAllText( fileName ) );

				settings["range"] = "false";
				task = new BumpCopyrightYear( settings, variables );

				File.WriteAllText( fileName, "Copyright 2009,2011 Author" );
				result = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
				Assert.IsTrue( result.IsSuccess );
				Assert.AreEqual( "Copyright 2009,2011,2012 Author" + Environment.NewLine, File.ReadAllText( fileName ) );

				settings["lines"] = "2-5";
				task = new BumpCopyrightYear( settings, variables );

				File.WriteAllText( fileName, "Copyright (C) 2009,2011 Author" );
				result = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
				Assert.IsTrue( result.IsSuccess );
				Assert.AreEqual( "Copyright (C) 2009,2011 Author", File.ReadAllText( fileName ) );

				settings["lines"] = "2-";
				task = new BumpCopyrightYear( settings, variables );

				File.WriteAllText( fileName, "Copyright (C) 2009,2011 Author" );
				result = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
				Assert.IsTrue( result.IsSuccess );
				Assert.AreEqual( "Copyright (C) 2009,2011 Author", File.ReadAllText( fileName ) );

				File.WriteAllText( fileName, Environment.NewLine + "Copyright (C) 2009,2011 Author" );
				result = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
				Assert.IsTrue( result.IsSuccess );

				Assert.AreEqual( Environment.NewLine + "Copyright (C) 2009,2011,2012 Author" + Environment.NewLine, File.ReadAllText( fileName ) );

				settings["lines"] = "2";
				task = new BumpCopyrightYear( settings, variables );

				File.WriteAllText( fileName, "Copyright (C) 2009,2011 Author" );
				result = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
				Assert.IsTrue( result.IsSuccess );
				Assert.AreEqual( "Copyright (C) 2009,2011 Author", File.ReadAllText( fileName ) );

				File.WriteAllText( fileName, Environment.NewLine + "Copyright (C) 2009,2011 Author" );
				result = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
				Assert.IsTrue( result.IsSuccess );

				Assert.AreEqual( Environment.NewLine + "Copyright (C) 2009,2011,2012 Author" + Environment.NewLine, File.ReadAllText( fileName ) );

				settings["lines"] = "-1";
				task = new BumpCopyrightYear( settings, variables );

				File.WriteAllText( fileName, Environment.NewLine + "Copyright (C) 2009,2011 Author" );
				result = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
				Assert.IsTrue( result.IsSuccess );
				Assert.AreEqual( Environment.NewLine + "Copyright (C) 2009,2011 Author", File.ReadAllText( fileName ) );

				settings["lines"] = "-2";
				task = new BumpCopyrightYear( settings, variables );

				File.WriteAllText( fileName, Environment.NewLine + "Copyright (C) 2009,2011 Author" );
				result = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
				Assert.IsTrue( result.IsSuccess );
				Assert.AreEqual( Environment.NewLine + "Copyright (C) 2009,2011,2012 Author" + Environment.NewLine, File.ReadAllText( fileName ) );
			}
		}

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

			settings["range"] = "true";
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
	}
}