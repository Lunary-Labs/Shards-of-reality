using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room2 : MonoBehaviour {
  [SerializeField] private bool north;
  [SerializeField] private bool east;
  [SerializeField] private bool south;
  [SerializeField] private bool west;

  private string roomType;
  private Vector2Int position;

  private Room2 northNeighbor;
  private Room2 eastNeighbor;
  private Room2 southNeighbor;
  private Room2 westNeighbor;

  public void Initialize(string roomType, Vector2Int position) {
    this.roomType = roomType;
    this.position = position;
    this.transform.name = roomType + " " + position.x + ", " + position.y;

    // temp for visual debugging.
    if (roomType == "spawn") {
      GetComponent<SpriteRenderer>().color = Color.green;
    } else if (roomType == "boss") {
      GetComponent<SpriteRenderer>().color = Color.red;
    } else if (roomType == "treasure") {
      GetComponent<SpriteRenderer>().color = Color.yellow;
    } else if (roomType == "shop") {
      GetComponent<SpriteRenderer>().color = Color.blue;
    }
  }

  public bool North {
    get { return north; }
    set { north = value; }
  }

  public bool South {
    get { return south; }
    set { south = value; }
  }

  public bool East {
    get { return east; }
    set { east = value; }
  }

  public bool West {
    get { return west; }
    set { west = value; }
  }

  public bool[] GetNeighbors() {
    return new bool[] {north, east, south, west};
  }
}
