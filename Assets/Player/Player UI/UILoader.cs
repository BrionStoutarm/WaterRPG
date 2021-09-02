using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoader : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //can load different menus or whatnot here

        //basic UI
        if (Input.GetKeyDown(KeyCode.F1)) {
            if(SceneManager.GetSceneByName("UI").isLoaded == false) {
                SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
            }
            else {
                SceneManager.UnloadSceneAsync("UI");
            }
        }
    }
}
