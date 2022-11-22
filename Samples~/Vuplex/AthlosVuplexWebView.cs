//TODO [ATH-1562] License 

using System;
using System.Threading.Tasks;
using UnityEngine;
using Vuplex.WebView;

namespace Athlos.WebView
{
  public sealed class AthlosVuplexWebView : AthlosWebView
  {
    [Header("Vuplex WebView")]
    [SerializeField] private BaseWebViewPrefab webView;
    [SerializeField] private bool enableConsoleLogging;

    private void Awake()
    {
      webView.InitialUrl = InitialUrl;
      webView.Initialized += OnWebviewInitialized;
    }

    private void OnDestroy()
    {
      webView.WebView.MessageEmitted -= OnWebViewMessageEmitted;
    }

    private async void OnWebviewInitialized(object sender, EventArgs e)
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
      webView.LogConsoleMessages = enableConsoleLogging;
#endif
      webView.Initialized -= OnWebviewInitialized;
      string authenticationScript = AuthenticationScript;
      while (authenticationScript == null)
      {
        await Task.Delay(1000 / Application.targetFrameRate);
        authenticationScript = AuthenticationScript;
      }
      webView.WebView.MessageEmitted += OnWebViewMessageEmitted;
      webView.WebView.PageLoadScripts.Add(authenticationScript);
      await webView.WebView.WaitForNextPageLoadToFinish();
    }

    private void OnWebViewMessageEmitted(object sender, EventArgs<string> e)
    {
      Debug.Log(e.Value);
    }

    public BaseWebViewPrefab WebView
    {
      get
      {
        return webView;
      }
    }
  }
}