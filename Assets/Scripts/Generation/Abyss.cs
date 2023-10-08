using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : MonoBehaviour {
  // Abyss variables
  private int floorAmount = 7;
  private List<Floor> floors = new List<Floor>();

  // Generation variables
  private int seed = -1;
  private int offsetBetweenFloors = 750;

  void Start() {
    if(seed == -1) {
      this.seed = System.DateTime.Now.Millisecond;
    }

    Random.InitState(this.seed);
    GenerateAbyss();
  }

  // Getters / Setters
  public int GetFloorAmount() { return this.floorAmount; }
  public List<Floor> GetFloors() { return this.floors; }
  public int GetSeed() { return this.seed; }

  public void SetFloorAmount(int floorAmount) { this.floorAmount = floorAmount; }
  public void SetFloors(List<Floor> floors) { this.floors = floors; }
  public void SetSeed(int seed) { this.seed = seed; }

  // Cleaners
  public void DestroyAbyss() { Destroy(this.gameObject); }

  // Generation
  public void GenerateAbyss() {
    for(int i = 1; i <= this.floorAmount; i++) {
      GameObject newFloor = new GameObject("Floor " + i);
      newFloor.transform.parent = this.transform;
      newFloor.transform.position = new Vector3(offsetBetweenFloors * i, 0, 0);
      Floor floor = newFloor.AddComponent<Floor>();
      floor.Initialize(i, "Floor " + i);
      this.floors.Add(floor);
      floor.GenerateFloor();
    }
  }
}
