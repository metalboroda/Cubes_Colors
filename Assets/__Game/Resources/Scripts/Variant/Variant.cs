using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.Train;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.__Game.Resources.Scripts.Variant
{
  public class Variant : MonoBehaviour
  {
    [SerializeField] private Image _variantImage;
    [SerializeField] private GameObject _unknownTextObject;
    [Space]
    [SerializeField] private Color _transparentColor;
    [Header("Effects")]
    [SerializeField] private GameObject _correctParticlesPrefab;
    [SerializeField] private GameObject _incorrectParticlesPrefab;

    public bool ShowSprite { get; private set; }
    public Sprite VariantSprite { get; private set; }
    public Sprite ReceivedAnswer { get; private set; }

    private void OnDestroy()
    {
      DOTween.Kill(transform);
    }

    public void SetSpriteAndImage(Sprite variantSprite, bool showSprite)
    {
      _variantImage.sprite = null;
      VariantSprite = variantSprite;
      ShowSprite = showSprite;

      if (showSprite == true)
      {
        _variantImage.sprite = VariantSprite;
        _unknownTextObject.SetActive(false);
      }
      else
      {
        _variantImage.color = _transparentColor;
        _unknownTextObject.SetActive(true);
      }
    }

    public void Place(Answer answerToPlace)
    {
      answerToPlace.transform.DOMove(transform.position, 0.1f)
        .OnComplete(() =>
        {
          answerToPlace.transform.SetParent(transform);

          CheckForCorrectAnswer();
        });

      ShowSprite = true;
      ReceivedAnswer = answerToPlace.AnswerSprite;
      _unknownTextObject.SetActive(false);
    }

    private void CheckForCorrectAnswer()
    {
      if (VariantSprite == ReceivedAnswer)
      {
        EventBus<EventStructs.CorrectAnswerEvent>.Raise(new EventStructs.CorrectAnswerEvent());

        SpawnParticles(_correctParticlesPrefab);
      }
      else
      {
        EventBus<EventStructs.IncorrectCancelEvent>.Raise(new EventStructs.IncorrectCancelEvent());

        SpawnParticles(_incorrectParticlesPrefab);
      }
    }

    private void SpawnParticles(GameObject particlesPrefab)
    {
      Instantiate(particlesPrefab, transform.position, Quaternion.identity);
    }
  }
}