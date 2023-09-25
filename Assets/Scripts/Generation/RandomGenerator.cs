using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class RandomGenerator : MonoBehaviour {
  [SerializeField]
  private Vector2Int startPosition = Vector2Int.zero;

  [SerializeField]
  private GameObject basicTile;

  [SerializeField]
  private int iterations = 10;

  [SerializeField]
  public int walkLenght = 10;

  [SerializeField]
  public bool startRandomly = true;

  [SerializeField]
  private Vector2 offset = new Vector2(0.5f, 0.25f);

  void Start() {
    createIsland();
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Space)) {
      destroyIsland();
      createIsland();
    }
  }

  // Random walk
  private void createIsland() {
    HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
    var currentPosition = startPosition;

    for (int i = 0; i < iterations; i++) { 
      HashSet<Vector2Int> path = new HashSet<Vector2Int>();
      path.Add(startPosition);
      var previousPosition = startPosition;
      for (int j = 0; j < walkLenght; j++) {
        var newPosition = previousPosition + Direction2D.getRandomCardinalDirection();
        path.Add(newPosition);
        previousPosition = newPosition;
      }
      floorPositions.UnionWith(path);
      if(startRandomly) {
        currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
      }
    }

    Vector2 size = new Vector2(floorPositions.Max(x => x.x) - floorPositions.Min(x => x.x) + 1, 
      floorPositions.Max(x => x.y) - floorPositions.Min(x => x.y) + 1);

    foreach (var position in floorPositions) {
      Debug.Log(position);
      var newTile = PrefabUtility.InstantiatePrefab(basicTile) as GameObject;
      newTile.transform.SetParent(transform);
      newTile.transform.localPosition = new Vector3((position[0] - position[1]) * offset.x,
       -(position[0] + position[1]) * offset.y, 
       -(position[0] + position[1]) / (size.x + size.y));
      newTile.name += " (" + position[0] + ", " + position[1] + ")";
    }
  }

  private void destroyIsland() {
    foreach (Transform child in transform) {
      Destroy(child.gameObject);
    }
  }
}

public static class Direction2D {
  public static List<Vector2Int> cardinalDirectionInt = new List<Vector2Int> {
    new Vector2Int(0, 1),   // UP
    new Vector2Int(1, 0),   // RIGHT
    new Vector2Int(0, -1),  // DOWN
    new Vector2Int(-1, 0)   // LEFT
  }; 

  public static Vector2Int getRandomCardinalDirection() {
    return cardinalDirectionInt[Random.Range(0, cardinalDirectionInt.Count)];
  }
}

