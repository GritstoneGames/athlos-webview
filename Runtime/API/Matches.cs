/*************************************************************************
 * 
 * GFINITY CONFIDENTIAL
 * __________________
 * 
 *  [2023] Gfinity PLC
 *  All Rights Reserved.
 * 
 * NOTICE:  All information contained herein is, and remains
 * the property of Gfinity PLC and its suppliers,
 * if any.  The intellectual and technical concepts contained
 * herein are proprietary to Gfinity PLC
 * and its suppliers and may be covered by U.S. and Foreign Patents,
 * patents in process, and are protected by trade secret or copyright law.
 * Dissemination of this information or reproduction of this material
 * is strictly forbidden unless prior written permission is obtained
 * from Gfinity PLC.
 */

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
