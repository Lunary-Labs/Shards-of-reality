using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Floor : MonoBehaviour {
  // Floor variables
  private int floorId;
  private string floorName;
  [SerializeField]
  private List<GameObject> rooms = new List<GameObject>();

  // Generation variables
  private Vector2Int startPosition = Vector2Int.zero;
  private int walkLenght = 12;
  private Vector2 offset = new Vector2(2.0f, 1.0f);
  private bool startRandomly = true;
  private bool hasSpecialItemRoom;
  private string specialItemRoomType;

  // Constructors
  public void Initialize(int floorId, string floorName) {
    this.floorId = floorId;
    this.floorName = floorName;
    this.hasSpecialItemRoom = UnityEngine.Random.value < 0.5f; // Must depend on player luck and special room chances.
    this.specialItemRoomType = (UnityEngine.Random.value < 0.5f) ? "blessing" : "malediction";  // Must depend on player luck and special room chances.
  }

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
        var newPosition = currentPosition + Direction2D.GetRandomCardinalDirection();
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

    List<Vector2Int> tempPositions = new List<Vector2Int>(positions);

    // Determine special rooms positions.
    var max = 0;
    var spawnRoomPosition = new Vector2Int();
    foreach(var position in positions) {
      var neighborsAmount = GetNeighborsAmount(position, positions);
      if(neighborsAmount > max) {
        max = neighborsAmount;
        spawnRoomPosition = position;
      }
    }
    tempPositions.Remove(spawnRoomPosition);

    // Find the most distant room from the spawn room that have only one neighbor.
    List<Vector2Int> oneNeighborPositions = new List<Vector2Int>();
    foreach(var position in positions) {
      if(GetNeighborsAmount(position, positions) == 1) {
        oneNeighborPositions.Add(position);
      }
    }

    var maxDistance = 0;
    var bossRoomPosition = new Vector2Int();
    foreach(var position in oneNeighborPositions) {
      var distance = Mathf.Abs(position.x - spawnRoomPosition.x) + Mathf.Abs(position.y - spawnRoomPosition.y);
      if(distance > maxDistance) {
        maxDistance = distance;
        bossRoomPosition = position;
      }
    }
    tempPositions.Remove(bossRoomPosition);

    // Place special roooms.
    // TODO: Place them only on 1 neighboor rooms once generation is fixed.
    var treasureRoomPosition = tempPositions[Random.Range(0, tempPositions.Count)];
    tempPositions.Remove(treasureRoomPosition);
    var shopRoomPosition = tempPositions[Random.Range(0, tempPositions.Count)];
    tempPositions.Remove(shopRoomPosition);
    var specialItemRoomPosition = new Vector2Int();
    if (this.hasSpecialItemRoom) {
      specialItemRoomPosition = tempPositions[Random.Range(0, tempPositions.Count)];
      tempPositions.Remove(specialItemRoomPosition);
    }

    // Create rooms.
    foreach (var position in positions) {
      var neighborsAmount = GetNeighborsAmount(position, positions);
      var neighbors = GetPositionNeighbors(position, positions);
      GameObject[] possibleRooms = Resources.LoadAll<GameObject>("Prefabs/Rooms/" + neighborsAmount);
      List<GameObject> validRooms = new List<GameObject>();
      foreach(var r in possibleRooms) {
        if (neighbors.SequenceEqual(r.GetComponent<Room>().GetNeighbors())) {
          validRooms.Add(r);
        }
      }

      // Instantiate a random room from the valid rooms list.
      GameObject roomPrefab = validRooms[Random.Range(0, validRooms.Count)];
      var room = Instantiate(roomPrefab);
      room.transform.parent = this.transform;
      room.transform.localPosition = new Vector3((position[0] - position[1]) * offset[0],
                                                -(position[0] + position[1])  * offset[1],
                                                0);
      string roomType = "";
      if(position == treasureRoomPosition) {
        roomType = "treasure";
      } else if(position == shopRoomPosition) {
        roomType = "shop";
      } else if(position == bossRoomPosition) {
        roomType = "boss";
      } else if(position == spawnRoomPosition) {
        roomType = "spawn";
      } else if(position == specialItemRoomPosition) {
        roomType = specialItemRoomType;
      } else {
        roomType = "basic";
      }
      rooms.Add(room);
      room.GetComponent<Room>().Initialize(roomType, position);
    }
  }

  private bool[] GetPositionNeighbors(Vector2Int position, HashSet<Vector2Int> positions) {
    bool[] neighbors = new bool[4];
    neighbors[(int)Directions.North] = positions.Contains(position + Direction2D.GetCardinalDirection(Directions.North));
    neighbors[(int)Directions.East] = positions.Contains(position + Direction2D.GetCardinalDirection(Directions.East));
    neighbors[(int)Directions.South] = positions.Contains(position + Direction2D.GetCardinalDirection(Directions.South));
    neighbors[(int)Directions.West] = positions.Contains(position + Direction2D.GetCardinalDirection(Directions.West));
    return neighbors;
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
    double baseRooms = this.floorId * 4 + 8;
    double adjustedRooms = baseRooms - Mathf.Log(this.floorId + 1);
    return (int)adjustedRooms;
  }
}
