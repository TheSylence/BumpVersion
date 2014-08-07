// $Id$

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BumpVersion.Tests
{
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	internal static class TestData
	{
		internal static string CompleteTestContent
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine( "<?xml version=\"1.0\"?>" );
				sb.AppendLine( "<bumpversion currentVersion=\"0.1\">" );
				sb.AppendLine( "<task type=\"CreateVariable\">" );
				sb.AppendLine( "<key name=\"name\" value=\"files\" />" );
				sb.AppendLine( "<key name=\"value\" value=\"out1.txt;out2.txt\" />" );
				sb.AppendLine( "</task>" );
				sb.AppendLine( "<task type=\"WriteToFile\">" );
				sb.AppendLine( "<key name=\"files\" value=\"@files\" />" );
				sb.AppendLine( "</task>" );
				sb.AppendLine( "</bumpversion>" );

				return sb.ToString();
			}
		}

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
				sb.AppendLine( "<bumpversion currentVersion=\"0.1\">" );
				sb.AppendLine( "<task type=\"WriteToFile\">" );
				sb.AppendLine( "<key name=\"files\" value=\"out.txt\" />" );
				sb.AppendLine( "</task>" );
				sb.AppendLine( "</bumpversion>" );

				return sb.ToString();
			}
		}

		internal static string SimpleVSProject
		{
			get
			{
				StringBuilder sb = new StringBuilder();

				sb.AppendLine( "<?xml version=\"1.0\" encoding=\"utf-8\"?>" );
				sb.AppendLine( "<Project ToolsVersion=\"12.0\" DefaultTargets=\"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">" );
				sb.AppendLine( "<Import Project=\"$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props\" Condition=\"Exists('$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props')\" />" );
				sb.AppendLine( "<PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' \">" );
				sb.AppendLine( "<PlatformTarget>AnyCPU</PlatformTarget>" );
				sb.AppendLine( "</PropertyGroup>" );
				sb.AppendLine( "<PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' \">" );
				sb.AppendLine( "<PlatformTarget>AnyCPU</PlatformTarget>" );
				sb.AppendLine( "</PropertyGroup>" );
				sb.AppendLine( "<ItemGroup>" );
				sb.AppendLine( "<Reference Include=\"System\" />" );
				sb.AppendLine( "<Reference Include=\"System.Data\" />" );
				sb.AppendLine( "<Reference Include=\"System.Drawing\" />" );
				sb.AppendLine( "<Reference Include=\"System.Xaml\">" );
				sb.AppendLine( "<RequiredTargetFramework>4.0</RequiredTargetFramework>" );
				sb.AppendLine( "</Reference>" );
				sb.AppendLine( "</ItemGroup>" );
				sb.AppendLine( "<ItemGroup>" );
				sb.AppendLine( "<Compile Include=\"Controls\\Window.xaml.cs\">" );
				sb.AppendLine( "<DependentUpon>Window.xaml</DependentUpon>" );
				sb.AppendLine( "</Compile>" );
				sb.AppendLine( "<Page Include=\"Controls\\Window.xaml\">" );
				sb.AppendLine( "<SubType>Designer</SubType>" );
				sb.AppendLine( "<Generator>MSBuild:Compile</Generator>" );
				sb.AppendLine( "</Page>" );
				sb.AppendLine( "</ItemGroup>" );
				sb.AppendLine( "<ItemGroup>" );
				sb.AppendLine( "<None Include=\"Properties\\Version.txt\" />" );
				sb.AppendLine( "<Resource Include=\"Properties\\Resource.res\" />" );
				sb.AppendLine( "<EmbeddedResource Include=\"Properties\\Resources.resx\">" );
				sb.AppendLine( "<Generator>PublicResXFileCodeGenerator</Generator>" );
				sb.AppendLine( "<LastGenOutput>Resources.Designer.cs</LastGenOutput>" );
				sb.AppendLine( "</EmbeddedResource>" );
				sb.AppendLine( "</ItemGroup>" );
				sb.AppendLine( "<ItemGroup>" );
				sb.AppendLine( "<None Include=\"App.config\" />" );
				sb.AppendLine( "</ItemGroup>" );
				sb.AppendLine( "<Import Project=\"$(MSBuildToolsPath)\\Microsoft.CSharp.targets\" />" );
				sb.AppendLine( "</Project>" );

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