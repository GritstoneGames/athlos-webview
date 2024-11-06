using UnityEngine;

namespace Athlos
{
  public abstract class AthlosWebView : MonoBehaviour
  {
    [Header("Initial Colors")]
    [SerializeField] private bool setInitialColors;
    [SerializeField] private Color backgroundColor;
    [SerializeField] private Color spinnerColor;

    private string BaseURL
    {
      get
      {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        return $"https://{(Athlos.TestAuthentication ? "test" : "inapp")}.athlos.gg";
#else
        return "https://inapp.athlos.gg";
#endif
      }
    }
    
    private string InitialColorParams
    {
      get
      {
        if (!setInitialColors)
        {
          return "";
        }

        return $"&bg={ColorUtility.ToHtmlStringRGB(backgroundColor)}&spinner={ColorUtility.ToHtmlStringRGB(spinnerColor)}";
      }
    }

    protected string InitialURL
    {
      get
      {
        string url = $"{BaseURL}?tenant={Athlos.Tenant}{InitialColorParams}";
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (Athlos.TestAuthentication)
        {
          url = $"{BaseURL}";
        }
#endif        
        return url;
      }
    }

    protected string AuthenticationScript
    {
      get
      {
        Debug.Assert(!string.IsNullOrEmpty(Athlos.JWT), "JWT is null or empty");
        return $@"
window.__athlos_auth = () => {{
  return {{
    ""jwt"": ""{Athlos.JWT}""
  }}
}}";
      }
    }
  }
}