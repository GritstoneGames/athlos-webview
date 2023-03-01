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

using System;
using System.Threading.Tasks;
using UnityEngine;
using Vuplex.WebView;

namespace Athlos.WebView
{
  public class AthlosVuplexWebView : AthlosWebView
  {
    [Header("Vuplex WebView")]
    [SerializeField] private BaseWebViewPrefab webView;
    [SerializeField] private bool enableConsoleLogging;

    public BaseWebViewPrefab WebView { get { return webView; } }

    public bool Initialized { get; private set; }

    protected override void Awake()
    {
      base.Awake();
      Initialized = false;
      webView.InitialUrl = InitialUrl;
      webView.Initialized += OnWebviewInitialized;
    }

    private void OnDestroy()
    {
      webView.WebView.MessageEmitted -= OnWebViewMessageEmitted;
    }

    private async void OnWebviewInitialized(object sender, EventArgs e)
    {
      Initialized = true;
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
      PageLoaded = true;
      OnPageLoaded(null);
      webView.WebView.LoadProgressChanged += OnLoadProgressChanged;
    }

    private void OnLoadProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      PageLoaded = e.Type == ProgressChangeType.Finished;
      if (PageLoaded)
      {
        OnPageLoaded(null);
      }
    }

    private void OnWebViewMessageEmitted(object sender, EventArgs<string> e)
    {
      OnMessageReceived(e.Value);
    }

    public override void ExecuteJavascript(string javascript)
    {
      if (PageLoaded)
      {
        webView.WebView.ExecuteJavaScript(javascript);
      }
    }
  }
}