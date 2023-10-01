using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Floor : MonoBehaviour {
  // Floor variables
  private int floorId;
  private string floorName;

  private List<Room> rooms = new List<Room>();

  // Generation variables
  private Vector2Int startPosition = Vector2Int.zero;
  private int iterations = 4;
  private int walkLenght = 12;
  private int distanceBetweenRooms = 50;
  private Vector2Int offset = new Vector2Int(25, -25);
  private bool startRandomly = true;

  // Constructors
  public void Initialize(int floorId, string floorName) {
    this.floorId = floorId;
    this.floorName = floorName;
  }

  // Getters / Setters
  public int getFloorId() { return floorId; }
  public string getFloorName() { return floorName; }

  public void setFloorId(int floorId) { this.floorId = floorId; }
  public void setFloorName(string floorName) { this.floorName = floorName; }

  public override string ToString() {
    return "Floor: " + floorId + " " + floorName;
  }

  // Cleaners
  public void destroyFloor() { Destroy(this.gameObject); }

  public void generateFloor() {
    // Create room positions list.
    HashSet<Vector2Int> positions = new HashSet<Vector2Int>();
    positions.Add(startPosition);
    var currentPosition = startPosition;
    for (int i = 0; i < iterations; i++) {
      HashSet<Vector2Int> path = new HashSet<Vector2Int>();
      path.Add(startPosition);
      currentPosition = startPosition;
      for (int j = 0; j < walkLenght; j++) {
        var newPosition = currentPosition + Direction2D.getRandomCardinalDirection();
        path.Add(newPosition);
        currentPosition = newPosition;
      }
      positions.UnionWith(path);
      if(startRandomly) {
        currentPosition = positions.ElementAt(Random.Range(0, positions.Count));
      }
    }

    // Find special rooms possible positions.
    var oneNeighborPosition = new HashSet<Vector2Int>();
    var twoNeighborPosition = new HashSet<Vector2Int>();
    var maxNeighborPosition = new Vector2Int();
    int max = 0;
    foreach (var position in positions) {
      if (getNeighborsAmount(position, positions) == 1) {
        oneNeighborPosition.Add(position);
      } else if (getNeighborsAmount(position, positions) == 2) {
        twoNeighborPosition.Add(position);
      }
      if (getNeighborsAmount(position, positions) > max) {
        maxNeighborPosition = position;
      }
    }

    // // Place special rooms.
    // // TODO: Determine if theses random coordinates need to be removed from the global room positions list.
    // //       Will probably cause problems when linking rooms together.
    // var treasureRoomPosition = twoNeighborPosition.ElementAt(Random.Range(0, twoNeighborPosition.Count));
    // twoNeighborPosition.Remove(treasureRoomPosition);
    // // This room cause a bug if there is not at least one neighbor room.
    // // TODO: Find a way to fix this.
    // var bossRoomPosition = oneNeighborPosition.ElementAt(Random.Range(0, oneNeighborPosition.Count));
    // oneNeighborPosition.Remove(bossRoomPosition);
    // var shopRoomPosition = twoNeighborPosition.ElementAt(Random.Range(0, twoNeighborPosition.Count));
    // twoNeighborPosition.Remove(shopRoomPosition);
    // var spawnRoomPosition = maxNeighborPosition;

    foreach(Vector2Int position in positions) {
      GameObject newRoom = new GameObject("Room " + position);
      newRoom.transform.parent = this.transform;
      newRoom.transform.localPosition = new Vector3((position[0] - position[1]) * distanceBetweenRooms,
                                                    -(position[0] + position[1]) * distanceBetweenRooms,
                                                    0);
      Room room = newRoom.AddComponent<Room>();
      room.Initialize(position, "Room " + position, 0);
      this.rooms.Add(room);
      room.generateRoom();
    }

  }

  private int getNeighborsAmount(Vector2Int position, HashSet<Vector2Int> positions) {
    int amount = 0;
    foreach (var direction in Direction2D.cardinalDirectionInt) {
      if (positions.Contains(position + direction)) {
        amount++;
      }
    }
    return amount;
  }
}
