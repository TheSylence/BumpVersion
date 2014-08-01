// $Id$

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BumpVersion.Tests
{
	internal static class TestData
	{
		internal static string EmptyFileContent
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

		internal static string SimpleFileContent
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

		internal static string UnknownTypeContent
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
	}
}