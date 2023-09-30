using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tiles {
  Basic,
  BorderRight,
  BorderLeft,
  BorderTop,
  BorderBottom,
  CornerTopLeft,
  CornerTopRight,
  CornerBottomLeft,
  CornerBottomRight
}

public class Tileset : MonoBehaviour {
  private static Tileset instance;
  private GameObject[] tiles;

  private void Awake() {
    if (instance == null) {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else {
      Destroy(gameObject);
    }

    tiles = new GameObject[Tiles.GetValues(typeof(Tiles)).Length];
    loadTilesFromResources();
  }

  public void loadTilesFromResources() {
    GameObject[] loadedTiles = Resources.LoadAll<GameObject>("Tiles");
    foreach (GameObject tile in loadedTiles) {
      tiles[(int)tile.GetComponent<Tile>().tileType] = tile;
    }
  }

  public static GameObject getTile(Tiles tileType) {
    if (instance != null && (int)tileType < instance.tiles.Length) {
      return instance.tiles[(int)tileType];
    }
    return null;
  }
}
