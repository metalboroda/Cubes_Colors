using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.Character;
using System.Collections;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Train
{
  public class TrainAnimationHandler : MonoBehaviour
  {
    [Header("Train")]
    [SerializeField] private float _trainMovementDuration;
    [SerializeField] private Vector3 _trainFirstPoint;
    [SerializeField] private Vector3 _trainSecondPoint;
    [Header("")]
    [SerializeField] private CharacterAnimationHandler _characterAnimationHandler;

    private EventBinding<EventStructs.WinEvent> _winEvent;

    private void OnEnable()
    {
      _winEvent = new EventBinding<EventStructs.WinEvent>(MoveTrainToSecondPoint);
    }

    private void OnDisable()
    {
      _winEvent.Remove(MoveTrainToSecondPoint);
    }

    private void OnDestroy()
    {
      StopAllCoroutines();
    }

    private void Start()
    {
      MoveTrainToFirstPoint();
    }

    private void MoveTrainToFirstPoint()
    {
      EventBus<EventStructs.TrainMovementEvent>.Raise(new EventStructs.TrainMovementEvent { IsMoving = true });

      StartCoroutine(MoveTrain(transform.localPosition, _trainFirstPoint, _trainMovementDuration));

      _characterAnimationHandler.PlayPushMovementAnimation();
    }

    private void MoveTrainToSecondPoint(EventStructs.WinEvent winEvent)
    {
      EventBus<EventStructs.TrainMovementEvent>.Raise(new EventStructs.TrainMovementEvent { IsMoving = true });

      StartCoroutine(MoveTrain(transform.localPosition, _trainSecondPoint, _trainMovementDuration * 1.5f));

      _characterAnimationHandler.PlayPushMovementAnimation();
    }

    private IEnumerator MoveTrain(Vector3 start, Vector3 end, float duration)
    {
      float elapsed = 0f;

      while (elapsed < duration)
      {
        transform.localPosition = Vector3.Lerp(start, end, elapsed / duration);
        elapsed += Time.deltaTime;

        yield return null;
      }

      transform.localPosition = end;

      EventBus<EventStructs.TrainMovementEvent>.Raise(new EventStructs.TrainMovementEvent { IsMoving = false });

      _characterAnimationHandler.PlayIdleAnimation();
    }
  }
}