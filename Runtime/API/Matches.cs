//TODO [ATH-1562] License

using Athlos.Models.Common.Matches;
using System;

namespace Athlos.API.WebView
{
  public partial class AthlosClientAPI
  {
    public static void ReportMatchResult(MatchResult match, Action onSuccess, Action<AthlosError[]> onFail)
    {
      AthlosAPI.Patch(AthlosAPI.MatchesCategory, match.Url, match, onSuccess, onFail);
    }
  }
}
