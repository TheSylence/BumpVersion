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
	public class BumpTaskTests
	{
		[TestMethod]
		public void GetValueTest()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Dictionary<string, string> variables = new Dictionary<string, string>();

			settings.Add( "var", "@variable" );
			settings.Add( "key", "value" );
			settings.Add( "test", "@nonexisting" );
			variables.Add( "variable", "value_var" );

			MockTask task = new MockTask( settings, variables );

			Assert.AreEqual( "value", task.GetValue( "key" ) );
			Assert.AreEqual( "value_var", task.GetValue( "var" ) );

			Assert.IsNull( task.GetValue( "non existing key" ) );
			Assert.IsNull( task.GetValue( "test" ) );
		}
	}

	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	internal class MockTask : BumpTask
	{
		public MockTask( Dictionary<string, string> settings, Dictionary<string, string> variables )
			: base( settings, variables )
		{
		}

		public override OperationResult Bump( Version oldVersion, Version newVersion )
		{
			throw new NotImplementedException();
		}

		public override OperationResult Validate()
		{
			throw new NotImplementedException();
		}
	}
}