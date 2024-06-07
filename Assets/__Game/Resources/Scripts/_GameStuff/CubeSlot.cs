using Assets.__Game.Resources.Scripts.Enums;
using DG.Tweening;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CubeSlot : MonoBehaviour
  {
    public bool CanReceive = false;
    [Header("")]
    [SerializeField] private CubeColorEnums _cubeColor;

    private MeshRenderer _meshRenderer;

    private CubeStack _cubeStack;

    private void Awake()
    {
      _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void Init(CubeColorEnums cubeColor, CubeStack cubeStack)
    {
      _cubeColor = cubeColor;
      _cubeStack = cubeStack;
    }

    public void SwitchVisual()
    {
      _meshRenderer.enabled = CanReceive;
    }

    public void Place(Cube cube)
    {
      cube.transform.DOMove(transform.position, 0.2f);
      cube.transform.DORotateQuaternion(transform.rotation, 0.2f);

      if (cube.CubeColor == _cubeColor)
      {
        CanReceive = false;

        SwitchVisual();

        _cubeStack.ActivateNextSlot();
      }
    }
  }
}