/*************************************************************************
 * 
 * ATHLOS GAME TECHNOLOGIES LTD. CONFIDENTIAL
 * __________________
 * 
 *  [2023] Athlos Game Technologies Ltd.
 *  All Rights Reserved.
 * 
 * NOTICE:  All information contained herein is, and remains
 * the property of Athlos Game Technologies Ltd. and its suppliers,
 * if any.  The intellectual and technical concepts contained
 * herein are proprietary to Athlos Game Technologies Ltd.
 * and its suppliers and may be covered by U.S. and Foreign Patents,
 * patents in process, and are protected by trade secret or copyright law.
 * Dissemination of this information or reproduction of this material
 * is strictly forbidden unless prior written permission is obtained
 * from Athlos Game Technologies Ltd.
 */

using System;

namespace Athlos.API.WebView
{
  public partial class AthlosWebViewAPI
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
      AthlosAPI.Patch(AthlosAPI.MatchesCategory, Models.Common.Matches.MatchResult.Url(match.MatchSeriesId, match.MatchNumber), Authentication, match.Result, onSuccess, onFail);
    }
  }
}
