//TODO [ATH-1562] License

using Athlos.Models.Client.Matches;
using Athlos.API.Common.Matches;
using System;

namespace Athlos.API.WebView
{
  public partial class AthlosClientAPI
  {
    public static void ReportMatchResult(Match match, Action onSuccess, Action<AthlosError[]> onFail)
    {
      string url = $"match-series/{match.Id}/matches/1/results";
      AthlosAPI.Patch(AthlosAPI.MatchesCategory, url, 
        new ReportMatchResultRequest(match), onSuccess, onFail);
    }
  }
}
