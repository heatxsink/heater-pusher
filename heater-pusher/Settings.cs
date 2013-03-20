using System;
using System.Configuration;
using System.Collections.Specialized;

namespace Heater.Pusher
{
	public class Settings
	{	
		public static string WatcherPath { get; private set; }
		public static string WatcherPaths { get; private set; }
		public static string WatcherFilters { get; private set; }
		public static string ScriptWorkingDirectory { get; private set; }
		public static string ScriptToHook { get; private set; }
		public static bool DebugFlag { get; private set; }
		
		static Settings()
		{
			NameValueCollection settings = ConfigurationManager.AppSettings;
			WatcherPath = GetOrEmpty("watcher-path", settings);
			WatcherPaths = GetOrEmpty("watcher-paths", settings);
			WatcherFilters = GetOrEmpty("watcher-filters", settings);
			ScriptWorkingDirectory = GetOrEmpty("script-working-directory", settings);
			ScriptToHook = GetOrEmpty("script-to-hook", settings);
			DebugFlag = GetFlag("debug-flag", settings);
		}
		
		public static bool GetFlag(string name, NameValueCollection settings)
		{
			return GetOrEmpty(name, settings) == "1";
		}
		
		public static string GetOrEmpty(string name, NameValueCollection settings)
		{
			string return_value = string.Empty;
			
			if (ContainsSetting(name, settings))
			{
				Console.WriteLine("{0} => {1}", name, settings[name]);
				return_value = settings[name];
			}
			else
			{
				Console.WriteLine("AppSettings key {0} not found. Returning string.Empty.", name);
			}
			
			return return_value;
		}
		
		public static bool ContainsSetting(string name, NameValueCollection settings)
		{
			bool return_value = false;
			
			foreach(string key in settings.AllKeys)
			{
				if(key == name)
				{
					return_value = true;
					break;
				}
			}
			
			return return_value;
		}
	}
}

