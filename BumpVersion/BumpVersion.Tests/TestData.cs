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

		internal static string InvalidWixFile
		{
			get
			{
				StringBuilder sb = new StringBuilder();

				sb.AppendLine( "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" );
				sb.AppendLine( "<Wix xmlns=\"http://schemas.microsoft.com/wix/2006/wi\">" );
				sb.AppendLine( "</Wix>" );

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

		internal static string WixFile
		{
			get
			{
				StringBuilder sb = new StringBuilder();

				sb.AppendLine( "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" );
				sb.AppendLine( "<Wix xmlns=\"http://schemas.microsoft.com/wix/2006/wi\">" );
				sb.AppendLine( "<Product Id=\"2CCC37ED-6F75-48FC-ADEB-D3008B326B78\" Name=\"ProjectName\" UpgradeCode=\"073D85EE-9EAE-4DA8-A58C-BA2308C58A85\">" );
				sb.AppendLine( "</Product>" );
				sb.AppendLine( "</Wix>" );

				return sb.ToString();
			}
		}
	}
}