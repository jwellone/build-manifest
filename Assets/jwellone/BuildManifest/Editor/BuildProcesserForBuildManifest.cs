using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using jwellone.Editor;

namespace jwellone.Build.Editor
{
	public class BuildProcesserForBuildManifest : IPreprocessBuildWithReport, IPostprocessBuildWithReport
	{
		public int callbackOrder => 0;

		public void OnPreprocessBuild(BuildReport report)
		{
			var path = Path.Combine(Application.dataPath, BuildManifest.FOLDER);
			path = Path.Combine(path, BuildManifest.FILE_NAME);

			try
			{
				if (!AssetDatabase.IsValidFolder(Path.Combine("Assets", BuildManifest.FOLDER)))
				{
					var folders = new List<string>();
					folders.AddRange(BuildManifest.FOLDER.Split('/'));
					var work = "Assets";
					for (var i = 0; i < folders.Count; ++i)
					{
						var parent = work;
						work = Path.Combine(parent, folders[i]);
						if (!AssetDatabase.IsValidFolder(work))
						{
							AssetDatabase.CreateFolder(parent, folders[i]);
						}
					}
					AssetDatabase.SaveAssets();
				}

				var tCommitId = GitCommand.GetShortCommitId();
				tCommitId.Wait();

				var tBranchName = GitCommand.GetBranchName();
				tBranchName.Wait();

				var shortCommitId = tCommitId.Result;
				var branchName = tBranchName.Result;
				var json = JsonUtility.ToJson(
					new BuildManifest(
						shortCommitId,
						branchName
						));

				File.WriteAllText(path, json);

				var assetPath = Path.Combine("Assets", BuildManifest.FOLDER);
				assetPath = Path.Combine(assetPath, BuildManifest.FILE_NAME);
				AssetDatabase.ImportAsset(assetPath);
			}
			catch (Exception ex)
			{
				Debug.LogWarning($"[BuildManifest]{path} Write failed. {ex}");
			}
		}

		public void OnPostprocessBuild(BuildReport report)
		{
			try
			{
				var folder = Path.Combine(Application.dataPath, BuildManifest.FOLDER.Split('/')[0]);
				Directory.Delete(folder, true);
				File.Delete(folder + ".meta");
			}
			catch (Exception ex)
			{
				Debug.Log($"[BuildManifest]Delete failed. {ex}");
			}
		}
	}
}
