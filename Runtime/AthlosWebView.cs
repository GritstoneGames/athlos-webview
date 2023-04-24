/*************************************************************************
 * 
 * GFINITY CONFIDENTIAL
 * __________________
 * 
 *  [2023] Gfinity PLC
 *  All Rights Reserved.
 * 
 * NOTICE:  All information contained herein is, and remains
 * the property of Gfinity PLC and its suppliers,
 * if any.  The intellectual and technical concepts contained
 * herein are proprietary to Gfinity PLC
 * and its suppliers and may be covered by U.S. and Foreign Patents,
 * patents in process, and are protected by trade secret or copyright law.
 * Dissemination of this information or reproduction of this material
 * is strictly forbidden unless prior written permission is obtained
 * from Gfinity PLC.
 */

using Athlos.HTTP;
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
    struct InitialColors
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
    [SerializeField] private InitialColors initialColors;

    public bool PageLoaded { get; protected set; }
    private UnityEvent<string> onPageLoaded;
    private UnityEvent<string> onMessageReceived;

    protected string InitialUrl
    {
      get
      {
        string initialColorsParam = initialColors.enabled ? $"&bg={initialColors.BackgroundHex}&spinner={initialColors.SpinnerHex}" : "";
        return 
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        scope == Scope.ProofOfConcept ? $"https://test.athlos.gg?tenant={AthlosHTTP.Tenant}{initialColorsParam}" :
#endif
        scope == Scope.StagingInApp ?   $"https://staging-inapp.athlos.gg?tenant={AthlosHTTP.Tenant}{initialColorsParam}" :
                                        $"https://ingame.gfinity.gg?tenant={AthlosHTTP.Tenant}{initialColorsParam}";
      }
    }

    protected string AuthenticationScript
    {
      get
      {
        Debug.Assert(!string.IsNullOrEmpty(AthlosHTTP.JWT), "JWT missing. Fetch a JWT and assign it to your webview via the JWT property");
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

    public void AddOnPageLoadedListener(UnityAction<string> onPageLoaded)
    {
      if (this.onPageLoaded == null)
      {
        this.onPageLoaded = new UnityEvent<string>();
      }
      this.onPageLoaded.AddListener(onPageLoaded);
    }

    public void RemoveOnPageLoadedListener(UnityAction<string> onPageLoaded)
    {
      this.onPageLoaded?.RemoveListener(onPageLoaded);
    }

    protected void OnPageLoaded(string message)
    {
      onPageLoaded?.Invoke(message);
    }
      
    public void AddMessageReceivedListener(UnityAction<string> onMessageReceived)
    {
      if (this.onMessageReceived == null)
      {
        this.onMessageReceived = new UnityEvent<string>();
      }
      this.onMessageReceived.AddListener(onMessageReceived);
    }

    public void RemoveMessageReceivedListener(UnityAction<string> onMessageReceived)
    {
      this.onMessageReceived?.RemoveListener(onMessageReceived);
    }

    protected void OnMessageReceived(string message)
    {
      onMessageReceived?.Invoke(message);
    }
  }
}