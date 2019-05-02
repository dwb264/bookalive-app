using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExitScene : MonoBehaviour
{
    public Button exitButton;
    public Button resetButton;

    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(Exit);
        resetButton.onClick.AddListener(Reset);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Exit()
    {
        Debug.Log("Click");
        SceneManager.LoadScene(sceneName: "Menu");
    }

    public void Reset()
    {
        GameObject[] cars = GameObject.FindGameObjectsWithTag("CarMaterial");
        foreach (var item in cars)
        {
            Destroy(item);
        }
        GameObject.Find("Ratio").GetComponent<TextMesh>().text = "0 : 0";
        ArrowRotation a = GameObject.Find("Arrow").GetComponent<ArrowRotation>();
        a.ratio_left = 0;
        a.ratio_right = 0;
        
    }
}
