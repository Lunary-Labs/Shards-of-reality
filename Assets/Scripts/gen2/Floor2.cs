using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Floor2 : MonoBehaviour {
  // Floor variables
  private int floorId;
  private string floorName;
  private List<Room2> Rooms = new List<Room2>();

  // Generation variables
  private Vector2Int startPosition = Vector2Int.zero;
  private int walkLenght = 12;
  private Vector2Int distanceBetweenRooms = new Vector2Int(2, 1);
  private Vector2Int offset = new Vector2Int(25, -25);
  private bool startRandomly = true;

  void Start() {
    this.floorId = 1; // TODO: Not hardcode this.
    GenerateFloor();
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

    // Create rooms.
    foreach (var position in positions) {
      var neighborsAmount = GetNeighborsAmount(position, positions);
      var neighbors = GetPositionNeighbors(position, positions);
      // instantiate a random room based on the amount of neighbors.
      GameObject[] possibleRooms = Resources.LoadAll<GameObject>("Prefabs/Rooms/" + neighborsAmount);
      // Find all rooms that have corresponding neigbors directions.
      List<GameObject> validRooms = new List<GameObject>();

      // TODO: fix this check to goes out of bound sometimes.
      foreach(var r in possibleRooms) {
        if (neighbors.SequenceEqual(r.GetComponent<Room2>().getNeighbors())) {
          validRooms.Add(r);
        }
      }

      // Instantiate a random room from the valid rooms.
      GameObject roomPrefab = validRooms[Random.Range(0, validRooms.Count)];
      var room = Instantiate(roomPrefab, new Vector3(position.x * distanceBetweenRooms[0] + offset.x, position.y * distanceBetweenRooms[1] + offset.y, 0), Quaternion.identity);
      room.transform.parent = this.transform;
      room.name = "Room " + position.x + " " + position.y;
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
    return this.floorId * 3 + 8;
  }
}
