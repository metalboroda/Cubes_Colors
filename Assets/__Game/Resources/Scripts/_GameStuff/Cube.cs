using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.Enums;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.__Game.Resources.Scripts._GameStuff
{
  public class Cube : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
  {
    [field: SerializeField] public CubeColorEnums CubeColor { get; private set; }

    private BoxCollider _boxCollider;

    private CubeStack _cubeStack;

    private EventBinding<EventStructs.ComponentEvent<CubeStack>> _cubeStackComponentEvent;

    private Vector3 _initPosition;
    private Quaternion _initRotation;
    private bool _isDragging = false;
    private Vector3 _offset;
    private bool _isPlaced = false;

    private void Awake()
    {
      _boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
      _initPosition = transform.position;
      _initRotation = transform.rotation;
    }

    private void OnEnable()
    {
      _cubeStackComponentEvent = new EventBinding<EventStructs.ComponentEvent<CubeStack>>(ReceiveCubeStack);
    }

    private void OnDisable()
    {
      _cubeStackComponentEvent.Remove(ReceiveCubeStack);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (_isDragging == false) return;

      if (other.TryGetComponent(out CubeSlot cubeSlot))
      {
        if (cubeSlot.CanReceive == true && cubeSlot.IsBlocked == false)
        {
          cubeSlot.Place(this);

          _boxCollider.enabled = false;

          _isDragging = false;
          _isPlaced = true;
        }
      }
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
        Vector3 newPosition = new Vector3(worldPosition.x, worldPosition.y, _cubeStack.transform.position.z) + _offset;

        if (newPosition.y < _initPosition.y)
          newPosition.y = _initPosition.y;

        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

        transform.DOLocalMoveZ(newPosition.z, 0.1f);
        transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.1f);
      }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      _isDragging = false;

      if (_isPlaced == false)
      {
        transform.DOMove(_initPosition, 0.2f).OnComplete(() =>
        {
          _boxCollider.enabled = true;
        });
        transform.DORotateQuaternion(_initRotation, 0.2f);
      }
    }
  }
}