using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tiles {
  Basic,

  BorderLeft,
  BorderRight,
  BorderTop,
  BorderBottom,

  CornerTopLeft,
  CornerTopRight,
  CornerBottomLeft,
  CornerBottomRight,

  CorridorLeft,
  CorridorRight,

  EndLeft,
  EndRight,
  EndTop,
  EndBottom
}

public enum Tilesets {
  Basic,
  Boss,
  Shop,
  Spawn,
  Treasure
}

public class Tileset : MonoBehaviour {
  private static Tileset instance;
  private GameObject[][] tilesets;

  private void Awake() {
    if (instance == null) {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else {
      Destroy(gameObject);
    }
    LoadTilesFromResources();
  }

  public void LoadTilesFromResources() {
    Tilesets[] tilesetValues = (Tilesets[])Enum.GetValues(typeof(Tilesets));
    tilesets = new GameObject[tilesetValues.Length][];
    for (int i = 0; i < tilesetValues.Length; i++) {
        // Load all the GameObjects in the tileset folder
        GameObject[] loadedTiles = Resources.LoadAll<GameObject>("Tilesets/" + tilesetValues[i].ToString());

        // Initialize the tileset array
        tilesets[i] = new GameObject[loadedTiles.Length];

        foreach (GameObject tile in loadedTiles) {
          tilesets[i][(int)tile.GetComponent<Tile>().tileType] = tile;
        }
    }
  }

  public static GameObject getTile(Tilesets tileset, Tiles tileType) {
    if (instance != null && (int)tileType < instance.tilesets[(int)tileset].Length) {
      return instance.tilesets[(int)tileset][(int)tileType];
    }
    return null;
  }
}
