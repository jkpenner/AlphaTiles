using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameBoardManager : Singleton<GameBoardManager> {
    public GameObject prefabTileObject;

    public GameBoard gameBoardStart;
    public GameBoard gameBoardResult;

    public int boardSize = 2;
    public int boardSizeMin = 2;
    public int boardSizeMax = 13;

    public float delayTillFlip = 1f;

    public bool IsGenerating { get; set; }

    static public int BoardSize {
        get {
            if (Instance != null) {
                return Instance.boardSize;
            }
            return 0;
        }
    }

    static public int BoardSizeMin {
        get {
            if (Instance != null) {
                return Instance.boardSizeMin;
            }
            return 0;
        }
    }

    static public int BoardSizeMax {
        get {
            if (Instance != null) {
                return Instance.boardSizeMax;
            }
            return 0;
        }
    }

    private void Awake() {
        gameBoardResult.OnGameBoardFilled += OnResultFilled;
    }

    public void ModifyBoardSize(int amount) {
        SetBoardSize(boardSize + amount);
    }

    public void SetBoardSize(int value) {
        boardSize = value;
        if (boardSize < boardSizeMin) {
            boardSize = boardSizeMin;
        } else if (boardSize > boardSizeMax) {
            boardSize = boardSizeMax;
        }
        Debug.Log(string.Format("[{0}] Setting Board Size to {1}", this.name, boardSize));
    }

    public void GenerateBoard(int size) {
        SetBoardSize(size);

        Debug.Log(string.Format("[{0}] Ganerating Board", this.name));
        IsGenerating = true;
        GameManager.SetState(GameState.Popuplating);
        gameBoardStart.GenerateBoard(boardSize, 1);
        gameBoardResult.GenerateBoard(boardSize, 1);

        StartCoroutine("PopulateGameBoard");
    }

    // Will Generate the Board with the current boardSize
    public void GenerateBoard() {
        GenerateBoard(boardSize);
    }

    // Populates the Gameboard with delayed spawning and animations
    private IEnumerator PopulateGameBoard() {
        var letters = GenerateRandomLetters(gameBoardStart.slots.GetLength(0));

        for (int i = 0; i < gameBoardStart.slots.GetLength(0); i++) {
            var tile = CreateTile();
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
        Debug.Log(string.Format("[{0}] Ganerating Board Complete", this.name));
        GameManager.SetState(GameState.Active);
        IsGenerating = false;
    }

    public Tile CreateTile() {
        if (prefabTileObject != null) {
            var go = GameObject.Instantiate(prefabTileObject);
            var tile = go.GetComponent<Tile>();
            if (tile == null) {
                Debug.LogError(string.Format("[{0}] Tile Object Prefab does not have component 'Tile'", this.name));
            }
            return tile;
        }
        return null;
    }

    // Creates a set of letters with no matching letters
    public string[] GenerateRandomLetters(int count) {
        List<int> values = new List<int>();
        while (values.Count < count) {
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
        Debug.Log(string.Format("[{0}] Reveiling Results", this.name));
        GameManager.SetState(GameState.Checking);
        StartCoroutine("RevealTileResult");
    }

    private IEnumerator RevealTileResult() {
        string letters = "";
        for (int i = 0; i < gameBoardResult.slots.GetLength(0); i++) {
            letters += gameBoardResult.slots[i, 0].SlotTile.textmesh.text;
        }
        bool isCorrect = IsAlphabetical(letters);
        if (isCorrect) {
            ScoreManager.Instance.AddCorrect(BoardSize);
        } else {
            ScoreManager.Instance.AddInCorrect(BoardSize);
        }

        // Flip all tiles up
        for (int i = 0; i < gameBoardResult.slots.GetLength(0); i++) {
            gameBoardResult.slots[i, 0].SlotTile.FlipUp();
            yield return new WaitForSeconds(0.2f);
        }

        Debug.Log(string.Format("[{0}] Reveiling Results Complete", this.name));
        if (isCorrect) {
            GameManager.SetState(GameState.GameWin);
        } else {
            GameManager.SetState(GameState.GameOver);
        }
    }

    private bool IsAlphabetical(string letters) {
        return letters.SequenceEqual(letters.OrderBy(c => c));
    }
}
