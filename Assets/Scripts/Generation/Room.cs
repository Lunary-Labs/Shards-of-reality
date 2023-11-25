using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
  [SerializeField] private bool north;
  [SerializeField] private bool east;
  [SerializeField] private bool south;
  [SerializeField] private bool west;

  private string roomType;
  private Vector2Int position;

  private Room northNeighbor;
  private Room eastNeighbor;
  private Room southNeighbor;
  private Room westNeighbor;

  // Constructor
  public void Initialize(string roomType, Vector2Int position) {
    this.roomType = roomType;
    this.position = position;
    this.transform.name = roomType + " " + position.x + ", " + position.y;
  }

  // Getters / Setters
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
