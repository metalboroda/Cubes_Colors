using __Game.Resources.Scripts.EventBus;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class Cube : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
  {
    private CubeStack _cubeStack;

    private EventBinding<EventStructs.ComponentEvent<CubeStack>> _cubeStackComponentEvent;

    private Vector3 _initPosition;
    private bool _isDragging = false;
    private Vector3 _offset;
    private bool _isPlaced = false;

    private void Start()
    {
      _initPosition = transform.position;
    }

    private void OnEnable()
    {
      _cubeStackComponentEvent = new EventBinding<EventStructs.ComponentEvent<CubeStack>>(ReceiveCubeStack);
    }

    private void OnDisable()
    {
      _cubeStackComponentEvent.Remove(ReceiveCubeStack);
    }

    private void ReceiveCubeStack(EventStructs.ComponentEvent<CubeStack> cubeStackComponentEvent)
    {
      _cubeStack = cubeStackComponentEvent.Data;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      _isDragging = true;

      Vector3 mousePosition = Input.mousePosition;

      mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;

      Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

      _offset = transform.position - worldPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
      if (_isDragging && _cubeStack != null)
      {
        Vector3 mousePosition = Input.mousePosition;

        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        worldPosition.z = _cubeStack.transform.position.z;

        Vector3 newPosition = worldPosition + _offset;

        if (newPosition.y < _initPosition.y)
          newPosition.y = _initPosition.y;

        transform.position = newPosition;
      }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      _isDragging = false;

      if (_isPlaced == false)
        transform.DOMove(_initPosition, 0.2f);
    }
  }
}