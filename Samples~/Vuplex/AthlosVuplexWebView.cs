#if VUPLEX_STANDALONE

using System;
using UnityEngine;
using Vuplex.WebView;

namespace Athlos.WebView
{
  [RequireComponent(typeof(BaseWebViewPrefab))]
  public class AthlosVuplexWebView : AthlosWebView
  {
    private BaseWebViewPrefab _vuplexWebView;
    
    private void Awake()
    {
      _vuplexWebView = GetComponent<BaseWebViewPrefab>();
      _vuplexWebView.Initialized += OnVuplexWebViewInitialized;
      _vuplexWebView.InitialUrl = InitialURL;
    }

    private void OnVuplexWebViewInitialized(object sender, EventArgs e)
    {
      _vuplexWebView.Initialized -= OnVuplexWebViewInitialized;
      _vuplexWebView.WebView.PageLoadScripts.Add(AuthenticationScript);
    }
  }
}
#endif