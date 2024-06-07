using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Train
{
  public class CartHandler : MonoBehaviour
  {
    [field: SerializeField] public Transform CartJoint { get; private set; }
    [field: Space]
    [field: SerializeField] public Transform AnswerPlacePoint { get; private set; }
    [Header("")]
    [SerializeField] private Renderer[] _renderers;

    public void DisableVisual()
    {
      foreach (Renderer renderer in _renderers)
      {
        renderer.enabled = false;
      }
    }
  }
}