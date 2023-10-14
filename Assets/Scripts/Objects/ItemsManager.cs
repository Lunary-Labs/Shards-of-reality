using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour {
  // Global item list. Index of an item corresponds to its ID.
  private List<Item> itemList = new List<Item>();

  // Item pools.
  private List<Item> ancientPool = new List<Item>();
  private List<Item> artifactPool = new List<Item>();
  private List<Item> blessingPool = new List<Item>();
  private List<Item> maledictionPool = new List<Item>();

  // Getters/Setters
  public Item GetItem(int index) { return itemList[index]; }
  public Item GetRandomItem() { return itemList[Random.Range(0, itemList.Count)]; }
  public Item GetRandomAncient() { return ancientPool[Random.Range(0, ancientPool.Count)]; }
  public Item GetRandomArtifact() { return artifactPool[Random.Range(0, artifactPool.Count)]; }
  public Item GetRandomBlessing() { return blessingPool[Random.Range(0, blessingPool.Count)]; }
  public Item GetRandomMalediction() { return maledictionPool[Random.Range(0, maledictionPool.Count)]; }

  void Start() {
    // Fill the items lists. (Load all the scripts in Items folders)
  }
}
