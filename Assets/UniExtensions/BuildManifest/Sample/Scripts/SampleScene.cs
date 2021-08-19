using UnityEngine;
using UnityEngine.UI;

namespace UniExtensions.Build.Sample
{
    public class SampleScene : MonoBehaviour
    {
        [SerializeField] Text m_text;

		private void Start()
		{
			var sb = new System.Text.StringBuilder();
			var manifest = BuildManifest.Get();

			sb.Append("AppVer:").AppendLine(manifest.appVersion);
			sb.Append("UnityVer:").AppendLine(manifest.unityVersion);
			sb.Append("ProductName:").AppendLine(manifest.productName);
			sb.Append("BuildTime:").AppendLine(manifest.buildTime);
			sb.Append("Defines:").AppendLine(manifest.defineSymbols);
			sb.Append("CommitId:").AppendLine(manifest.shortCommitId);
			sb.Append("BranchName:").AppendLine(manifest.branchName);

			m_text.text = sb.ToString();
		}
	}
}