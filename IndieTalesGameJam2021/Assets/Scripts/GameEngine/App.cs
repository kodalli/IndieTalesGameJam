using System;
using UnityEngine;


public static class App {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap() {
        var app = UnityEngine.Object.Instantiate(Resources.Load("App")) as GameObject;
        if (app == null) {
            throw new ApplicationException();
        }

        app.name = "App";

        UnityEngine.Object.DontDestroyOnLoad(app);
    }
}