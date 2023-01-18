//TODO [ATH-1562] License

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