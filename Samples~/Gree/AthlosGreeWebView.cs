//TODO [ATH-1562] License

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

    public WebViewObject WebView { get { return webView; } }

    private bool inited;

    protected override void Awake()
    {
      base.Awake();
      inited = false;

      OnError = new UnityEvent<string>();
      OnHTTPError = new UnityEvent<string>();
      OnStarted = new UnityEvent<string>();
      OnHooked = new UnityEvent<string>();

      webView.Init(
        _OnCallback, _OnError, _OnHTTPError, _OnLoaded, _OnStarted, _OnHooked,                      //callbacks
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
      PageLoaded = true;
      OnPageLoaded(message);
    }

    private void _OnHooked(string message)
    {
      OnHooked?.Invoke(message);
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