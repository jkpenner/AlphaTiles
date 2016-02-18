using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : Singleton<ScoreManager> {
    public class DifficultyInfo {
        public int Correct { get; set; }
        public int Incorrect { get; set; }
    }

    private Dictionary<int, DifficultyInfo> _difficultyInfos;
    public Dictionary<int, DifficultyInfo> DifficultyInfos {
        get {
            if (_difficultyInfos == null) {
                _difficultyInfos = new Dictionary<int, DifficultyInfo>();
            }
            return _difficultyInfos;
        }
    }

    public void AddCorrect(int difficulty) {
        if (!DifficultyInfos.ContainsKey(difficulty)) {
            DifficultyInfos.Add(difficulty, new DifficultyInfo());
            DifficultyInfos[difficulty].Correct = 0;
            DifficultyInfos[difficulty].Incorrect = 0;
        }
        DifficultyInfos[difficulty].Correct++;
    }

    public void AddInCorrect(int difficulty) {
        if (!DifficultyInfos.ContainsKey(difficulty)) {
            DifficultyInfos.Add(difficulty, new DifficultyInfo());
            DifficultyInfos[difficulty].Correct = 0;
            DifficultyInfos[difficulty].Incorrect = 0;
        }
        DifficultyInfos[difficulty].Incorrect++;
    }

    public int GetCorrect(int difficulty) {
        if (DifficultyInfos.ContainsKey(difficulty)) {
            return DifficultyInfos[difficulty].Correct;
        }
        return 0;
    }

    public int GetInCorrect(int difficulty) {
        if (DifficultyInfos.ContainsKey(difficulty)) {
            return DifficultyInfos[difficulty].Incorrect;
        }
        return 0;
    }
}
