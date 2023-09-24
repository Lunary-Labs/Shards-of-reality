using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class island_generator : MonoBehaviour {
  public GameObject basic_tile;
  public Vector2 size;
  public Vector2 offset;

  void Start() {
    generate_island();
  }

  void generate_island()  {
    destroy_island();
    for (int i = 0; i < size.x; i++) {
      for (int j = 0; j < size.y; j++) {
        var newTile = PrefabUtility.InstantiatePrefab(basic_tile) as GameObject;
        newTile.transform.SetParent(transform);
        newTile.transform.localPosition = new Vector3((i - j) * offset.x, -(i + j) * offset.y, (i + j) / (size.x + size.y));
        newTile.name += " (" + i + ", " + j + ")";
      }
    }
  }

  void destroy_island() {
    for (int i = transform.childCount - 1; i >= 0; i--) {
      DestroyImmediate(transform.GetChild(i).gameObject);
    }
  }
}
