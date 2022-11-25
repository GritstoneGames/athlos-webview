//TODO [ATH-1562] License

using UnityEngine;

namespace Athlos.WebView
{
  public abstract class AthlosWebView : MonoBehaviour
  {
    public enum Scope
    {
      ProofOfConcept,
      StagingInApp,
      Release
    }

    [Header("Configuration")]
    [SerializeField] private string tenantName;
    [SerializeField] private Scope scope;

    private string jwt;
    public string JWT { set { jwt = value; } }

    protected string InitialUrl
    {
      get
      {
        return 
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        scope == Scope.ProofOfConcept ? $"https://test.athlos.gg?tenant={tenantName}" :
#endif
        scope == Scope.StagingInApp ?   $"https://staging-inapp.athlos.gg?tenant={tenantName}" :
                                        $"https://ingame.gfinity.gg?tenant={tenantName}";
      }
    }

    protected string AuthenticationScript
    {
      get
      {
        Debug.Assert(!string.IsNullOrEmpty(jwt), "JWT missing. Fetch a JWT and assign it to your webview via the JWT property");
        return jwt == null ? null : $@"
window.__engage_sync = () => {{
    return {{
        ""jwt"": ""{jwt}""
    }}
}}";
      }
    }
  }
}