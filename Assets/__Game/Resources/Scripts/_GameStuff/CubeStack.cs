using __Game.Resources.Scripts.EventBus;
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

    private int _currentActiveSlotIndex = 0;

    private void Start()
    {
      SpawnCubeSlots();

      StartCoroutine(DoSendCubeStack());
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
    }

    public void ActivateNextSlot()
    {
      if (_currentActiveSlotIndex < _cubeSlotItems.Length - 1)
      {
        _currentActiveSlotIndex++;

        CubeSlot nextSlot = transform.GetChild(_currentActiveSlotIndex).GetComponent<CubeSlot>();

        nextSlot.CanReceive = true;

        nextSlot.SwitchVisual();
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
    }

    private IEnumerator DoSendCubeStack()
    {
      yield return new WaitForEndOfFrame();

      EventBus<EventStructs.ComponentEvent<CubeStack>>.Raise(new EventStructs.ComponentEvent<CubeStack> { Data = this });
    }
  }
}