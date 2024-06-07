using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.Enums;
using Assets.__Game.Resources.Scripts.Game.States;
using Assets.__Game.Scripts.Infrastructure;
using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CubeSlot : MonoBehaviour
  {
    public event Action CorrectItem;
    public event Action IncorrectItem;

    public bool CanReceive = false;
    [Header("")]
    [SerializeField] private CubeColorEnums _cubeColor;

    public bool IsBlocked { get; private set; }

    private MeshRenderer _meshRenderer;

    private GameBootstrapper _gameBootstrapper;
    private CubeStack _cubeStack;

    private void Awake()
    {
      _gameBootstrapper = GameBootstrapper.Instance;

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
      if (IsBlocked == true) return;

      cube.transform.DOMove(transform.position, 0.2f);
      cube.transform.DORotateQuaternion(transform.rotation, 0.2f);

      if (cube.CubeColor == _cubeColor)
      {
        CanReceive = false;

        SwitchVisual();

        _cubeStack.ActivateNextSlot();

        CorrectItem?.Invoke();
      }
      else
      {
        IsBlocked = true;

        EventBus<EventStructs.LoseEvent>.Raise(new EventStructs.LoseEvent());

        _gameBootstrapper.StateMachine.ChangeState(new GameLoseState(_gameBootstrapper));

        IncorrectItem?.Invoke();
      }
    }
  }
}