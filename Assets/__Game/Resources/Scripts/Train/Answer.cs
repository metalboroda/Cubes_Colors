using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.__Game.Resources.Scripts.Train
{
  public class Answer : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
  {
    [SerializeField] public Image _image;
    [Header("Books")]
    [SerializeField] private bool _spawnBook = false;
    [SerializeField] private GameObject[] _booksToSpawn;

    public Sprite AnswerSprite { get; private set; }

    private Vector3 _initLocalPosition;
    private Vector3 _offset;
    private bool _placed = false;

    private Camera _mainCamera;
    private BoxCollider _boxCollider;
    private float _zDistanceToCamera;

    void Awake()
    {
      _mainCamera = Camera.main;
      _boxCollider = GetComponent<BoxCollider>();

      _initLocalPosition = transform.localPosition;
    }

    private void Start()
    {
      SetSpriteAndImage(_image.sprite);

      if (_spawnBook == true)
        Instantiate(_booksToSpawn[Random.Range(0, _booksToSpawn.Length)], transform);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (_placed) return;

      if (other.TryGetComponent(out Variant.Variant variant))
      {
        if (variant.ShowSprite == false)
        {
          variant.Place(this);

          _placed = true;
        }
      }
    }

    public void DisableVisual()
    {
      _boxCollider.enabled = false;

      _image.gameObject.SetActive(false);
    }

    public void SetSpriteAndImage(Sprite sprite)
    {
      AnswerSprite = sprite;
      _image.sprite = AnswerSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(
        new Vector3(eventData.position.x, eventData.position.y, _mainCamera.WorldToScreenPoint(transform.position).z));

      _offset = transform.position - worldPosition;
      _zDistanceToCamera = _mainCamera.WorldToScreenPoint(transform.position).z;
    }

    public void OnDrag(PointerEventData eventData)
    {
      if (_placed == true) return;

      Vector3 screenPosition = new Vector3(eventData.position.x, eventData.position.y, _zDistanceToCamera);
      Vector3 newPosition = _mainCamera.ScreenToWorldPoint(screenPosition) + _offset;

      transform.position = newPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      if (_placed == true) return;

      transform.DOLocalMove(_initLocalPosition, 0.25f);
    }
  }
}