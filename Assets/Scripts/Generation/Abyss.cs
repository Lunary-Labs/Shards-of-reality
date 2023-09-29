using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : MonoBehaviour {
  // Abyss variables
  private int floorAmount;
  private List<Floor> floors = new List<Floor>();
  private int seed;

  // Constructors
  public Abyss(int seed, int floorAmount = 7) {
    if(seed == -1) { seed = System.DateTime.Now.Millisecond; }
    else { this.seed = seed; }

    this.floorAmount = floorAmount;

    Random.InitState(this.seed);
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
    for(int i = 0; i < this.floorAmount; i++) {
      Floor floor = new Floor(i, "Floor " + i);
      floor.generateFloor();
      this.floors.Add(floor);
    }
  }
}
