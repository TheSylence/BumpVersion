// $Id$

using System;
using System.Collections.Generic;
using System.Linq;
using BumpVersion.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BumpVersion.Tests.Tasks
{
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	[TestClass]
	public class CreateVariableTests
	{
		[TestMethod]
		public void BumpTest()
		{
			// Bump does nothing so we simply expect a successful operation
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Dictionary<string, string> variables = new Dictionary<string, string>();
			CreateVariable task = new CreateVariable( settings, variables );
			OperationResult result = task.Bump( new Version( 0, 1 ), new Version( 0, 2 ) );

			Assert.IsTrue( result.IsSuccess );
		}

		[TestMethod]
		public void GetVariablesTest()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Dictionary<string, string> variables = new Dictionary<string, string>();
			settings.Add( "name", "name" );
			settings.Add( "value", "123" );

			CreateVariable task = new CreateVariable( settings, variables );
			variables = task.GetVariables();

			Assert.IsTrue( variables.ContainsKey( "name" ) );
			Assert.AreEqual( "123", variables["name"] );
		}

		[TestMethod]
		public void ValidateTest()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Dictionary<string, string> variables = new Dictionary<string, string>();
			CreateVariable task = new CreateVariable( settings, variables );
			OperationResult validation = task.Validate();

			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "No name given" ) );
			Assert.IsTrue( validation.Errors.Contains( "No value given" ) );

			settings.Add( "name", "name" );
			task = new CreateVariable( settings, variables );
			validation = task.Validate();
			Assert.IsFalse( validation.IsSuccess );
			Assert.IsFalse( validation.Errors.Contains( "No name given" ) );
			Assert.IsTrue( validation.Errors.Contains( "No value given" ) );

			settings.Add( "value", "123" );
			task = new CreateVariable( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );
		}
	}
}