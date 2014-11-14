// $Id$

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BumpVersion.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BumpVersion.Tests.Tasks
{
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	[TestClass]
	public class ReadVSProjectTests
	{
		[TestMethod]
		public void BumpTest()
		{
			// Bump does nothing so we simply expect a successful operation
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Dictionary<string, string> variables = new Dictionary<string, string>();
			ReadVSProject task = new ReadVSProject( settings, variables );
			OperationResult result = task.Bump( new Version( 0, 1 ), new Version( 0, 2 ) );

			Assert.IsTrue( result.IsSuccess );
		}

		[TestMethod]
		public void GetVariablesTest()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Dictionary<string, string> variables = new Dictionary<string, string>();

			ReadVSProject task = new ReadVSProject( settings, variables );
			variables = task.GetVariables();
			Assert.AreEqual( 0, variables.Count );

			settings.Add( "output", "files" );
			task = new ReadVSProject( settings, variables );
			variables = task.GetVariables();
			Assert.AreEqual( 0, variables.Count );

			settings.Add( "projectFile", "project.csproj" );
			File.WriteAllText( "project.csproj", TestData.SimpleVSProject );

			task = new ReadVSProject( settings, variables );
			variables = task.GetVariables();

			Assert.IsTrue( variables.ContainsKey( "files" ) );
			string[] files = variables["files"].Split( ';' );

			string basePath = Path.GetDirectoryName( "project.csproj" );

			CollectionAssert.Contains( files, Path.Combine( basePath, "Controls\\Window.xaml.cs" ) );
			CollectionAssert.Contains( files, Path.Combine( basePath, "Controls\\Window.xaml" ) );
			CollectionAssert.Contains( files, Path.Combine( basePath, "Properties\\Version.txt" ) );
			CollectionAssert.Contains( files, Path.Combine( basePath, "App.config" ) );

			CollectionAssert.DoesNotContain( files, Path.Combine( basePath, "Properties\\Resource.res" ) );
			CollectionAssert.DoesNotContain( files, Path.Combine( basePath, "Properties\\Resources.resx" ) );

			settings.Add( "elements", "Compile;None" );
			task = new ReadVSProject( settings, variables );
			variables = task.GetVariables();

			Assert.IsTrue( variables.ContainsKey( "files" ) );
			files = variables["files"].Split( ';' );

			CollectionAssert.Contains( files, Path.Combine( basePath, "Controls\\Window.xaml.cs" ) );
			CollectionAssert.Contains( files, Path.Combine( basePath, "Properties\\Version.txt" ) );
			CollectionAssert.Contains( files, Path.Combine( basePath, "App.config" ) );

			CollectionAssert.DoesNotContain( files, Path.Combine( basePath, "Controls\\Window.xaml" ) );
			CollectionAssert.DoesNotContain( files, Path.Combine( basePath, "Properties\\Resource.res" ) );
			CollectionAssert.DoesNotContain( files, Path.Combine( basePath, "Properties\\Resources.resx" ) );
		}

		[TestMethod]
		public void ValidateTest()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Dictionary<string, string> variables = new Dictionary<string, string>();
			ReadVSProject task = new ReadVSProject( settings, variables );
			OperationResult validation = task.Validate();

			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "No project file specified" ) );

			File.Delete( "test.csproj" );
			settings.Add( "projectFile", "test.csproj" );
			task = new ReadVSProject( settings, variables );
			validation = task.Validate();

			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "File 'test.csproj' does not exist" ) );

			File.WriteAllText( "test.csproj", TestData.SimpleVSProject );
			task = new ReadVSProject( settings, variables );
			validation = task.Validate();

			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "No output variable defined" ) );

			// Reading files is tested in another test so we just check if the message is present
			Assert.IsTrue( validation.Errors.Contains( "No files found in Visual Studio project" ) );
		}
	}
}