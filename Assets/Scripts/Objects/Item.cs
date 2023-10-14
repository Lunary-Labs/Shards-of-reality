using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ItemEffect {
  // Item properties
  private int id;
  private string itemName;
  private int quality;
  private string pool;
  private string description;

  // Getters/Setters
  public int GetId() { return id; }
  public string GetName() { return itemName; }
  public int GetQuality() { return quality; }
  public string GetPool() { return pool; }
  public string GetDescription() { return description; }
}
