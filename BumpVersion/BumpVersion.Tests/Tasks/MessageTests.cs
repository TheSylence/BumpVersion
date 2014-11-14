using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BumpVersion.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BumpVersion.Tests.Tasks
{
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	[TestClass]
	public class MessageTests
	{
		[TestMethod]
		public void BumpTest()
		{
			OperationResult result;
			Dictionary<string, string> variables = new Dictionary<string, string>();
			Dictionary<string, string> settings = new Dictionary<string, string>();
			settings.Add( "text", "This is a test" );
			Message task = new Message( settings, variables );

			using( StringWriter writer = new StringWriter() )
			{
				Console.SetOut( writer );

				result = task.Bump( new Version(), new Version() );
				Assert.IsTrue( result.IsSuccess );

				Assert.AreEqual( "This is a test" + Environment.NewLine, writer.ToString() );
			}

			settings.Add( "stderr", "4232" );
			using( StringWriter writer = new StringWriter() )
			{
				Console.SetOut( writer );

				result = task.Bump( new Version(), new Version() );
				Assert.IsTrue( result.IsSuccess );

				Assert.AreEqual( "This is a test" + Environment.NewLine, writer.ToString() );
			}

			settings["stderr"] = "true";
			using( StringWriter writer = new StringWriter() )
			{
				Console.SetError( writer );

				result = task.Bump( new Version(), new Version() );
				Assert.IsTrue( result.IsSuccess );

				Assert.AreEqual( "This is a test" + Environment.NewLine, writer.ToString() );
			}

			settings.Remove( "stderr" );
			using( StringWriter writer = new StringWriter() )
			{
				Console.SetError( writer );

				result = task.Bump( new Version(), new Version() );
				Assert.IsFalse( result.IsSuccess );
				Assert.IsTrue( result.Errors.First().StartsWith( "Failed to write message: " ) );
			}

			using( ShimsContext.Create() )
			{
				System.IO.Fakes.ShimStringWriter.AllInstances.WriteCharArrayInt32Int32 = ( writer, arr, i, j ) => { throw new IOException( "test" ); };

				using( StringWriter writer = new StringWriter() )
				{
					Console.SetOut( writer );
					result = task.Bump( new Version(), new Version() );
					Assert.IsFalse( result.IsSuccess );
					Assert.IsTrue( result.Errors.First().StartsWith( "Failed to write message: " ) );
				}
			}
		}

		[TestMethod]
		public void ValidateTest()
		{
			Dictionary<string, string> variables = new Dictionary<string, string>();
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Message task = new Message( settings, variables );
			OperationResult validation = task.Validate();

			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "No text given" ) );

			settings.Add( "text", "test" );
			task = new Message( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings.Add( "stderr", "4" );
			task = new Message( settings, variables );
			validation = task.Validate();

			Assert.IsTrue( validation.IsSuccess );
			Assert.IsTrue( validation.Warnings.Contains( "4 is not a valid bool. Falling back to stdout" ) );
		}
	}
}