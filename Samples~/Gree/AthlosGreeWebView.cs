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

using UnityEngine;
using UnityEngine.Events;

namespace Athlos.WebView
{  
  public class AthlosGreeWebView : AthlosWebView
  {
    public enum AndroidForceDarkMode : int
    {
      FollowSystem,
      ForceOff,
      ForceOn
    }

    public enum IOSContentMode
    {
      Recommended,
      Mobile,
      Desktop
    }

    [Header("Gree WebView")]
    [SerializeField] private WebViewObject webView;
    [Header("Initialisation Settings")]    
    [SerializeField] private bool transparent = false;
    [SerializeField] private bool zoom = true;
    [SerializeField] private string ua;
    [SerializeField] private int roundedRadius = 0;
    [Space(10)]
    [SerializeField] private AndroidForceDarkMode androidForceDarkMode;
    [Space(10)]
    [SerializeField] private bool iOSEnableWKWebView = true;
    [SerializeField] private IOSContentMode iOSContentMode;
    [SerializeField] private bool iOSAllowLinkPreview = true;
    [SerializeField] private bool iOSAllowBackForwardGestures = true;
    [Space(10)]
    [SerializeField] private bool editorSeparated;

    public UnityEvent<string> OnError { get; private set; }
    public UnityEvent<string> OnHTTPError { get; private set; }
    public UnityEvent<string> OnStarted { get; private set; }
    public UnityEvent<string> OnHooked { get; private set; }
    public UnityEvent<string> OnCookies { get; private set; }

    public WebViewObject WebView { get { return webView; } }

    private bool inited;

    public override string CurrentUrl
    {
      get
      {
        return currentUrl;
      }
    }
    private string currentUrl;

    protected override void Awake()
    {
      base.Awake();
      inited = false;

      OnError = new UnityEvent<string>();
      OnHTTPError = new UnityEvent<string>();
      OnStarted = new UnityEvent<string>();
      OnHooked = new UnityEvent<string>();
      OnCookies = new UnityEvent<string>();

      webView.Init(
        _OnCallback, _OnError, _OnHTTPError, _OnLoaded, _OnStarted, _OnHooked, _OnCookies,          //callbacks
        transparent, zoom, ua, roundedRadius,                                                       //properties
        (int)androidForceDarkMode,                                                                  //android
        iOSEnableWKWebView, (int)iOSContentMode, iOSAllowLinkPreview, iOSAllowBackForwardGestures,  //ios
        editorSeparated);                                                                           //editor
    }

    protected virtual void Start()
    {
      webView.LoadURL(InitialUrl);
      webView.SetVisibility(true);
    }

    private void _OnCallback(string message)
    {
      OnMessageReceived(message);
    }

    private void _OnError(string message)
    {
      OnError?.Invoke(message);
    }

    private void _OnHTTPError(string message)
    {
      OnHTTPError?.Invoke(message);
    }

    private void _OnStarted(string message)
    {
      if (!inited)
      {
        webView.EvaluateJS(AuthenticationScript);
        inited = true;
      }
      PageLoaded = false;
      OnStarted?.Invoke(message);
    }

    private void _OnLoaded(string message)
    {
      currentUrl = message;
      PageLoaded = true;
      OnPageLoaded(message);
    }

    private void _OnHooked(string message)
    {
      OnHooked?.Invoke(message);
    }

    private void _OnCookies(string message)
    {
      OnCookies?.Invoke(message);
    }

    public override void ExecuteJavascript(string javascript)
    {
      if (PageLoaded)
      {
        webView.EvaluateJS(javascript);
      }
    }
  }
}
