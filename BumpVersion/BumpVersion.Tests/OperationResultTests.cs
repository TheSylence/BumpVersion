// $Id$

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BumpVersion.Tests
{
	[TestClass]
	public class OperationResultTests
	{
		[TestMethod]
		public void IsSuccessTest()
		{
			OperationResult result = new OperationResult();
			Assert.IsTrue( result.IsSuccess );

			result.AddWarning( "Warning" );
			Assert.IsTrue( result.IsSuccess );

			result.AddError( "Error" );
			Assert.IsFalse( result.IsSuccess );
		}

		[TestMethod]
		public void MergeTest()
		{
			OperationResult result = new OperationResult();
			result.AddError( "Error1" );
			result.AddError( "Error2" );
			result.AddWarning( "Warning1" );

			OperationResult other = new OperationResult();
			other.AddError( "Error3" );
			other.AddWarning( "Warning2" );

			result.Merge( other );

			Assert.AreEqual( 3, result.Errors.Count );
			Assert.AreEqual( 2, result.Warnings.Count );

			CollectionAssert.Contains( result.Errors, "Error3" );
			CollectionAssert.Contains( result.Warnings, "Warning2" );
		}

		[TestMethod]
		public void ToStringTest()
		{
			OperationResult result = new OperationResult();
			result.AddWarning( "Warning1" );
			result.AddError( "Error1" );
			result.AddError( "Error2" );

			string actual = result.ToString( true, false );
			Assert.IsFalse( actual.Contains( "Warning" ) );
			Assert.IsTrue( actual.Contains( "Error" ) );

			actual = result.ToString( false, true );
			Assert.IsTrue( actual.Contains( "Warning" ) );
			Assert.IsFalse( actual.Contains( "Error" ) );

			actual = result.ToString( false, false );
			Assert.IsFalse( actual.Contains( "Warning" ) );
			Assert.IsFalse( actual.Contains( "Error" ) );
		}
	}
}