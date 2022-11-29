//TODO [ATH-1562] License

using Athlos.HTTP;
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
    [SerializeField] private Scope scope;

    protected string InitialUrl
    {
      get
      {
        return 
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        scope == Scope.ProofOfConcept ? $"https://test.athlos.gg?tenant={AthlosHTTP.Tenant}" :
#endif
        scope == Scope.StagingInApp ?   $"https://staging-inapp.athlos.gg?tenant={AthlosHTTP.Tenant}" :
                                        $"https://ingame.gfinity.gg?tenant={AthlosHTTP.Tenant}";
      }
    }

    protected string AuthenticationScript
    {
      get
      {
        Debug.Assert(!string.IsNullOrEmpty(AthlosHTTP.JWT), "JWT missing. Fetch a JWT and assign it to your webview via the JWT property");
        return AthlosHTTP.JWT == null ? null : $@"
window.__engage_sync = () => {{
    return {{
        ""jwt"": ""{AthlosHTTP.JWT}""
    }}
}}";
      }
    }
  }
}