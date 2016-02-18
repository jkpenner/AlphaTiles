using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {
    protected GameManager() { }

    public GameObject prefabTileObject;
    public GameBoard activeGameBoard;

    public int boardSize = 2;

    public void Start() {
        activeGameBoard.GenerateBoard(5, 5);
        for (int x = 0; x < 5; x++) {
            var tile = Instantiate(prefabTileObject).GetComponent<Tile>();
            tile.SetText("" + (char)((int)('A') + x));

            activeGameBoard.SetTile(x, 0, tile);
        }
    }

    public Tile CreateTile() {
        if (prefabTileObject != null) {
            var go = GameObject.Instantiate(prefabTileObject);
            var tile = go.GetComponent<Tile>();
            if (tile == null) {
                Debug.LogError("Tile Object Prefab does not have component 'Tile'");
            }
            return tile;
        }
        return null;
    }
}