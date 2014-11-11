using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using BumpVersion.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BumpVersion.Tests.Tasks
{
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	[TestClass]
	public class ImageWatermarkTests
	{
		[TestMethod]
		public void BumpTest()
		{
			Dictionary<string, string> variables = new Dictionary<string, string>();
			Dictionary<string, string> settings = new Dictionary<string, string>();
			settings.Add( "source", "SourceImage.png" );
			settings.Add( "dest", "default.png" );
			settings.Add( "rect", "0;0" );

			ImageWatermark task = new ImageWatermark( settings, variables );
			OperationResult validation = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
			Assert.IsTrue( validation.IsSuccess );
			CompareImageWithReference( "default.png" );

			settings.Add( "color", "red" );
			settings["dest"] = "defaultRed.png";
			task = new ImageWatermark( settings, variables );
			validation = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
			Assert.IsTrue( validation.IsSuccess );
			CompareImageWithReference( "defaultRed.png" );

			settings["rect"] = "100;10;10;20";
			settings["dest"] = "rectangleCut.png";
			settings.Remove( "color" );
			task = new ImageWatermark( settings, variables );
			validation = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
			Assert.IsTrue( validation.IsSuccess );
			CompareImageWithReference( "rectangleCut.png" );

			settings["rect"] = "0;0";
			settings["dest"] = "font.png";
			settings.Add( "font", "Consolas:13" );
			task = new ImageWatermark( settings, variables );
			validation = task.Bump( new Version( 1, 0 ), new Version( 1, 1 ) );
			Assert.IsTrue( validation.IsSuccess );
			CompareImageWithReference( "font.png" );
		}

		[TestMethod]
		public void RectangleValidationTest()
		{
			Dictionary<string, string> variables = new Dictionary<string, string>();
			Dictionary<string, string> settings = new Dictionary<string, string>();

			File.OpenWrite( "validationsourcefile" ).Close();

			settings.Add( "dest", "dest" );
			settings.Add( "source", "validationsourcefile" );
			settings.Add( "rect", "10;10;100;20" );

			ImageWatermark task = new ImageWatermark( settings, variables );
			OperationResult validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings["rect"] = "10;10";
			task = new ImageWatermark( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings["rect"] = "10;10;10";
			task = new ImageWatermark( settings, variables );
			validation = task.Validate();
			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "Invalid rectangle given: 10;10;10" ) );

			settings["rect"] = "10";
			task = new ImageWatermark( settings, variables );
			validation = task.Validate();
			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "Invalid rectangle given: 10" ) );

			settings["rect"] = "10;10;10;10;10";
			task = new ImageWatermark( settings, variables );
			validation = task.Validate();
			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "Invalid rectangle given: 10;10;10;10;10" ) );

			settings["rect"] = "-10;10";
			task = new ImageWatermark( settings, variables );
			validation = task.Validate();
			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "Negative value given: -10;10" ) );

			settings["rect"] = "10_;10";
			task = new ImageWatermark( settings, variables );
			validation = task.Validate();
			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "Invalid rectangle given: 10_;10" ) );
		}

		[TestMethod]
		public void ValidateTest()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			Dictionary<string, string> variables = new Dictionary<string, string>();
			ImageWatermark task = new ImageWatermark( settings, variables );
			OperationResult validation = task.Validate();

			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "No destination file given" ) );
			Assert.IsTrue( validation.Errors.Contains( "No rectangle given" ) );

			settings.Add( "source", "watermarksource" );
			if( File.Exists( "watermarksource" ) )
			{
				File.Delete( "watermarksource" );
			}

			task = new ImageWatermark( settings, variables );
			validation = task.Validate();

			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "Source file 'watermarksource' not found" ) );
			File.OpenWrite( "watermarksource" ).Close();

			task = new ImageWatermark( settings, variables );
			validation = task.Validate();

			Assert.IsFalse( validation.IsSuccess );
			Assert.IsFalse( validation.Errors.Contains( "Source file 'watermarksource' not found" ) );

			settings.Add( "dest", "watermarkdest" );
			settings.Add( "rect", "10;10;100;20" );

			task = new ImageWatermark( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings.Add( "font", "Tahoma" );
			task = new ImageWatermark( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings["font"] = "Arial:12";
			task = new ImageWatermark( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings["font"] = "Arial:ab";
			task = new ImageWatermark( settings, variables );
			validation = task.Validate();
			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "Invalid font size specified: ab" ) );

			settings["font"] = "nonexisting";
			task = new ImageWatermark( settings, variables );
			validation = task.Validate();
			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "Font 'nonexisting' is not installed on the system" ) );

			settings["font"] = "Arial";
			settings["color"] = "blue";
			task = new ImageWatermark( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings["color"] = "#fff";
			task = new ImageWatermark( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings["color"] = "#f1f1f1";
			task = new ImageWatermark( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings["color"] = "#123456";
			task = new ImageWatermark( settings, variables );
			validation = task.Validate();
			Assert.IsTrue( validation.IsSuccess );

			settings["color"] = "test";
			task = new ImageWatermark( settings, variables );
			validation = task.Validate();
			Assert.IsFalse( validation.IsSuccess );
			Assert.IsTrue( validation.Errors.Contains( "Color 'test' is not a valid color" ) );
		}

		private static void CompareImageWithReference( string fileName )
		{
			string refFileName = Path.Combine( "ReferenceData", fileName );

			Assert.IsTrue( File.Exists( fileName ), "Actual file does not exist" );
			Assert.IsTrue( File.Exists( refFileName ), "Reference file does not exist" );

			int errorPixels = 0;
			using( Bitmap refImg = (Bitmap)Bitmap.FromFile( refFileName ) )
			{
				using( Bitmap actImg = (Bitmap)Bitmap.FromFile( fileName ) )
				{
					Assert.AreEqual( refImg.Width, actImg.Width, "Images are different sized" );
					Assert.AreEqual( refImg.Height, actImg.Height, "Images are different sized" );

					for( int x = 0; x < refImg.Width; ++x )
					{
						for( int y = 0; y < refImg.Height; ++y )
						{
							if( refImg.GetPixel( x, y ) != actImg.GetPixel( x, y ) )
							{
								++errorPixels;
							}
						}
					}
				}
			}

			if( errorPixels > 0 )
			{
				Assert.Fail( string.Format( "{0} pixels did not match", errorPixels ) );
			}
		}
	}
}