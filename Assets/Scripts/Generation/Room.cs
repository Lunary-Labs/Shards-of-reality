using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class Room : MonoBehaviour {
  // Room variables.
  private Vector2Int position;
  private string type;
  private int form; // Utility ???

  // Generation variables.
  private Vector2Int startPosition = Vector2Int.zero;
  private int iterations = 500;
  private int walkLenght = 20;
  private bool startRandomly = true;
  private Vector2 offset = new Vector2(0.5f, 0.25f);

  // Constructors
  public void Initialize(Vector2Int position, string type, int form) {
    this.position = position;
    this.type = type;
    this.form = form;
  }

  // Getters / Setters
  public Vector2Int getPosition() { return this.position; }
  public string getType() { return this.type; }
  public int getForm() { return this.form; }

  public void setPosition(Vector2Int position) { this.position = position; }
  public void setType(string type) { this.type = type; }
  public void setForm(int form) { this.form = form; }

  public override string ToString() {
    return "Room: " + position + " " + type + " " + form;
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

    foreach (var position in positions) {
      var newTile = PrefabUtility.InstantiatePrefab(Tileset.getTile(Tiles.Basic)) as GameObject;
      newTile.transform.SetParent(transform);
      newTile.transform.localPosition = new Vector3((position[0] - position[1]) * offset.x,
                                                   -(position[0] + position[1]) * offset.y,
                                                   -(position[0] + position[1]) / (size.x + size.y));
      newTile.name += " (" + position[0] + ", " + position[1] + ")";
    }
  }
}
