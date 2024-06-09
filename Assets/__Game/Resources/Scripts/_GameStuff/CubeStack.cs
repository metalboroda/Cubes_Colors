using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.Game.States;
using Assets.__Game.Scripts.Infrastructure;
using System.Collections;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class CubeStack : MonoBehaviour
  {
    [SerializeField] private CubeSlot _cubeSlotPrefab;
    [Header("")]
    [SerializeField] private CubeSlotItem[] _cubeSlotItems;
    [Header("Tutorial")]
    [SerializeField] private bool _tutorial;
    [Space]
    [SerializeField] private Material[] _tutorialMaterials;
    [Header("Text")]
    [SerializeField] private string _questText;
    [Header("Stupor")]
    [SerializeField] private float _stuporTimeoutSeconds = 10;

    private int _currentActiveSlotIndex = 0;
    private Coroutine _stuporTimeoutRoutine;

    private GameBootstrapper _gameBootstrapper;

    private EventBinding<EventStructs.StateChanged> _stateChangedEvent;

    private void Awake()
    {
      _gameBootstrapper = GameBootstrapper.Instance;
    }

    private void OnEnable()
    {
      _stateChangedEvent = new EventBinding<EventStructs.StateChanged>(StuporTimerDependsOnState);
    }

    private void OnDisable()
    {
      _stateChangedEvent.Remove(StuporTimerDependsOnState);
    }

    private void Start()
    {
      SpawnCubeSlots();

      StartCoroutine(DoSendCubeStack());
      ResetAndStartStuporTimer();
    }

    private void SpawnCubeSlots()
    {
      Vector3 slotPosition = transform.position;
      BoxCollider slotCollider = _cubeSlotPrefab.GetComponent<BoxCollider>();
      float slotHeight = slotCollider.size.y * _cubeSlotPrefab.transform.localScale.y;

      for (int i = 0; i < _cubeSlotItems.Length; i++)
      {
        CubeSlot newSlot = Instantiate(_cubeSlotPrefab, slotPosition, Quaternion.identity, transform);

        if (_tutorial && i < _tutorialMaterials.Length)
        {
          newSlot.GetComponentInChildren<Renderer>().material = _tutorialMaterials[i];
        }

        newSlot.Init(_cubeSlotItems[i].CubeColor, this);

        if (i == 0)
          newSlot.CanReceive = true;
        else
          newSlot.CanReceive = false;

        newSlot.SwitchVisual();

        slotPosition.y += slotHeight;
      }

      EventBus<EventStructs.VariantsAssignedEvent>.Raise(new EventStructs.VariantsAssignedEvent());
      EventBus<EventStructs.QuestTextEvent>.Raise(new EventStructs.QuestTextEvent { QuestText = _questText });
    }

    public void ActivateNextSlot()
    {
      if (_currentActiveSlotIndex < _cubeSlotItems.Length - 1)
      {
        _currentActiveSlotIndex++;

        CubeSlot nextSlot = transform.GetChild(_currentActiveSlotIndex).GetComponent<CubeSlot>();

        nextSlot.CanReceive = true;

        nextSlot.SwitchVisual();

        ResetAndStartStuporTimer();
      }
      else
      {
        CheckAllSlotsOccupied();
      }
    }

    private void CheckAllSlotsOccupied()
    {
      foreach (Transform child in transform)
      {
        CubeSlot slot = child.GetComponent<CubeSlot>();

        if (slot.CanReceive) return;
      }

      OnAllSlotsOccupied();
    }

    private void OnAllSlotsOccupied()
    {
      EventBus<EventStructs.WinEvent>.Raise(new EventStructs.WinEvent());

      _gameBootstrapper.StateMachine.ChangeState(new GameWinState(_gameBootstrapper));
    }

    private IEnumerator DoSendCubeStack()
    {
      yield return new WaitForEndOfFrame();

      EventBus<EventStructs.ComponentEvent<CubeStack>>.Raise(new EventStructs.ComponentEvent<CubeStack> { Data = this });
    }

    private void StuporTimerDependsOnState(EventStructs.StateChanged stateChanged)
    {
      if (stateChanged.State is GameplayState)
        ResetAndStartStuporTimer();
      else
      {
        if (_stuporTimeoutRoutine != null)
          StopCoroutine(_stuporTimeoutRoutine);
      }
    }

    private void ResetAndStartStuporTimer()
    {
      if (_stuporTimeoutRoutine != null)
        StopCoroutine(_stuporTimeoutRoutine);

      _stuporTimeoutRoutine = StartCoroutine(DoStuporTimerCoroutine());
    }

    private IEnumerator DoStuporTimerCoroutine()
    {
      yield return new WaitForSeconds(_stuporTimeoutSeconds);

      EventBus<EventStructs.StuporEvent>.Raise(new EventStructs.StuporEvent());

      ResetAndStartStuporTimer();
    }
  }
}