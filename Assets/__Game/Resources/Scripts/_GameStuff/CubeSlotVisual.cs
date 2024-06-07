using UnityEngine;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  [RequireComponent(typeof(CubeSlot))]
  public class CubeSlotVisual : MonoBehaviour
  {
    [SerializeField] private Transform _particlesPoint;
    [Space]
    [SerializeField] private GameObject _correctParticlesObject;
    [SerializeField] private GameObject _incorrectParticlesObject;

    private CubeSlot _cubeSlot;

    private void Awake()
    {
      _cubeSlot = GetComponent<CubeSlot>();
    }

    private void OnEnable()
    {
      _cubeSlot.CorrectItem += OnCorrectItem;
      _cubeSlot.IncorrectItem += OnIncorrectItem;
    }

    private void OnDisable()
    {
      _cubeSlot.CorrectItem -= OnCorrectItem;
      _cubeSlot.IncorrectItem -= OnIncorrectItem;
    }

    private void OnCorrectItem()
    {
      Instantiate(_correctParticlesObject, _particlesPoint.position, _particlesPoint.rotation);
    }

    private void OnIncorrectItem()
    {
      Instantiate(_incorrectParticlesObject, _particlesPoint.position, _particlesPoint.rotation);
    }
  }
}