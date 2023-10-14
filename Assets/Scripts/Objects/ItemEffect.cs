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

  public void skin() {}

  public void permanent() {}
  public void permanentEffect() {}
  public void onFire() {}
  public void onHit() {}
  public void onKill() {}
  public void onDeath() {}
  public void onPickup() {}
}