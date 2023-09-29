using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


