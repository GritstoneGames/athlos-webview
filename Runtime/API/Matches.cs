//TODO [ATH-1562] License

using System;

namespace Athlos.API.WebView
{
  public partial class AthlosClientAPI
  {
    public class MatchResult
    {
      private Models.Common.Matches.MatchResult result;
      private string matchSeriesId;
      private int matchNumber;

      public MatchResult(Models.Common.Matches.MatchResult result, string matchSeriesId, int matchNumber)
      {
        this.result = result;
        this.matchSeriesId = matchSeriesId;
        this.matchNumber = matchNumber;
      }

      public Models.Common.Matches.MatchResult Result { get { return result; } }
      public string MatchSeriesId { get { return matchSeriesId; } }
      public int MatchNumber { get { return matchNumber; } }
    }

    public static void ReportMatchResult(MatchResult match, Action onSuccess, Action<Error[]> onFail)
    {
      AthlosAPI.Patch(AthlosAPI.MatchesCategory, Models.Common.Matches.MatchResult.Url(match.MatchSeriesId, match.MatchNumber), match.Result, onSuccess, onFail);
    }
  }
}
