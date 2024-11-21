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

    /// <summary>
    /// Your implementation of acquiring the JWT for the player
    /// </summary>
    public delegate Task<string> AthlosAuthenticate();

    private AthlosAuthenticate _authMethod;
    private string _jwt;

    /// <summary>
    /// The base tenant of your Athlos solution
    /// </summary>
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
    
    /// <summary>
    /// The Json Web Token for the player, if authenticated
    /// </summary>
    public static string JWT => _instance._jwt;

    /// <summary>
    /// Are you testing your authentication process with Athlos?
    /// </summary>
    public static bool TestAuthentication =>
#if !UNITY_EDITOR && !DEVELOPMENT_BUILD
      false;
#else
      _instance.testAuthentication;
#endif
    /// <summary>
    /// The sub tenant for your Athlos solution. This will affect the resulting endpoint URL for your API calls.
    /// </summary>
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

    /// <summary>
    /// Authenticate the player using your Athlos authentication method
    /// </summary>
    public static async void Authenticate()
    {
      if (_instance._authMethod != null)
      {
        _instance._jwt = await _instance._authMethod.Invoke();
      }
    }

    /// <summary>
    /// Is the player authenticated with Athlos?
    /// </summary>
    public static bool Authenticated => !string.IsNullOrEmpty(_instance._jwt);

    /// <summary>
    /// Set the authentication method to be used to acquire the JWT for the player
    /// </summary>
    /// <param name="authMethod">Your authentication method</param>
    public static void SetAuthenticationMethod(AthlosAuthenticate authMethod)
    {
      _instance._authMethod = authMethod;
    }

    /// <summary>
    /// Discard your player's JWT. You will need to call Authenticate again to acquire a new one.
    /// </summary>
    public static void ForgetCredentials()
    {
      _instance._jwt = null;
    }
  }
}