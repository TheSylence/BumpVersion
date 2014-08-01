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
	public class Bumper
	{
		private Dictionary<string, Type> KnownTasks;
		private List<BumpTask> TaskList;

		public Bumper( string fileName )
		{
			ReadKnownTasks();
			ParseFile( fileName );
		}

		public OperationResult Bump()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Validates the project and all containing tasks.
		/// </summary>
		/// <returns>The validation result</returns>
		public OperationResult Vaildate()
		{
			OperationResult result = new OperationResult();
			if( TaskList.Count == 0 )
			{
				result.AddError( "No tasks in project" );
			}

			foreach( BumpTask task in TaskList )
			{
				OperationResult taskValidationResult = task.Validate();
				if( !taskValidationResult.IsSuccess )
				{
					result.Merge( taskValidationResult );
				}
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

			XmlNodeList taskNodes = doc.DocumentElement.GetElementsByTagName( "task" );
			// Yeah it's all about optimization ;)
			TaskList = new List<BumpTask>( taskNodes.Count );

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
				try
				{
					TaskList.Add( (BumpTask)Activator.CreateInstance( taskType, settings ) );
				}
				catch( TargetInvocationException ex )
				{
					Console.Error.WriteLine( "Failed to create task of type '{0}':", type );
					Console.Error.WriteLine( ex );
				}
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