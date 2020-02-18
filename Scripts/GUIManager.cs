//Belong to:
//Version:0.1.0
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey;
using CodeMonkey.Utils;

public class GUIManager : MonoBehaviour {
    public enum States//状态
    {
        Init,
        MainScreen,
        SelectSaves,
        SelectLevels,
        InGame,
        Pause,
        Gameover,
        Outfit
    }

    public static List<GameObject> UIlayers;
    public static List<GameObject> shipGUIs;
    public static GameObject[] outfitTurrets;//当前的所有炮塔
    //各个UI界面
    static GameObject mainScreen;
    static GameObject saveScreen;
    static GameObject levelScreen;
    static GameObject gameScreen;
    static GameObject pauseScreen;
    static GameObject outfitScreen;
    static GameObject gameoverScreen;
    //界面下组件
    static GameObject outfitShip;//显示的船
    public static GameObject showTurret;//显示的炮塔
    public static GameObject leftButton;//左键
    public static GameObject rightButton;//右键
    public static GameObject PlayerScore;//都是玩家分数
    public static GameObject GameoverScore;//游戏结束界面的玩家分数
    private static int playerShipType;//当前显示的船型号
    static int[] shipOutfitState;//正在编辑的船的状态
    

    internal static void ShowGUI(States Need)//显示特定层的UI接口
    {
        ShowUICom(Need);
    }

    void Awake()
    {
        UIlayers = new List<GameObject>();
        outfitTurrets = null;
        showTurret = null;
    }

    // Use this for initialization
    void Start () {
        //获取各个UI层
        mainScreen = GameObject.Find("MainScreen");
        saveScreen = GameObject.Find("SaveScreen");
        levelScreen = GameObject.Find("LevelScreen");
        gameScreen = GameObject.Find("GameScreen");
        pauseScreen = GameObject.Find("PauseScreen");
        outfitScreen = GameObject.Find("OutfitScreen");
        gameoverScreen = GameObject.Find("GameoverScreen");

        leftButton = GameObject.Find("LeftButton");
        rightButton = GameObject.Find("RightButton");
        GameoverScore = GameObject.Find("ScorePanel");
        PlayerScore = GameObject.Find("ScoreShow");
        //添加到
        UIlayers.Add(mainScreen);
        UIlayers.Add(saveScreen);
        UIlayers.Add(levelScreen);
        UIlayers.Add(gameScreen);
        UIlayers.Add(pauseScreen);
        UIlayers.Add(outfitScreen);
        UIlayers.Add(gameoverScreen);

        playerShipType = 0;
        

        ShowUICom(States.MainScreen);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private static void ShowUICom(States Need)//显示UI组件的私有方法
    {
        foreach (GameObject screen in UIlayers)
        {
            if (screen.activeSelf)
            {
                screen.SetActive(false);
            }
        }
        switch (Need)
        {
            case States.MainScreen:
                mainScreen.SetActive(true);
                GameData.gameState = States.MainScreen;
                break;
            case States.SelectSaves:
                saveScreen.SetActive(true);
                GameData.gameState = States.SelectSaves;
                break;
            case States.SelectLevels:
                levelScreen.SetActive(true);
                GameData.gameState = States.SelectLevels;
                break;
            case States.InGame:
                //Time.timeScale = 1.0f;
                gameScreen.SetActive(true);
                GameData.gameState = States.InGame;
                break;
            case States.Pause:
                gameScreen.SetActive(true);
                pauseScreen.SetActive(true);
                GameData.gameState = States.Pause;
                //Time.timeScale = 0f;
                break;
            case States.Gameover:
                GameData.gameState = States.Gameover;
                gameScreen.SetActive(true);
                gameoverScreen.SetActive(true);
                break;
            case States.Outfit:
                GameData.gameState = States.Outfit;
                PlayerScore.GetComponent<Text>().text = "SCORE:"+GameData.save.Score;
                outfitScreen.SetActive(true);//激活装配界面
                //先隐藏左右按键
                leftButton.SetActive(false);
                rightButton.SetActive(false);
                //显示玩家当前的配置
                outfitShip = GameData.CreateShip(GameData.save.Type, "Outfit", GameData.save.Turrets);
                outfitShip.transform.parent = outfitScreen.transform;
                outfitShip.transform.position = new Vector3(-40, 21);
                outfitTurrets = GameObject.FindGameObjectsWithTag("Turret");
                playerShipType = GameData.save.Type;
                shipOutfitState = new int[2] { -1, -1 };//前一个是安装炮塔的序号，后一个是安装炮塔的类型
                break;
            default:
                break;
        }
    }
    public void CleanOutfitScreen()//退出装配界面时清除所有对象，保存save文件
    {
        Destroy(outfitShip);
        if(showTurret!=null)
        {
            Destroy(showTurret);
        }
        if (outfitTurrets != null)
        {
            foreach (GameObject go in outfitTurrets)
            {
                Destroy(go);
            }
            outfitTurrets = null;
        }
        shipOutfitState = new int[2] { -1, -1 };
        TestSave.s.SetSave();
    }
    public void NextOutfitShip()//下一个按键
    {
        if(GameData.save.Score/300<playerShipType)
        {
            return;
        }
        Destroy(outfitShip);
        Destroy(showTurret);
        //TestSave.s.SetSave();
        if (outfitTurrets != null)
        {
            foreach(GameObject go in outfitTurrets)
            {
                Destroy(go);
            }
            outfitTurrets = null;
        }
        playerShipType = playerShipType > 4 ? 5 : ++playerShipType;
        outfitShip = GameData.CreateShip(playerShipType, "Outfit");
        outfitShip.transform.parent = outfitScreen.transform;
        outfitShip.transform.position = new Vector3(-40, 21);
        outfitTurrets = GameObject.FindGameObjectsWithTag("Turret");
        //outfitTurrets = FlushNullInArray(outfitTurrets);
        foreach(GameObject go in outfitTurrets)
        {
            go.transform.parent = null;
        }
        GameData.save.Type = playerShipType;
        GameData.save.Turrets = new List<int>();
        foreach(GameObject gp in outfitTurrets)
        {
            while(GameData.save.Turrets.Count<gp.GetComponent<OutfitTurretsCom>().seq+1)
            {
                GameData.save.Turrets.Add(0);
            }
            GameData.save.Turrets[gp.GetComponent<OutfitTurretsCom>().seq] = gp.GetComponent<OutfitTurretsCom>().type;
        }
    }
    public void PrevOutfitShip()//上一个按键
    {
        //删除现有的船和显示的炮塔
        Destroy(outfitShip);
        Destroy(showTurret);
        //TestSave.s.SetSave();
        //删除所有的炮塔组件
        if (outfitTurrets != null)
        {
            foreach (GameObject go in outfitTurrets)
            {
                Destroy(go);
            }
            outfitTurrets = null;
        }
        //边界控制
        playerShipType = playerShipType < 1 ? 0 : --playerShipType;
        //根据目前的船型号再创建一个
        outfitShip = GameData.CreateShip(playerShipType, "Outfit");
        outfitShip.transform.parent = outfitScreen.transform;
        outfitShip.transform.position = new Vector3(-40, 21);
        //把炮塔集合起来
        outfitTurrets = GameObject.FindGameObjectsWithTag("Turret");
        //outfitTurrets = FlushNullInArray(outfitTurrets);
        //取消船的父对象，这样OnMouseDown()才生效
        foreach (GameObject go in outfitTurrets)
        {
            go.transform.parent = null;
        }
        GameData.save.Type = playerShipType;
        GameData.save.Turrets = new List<int>();
        foreach (GameObject gp in outfitTurrets)
        {
            while (GameData.save.Turrets.Count < gp.GetComponent<OutfitTurretsCom>().seq + 1)
            {
                GameData.save.Turrets.Add(0);
            }
            GameData.save.Turrets[gp.GetComponent<OutfitTurretsCom>().seq] = gp.GetComponent<OutfitTurretsCom>().type;
        }
    }
    public void PrevTurret()
    {
        if (shipOutfitState[0] != -1 && shipOutfitState[1] != -1)//如果玩家已经正确选择了炮塔和类型
        {
            if(shipOutfitState[1]>GameData.d.GetTurretSize(playerShipType,shipOutfitState[0]))//型号数字>0
            {
                shipOutfitState[1]--;//可以-1
            }
            if(shipOutfitState[1]<GameData.d.GetTurretSize(playerShipType,shipOutfitState[0]))
            {
                shipOutfitState[1] = GameData.d.GetTurretSize(playerShipType, shipOutfitState[0]);
            }
            if (shipOutfitState[1] > GameData.d.GetTurretSize(playerShipType, shipOutfitState[0]) + 2)
            {
                shipOutfitState[1] = GameData.d.GetTurretSize(playerShipType, shipOutfitState[0]) + 2;
            }
            if (playerShipType==GameData.save.Type)
            {
                while (GameData.save.Turrets.Count < shipOutfitState[0] + 1)
                {
                    GameData.save.Turrets.Add(0);
                }
                GameData.save.Turrets[shipOutfitState[0]] = shipOutfitState[1];//存储
            }
            //清除之前的显示炮塔
            if (GUIManager.showTurret != null)
            {
                Destroy(GUIManager.showTurret);
            }
            //显示炮塔改成自己
            GUIManager.showTurret = GameData.CreateTurret(shipOutfitState[1], shipOutfitState[0],"Outfit");
            GUIManager.showTurret.transform.position = new Vector3(40, 21);
            GUIManager.showTurret.transform.localScale = new Vector3(10, 10);
            //改变显示的炮塔贴图
            outfitTurrets[shipOutfitState[0]].GetComponent<SpriteRenderer>().sprite = GameData.d.GetTurretSpriteFromNumber(shipOutfitState[1]);

        }
    }
    public void NextTurret()
    {
        if (shipOutfitState[0] != -1 && shipOutfitState[1] != -1)//如果玩家已经正确选择了炮塔和类型
        {
            if (shipOutfitState[1] < GameData.d.GetTurretSize(playerShipType, shipOutfitState[0]) + 2)
            {
                shipOutfitState[1]++;
            }
            if (shipOutfitState[1] < GameData.d.GetTurretSize(playerShipType, shipOutfitState[0]))
            {
                shipOutfitState[1] = GameData.d.GetTurretSize(playerShipType, shipOutfitState[0]) + 2;
            }
            if (shipOutfitState[1] > GameData.d.GetTurretSize(playerShipType, shipOutfitState[0]) + 2)
            {
                shipOutfitState[1] = GameData.d.GetTurretSize(playerShipType, shipOutfitState[0]);
            }
            if (playerShipType == GameData.save.Type)
            {
                while (GameData.save.Turrets.Count < shipOutfitState[0] + 1)
                {
                    GameData.save.Turrets.Add(0);
                }
                GameData.save.Turrets[shipOutfitState[0]] = shipOutfitState[1];//存储
            }
            //清除之前的显示炮塔
            if (GUIManager.showTurret != null)
            {
                Destroy(GUIManager.showTurret);
            }
            //显示炮塔改成自己
            GUIManager.showTurret = GameData.CreateTurret(shipOutfitState[1], shipOutfitState[0],"Outfit");
            GUIManager.showTurret.transform.position = new Vector3(40, 21);
            GUIManager.showTurret.transform.localScale = new Vector3(10, 10);
            outfitTurrets[shipOutfitState[0]].GetComponent<SpriteRenderer>().sprite = GameData.d.GetTurretSpriteFromNumber(shipOutfitState[1]);

        }
    }

    public static void ChangeShipOutfitState(int seq,int type)
    {
        //改写数据
        shipOutfitState[0] = seq;shipOutfitState[1] = type;
        //显示切换按键
        leftButton.SetActive(true);
        rightButton.SetActive(true);
    }
    public static GameObject[] FlushNullInArray(GameObject[] a)
    {
        List<GameObject> list = new List<GameObject>();
        foreach(GameObject go in a)
        {
            if(go!=null)
            {
                list.Add(go);
            }
        }
        GameObject[] result = list.ToArray();
        return result;
    }
    public static void GameoverScoreChange(int score)
    {
        GameoverScore.GetComponent<Text>().text = "SCORE+"+score.ToString();
    }
}
