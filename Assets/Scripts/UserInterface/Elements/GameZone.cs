using UnityEngine;
using System.Collections;
using System;

public class GameZone : GameInterfaceElement {
    public GameBoard start;
    public GameBoard result;

    public override bool IsElementVisible(GameState state) {
        return state == GameState.Active ||
            state == GameState.Checking ||
            state == GameState.GameOver ||
            state == GameState.GameWin ||
            state == GameState.Popuplating;
    }

    public override void OnElementVisible(GameState state) {
        
    }

    public override void OnVisible() {
        base.OnVisible();
        start.gameObject.SetActive(true);
        result.gameObject.SetActive(true);
    }

    public override void OnHidden() {
        base.OnHidden();
        start.gameObject.SetActive(false);
        result.gameObject.SetActive(false);
    }

    void Start () {
	
	}

}
