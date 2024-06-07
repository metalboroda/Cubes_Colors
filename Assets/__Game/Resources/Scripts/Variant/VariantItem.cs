using System;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Variant
{
  [Serializable]
  public class VariantItem
  {
    [field: SerializeField] public Sprite VariantSprite { get; private set; }
    [field: SerializeField] public bool ShowSprite = true;
  }
}