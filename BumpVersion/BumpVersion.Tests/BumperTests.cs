// $Id$

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BumpVersion.Tests
{
	[TestClass]
	public class BumperTests
	{
		#region Test Data

		private static string EmptyFileContent
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine( "<?xml version=\"1.0\"?>" );
				sb.AppendLine( "<bumpversion>" );
				sb.AppendLine( "</bumpversion>" );

				return sb.ToString();
			}
		}

		private static string SimpleFileContent
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine( "<?xml version=\"1.0\"?>" );
				sb.AppendLine( "<bumpversion>" );
				sb.AppendLine( "<task type=\"WriteToFile\">" );
				sb.AppendLine( "<key name=\"files\" value=\"out.txt\" />" );
				sb.AppendLine( "</task>" );
				sb.AppendLine( "</bumpversion>" );

				return sb.ToString();
			}
		}

		private static string UnknownTypeContent
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine( "<?xml version=\"1.0\"?>" );
				sb.AppendLine( "<bumpversion>" );
				sb.AppendLine( "<task type=\"UnknownType\">" );
				sb.AppendLine( "</task>" );
				sb.AppendLine( "</bumpversion>" );

				return sb.ToString();
			}
		}

		#endregion Test Data

		[TestMethod]
		public void LoadExceptionTest()
		{
			try
			{
				File.WriteAllText( "simpleException.xml", SimpleFileContent );

				using( StringWriter sw = new StringWriter() )
				{
					Console.SetError( sw );
					using( ShimsContext.Create() )
					{
						System.Fakes.ShimActivator.CreateInstanceTypeObjectArray = ( Type, args ) =>
						{
							throw new TargetInvocationException( "test exception", new Exception( "inner exception" ) );
						};

						Bumper bumper = new Bumper( "simpleException.xml" );
					}

					string expected = "Failed to create task of type 'WriteToFile':";
					Assert.IsTrue( sw.ToString().Contains( expected ) );
					Assert.IsTrue( sw.ToString().Contains( "test exception" ) );
					Assert.IsTrue( sw.ToString().Contains( "inner exception" ) );
				}
			}
			finally
			{
				File.Delete( "simpleException.xml" );
			}
		}

		[TestMethod, ExpectedException( typeof( FileNotFoundException ) )]
		public void LoadNonExistingTest()
		{
			Bumper bump = new Bumper( "non.existing.file" );
		}

		[TestMethod]
		public void LoadTest()
		{
			try
			{
				File.WriteAllText( "simple.xml", SimpleFileContent );

				Bumper bumper = new Bumper( "simple.xml" );
			}
			finally
			{
				File.Delete( "simple.xml" );
			}
		}

		[TestMethod]
		public void SimpleValidateTest()
		{
			try
			{
				File.WriteAllText( "simpleValidate.xml", SimpleFileContent );

				Bumper bumper = new Bumper( "simpleValidate.xml" );
				OperationResult result = bumper.Vaildate();

				Assert.IsTrue( result.IsSuccess );
			}
			finally
			{
				File.Delete( "simpleValidate.xml" );
			}
		}

		[TestMethod]
		public void UnknownTypeTest()
		{
			try
			{
				File.WriteAllText( "unknownType.xml", UnknownTypeContent );

				using( StringWriter sw = new StringWriter() )
				{
					Console.SetError( sw );

					Bumper bumper = new Bumper( "unknownType.xml" );

					string expected = "Unknown type 'UnknownType' found in project" + Environment.NewLine;
					Assert.AreEqual( expected, sw.ToString() );
				}
			}
			finally
			{
				File.Delete( "unknownType.xml" );
			}
		}

		[TestMethod]
		public void ValidateTest()
		{
			try
			{
				File.WriteAllText( "validate.xml", EmptyFileContent );

				Bumper bumper = new Bumper( "validate.xml" );
				OperationResult result = bumper.Vaildate();

				Assert.IsFalse( result.IsSuccess );
				Assert.IsTrue( result.ToString( true ).Contains( "No tasks in project" ) );
			}
			finally
			{
				File.Delete( "validate.xml" );
			}
		}
	}
}