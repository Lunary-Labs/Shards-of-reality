using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class FloorGenerator : MonoBehaviour {
  private Vector2Int startPosition = Vector2Int.zero;

  [SerializeField]
  private int iterations = 4;

  [SerializeField]
  private int walkLenght = 12;

  [SerializeField]
  private int distanceBetweenIslands = 50;

  [SerializeField]
  private Vector2Int offset = new Vector2Int(25, -25);

  [SerializeField]
  private bool startRandomly = true;

  void Start() {
    createFloor(-1);
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Space)) {
      destroyFloor();
      createFloor(-1);
    }
  }

  private void createFloor(int seed) {
    // Set seed
    if (seed == -1) {
      Random.InitState(System.DateTime.Now.Millisecond);
    } else {
      Random.InitState(seed);
    }

    // Create island positions list.
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

    // Place special rooms.
    // TODO: Determine if theses random coordinates need to be removed from the global room positions list.
    //       Will probably cause problems when linking rooms together.
    var treasureRoomPosition = twoNeighborPosition.ElementAt(Random.Range(0, twoNeighborPosition.Count));
    twoNeighborPosition.Remove(treasureRoomPosition);

    // This room cause a bug if there is not at least one neighbor room.
    // TODO: Find a way to fix this.
    var bossRoomPosition = oneNeighborPosition.ElementAt(Random.Range(0, oneNeighborPosition.Count));
    oneNeighborPosition.Remove(bossRoomPosition);

    var shopRoomPosition = twoNeighborPosition.ElementAt(Random.Range(0, twoNeighborPosition.Count));
    twoNeighborPosition.Remove(shopRoomPosition);

    var spawnRoomPosition = maxNeighborPosition;

    // Instantiate islands.
    foreach (var position in positions) {
      var newIsland = new GameObject();
      newIsland.transform.SetParent(transform);
      Vector2Int normalPosition = new Vector2Int(2, 3);
      newIsland.transform.localPosition = new Vector3((position[0] - position[1]) * distanceBetweenIslands,
                                                      -(position[0] + position[1]) * distanceBetweenIslands,
                                                      0);
      // TODO: Rework and refactor to make this beautiful.
      if (position == treasureRoomPosition) {
        newIsland.name = "Treasure Room" + position[0] + " " + position[1];
      } else if (position == bossRoomPosition) {
        newIsland.name = "Boss Room" + position[0] + " " + position[1];
      } else if (position == shopRoomPosition) {
        newIsland.name = "Shop Room" + position[0] + " " + position[1];
      } else if (position == spawnRoomPosition) {
        newIsland.name = "Spawn Room" + position[0] + " " + position[1];
      } else {
        newIsland.name = "Island " + position[0] + " " + position[1];
      }
      var islandGenerator = newIsland.AddComponent<IslandGenerator>();
    }
  }

  private void createBasicIsland(Vector2Int position) {
    var newIsland = new GameObject();
    newIsland.transform.SetParent(transform);
    Vector2Int normalPosition = new Vector2Int(2, 3);
    newIsland.transform.localPosition = new Vector3((position[0] - position[1]) * distanceBetweenIslands,
                                                    -(position[0] + position[1]) * distanceBetweenIslands,
                                                    0);
    newIsland.name = "Island " + position[0] + " " + position[1];
    var islandGenerator = newIsland.AddComponent<IslandGenerator>();
  }

  private void createTreasureIsland(Vector2Int position) {
    //TODO
  }

  private void createBossIsland(Vector2Int position) {
    //TODO
  }

  private void createShopIsland(Vector2Int position) {
    //TODO
  }

  private void createSpawnIsland(Vector2Int position) {
    //TODO
  }

  private void destroyFloor() {
    foreach (Transform child in transform) {
      Destroy(child.gameObject);
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
