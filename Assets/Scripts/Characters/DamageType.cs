using UnityEngine;
using UnityEngine.UI;

namespace UnityPlatformer.Characters {
  public enum DamageTypes {
    DEFAULT,
    PHYSICAL,
    MAGICAL,
    FIRE,
    WATER,
    ELECTRICAL,
    POISON,
    SHADOW
    // Choose your pain!
  };

  [RequireComponent (typeof (Rigidbody2D))]
  public class DamageType : MonoBehaviour {

    [Comment("Damage amount")]
    public int amount = 1;
    public DamageTypes type = DamageTypes.DEFAULT;
    // TODO Causer / Owner

    [HideInInspector]
    // direction will be calc
    public Vector3 direction;
  }
}