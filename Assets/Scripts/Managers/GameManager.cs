using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {
    protected GameManager() { }

    public GameObject prefabTileObject;
    public GameBoard activeGameBoard;

    public int boardSize = 2;

    public float delayTillFlip = 1f;

    public GameState ActiveState {
        get; private set;
    }

    public void Start() {
        ActiveState = GameState.Cinematic;
        activeGameBoard.GenerateBoard(5, 5);
        StartCoroutine("PopulateGameBoard");
    }

    private IEnumerator PopulateGameBoard() {
        for (int i = 0; i < activeGameBoard.slots.GetLength(0); i++) {
            var tile = Instantiate(prefabTileObject).GetComponent<Tile>();
            tile.SetText("" + (char)((int)('A') + i));
            activeGameBoard.SetTile(i, 0, tile);
            tile.Drop(activeGameBoard.slots[i, 0].transform);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(delayTillFlip);

        for (int i = 0; i < activeGameBoard.slots.GetLength(0); i++) {
            activeGameBoard.slots[i, 0].SlotTile.FlipDown();
            yield return new WaitForSeconds(0.2f);
        }

        ActiveState = GameState.Active;
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