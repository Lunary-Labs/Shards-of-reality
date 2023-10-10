using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemEffectType {
  Health,
  DamageFlat,
  DamageMultiplier,
  ArmorFlat,
  ArmorMultiplier,
  ProjectileSpeedFlat,
  ProjectileSpeedMultiplier,
  CritChanceFlat,
  CritChanceMultiplier,
  CritDamageFlat,
  CritDamageMultiplier,
  FireRateFlat,
  FireRateMultiplier,
  ProjectileAmount,
  Luck,
  Speed,
  Money
}

public class ItemEffect : MonoBehaviour {
  private List<int> effects;

  // Getters / Setters.
  public int GetEffect(ItemEffectType type) {
    return effects[(int)type];
  }

// dans l'idée :
// on a des items avec différents effets
// exemple :
//   - les projectiles suivent l'enemi avec un certain item
//   - les projectiles se séparent en 3 à l'impact
//   - les projectiles explosent au contact

// donc 3 items qui appliquent chacun leur effets (pas forcément un seul effet par item)
// comment faire pour que le projectile prenne ces effets sans passer par des conditions interminables(avec 10 items on se retrouve avec un tres long code par ex) ?

// j'ai vu des binds (connait pas trop)

// dans des jeux comme isaac, la quasi totalité des items ont des synergies et des effets qui se cumulent.
// certaines sont harcodées mais on croit que certaines se cumullent toutes seules.


// sinon je pensais aussi faire les fonctions d'en dessous, et pour certains effets combinés qui doivent etre hardodés 
// il faudrait retirer les deux effets et en ajouter un qui les "override"


  public void skin() {}

  public void permanent() {}
  public void permanentEffect() {}
  public void onFire() {}
  public void onHit() {}
  public void onKill() {}
  public void onDeath() {}
  public void onPickup() {}
}