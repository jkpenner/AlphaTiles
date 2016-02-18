using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : Singleton<GameManager> {
    protected GameManager() { }

    public GameObject prefabTileObject;

    public GameBoard gameBoardStart;
    public GameBoard gameBoardResult;

    public int boardSize = 2;

    public float delayTillFlip = 1f;

    public GameState ActiveState {
        get; private set;
    }

    public void Start() {
        ActiveState = GameState.Cinematic;
        gameBoardStart.GenerateBoard(boardSize, 1);
        gameBoardResult.GenerateBoard(boardSize, 1);
        gameBoardResult.OnGameBoardFilled += OnResultFilled;
        StartCoroutine("PopulateGameBoard");
    }

    private IEnumerator PopulateGameBoard() {
        var letters = GenerateRandomLetters(gameBoardStart.slots.GetLength(0));

        for (int i = 0; i < gameBoardStart.slots.GetLength(0); i++) {
            var tile = Instantiate(prefabTileObject).GetComponent<Tile>();
            tile.SetText(letters[i]);
            gameBoardStart.SetTile(i, 0, tile);
            tile.Drop(gameBoardStart.slots[i, 0].transform);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(delayTillFlip);

        for (int i = 0; i < gameBoardStart.slots.GetLength(0); i++) {
            gameBoardStart.slots[i, 0].SlotTile.FlipDown();
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

    public string[] GenerateRandomLetters(int count) {
        List<int> values = new List<int>();
        while(values.Count < count) {
            var num = Random.Range(65, 91);
            if (!values.Contains(num)) {
                values.Add(num);
            }
        }

        var letters = new string[count];
        for (int i = 0; i < count; i++) {
            letters[i] = "" + (char)values[i];
        }
        return letters;
    }

    public void OnResultFilled() {
        ActiveState = GameState.Cinematic;
        StartCoroutine("RevealTileResult");
    }

    private IEnumerator RevealTileResult() {
        string letters = "";
        for (int i = 0; i < gameBoardResult.slots.GetLength(0); i++) {
            gameBoardResult.slots[i, 0].SlotTile.FlipUp();
            letters += gameBoardResult.slots[i, 0].SlotTile.textmesh.text;
            yield return new WaitForSeconds(0.2f);
        }

        if (IsAlphabetical(letters)) {
            Debug.Log("Correct");
        } else {
            Debug.Log("Not Correct");
        }
    }

    private bool IsAlphabetical(string letters) {
        return letters.SequenceEqual(letters.OrderBy(c => c));
    }
}