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

using UnityEngine;
using UnityEngine.UI;

namespace Athlos.WebView
{
  /// <summary>
  /// Attach this to the gameobject with the Camera component if embedding the Gameface view in a Canvas hierarchy
  /// </summary>
  public class GamefaceRenderToCanvas : MonoBehaviour
  {
    [SerializeField] protected AthlosGamefaceView view;
    [SerializeField] protected RawImage image;

    protected virtual void Start()
    {
      image.texture = view.View.ViewTexture;
      RectTransform rectTransorm = view.View.GetComponent<RectTransform>();
      rectTransorm.localScale = new Vector3(1, -1, 1);
    }

    protected virtual void OnPreRender()
    {
      view.View.Draw(); //Mimics (replaces) the standard implementation
    }
  }
}