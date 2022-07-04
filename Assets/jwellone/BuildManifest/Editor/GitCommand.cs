using System;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using ProcessStartInfo = System.Diagnostics.ProcessStartInfo;

namespace jwellone.Editor
{
	public static class GitCommand
	{
		static readonly string GIT_NAME = SystemInfo.operatingSystem.Contains("Windows") ? "git.exe" : "git";
		static readonly string KEY_WORKING_DIRECTORY = "USER_SETTING_GIT_UTIL_WORKING_DIRECTORY";

		public static string GetWorkingDirectory()
		{
			var path = EditorUserSettings.GetConfigValue(KEY_WORKING_DIRECTORY);
			return string.IsNullOrEmpty(path) ? Application.dataPath : path;
		}

		public static void SetWorkingDirectory(string directory)
		{
			EditorUserSettings.SetConfigValue(KEY_WORKING_DIRECTORY, directory);
		}

		public static Task<string> Exec(string args)
		{
			return ProcessStart(GIT_NAME, args);
		}

		public static Task<string> GetVersion()
		{
			return Exec("--version");
		}

		public static Task<string> GetCommitId()
		{
			return Exec("rev-parse @");
		}

		public static Task<string> GetShortCommitId()
		{
			return Exec("rev-parse --short @");
		}

		public static Task<string> GetBranchName()
		{
			return Exec("rev-parse --abbrev-ref @");
		}

		public static Task<string> GetLog()
		{
			return Exec("log -n 1");
		}

		public static Task<string> GetAuthor()
		{
			return Exec("log -1 --pretty=format:'%an'");
		}

		public static Task<string> GetCommitter()
		{
			return Exec("log -1 --pretty=format:'%cn'");
		}

		static Task<string> ProcessStart(string fileName, string args)
		{
			try
			{
				var info = CreateProcessStartInfo(fileName, args);
				var process = System.Diagnostics.Process.Start(info);
				var taskCompleteSource = new TaskCompletionSource<string>();
				process.EnableRaisingEvents = true;
				process.Exited += (sender, e) =>
				{
					var data = process.StandardOutput.ReadToEnd().Trim();
					process.Dispose();
					process = null;
					taskCompleteSource.TrySetResult(data);
				};

				return taskCompleteSource.Task;
			}
			catch (Exception e)
			{
				Debug.LogError(e);
				return Task.FromResult<string>(null);
			}
		}

		static ProcessStartInfo CreateProcessStartInfo(string fileName, string args)
		{
			return new ProcessStartInfo
			{
				FileName = fileName,
				Arguments = args,
				CreateNoWindow = true,
				WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
				StandardOutputEncoding = Encoding.UTF8,
				StandardErrorEncoding = Encoding.UTF8,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				WorkingDirectory = GetWorkingDirectory()
			};
		}
	}
}
