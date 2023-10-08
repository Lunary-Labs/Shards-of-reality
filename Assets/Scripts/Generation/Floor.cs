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
  public int GetFloorId() { return floorId; }
  public string GetFloorName() { return floorName; }

  public void SetFloorId(int floorId) { this.floorId = floorId; }
  public void SetFloorName(string floorName) { this.floorName = floorName; }

  public override string ToString() {
    return "Floor: " + floorId + " " + floorName;
  }

  // Cleaners
  public void DestroyFloor() { Destroy(this.gameObject); }

  public void GenerateFloor() {
    // Create room positions list.
    HashSet<Vector2Int> positions = new HashSet<Vector2Int>();
    positions.Add(startPosition);
    var currentPosition = startPosition;
    while (positions.Count < GetRoomsAmount()) {
      HashSet<Vector2Int> path = new HashSet<Vector2Int>();
      path.Add(startPosition);
      currentPosition = startPosition;
      for (int j = 0; j < walkLenght; j++) {
        var newPosition = currentPosition + Direction2D.getRandomCardinalDirection();
        path.Add(newPosition);
        currentPosition = newPosition;
        positions.UnionWith(path);
        if(positions.Count >= GetRoomsAmount()) {
          break;
        }
      }
      if(startRandomly) {
        currentPosition = positions.ElementAt(Random.Range(0, positions.Count));
      }
    }

    // Place special rooms.
    HashSet<Vector2Int> temp_positions = new HashSet<Vector2Int>(positions);
    var spawnRoomPosition = new Vector2Int();
    int max = 0;
    foreach (var position in temp_positions) {
      if (GetNeighborsAmount(position, temp_positions) > max) {
        spawnRoomPosition = position;
      }
    }

    temp_positions.Remove(spawnRoomPosition);
    var treasureRoomPosition = temp_positions.ElementAt(Random.Range(0, temp_positions.Count));
    temp_positions.Remove(treasureRoomPosition);
    var bossRoomPosition = temp_positions.ElementAt(Random.Range(0, temp_positions.Count));
    temp_positions.Remove(bossRoomPosition);
    var shopRoomPosition = temp_positions.ElementAt(Random.Range(0, temp_positions.Count));
    temp_positions.Remove(shopRoomPosition);

    foreach(Vector2Int position in positions) {
      var str = "";
      if (position == treasureRoomPosition) {
        str = "Treasure";
      } else if (position == bossRoomPosition) {
        str = "Boss";
      } else if (position == shopRoomPosition) {
        str = "Shop";
      } else if (position == spawnRoomPosition) {
        str = "Spawn";
      } else {
        str = "Basic";
      }
      GameObject newRoom = new GameObject(str + " room " + position);
      newRoom.transform.parent = this.transform;
      newRoom.transform.localPosition = new Vector3((position[0] - position[1]) * distanceBetweenRooms,
                                                    -(position[0] + position[1]) * distanceBetweenRooms,
                                                    0);
      Room room = newRoom.AddComponent<Room>();
      room.Initialize(position, str, this);
      this.rooms.Add(room);
      room.GenerateRoom();
    }
  }

  private int GetNeighborsAmount(Vector2Int position, HashSet<Vector2Int> positions) {
    int amount = 0;
    foreach (var direction in Direction2D.cardinalDirectionInt) {
      if (positions.Contains(position + direction)) {
        amount++;
      }
    }
    return amount;
  }

  private int GetRoomsAmount() {
    return this.floorId * 3 + 8;
  }
}
