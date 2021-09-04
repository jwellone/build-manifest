using UnityEngine;
using UnityEngine.UI;

namespace jwellone.Build.Sample
{
    public class SampleScene : MonoBehaviour
    {
        [SerializeField] Text m_text;

		private void Start()
		{
			var sb = new System.Text.StringBuilder();
			var manifest = BuildManifest.Get();

			sb.Append("Unity Version:").AppendLine(manifest.unityVersion);
			sb.Append("Company Name:").AppendLine(manifest.companyName);
			sb.Append("Product Name:").AppendLine(manifest.productName);
			sb.Append("App Version:").AppendLine(manifest.appVersion);
			sb.Append("Scripting Define Symbols:").AppendLine(manifest.defineSymbols);
			sb.Append("Build Time:").AppendLine(manifest.buildTime);
			sb.Append("Commit Id:").AppendLine(manifest.shortCommitId);
			sb.Append("Branch Name:").AppendLine(manifest.branchName);
			sb.Append("Build Location:").AppendLine(manifest.buildLocation);

			m_text.text = sb.ToString();
		}
	}
}