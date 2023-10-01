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
    generateAbyss();
  }

  // Getters / Setters
  public int getFloorAmount() { return this.floorAmount; }
  public List<Floor> getFloors() { return this.floors; }
  public int getSeed() { return this.seed; }

  public void setFloorAmount(int floorAmount) { this.floorAmount = floorAmount; }
  public void setFloors(List<Floor> floors) { this.floors = floors; }
  public void setSeed(int seed) { this.seed = seed; }

  // Cleaners
  public void destroyAbyss() { Destroy(this.gameObject); }

  // Generation
  public void generateAbyss() {
    for(int i = 1; i <= this.floorAmount; i++) {
      GameObject newFloor = new GameObject("Floor " + i);
      newFloor.transform.parent = this.transform;
      newFloor.transform.position = new Vector3(offsetBetweenFloors * i, 0, 0);
      Floor floor = newFloor.AddComponent<Floor>();
      floor.Initialize(i, "Floor " + i);
      this.floors.Add(floor);
      floor.generateFloor();
    }
  }
}
