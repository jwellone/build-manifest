using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace jwellone.Build
{
	public class BuildManifest
	{
		public static readonly string FOLDER = "BuildManifest/Resources";
		public static readonly string FILE_NAME = "build_manifest.json";

		[SerializeField] string m_unityVersion;
		[SerializeField] string m_companyName;
		[SerializeField] string m_productName;
		[SerializeField] string m_appVersion;
		[SerializeField] string m_defineSymbols;
		[SerializeField] string m_buildTime;
		[SerializeField] string m_shortCommitId;
		[SerializeField] string m_branchName;
		[SerializeField] string m_buildLocation;
		[SerializeField] string[] m_scriptCompilationDefines;

		public string companyName => m_companyName;
		public string productName => m_productName;
		public string appVersion => m_appVersion;
		public string unityVersion => m_unityVersion;
		public string defineSymbols => m_defineSymbols;
		public string buildTime => m_buildTime;
		public string shortCommitId => m_shortCommitId;
		public string branchName => m_branchName;
		public string buildLocation => m_buildLocation;
		public string[] scriptCompilationDefines => m_scriptCompilationDefines;

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
#if UNITY_EDITOR
				s_instance = new BuildManifest(string.Empty, string.Empty);
#else
				s_instance = new BuildManifest();
#endif
			}

			return s_instance;
		}

		private BuildManifest()
		{
		}

#if UNITY_EDITOR
		public BuildManifest(string shortCommitId, string branchName)
		{
			var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
			m_companyName = Application.companyName;
			m_productName = Application.productName;
			m_appVersion = Application.version;
			m_unityVersion = Application.unityVersion;
			m_buildTime = System.DateTime.Now.ToString("yyyy/M/d/H:mm:ss");
			m_defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
			m_shortCommitId = shortCommitId;
			m_branchName = branchName;
			m_buildLocation = EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget);
			m_scriptCompilationDefines = EditorUserBuildSettings.activeScriptCompilationDefines;
		}
#endif
	}
}
