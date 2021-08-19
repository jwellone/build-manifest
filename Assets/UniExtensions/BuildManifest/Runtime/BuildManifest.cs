using System;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniExtensions.Build
{
	public class BuildManifest
	{
		public static readonly string FOLDER = "BuildManifest/Resources";
		public static readonly string FILE_NAME = "build_manifest.json";

		[SerializeField] string m_buildTime;
		[SerializeField] string m_defineSymbols;
		[SerializeField] string m_shortCommitId;
		[SerializeField] string m_branchName;

		public string productName => Application.productName;
		public string appVersion => Application.version;
		public string unityVersion => Application.unityVersion;
		public string defineSymbols => m_defineSymbols;
		public string buildTime => m_buildTime;
		public string shortCommitId => m_shortCommitId;
		public string branchName => m_branchName;

		static BuildManifest s_instance;

		public static BuildManifest Get()
		{
			if (s_instance != null)
			{
				return s_instance;
			}

			try
			{
				var path = Path.GetFileNameWithoutExtension(FILE_NAME);
				var asset = Resources.Load<TextAsset>(path);
				s_instance = JsonUtility.FromJson<BuildManifest>(asset.text);
				Resources.UnloadAsset(asset);
			}
			catch
			{
				s_instance = new BuildManifest();
			}

#if UNITY_EDITOR
			var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
			s_instance.m_defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
#endif

			return s_instance;
		}

		public BuildManifest(string buildTime, string defineSymbols, string shortCommitId, string branchName)
		{
			m_buildTime = buildTime;
			m_defineSymbols = defineSymbols;
			m_shortCommitId = shortCommitId;
			m_branchName = branchName;
		}

		BuildManifest() { }
	}
}
