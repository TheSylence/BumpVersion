// $Id$

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BumpVersion.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BumpVersion.Tests
{
	[TestClass]
	public class WixProductIDTests
	{
		[TestMethod]
		public void BumpExceptionTest()
		{
			File.WriteAllText( "exception.wxs", TestData.InvalidWixFile );

			Dictionary<string, string> settings = new Dictionary<string, string>
			{
				{ "wixFile", "exception.wxs" }
			};

			WixProductID task = new WixProductID( settings, new Dictionary<string, string>() );
			OperationResult result = task.Bump( new Version( 1, 0 ) );

			Assert.IsFalse( result.IsSuccess );
			Assert.IsTrue( result.ToString().Contains( "NullReferenceException" ) );
		}

		[TestMethod]
		public void BumpTest()
		{
			File.WriteAllText( "valid.wxs", TestData.WixFile );

			Dictionary<string, string> settings = new Dictionary<string, string>
			{
				{ "wixFile", "valid.wxs" }
			};

			WixProductID task = new WixProductID( settings, new Dictionary<string, string>() );
			OperationResult result = task.Bump( new Version( 1, 0 ) );

			Assert.IsTrue( result.IsSuccess );

			string updated = File.ReadAllText( "valid.wxs" );
			Assert.IsTrue( updated.Contains( "UpgradeCode=\"073D85EE-9EAE-4DA8-A58C-BA2308C58A85\"" ) );
			Assert.IsFalse( updated.Contains( "Product Id=\"2CCC37ED-6F75-48FC-ADEB-D3008B326B78\"" ) );
		}

		[TestMethod]
		public void ValidateTest()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			WixProductID task = new WixProductID( settings, new Dictionary<string, string>() );
			OperationResult validation = task.Validate();

			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.ToString( true ).Contains( "No files given" ) );

			settings["wixFile"] = string.Empty;
			task = new WixProductID( settings, new Dictionary<string, string>() );
			validation = task.Validate();

			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.ToString( true ).Contains( "No files given" ) );

			settings["wixFile"] = "test.wxs";
			task = new WixProductID( settings, new Dictionary<string, string>() );
			File.Delete( "test.wxs" );
			validation = task.Validate();

			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.ToString( true ).Contains( "WiX file 'test.wxs' does not exist" ) );

			File.WriteAllText( "test.wxs", "test" );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );
		}
	}
}