using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

	public void PlayMath () {
        SceneManager.LoadScene(sceneName: "Ratios");
    }

    public void PlayHistory()
    {
        SceneManager.LoadScene(sceneName: "AncientRome");
    }

    public void QuitGame ()
    {
        Debug.Log("QuitGame!");
        Application.Quit();
    }

}
