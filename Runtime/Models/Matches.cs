//TODO [ATH-1562] License

using Athlos.Models.Common.Matches;
using Athlos.HTTP;
using Newtonsoft.Json;

namespace Athlos.Models.Client.Matches
{
  #region Requests
  [JsonObject(MemberSerialization.OptIn)]
  public class ReportMatchResultRequest : PostForm
  {
    [JsonProperty] private Competitor[] competitors;
    [JsonProperty] private Period[] periods;

    public ReportMatchResultRequest(Match match) : base(null) 
    {
      competitors = match.Competitors;
      periods = match.Periods;
    }
  }
  #endregion
}
