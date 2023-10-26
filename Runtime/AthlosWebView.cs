/*************************************************************************
 * 
 * ATHLOS GAME TECHNOLOGIES LTD. CONFIDENTIAL
 * __________________
 * 
 *  [2023] Athlos Game Technologies Ltd.
 *  All Rights Reserved.
 * 
 * NOTICE:  All information contained herein is, and remains
 * the property of Athlos Game Technologies Ltd. and its suppliers,
 * if any.  The intellectual and technical concepts contained
 * herein are proprietary to Athlos Game Technologies Ltd.
 * and its suppliers and may be covered by U.S. and Foreign Patents,
 * patents in process, and are protected by trade secret or copyright law.
 * Dissemination of this information or reproduction of this material
 * is strictly forbidden unless prior written permission is obtained
 * from Athlos Game Technologies Ltd.
 */

using Athlos.HTTP;
using Athlos.API.WebView;
using System;
using UnityEngine;
using UnityEngine.Events;

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

    [Serializable]
    protected struct InitialColors
    {
      private const string HexFormat = "x2";

      public bool enabled;
      public Color background;
      public Color spinner;

      private string ToHex(Color color)
      {
        int r = (int)(color.r * 255f);
        int g = (int)(color.g * 255f);
        int b = (int)(color.b * 255f);

        return $"{r.ToString(HexFormat)}{g.ToString(HexFormat)}{b.ToString(HexFormat)}";
      }

      public string BackgroundHex
      {
        get
        {
          return $"{ToHex(background)}";
        }
      }

      public string SpinnerHex
      {
        get
        {
          return $"{ToHex(spinner)}";
        }
      }
    }

    [Header("Configuration")]
    [SerializeField] protected Scope scope;
    [SerializeField] protected InitialColors initialColors;

    public bool PageLoaded { get; protected set; }
    private UnityEvent<string> onPageLoaded;
    private UnityEvent<string> onMessageReceived;

    protected virtual string BaseUrl
    {
      get
      {
        string scopeString = null;
        switch (scope)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
          case Scope.ProofOfConcept:
          {
            scopeString = "test";
            break;
          }
#endif
          case Scope.StagingInApp:
          {
            scopeString = "staging-inapp";
            break;
          }
          case Scope.Release:
          {
            scopeString = "inapp";
            break;
          }
        }
        return $"https://{scopeString}.athlos.gg";
      }
    }

    protected virtual string InitialUrl
    {
      get
      {
        string initialColorsParam = initialColors.enabled ? $"&bg={initialColors.BackgroundHex}&spinner={initialColors.SpinnerHex}" : "";
        return $"{BaseUrl}?tenant={AthlosHTTP.Tenant}{initialColorsParam}";
      }
    }

    public abstract string CurrentUrl { get; }

    protected string AuthenticationScript
    {
      get
      {
        Debug.Assert(!string.IsNullOrEmpty(AthlosHTTP.JWT), "JWT missing. Fetch a JWT and assign it to your webview via the JWT property");
        Debug.Assert(AthlosWebViewAPI.Authenticated, $"JWT is either missing or has expired (time remaining: {AthlosHTTP.JWTRemainingTime.Seconds}s");
        return AthlosHTTP.JWT == null ? null : $@"
window.__athlos_auth = () => {{
    return {{
        ""jwt"": ""{AthlosHTTP.JWT}""
    }}
}}";
      }
    }

    protected virtual void Awake()
    {
      PageLoaded = false;
    }

    private void OnDestroy()
    {
      onMessageReceived = null;
    }

    public abstract void ExecuteJavascript(string javascript);

    public void AddOnPageLoadedListener(UnityAction<string> onLoaded)
    {
      if (onPageLoaded == null)
      {
        onPageLoaded = new UnityEvent<string>();
      }
      onPageLoaded.AddListener(onLoaded);
    }

    public void RemoveOnPageLoadedListener(UnityAction<string> onLoaded)
    {
      onPageLoaded?.RemoveListener(onLoaded);
    }

    protected void OnPageLoaded(string message)
    {
      onPageLoaded?.Invoke(message);
    }
      
    public void AddMessageReceivedListener(UnityAction<string> onReceived)
    {
      if (onMessageReceived == null)
      {
        onMessageReceived = new UnityEvent<string>();
      }
      onMessageReceived.AddListener(onReceived);
    }

    public void RemoveMessageReceivedListener(UnityAction<string> onReceived)
    {
      onMessageReceived?.RemoveListener(onReceived);
    }

    protected void OnMessageReceived(string message)
    {
      onMessageReceived?.Invoke(message);
    }
  }
}