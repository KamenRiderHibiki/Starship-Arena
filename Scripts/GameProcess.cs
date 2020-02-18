using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class GameProcess : MonoBehaviour {

    public static List<GameObject> ships;
    public static List<GameObject> shells;
    public static List<GameObject> shipGUIs;
    public int level;
	// Use this for initialization
	void Start () {
        ships = shells = shipGUIs = new List<GameObject>();
        level = -1;
	}
	
	// Update is called once per frame
	void Update () {
        if(GameData.gameState==GUIManager.States.InGame||GameData.gameState==GUIManager.States.Pause)
        {
            //实现点击移动
            if (Input.GetMouseButtonDown(0))
            {
                
                //CMDebug.TextPopupMouse(GameData.save.Progress.ToString()+","+level);
            }
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (Time.timeScale > 0.0f)
                {
                    moveObject.PauseGame();
                }
                else
                {
                    moveObject.m.ResumeGame();
                }

            }
            GameData.UpdateShipStatus();
            FlushGameLists(shells);
        }
        if (GameData.gameState == GUIManager.States.Outfit)
        {
            
            if(GUIManager.outfitTurrets!=null)
                GUIManager.outfitTurrets = GUIManager.FlushNullInArray(GUIManager.outfitTurrets);
            //有泄漏就明显报错
            foreach(GameObject i in GUIManager.outfitTurrets)
            {
                if(i==null)
                {
                    CMDebug.TextPopupMouse("!!");
                }
            }
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                GameObject.Find("EventSystem").GetComponent<GUIManager>().CleanOutfitScreen();
                GUIManager.ShowGUI(GUIManager.States.SelectLevels);
            }
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    CMDebug.TextPopupMouse(GameData.gameState.ToString());
        //}
    }

    List<GameObject> FindCurrentShips()
    {
        GameObject[] n;
        List<GameObject> ss = new List<GameObject>();
        n = GameObject.FindGameObjectsWithTag("Ship");
        foreach(GameObject s in n)
        {
            ss.Add(s);
        }
        n = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in n)
        {
            ss.Add(p);
        }
        return ss;
    }

    public void  StartGame(int level)
    {
        GameObject ship1 = GameData.CreateShip(GameData.save.Type, "Player", GameData.save.Turrets);
        ship1.transform.position = new Vector3(20, 0);
        GameObject ship2 = new GameObject();
        switch(level)
        {
            case 0:
                break;
            case 1:
                ship2 = GameData.CreateShip(0, "Ship");
                this.level = 1;
                break;
            case 2:
                ship2 = GameData.CreateShip(1, "Ship");
                this.level = 2;
                break;
            case 3:
                ship2 = GameData.CreateShip(2, "Ship");
                this.level = 3;
                break;
            case 4:
                ship2 = GameData.CreateShip(3, "Ship");
                this.level = 4;
                break;
            case 5:
                ship2 = GameData.CreateShip(4, "Ship");
                this.level = 5;
                break;
            default:
                ship2 = GameData.CreateShip(5, "Ship");
                this.level = 6;
                break;
        }
        ship2.transform.position = new Vector3(-20, 0);
        ships = FindCurrentShips();
        shipGUIs = GameData.d.CreateShipStatus();
    }
    public void Endgame(GameObject source=null)
    {
        if(source!=null)
        {
            if (source.tag == "Player")
            {
                GUIManager.ShowGUI(GUIManager.States.Gameover);
                Time.timeScale = 0.0f;
                int score = 0;
                GUIManager.GameoverScoreChange(score);
                if (source != null)
                {
                    Destroy(source);
                    ships = new List<GameObject>(GUIManager.FlushNullInArray(ships.ToArray()));
                }

            }
            else if (source.tag == "Ship")
            {
                GUIManager.ShowGUI(GUIManager.States.Gameover);
                Time.timeScale = 0.0f;
                int score = 0;
                if (GameData.save.Progress < level)
                {
                    score = (level - GameData.save.Progress) * 300;
                    GameData.save.Progress = level;
                }
                else
                {
                    score = 50;
                }
                GUIManager.GameoverScoreChange(score);
                GameData.save.Score += score;
                if (source != null)
                {
                    Destroy(source);
                    ships = new List<GameObject>(GUIManager.FlushNullInArray(ships.ToArray()));
                }
            }
        }
        else
        {
            SafeCleanLists(ships);
            SafeCleanLists(shells);
            SafeCleanLists(shipGUIs);
            Time.timeScale = 1.0f;
            GUIManager.ShowGUI(GUIManager.States.SelectLevels);
            level = -1;
        }
    }
    public void ButtonEndgame()
    {
        Endgame();
    }
    private void FlushGameLists(List<GameObject> objects)
    {
        foreach(GameObject i in objects)
        {
            if(i==null)
            {
                shells.Remove(i);
                break;
            }
        }
    }

    private void SafeCleanLists(List<GameObject> objects)
    {
        foreach (GameObject g in objects)
        {
            GameObject.Destroy(g);
        }
        ships = new List<GameObject>();
    }
}
