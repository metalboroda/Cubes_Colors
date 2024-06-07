using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CubeSlot : MonoBehaviour
  {
    private CubeStack _cubeStack;

    public void Init(CubeStack cubeStack)
    {
      _cubeStack = cubeStack;
    }
  }
}