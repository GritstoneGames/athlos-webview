using Athlos.HTTP;

namespace Athlos.API.WebView
{
  public partial class AthlosWebViewAPI
  {
    public static bool Authenticated
    {
      get
      {
        return Authentication();
      }
    }

    private static bool Authentication()
    {
      return !string.IsNullOrEmpty(AthlosHTTP.JWT) && AthlosHTTP.JWTRemainingTime.TotalSeconds > 0;
    }
  }
}
