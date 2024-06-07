using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Train
{
  public class AnswerContainer : MonoBehaviour
  {
    [SerializeField] private Sprite _sprite;

    private Answer _answer;

    private void Awake()
    {
      _answer = GetComponentInChildren<Answer>();
    }

    private void Start()
    {
      _answer.SetSpriteAndImage(_sprite);
    }
  }
}