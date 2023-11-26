using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : MonoBehaviour {
  // Abyss variables
  private int floorAmount = 7;
  private List<GameObject> floors = new List<GameObject>();

  // Generation variables
  private int seed = -1;
  private int offsetBetweenFloors = 75;
  private float lastTime;

  void Start() {
    if(seed == -1) {
      this.seed = System.DateTime.Now.Millisecond;
    }

    Random.InitState(this.seed);
    GenerateAbyss();
  }

  void Update() {
    if(Input.GetKeyDown(KeyCode.R)) {
      Debug.Log("Regenerating Abyss.");
      CleanAbyss();
      RandomizeSeed();
      Random.InitState(this.seed);
      GenerateAbyss();
    }
  }

  // Getters / Setters
  public int GetFloorAmount() { return this.floorAmount; }
  public List<GameObject> GetFloors() { return this.floors; }
  public GameObject GetFloor(int index) { return this.floors[index]; }
  public int GetSeed() { return this.seed; }

  public void SetFloorAmount(int floorAmount) { this.floorAmount = floorAmount; }
  public void SetFloors(List<GameObject> floors) { this.floors = floors; }
  public void SetFloor(int index, GameObject floor) { this.floors[index] = floor; }
  public void SetSeed(int seed) { this.seed = seed; }

  // Cleaners
  public void DestroyAbyss() { Destroy(this.gameObject); }
  public void CleanAbyss() {
    foreach(Transform child in this.transform) { Destroy(child.gameObject); }
    floors = new List<GameObject>();
  }

  public void RandomizeSeed() {
    this.seed = System.DateTime.Now.Millisecond;
    Random.InitState(this.seed);
  }

  // Generation
  // TODO: Generate each floor when changing floor because player stats can change generation parameters.
  public void GenerateAbyss() {
    for(int i = 1; i <= this.floorAmount; i++) {
      GameObject newFloor = new GameObject("Floor " + i);
      newFloor.transform.parent = this.transform;
      newFloor.transform.localPosition = new Vector3(offsetBetweenFloors * (i - 1), 0, 0);
      Floor floor = newFloor.AddComponent<Floor>();
      floor.Initialize(i, "Floor " + i);
      this.floors.Add(newFloor);
      floor.GenerateFloor();
    }
  }
}
