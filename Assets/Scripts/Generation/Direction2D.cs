using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions {
  North,
  East,
  South,
  West
}

public static class Direction2D {
  public static List<Vector2Int> cardinalDirectionInt = new List<Vector2Int> {
    new Vector2Int(0, -1),   // North
    new Vector2Int(1, 0),   // East
    new Vector2Int(0, 1),  // South
    new Vector2Int(-1, 0)   // West
  };

  public static Vector2Int getRandomCardinalDirection() {
    return cardinalDirectionInt[Random.Range(0, cardinalDirectionInt.Count)];
  }

  public static Vector2Int getCardinalDirection(Directions direction) {
    return cardinalDirectionInt[(int)direction];
  }
}


