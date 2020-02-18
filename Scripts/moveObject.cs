//Belong to:
//Version:0.1.0
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveObject : MonoBehaviour {
    [Header("目标物体")]
    public GameObject TargetBody;
    [Header("自身")]
    public GameObject SelfBody;
    [Header("按钮移动目的地")]
    public GUIManager.States Target;
    public static moveObject m;
    // Use this for initialization
    void Start () {
        m = this;
	}
	
	// Update is called once per frame
	void Update () {

	}
    public void Clicked(){
        string name = this.name;
        GUIManager.ShowGUI(Target);
        
    }

    public void ResumeGame()
    {
        GUIManager.ShowGUI(GUIManager.States.InGame);
        Time.timeScale = 1f;
        foreach(GameObject go in GameProcess.ships)
        {
            go.GetComponent<RocketMove>().PauseShip();
        }
    }

    public static void PauseGame()
    {
        GUIManager.ShowGUI(GUIManager.States.Pause);
        Time.timeScale = 0f;
        foreach (GameObject go in GameProcess.ships)
        {
            go.GetComponent<RocketMove>().ResumeShip();
        }
    }
}
