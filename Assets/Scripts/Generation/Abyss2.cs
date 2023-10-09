using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss2 : MonoBehaviour {
  // Abyss variables
  private int floorAmount = 7;
  private List<Floor2> floors = new List<Floor2>();

  // Generation variables
  private int seed = -1;
  private int offsetBetweenFloors = 75;

  void Start() {
    if(seed == -1) {
      this.seed = System.DateTime.Now.Millisecond;
    }

    Random.InitState(this.seed);
    GenerateAbyss();
  }

  void Update() {
    if(Input.GetKeyDown(KeyCode.R)) {
      RandomizeSeed();
      CleanAbyss();
      GenerateAbyss();
    }
  }

  // Getters / Setters
  public int GetFloorAmount() { return this.floorAmount; }
  public List<Floor2> GetFloors() { return this.floors; }
  public Floor2 GetFloor(int index) { return this.floors[index]; }
  public int GetSeed() { return this.seed; }

  public void SetFloorAmount(int floorAmount) { this.floorAmount = floorAmount; }
  public void SetFloors(List<Floor2> floors) { this.floors = floors; }
  public void SetFloor(int index, Floor2 floor) { this.floors[index] = floor; }
  public void SetSeed(int seed) { this.seed = seed; }

  // Cleaners
  public void DestroyAbyss() { Destroy(this.gameObject); }
  public void CleanAbyss() {
    foreach(Transform child in this.transform) { Destroy(child.gameObject); }
    foreach(Floor2 floor in this.floors) { UnityEngine.Object.Destroy(floor.gameObject); }
  }

  public void RandomizeSeed() {
    this.seed = System.DateTime.Now.Millisecond;
    Random.InitState(this.seed);
  }

  // Generation
  public void GenerateAbyss() {
    for(int i = 1; i <= this.floorAmount; i++) {
      GameObject newFloor = new GameObject("Floor " + i);
      newFloor.transform.parent = this.transform;
      newFloor.transform.localPosition = new Vector3(offsetBetweenFloors * (i - 1), 0, 0);
      Floor2 floor = newFloor.AddComponent<Floor2>();
      floor.Initialize(i, "Floor " + i);
      this.floors.Add(floor);
      floor.GenerateFloor();
    }
  }
}
