using System.Threading.Tasks;
using UnityEngine;

namespace Athlos
{
  public class Athlos : MonoBehaviour
  { 
    [SerializeField] private string tenant;
    [SerializeField] private Environment environment;
    [SerializeField] private bool testAuthentication;
    
    private static Athlos _instance;

    public delegate Task<string> AthlosAuthenticate();

    private AthlosAuthenticate _authMethod;
    private string _jwt;

    public static string Tenant
    {
      get
      {
        if (Environment != Environment.Custom)
        {
          string environment = $"{Environment}";
          return $"{_instance.tenant}/{environment.ToLowerInvariant()}";
        }
        return _instance.tenant;
      }
      set
      {
        if (Environment == Environment.Custom)
        {
          _instance.tenant = value;
        }
      }
    }
    
    public static string JWT => _instance._jwt;

    public static bool TestAuthentication =>
#if !UNITY_EDITOR && !DEVELOPMENT_BUILD
      false;
#else
      _instance.testAuthentication;
#endif
    public static Environment Environment => _instance.environment;

    private void Awake()
    {
      if (_instance == null)
      {
        _instance = this;
        DontDestroyOnLoad(this);
      }
      else
      {
        DestroyImmediate(gameObject);
      }
    }

    public static async void Authenticate()
    {
      if (_instance._authMethod != null)
      {
        _instance._jwt = await _instance._authMethod.Invoke();
      }
    }

    public static bool Authenticated => !string.IsNullOrEmpty(_instance._jwt);

    public static void SetAuthenticationMethod(AthlosAuthenticate authMethod)
    {
      _instance._authMethod = authMethod;
    }
  }
}