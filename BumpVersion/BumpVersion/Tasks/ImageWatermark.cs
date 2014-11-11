using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace BumpVersion.Tasks
{
	/// <summary>
	/// Task that prints the version number on an image.
	/// </summary>
	internal class ImageWatermark : BumpTask
	{
		public ImageWatermark( Dictionary<string, string> settings, Dictionary<string, string> variables )
			: base( settings, variables )
		{
		}

		public override OperationResult Bump( Version oldVersion, Version newVersion )
		{
			OperationResult result = new OperationResult();

			string sourceFile = GetValue( SourceFileKey );
			string destFile = GetValue( DestFileKey );
			Color color = Color.White;
			string strColor = GetValue( ColorKey );
			if( strColor != null )
			{
				color = ColorTranslator.FromHtml( strColor );
			}
			SolidBrush brush = new SolidBrush( color );

			Font font = new Font( "Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel );
			string strFont = GetValue( FontKey );
			if( strFont != null )
			{
				font.Dispose();

				string name = "";
				int size = 12;

				int idxSep = strFont.IndexOf( ':' );
				if( idxSep > 0 )
				{
					size = int.Parse( strFont.Substring( idxSep + 1 ) );
					name = strFont.Substring( 0, idxSep );
				}
				else
				{
					name = strFont;
				}

				font = new Font( name, size, FontStyle.Regular, GraphicsUnit.Pixel );
			}

			string strRect = GetValue( RectangleKey );
			string[] rectParts = strRect.Split( ';' );
			bool changeDimensions = false;
			if( rectParts.Length == 2 )
			{
				changeDimensions = true;
				rectParts = new string[] { rectParts[0], rectParts[1], "0", "0" };
			}

			int[] rectSizes = rectParts.Select( s => Convert.ToInt32( s ) ).ToArray();

			RectangleF rect = new RectangleF( rectSizes[0], rectSizes[1], rectSizes[2], rectSizes[3] );

			try
			{
				using( font )
				{
					using( Bitmap sourceImage = (Bitmap)Bitmap.FromFile( sourceFile ) )
					{
						using( Bitmap destImage = new Bitmap( sourceImage ) )
						{
							if( changeDimensions )
							{
								rect.Width = destImage.Width - rect.X;
								rect.Height = destImage.Height - rect.Y;
							}

							using( Graphics g = Graphics.FromImage( destImage ) )
							{
								g.DrawString( newVersion.ToString(), font, brush, rect );
							}

							destImage.Save( destFile );
						}
					}
				}
			}
			catch( IOException ex )
			{
				result.AddError( ex.ToString() );
			}

			return result;
		}

		public override OperationResult Validate()
		{
			OperationResult result = new OperationResult();

			string source = GetValue( SourceFileKey );
			if( !File.Exists( source ) )
			{
				result.AddError( string.Format( "Source file '{0}' not found", source ) );
			}

			string dest = GetValue( DestFileKey );
			if( string.IsNullOrWhiteSpace( dest ) )
			{
				result.AddError( "No destination file given" );
			}

			string rect = GetValue( RectangleKey );
			if( rect != null )
			{
				string[] parts = rect.Split( ';' );
				if( parts.Length != 2 && parts.Length != 4 )
				{
					result.AddError( string.Format( "Invalid rectangle given: {0}", rect ) );
				}

				if( parts.Length == 2 )
				{
					parts = new string[] { parts[0], parts[1], "0", "0" };
				}

				foreach( string p in parts )
				{
					int v;
					if( !int.TryParse( p, out v ) )
					{
						result.AddError( string.Format( "Invalid rectangle given: {0}", rect ) );
					}

					if( v < 0 )
					{
						result.AddError( string.Format( "Negative value given: {0}", rect ) );
					}
				}
			}
			else
			{
				result.AddError( "No rectangle given" );
			}

			string font = GetValue( FontKey );
			if( font != null )
			{
				int size = 12;
				int idxSep = font.IndexOf( ':' );
				bool tryCreate = true;

				if( idxSep >= 0 )
				{
					string str = font.Substring( idxSep + 1 );
					if( !int.TryParse( str, out size ) )
					{
						tryCreate = false;
						result.AddError( string.Format( "Invalid font size specified: {0}", str ) );
					}
					else
					{
						font = font.Substring( 0, idxSep );
					}
				}

				if( tryCreate )
				{
					using( Font ft = new Font( font, size, FontStyle.Regular, GraphicsUnit.Pixel ) )
					{
						if( ft.Name != font )
						{
							result.AddError( string.Format( "Font '{0}' is not installed on the system", font ) );
						}
					}
				}
			}

			string color = GetValue( ColorKey );
			if( color != null )
			{
				try
				{
					ColorTranslator.FromHtml( color );
				}
				catch( Exception )
				{
					result.AddError( string.Format( "Color '{0}' is not a valid color", color ) );
				}
			}

			return result;
		}

		private const string ColorKey = "color";
		private const string DestFileKey = "dest";
		private const string FontKey = "font";
		private const string RectangleKey = "rect";
		private const string SourceFileKey = "source";
	}
}