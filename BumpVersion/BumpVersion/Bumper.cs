// $Id$

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BumpVersion.Tasks;

namespace BumpVersion
{
	/// <summary>
	/// The bumper that loads a project file and can execute the tasks defined in it.
	/// </summary>
	public class Bumper
	{
		private Version CurrentVersion;
		private Dictionary<string, Type> KnownTasks;
		private List<BumpTask> TaskList;

		public Bumper( string fileName )
		{
			ReadKnownTasks();
			ParseFile( fileName );
		}

		/// <summary>
		/// Does the actual bumping by executiing all loaded tasks.
		/// </summary>
		/// <param name="newVersion">The version to bump to</param>
		/// <returns>Result of all executed tasks</returns>
		public OperationResult Bump( Version newVersion )
		{
			OperationResult result = new OperationResult();

			foreach( BumpTask task in TaskList )
			{
				result.Merge( task.Bump( CurrentVersion, newVersion ) );
			}

			return result;
		}

		/// <summary>
		/// Saves the current version to the project file
		/// </summary>
		/// <param name="fileName">Path of the project file</param>
		/// <param name="version">The version to save</param>
		public void SaveCurrentVersion( string fileName, Version version )
		{
			XmlDocument doc = new XmlDocument();
			doc.Load( fileName );
			doc.DocumentElement.SetAttribute( "currentVersion", version.ToString() );
			doc.Save( fileName );
		}

		/// <summary>
		/// Validates the project and all containing tasks.
		/// </summary>
		/// <returns>The validation result</returns>
		public OperationResult Vaildate( Version newVersion )
		{
			OperationResult result = new OperationResult();
			if( TaskList.Count == 0 )
			{
				result.AddError( "No tasks in project" );
			}

			if( newVersion <= CurrentVersion )
			{
				result.AddError( string.Format( "New version ({0}) must be greater than current version ({1})", newVersion, CurrentVersion ) );
			}

			foreach( BumpTask task in TaskList )
			{
				result.Merge( task.Validate() );
			}

			return result;
		}

		/// <summary>
		/// Reads the project file and creates the needed BumpTasks
		/// </summary>
		/// <param name="fileName">The project file to load</param>
		private void ParseFile( string fileName )
		{
			XmlDocument doc = new XmlDocument();
			doc.Load( fileName );

			if( !Version.TryParse( doc.DocumentElement.GetAttribute( "currentVersion" ), out CurrentVersion ) )
			{
				CurrentVersion = new Version( 0, 0 );
			}

			XmlNodeList taskNodes = doc.DocumentElement.GetElementsByTagName( "task" );
			// Yeah it's all about optimization ;)
			TaskList = new List<BumpTask>( taskNodes.Count );

			Dictionary<string, string> variables = new Dictionary<string, string>();

			foreach( XmlElement taskNode in taskNodes )
			{
				string type = taskNode.GetAttribute( "type" );

				// Check if the given type is valid (=known)
				Type taskType;
				if( !KnownTasks.TryGetValue( type, out taskType ) )
				{
					Console.Error.WriteLine( "Unknown type '{0}' found in project", type );
					continue;
				}

				// Read all "key"-elements for this task
				Dictionary<string, string> settings = new Dictionary<string, string>();
				foreach( XmlElement settingNode in taskNode.GetElementsByTagName( "key" ) )
				{
					string key = settingNode.GetAttribute( "name" );
					string value = settingNode.GetAttribute( "value" );

					settings.Add( key, value );
				}

				// And finally create the task and feed it the settings
				BumpTask task;
				try
				{
					task = (BumpTask)Activator.CreateInstance( taskType, settings, variables );
				}
				catch( TargetInvocationException ex )
				{
					Console.Error.WriteLine( "Failed to create task of type '{0}':", type );
					Console.Error.WriteLine( ex );
					continue;
				}

				foreach( KeyValuePair<string, string> kvp in task.GetVariables() )
				{
					variables[kvp.Key] = kvp.Value;
				}

				TaskList.Add( task );
			}
		}

		/// <summary>
		/// Creates a list of all available <see cref="Tasks.BumpTask"/> implementations
		/// </summary>
		private void ReadKnownTasks()
		{
			KnownTasks = new Dictionary<string, Type>();
			Type baseType = typeof( BumpTask );

			// TODO: Maybe AppDomain to obtain all loaded assemblies? Plugins?
			foreach( Type type in Assembly.GetExecutingAssembly().GetTypes()
				.Where( t => !t.IsAbstract && baseType.IsAssignableFrom( t ) ) )
			{
				KnownTasks.Add( type.Name, type );
			}
		}
	}
}