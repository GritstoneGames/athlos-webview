//TODO [ATH-1562] License

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private bool transparent = false;
    [SerializeField] private bool zoom = true;
    [SerializeField] private string ua;
    
    [Header("Android")]
    [SerializeField] private AndroidForceDarkMode forceDarkMode;

    [Header("iOS")]
    [SerializeField] private bool enableWKWebView = true;
    [SerializeField] private IOSContentMode contentMode;
    [SerializeField] private bool allowLinkPreview = true;

    [Header("Editor")]
    [SerializeField] private bool separated;

    private void Awake()
    {
      webView.Init(
        OnCallback, OnError, OnHTTPError, OnLoaded, OnStarted, OnHooked,  //callbacks
        transparent, zoom, ua,                                            //properties
        (int)forceDarkMode,                                               //android
        enableWKWebView, (int)contentMode, allowLinkPreview,              //ios
        separated);                                                       //editor
    }

    private void Start()
    {
      webView.LoadURL(InitialUrl);
      webView.SetVisibility(true);
    }

    private void OnCallback(string message)
    {
      Debug.LogFormat("OnCallback: {0}", message);
    }

    private void OnError(string message)
    {
      Debug.LogErrorFormat("OnError: {0}", message);
    }

    private void OnHTTPError(string message)
    {
      Debug.LogErrorFormat("OnHTTPError: {0}", message);
    }

    private void OnStarted(string message)
    {
      Debug.LogFormat("OnStarted: {0}", message);
      webView.EvaluateJS(AuthenticationScript);
    }

    private void OnLoaded(string message)
    {
      Debug.LogFormat("OnLoaded: {0}", message);
    }

    private void OnHooked(string message)
    {
      Debug.LogFormat("OnHooked: {0}", message);
    }
  }
}