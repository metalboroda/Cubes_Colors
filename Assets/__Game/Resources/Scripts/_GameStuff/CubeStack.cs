using __Game.Resources.Scripts.EventBus;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CubeStack : MonoBehaviour
  {
    [SerializeField] private GameObject _cubeSlotObject;
    [Header("")]
    [SerializeField] private int _stackSize;

    private void Awake()
    {
      EventBus<EventStructs.ComponentEvent<CubeStack>>.Raise(new EventStructs.ComponentEvent<CubeStack> { Data = this });
    }
  }
}