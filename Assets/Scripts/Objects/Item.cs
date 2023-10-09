using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
  // Item properties
  private int id;
  private string itemName;
  private string pool;
  private string description;
  private ItemEffect effect;

  // Getters/Setters
  public int Id {
    get { return id; }
    set { id = value; }
  }

  public string Name {
    get { return name; }
    set { name = value; }
  }

  public string Pool {
    get { return pool; }
    set { pool = value; }
  }

  public string Description {
    get { return description; }
    set { description = value; }
  }
}
