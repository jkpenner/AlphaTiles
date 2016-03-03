using UnityEngine;
using System.Collections;
using GameUp;
using Facebook.Unity;

public class GameUpTest : MonoBehaviour {
	void Start () {
	    Client.ApiKey = "2f6d21d4a60247a0af58a87ff6ba1966";

        Client.Ping((status, reason) => {
            Debug.LogFormat("Could not connect to API got '{0}' with '{1}'.", status, reason);
        });

        if (!FB.IsInitialized) {
            FB.Init(() => {
                if (FB.IsInitialized) {
                    FB.ActivateApp();
                    var accessToken = AccessToken.CurrentAccessToken.TokenString;
                    Client.LoginOAuthFacebook(accessToken, (SessionClient sessionClient) => {
                        // You now have a SessionClient with a linked Facebook account.
                    }, (status, reason) => {
                        Debug.LogErrorFormat("Could not login to Facebook got '{0}'", reason);
                    });
                }
            });
        }
    }
}
