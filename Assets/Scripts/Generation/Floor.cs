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
  private Vector2 offset = new Vector2(4.0f, 4.0f);

  // Constructors
  public void Initialize(int floorId, string floorName) {
    this.floorId = floorId;
    this.floorName = floorName;
  }

  public HashSet<Vector2Int> GetPositions() {
    HashSet<Vector2Int> positions = new HashSet<Vector2Int>();
    HashSet<Vector2Int> queue = new HashSet<Vector2Int>();
    int deadEndsAmount = 0;
    int roomsAmount = GetRoomsAmount();
    queue.Add(Vector2Int.zero);
    while (positions.Count < roomsAmount) {
      var position = queue.ElementAt(Random.Range(0, queue.Count));
      queue.Remove(position);
      positions.Add(position);
      bool deadEnd = true;
      foreach (var direction in Direction2D.cardinalDirectionInt) {
        var newPosition = position + direction;
        if (!positions.Contains(newPosition) && GetNeighborsAmount(newPosition, positions) < 2) {
          queue.Add(newPosition);
          deadEnd = false;
        }
      }
      if (deadEnd) { deadEndsAmount++; }
    }
    return positions;
  }

  public void GenerateFloor() {
    Debug.Log("Generating floor " + this.floorId + " (" + this.floorName + ")");
    HashSet<Vector2Int> positions = GetPositions();
    int deadEndsAmount = 0;
    foreach (var position in positions) {
      if (GetNeighborsAmount(position, positions) == 1) {
        deadEndsAmount++;
      }
    }
    while (deadEndsAmount < 6) {
      positions = addDeadEnd(positions);
      deadEndsAmount++;
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

    // Two foreach so the first rooms of the list are 1 neighbors (priority) and the last are 2 neighbors (less priority
    List<Vector2Int> specialPossiblePosition = new List<Vector2Int>();
    foreach(var position in tempPositions) {
      if(GetNeighborsAmount(position, positions) == 1) {
        specialPossiblePosition.Add(position);
      }
    }

    foreach(var position in tempPositions) {
      if (GetNeighborsAmount(position, positions) == 2) {
        // add if the room doesn't have north souht or east west neighbors
        var neighbors = GetPositionNeighbors(position, positions);
        if(neighbors[(int)Directions.North] != neighbors[(int)Directions.South] && neighbors[(int)Directions.East] != neighbors[(int)Directions.West]) {
          specialPossiblePosition.Add(position);
        }
      }
    }

    // Determine boss room position.
    // Find the most distant room from the spawn room that have only one neighbor.
    var maxDistance = 0;
    var bossRoomPosition = new Vector2Int();
    foreach(var position in specialPossiblePosition) {
      var distance = Mathf.Abs(position.x - spawnRoomPosition.x) + Mathf.Abs(position.y - spawnRoomPosition.y);
      if(distance > maxDistance && GetNeighborsAmount(position, positions) == 1) {
        maxDistance = distance;
        bossRoomPosition = position;
      }
    }
    specialPossiblePosition.Remove(bossRoomPosition);

    // Determine special rooms positions.
    var maledictionRoomPosition = specialPossiblePosition[0];
    specialPossiblePosition.Remove(maledictionRoomPosition);
    var blessingRoomPosition = specialPossiblePosition[0];
    specialPossiblePosition.Remove(blessingRoomPosition);
    var treasureRoomPosition = specialPossiblePosition[0];
    specialPossiblePosition.Remove(treasureRoomPosition);
    var shopRoomPosition = specialPossiblePosition[0];
    specialPossiblePosition.Remove(shopRoomPosition);

    // Create rooms.
    foreach (var position in positions) {
      // Determine room type.
      string roomType;
      if(position == treasureRoomPosition) {
        roomType = "Treasure";
      } else if(position == shopRoomPosition) {
        roomType = "Shop";
      } else if(position == bossRoomPosition) {
        roomType = "Boss";
      } else if(position == spawnRoomPosition) {
        roomType = "Spawn";
      } else if(position == maledictionRoomPosition) {
        roomType = "Malediction";
      } else if(position == blessingRoomPosition) {
        roomType = "Blessing";
      } else {
        roomType = "Basic";
      }

      var neighborsAmount = GetNeighborsAmount(position, positions);
      var neighbors = GetPositionNeighbors(position, positions);
      string path = "Prefabs/Rooms/" + roomType + "/" + neighborsAmount + "/";
      GameObject[] possibleRooms = Resources.LoadAll<GameObject>(path);
      List<GameObject> validRooms = new List<GameObject>();
      foreach(var r in possibleRooms) {
        if (neighbors.SequenceEqual(r.GetComponent<Room>().GetNeighbors())) {
          validRooms.Add(r);
        }
      }

      // TODO: Remove this debug log.
      Debug.Log(roomType + neighborsAmount);

      // Instantiate a random room from the valid rooms list.
      GameObject roomPrefab = validRooms[Random.Range(0, validRooms.Count)];
      var room = Instantiate(roomPrefab);
      room.transform.parent = this.transform;
      room.transform.localPosition = new Vector3((position[0] - position[1]) * offset[0],
                                                0,
                                                -(position[0] + position[1])  * offset[1]);
      room.transform.localRotation = Quaternion.Euler(0, 45, 0);
      room.name = roomType + " " + position.x + ", " + position.y;

      rooms.Add(room);
      room.GetComponent<Room>().Initialize(roomType, position);
    }
  }

  private HashSet<Vector2Int> addDeadEnd(HashSet<Vector2Int> positions) {
    Debug.Log("Adding dead end");
    bool added = false;
    foreach (var position in GetPositions()) {
      if (added) { break; }
      if (GetNeighborsAmount(position, positions) == 4) {
        continue;
      } else {
        // test each neighbor, if this neighbor would have 1 neighbor, add it to the list
        foreach (var direction in Direction2D.cardinalDirectionInt) {
          if (GetNeighborsAmount(position + direction, positions) == 1) {
            positions.Add(position + direction);
            added = true;
            break;
          }
        }
      }
    }
    return positions;
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
