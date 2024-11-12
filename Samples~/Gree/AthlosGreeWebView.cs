using System.Collections;
using UnityEngine;

namespace Athlos.WebView
{
  [RequireComponent(typeof(WebViewObject))]
  public class AthlosGreeWebView : AthlosWebView
  {
    private enum AndroidForceDarkMode : int
    {
      FollowSystem,
      ForceOff,
      ForceOn
    }

    private enum IOSContentMode
    {
      Recommended,
      Mobile,
      Desktop
    }
    
    public delegate void Callback(string msg); 
    
    [Header("Gree Initialisation Settings")]
    [SerializeField] private bool transparent;
    [SerializeField] private bool zoom = true;
    [SerializeField] private string ua;
    [SerializeField] private int radius;
    
    [Space(10)]
    [SerializeField] private AndroidForceDarkMode androidForceDarkMode;
    
    [Space(10)]
    [SerializeField] private bool iOSEnableWKWebView = true;
    [SerializeField] private IOSContentMode iOSWKContentMode;
    [SerializeField] private bool iOSWKAllowsLinkPreviews = true;
    [SerializeField] private bool iOSWKAllowsBackForwardNavigationGestures = true;
    
    [Space(10)]
    [Tooltip("Open the Webview in a separate window. Allows use of the Safari debugger")]
    [SerializeField] private bool editorSeparated;
    
    private WebViewObject _greeWebView;
    private bool _inited;
    private Callback _callback;
    private Callback _error;
    private Callback _httpError;
    private Callback _loaded;
    private Callback _started;
    private Callback _hooked;
    private Callback _cookies;

    private void Awake()
    {
      _greeWebView = GetComponent<WebViewObject>();
    }

    private IEnumerator Start()
    {
      _greeWebView.Init(_Callback, _Error, _HttpError, _Loaded, _Started, _Hooked, _Cookies,
        transparent, zoom, ua, radius, 
        (int)androidForceDarkMode, 
        iOSEnableWKWebView, (int)iOSWKContentMode, iOSWKAllowsLinkPreviews, iOSWKAllowsBackForwardNavigationGestures, 
        editorSeparated);
      yield return new WaitUntil(() => _greeWebView.IsInitialized());
      _greeWebView.LoadURL(InitialURL);
    }

    private void OnDestroy()
    {
      _callback = null;
      _error = null;
      _httpError = null;
      _loaded = null;
      _started = null;
      _hooked = null;
      _cookies = null;
    }

    public void AddCallbackListener(Callback callback)
    {
      _callback += callback;
    }

    public void RemoveCallbackListener(Callback callback)
    {
      _callback -= callback;
    }
    
    private void _Callback(string msg)
    {
      _callback?.Invoke(msg);
    }
    
    public void AddErrorListener(Callback callback)
    {
      _error += callback;
    }

    public void RemoveErrorListener(Callback callback)
    {
      _error -= callback;
    }

    private void _Error(string msg)
    {
      _error?.Invoke(msg);
    }
    
    public void AddHTTPErrorListener(Callback callback)
    {
      _httpError += callback;
    }

    public void RemoveHTTPErrorListener(Callback callback)
    {
      _httpError -= callback;
    }

    private void _HttpError(string msg)
    {
      _httpError?.Invoke(msg);
    }
    
    public void AddStartedListener(Callback callback)
    {
      _started += callback;
    }

    public void RemoveStartedListener(Callback callback)
    {
      _started -= callback;
    }

    private void _Started(string msg)
    {
      _started?.Invoke(msg);
    }
    
    public void AddHookedListener(Callback callback)
    {
      _started += callback;
    }

    public void RemoveHookedListener(Callback callback)
    {
      _started -= callback;
    }

    private void _Hooked(string msg)
    {
      _hooked?.Invoke(msg);
    }
    
    public void AddCookiesListener(Callback callback)
    {
      _cookies += callback;
    }

    public void RemoveCookiesListener(Callback callback)
    {
      _cookies -= callback;
    }

    private void _Cookies(string msg)
    {
      _cookies?.Invoke(msg);
    }
    
    public void AddLoadedListener(Callback callback)
    {
      _loaded += callback;
    }

    public void RemoveLoadedListener(Callback callback)
    {
      _loaded -= callback;
    }

    private void _Loaded(string msg)
    {
      if (!_inited)
      {
        _greeWebView.EvaluateJS(AuthenticationScript);
        _inited = true;
      }
      _loaded?.Invoke(msg);
    }
  }
}