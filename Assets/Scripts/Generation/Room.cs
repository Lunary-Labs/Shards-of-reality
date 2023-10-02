using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class Room : MonoBehaviour {
  // Room variables.
  private Vector2Int position;
  private string type;

  // Generation variables.
  private Vector2Int startPosition = Vector2Int.zero;
  private int iterations = 500;
  private int walkLenght = 20;
  private bool startRandomly = true;
  private Vector2 offset = new Vector2(0.5f, 0.25f);

  // Constructors
  public void Initialize(Vector2Int position, string type) {
    this.position = position;
    this.type = type;
  }

  // Getters / Setters
  public Vector2Int getPosition() { return this.position; }
  public string getType() { return this.type; }

  // Return the coresponding tileset for the room type.
  // TODO: Refactor, this is ugly.
  // Later, staying with this will make a really long function.
  public Tilesets getTileset() {
    return Tilesets.Basic;
    // switch (this.type) {
    //   case "Spawn": return Tilesets.Spawn;
    //   case "Boss": return Tilesets.Boss;
    //   case "Shop": return Tilesets.Shop;
    //   case "Treasure": return Tilesets.Treasure;
    //   default: return Tilesets.Basic;
    // }
  }

  public void setPosition(Vector2Int position) { this.position = position; }
  public void setType(string type) { this.type = type; }

  public override string ToString() {
    return "Room: " + position + " " + type;
  }

  public bool[] getTileNeighbors(Vector2Int position, HashSet<Vector2Int> positions) {
    bool[] neighbors = new bool[4];
    neighbors[(int)Directions.North] = positions.Contains(position + Direction2D.getCardinalDirection(Directions.North));
    neighbors[(int)Directions.East] = positions.Contains(position + Direction2D.getCardinalDirection(Directions.East));
    neighbors[(int)Directions.South] = positions.Contains(position + Direction2D.getCardinalDirection(Directions.South));
    neighbors[(int)Directions.West] = positions.Contains(position + Direction2D.getCardinalDirection(Directions.West));
    return neighbors;
  }

  public Tiles getCorrespondingTile(bool[] neighbors) {
    if (neighbors.SequenceEqual(new bool[] { false, true, true, true })) { return Tiles.BorderTop; }
    else if (neighbors.SequenceEqual(new bool[] { true, false, true, true})) { return Tiles.BorderRight; }
    else if (neighbors.SequenceEqual(new bool[] { true, true, false, true})) { return Tiles.BorderBottom; }
    else if (neighbors.SequenceEqual(new bool[] { true, true, true, false})) { return Tiles.BorderLeft; }
    else if (neighbors.SequenceEqual(new bool[] { true, true, false, false})) { return Tiles.CornerBottomLeft; }
    else if (neighbors.SequenceEqual(new bool[] { true, false, false, true})) { return Tiles.CornerBottomRight; }
    else if (neighbors.SequenceEqual(new bool[] { false, true, true, false})) { return Tiles.CornerTopLeft; }
    else if (neighbors.SequenceEqual(new bool[] { false, false, true, true})) { return Tiles.CornerTopRight; }
    else { return Tiles.Basic; }
  }

  // Cleaners
  public void destroyRoom() { Destroy(this.gameObject); }

  // Generation
  public void generateRoom() {
    HashSet<Vector2Int> positions = new HashSet<Vector2Int>();
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
      positions.UnionWith(path);
      if(startRandomly) {
        currentPosition = positions.ElementAt(Random.Range(0, positions.Count));
      }
    }

    Vector2 size = new Vector2(positions.Max(x => x.x) - positions.Min(x => x.x) + 1,
                               positions.Max(x => x.y) - positions.Min(x => x.y) + 1);

    var tileset = getTileset();
    foreach (var position in positions) {
      var neighbors = getTileNeighbors(position, positions);
      var tile = getCorrespondingTile(neighbors);
      var newTile = PrefabUtility.InstantiatePrefab(Tileset.getTile(tileset, tile)) as GameObject;
      newTile.transform.SetParent(transform);
      newTile.transform.localPosition = new Vector3((position[0] - position[1]) * offset.x,
                                                   -(position[0] + position[1]) * offset.y,
                                                   -(position[0] + position[1]) / (size.x + size.y));
      newTile.name += " (" + position[0] + ", " + position[1] + ")";
    }
  }
}
