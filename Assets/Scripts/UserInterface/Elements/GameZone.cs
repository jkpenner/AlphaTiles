using UnityEngine;
using System.Collections;
using System;

public class GameZone : GameInterfaceElement {
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
    }

    public override void OnHidden() {
        base.OnHidden();
    }

    void Start () {
	
	}

}
