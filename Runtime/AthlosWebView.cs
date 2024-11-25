using UnityEngine;

namespace Athlos
{
  public abstract class AthlosWebView : MonoBehaviour
  {
    public enum Target
    {
      None,
      Event,
      MatchSeries,
      User
    }
    
    [Header("Initial Colors")]
    [SerializeField] private bool setInitialColors;
    [SerializeField] private Color backgroundColor;
    [SerializeField] private Color spinnerColor;
      
    private Target _target;
    private string _targetId;

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
    
    private string TargetURLDir
    {
      get
      {
        switch (_target)
        {
          case Target.Event:
          {
            return $"/events/{_targetId}";
          }
          case Target.MatchSeries:
          {
            return $"/match/{_targetId}";
          }
          case Target.User:
          {
            return $"/users/{_targetId}";
          }
        }
        return "";
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
        string url = $"{BaseURL}{TargetURLDir}?tenant={Athlos.Tenant}{InitialColorParams}";
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
    
    /// <summary>
    /// Open Athlos at a specific item such as an event, match or player
    /// </summary>
    /// <param name="target">The type of the item to open Athlos at</param>
    /// <param name="id">The unique ID of the item to open</param>
    public void OpenAt(Target target, string id)
    {
      _target = target;
      _targetId = id;
      OnInitialURLChanged();
    }
    
    protected abstract void OnInitialURLChanged();
  }
}