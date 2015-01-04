// Copyright (c) 2014 Matthias Specht
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BumpVersion.Tasks
{
	/// <summary>
	/// Task that is able to bump the copyright year in a file's header to the current year.
	/// </summary>
	internal class BumpCopyrightYear : BumpTask
	{
		public BumpCopyrightYear( Dictionary<string, string> settings, Dictionary<string, string> variables )
			: base( settings, variables )
		{
		}

		public override OperationResult Bump( Version oldVersion, Version newVersion )
		{
			OperationResult result = new OperationResult();
			string[] files = GetValue( FileKey ).Split( ';' );

			int[] linesToSearch = null;
			string linesValue = GetValue( LinesKey );
			if( linesValue != null )
			{
				int[] lineArray;

				if( linesValue.Contains( ';' ) )
				{
					lineArray = linesValue.Split( ';' ).Select( l => int.Parse( l ) ).ToArray();
				}
				else if( linesValue.Contains( '-' ) )
				{
					if( linesValue.StartsWith( "-" ) )
					{
						linesValue = "1" + linesValue;
					}
					else if( linesValue.EndsWith( "-" ) )
					{
						linesValue += int.MaxValue.ToString();
					}

					lineArray = linesValue.Split( new[] { '-' }, 2 ).Select( l => int.Parse( l ) ).ToArray();
				}
				else
				{
					lineArray = new[] { int.Parse( linesValue ) };
				}

				linesToSearch = lineArray.Select( l => Convert.ToInt32( l ) ).ToArray();
			}

			bool range = true;
			string strRange = GetValue( RangeKey );
			if( strRange != null )
			{
				range = bool.Parse( strRange );
			}

			Regex pattern = new Regex( DefaultPattern );

			foreach( string fileName in files )
			{
				bool touched = false;
				string[] lines = File.ReadAllLines( fileName );

				for( int i = 0; i < lines.Length; ++i )
				{
					if( linesToSearch != null && !linesToSearch.Contains( i + 1 ) )
					{
						continue;
					}

					Match match = pattern.Match( lines[i] );
					if( match.Success )
					{
						touched = true;

						int start = match.Groups[1].Index;
						int length = match.Groups[1].Length + match.Groups[2].Length;

						string strYears = lines[i].Substring( start, length );
						int[] years = strYears.Split( new[] { '-', ',' } ).Select( y => Convert.ToInt32( y ) ).ToArray();
						if( range )
						{
							// This will generate 2009-2012 from 2009,2011 in 2012
							// I guess the correct behavior would be to generate 2009,2011-2012
							years = new[] { years.Min(), Math.Max( years.Max(), DateTime.Now.Year ) };
						}
						else
						{
							years = years.Concat( new[] { DateTime.Now.Year } ).ToArray();
						}

						string line = lines[i].Substring( 0, start );
						if( range )
						{
							line += string.Format( "{0}-{1}", years[0], years[1] );
						}
						else
						{
							line += string.Join( ",", years );
						}
						line += lines[i].Substring( start + length );

						lines[i] = line;
					}
				}

				if( touched )
				{
					File.WriteAllLines( fileName, lines );
				}
			}

			return result;
		}

		public override OperationResult Validate()
		{
			OperationResult result = new OperationResult();

			string files = GetValue( FileKey );
			if( string.IsNullOrWhiteSpace( files ) )
			{
				result.AddError( "No files given" );
			}

			string lines = GetValue( LinesKey );
			if( lines != null )
			{
				string[] lineArray = null;

				if( lines.Contains( ';' ) )
				{
					lineArray = lines.Split( new[] { ';' }, StringSplitOptions.RemoveEmptyEntries );
				}
				else if( lines.Contains( '-' ) )
				{
					if( lines.StartsWith( "-" ) )
					{
						lines = "0" + lines;
					}
					else if( lines.EndsWith( "-" ) )
					{
						lines += int.MaxValue.ToString();
					}

					lineArray = lines.Split( new[] { '-' }, 2 );
				}
				else
				{
					lineArray = new[] { lines };
				}

				foreach( string line in lineArray )
				{
					int tmp;
					if( !int.TryParse( line, out tmp ) )
					{
						result.AddError( string.Format( "Invalid line: {0}", line ) );
					}
				}
			}

			string range = GetValue( RangeKey );
			if( range != null )
			{
				bool tmp;
				if( !bool.TryParse( range, out tmp ) )
				{
					result.AddError( string.Format( "Failed to parse {0} as boolean", range ) );
				}
			}

			return result;
		}

		private const string DefaultPattern = @"Copyright(?: \([cC]\))? (\d{4})((?:(?:,\d{4})+)|(?:(?:-\d{4})?))(?:.*)";
		private const string FileKey = "files";
		private const string LinesKey = "lines";
		private const string RangeKey = "range";
	}
}