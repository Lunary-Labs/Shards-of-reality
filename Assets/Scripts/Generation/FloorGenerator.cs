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

  void Start() {
    createFloor();
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Space)) {
      destroyFloor();
      createFloor();
    }
  }

  private void createFloor() {
    // Create island positions list
    HashSet<Vector2Int> positions = new HashSet<Vector2Int>();
    positions.Add(startPosition);
    var previousPosition = startPosition;
    for (int i = 0; i < iterations; i++) {
      HashSet<Vector2Int> path = new HashSet<Vector2Int>();
      path.Add(startPosition);
      previousPosition = startPosition;
      for (int j = 0; j < walkLenght; j++) {
        var newPosition = previousPosition + Direction2D.getRandomCardinalDirection();
        path.Add(newPosition);
        previousPosition = newPosition;
      }
      positions.UnionWith(path);
    }

    // Instantiate islands
    foreach (var position in positions) {
      var newIsland = new GameObject();
      newIsland.transform.SetParent(transform);
      newIsland.transform.localPosition = new Vector3(position[0] * distanceBetweenIslands, position[1] * distanceBetweenIslands, 0);
      newIsland.name = "Island " + position[0] + " " + position[1];
      var islandGenerator = newIsland.AddComponent<IslandGenerator>();
    }
  }

  private void destroyFloor() {
    foreach (Transform child in transform) {
      Destroy(child.gameObject);
    }
  }
}
