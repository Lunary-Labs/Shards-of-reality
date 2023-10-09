using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room2 : MonoBehaviour {
  [SerializeField] private bool north;
  [SerializeField] private bool south;
  [SerializeField] private bool east;
  [SerializeField] private bool west;

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

  public bool[] getNeighbors() {
    return new bool[] {north, north, east, west};
  }
}
