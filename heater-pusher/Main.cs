using System;
using System.IO;
using System.Diagnostics;

namespace Heater.Pusher
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			foreach(string micro_path in Settings.WatcherPaths.Split(';'))
			{
				string path = string.Format("{0}/{1}", Settings.WatcherPath, micro_path);
				FileSystemWatcher watcher = new FileSystemWatcher();
				watcher.Path = path;
				watcher.Filter = "*.*";
				watcher.EnableRaisingEvents = true;
				watcher.IncludeSubdirectories = false;
				watcher.NotifyFilter = NotifyFilters.FileName;
				watcher.Changed += new FileSystemEventHandler(watcher_file_changed);
				watcher.Created += new FileSystemEventHandler(watcher_file_created);
			}
			
			Console.WriteLine("press [enter] to quit");
			Console.ReadLine();
		}
		
		static void watcher_file_created(Object obj, FileSystemEventArgs args)
		{
			string full_path = args.FullPath;
			
			if (IsFilteredExtention(full_path))
			{
				invoke_script_hook();
			}
			
			Console.WriteLine("{0} has been created.", args.FullPath);
		}

		static void watcher_file_changed(Object obj, FileSystemEventArgs args)
		{
			string full_path = args.FullPath;
			
			if (IsFilteredExtention(full_path))
			{
				invoke_script_hook();
			}
			
			Console.WriteLine("{0} has changed.", args.FullPath);
		}
		
		static bool IsFilteredExtention(string full_path)
		{
			bool return_value = false;
			
			if (File.Exists(full_path)) {
				string the_extention = Path.GetExtension(full_path).Replace(".", "");
				foreach(string ext in Settings.WatcherFilters.Split(';'))
				{
					if(ext == the_extention)
					{
						return_value = true;
					}
				}
			}
			
			return return_value;
		}	
		
		static void invoke_script_hook()
		{
			System.Diagnostics.Process process = new System.Diagnostics.Process();
			process.StartInfo.WorkingDirectory = Settings.ScriptWorkingDirectory;
			process.StartInfo.FileName = Settings.ScriptToHook;
			process.StartInfo.UseShellExecute = false;
			process.EnableRaisingEvents = false;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.StartInfo.CreateNoWindow = false;
			process.Start();
			StreamReader stream_error = process.StandardError;
			StreamReader stream_output = process.StandardOutput;
			
			if (Settings.DebugFlag)
			{
				Console.WriteLine("standard_output: {0}", stream_output.ReadLine());
				Console.WriteLine("standard_error: {0}", stream_error.ReadLine());				
			}
			
			stream_error.Close();
			stream_output.Close();
			process.Close();
			Console.WriteLine("invoke_script_hook has been hooked.");
		}
	}
}