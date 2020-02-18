using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public enum shipStates
{
    Live,
    Dead,
    Pause
}
public class GameData : MonoBehaviour {

    public static GameData d;//数据实例
    public static TestSave.SaveClass save;//玩家数据
    public static GUIManager.States gameState;
    public float left, right, up, down;
    public static string savePath;//存档路径
    //默认炮塔位置
    private static Vector3[][] ShipTurretConf;
    //默认炮塔类型
    private static int[][] ShipTurretType;
    private readonly int[] ShipHP = new int[6] { 300, 500, 700, 1000, 1245, 1500 };//船体的初始HP
    [Header("船体0")]
    public Sprite ship_0;
    [Header("船体1")]
    public Sprite ship_1;
    [Header("船体2")]
    public Sprite ship_2;
    [Header("船体3")]
    public Sprite ship_3;
    [Header("船体4")]
    public Sprite ship_4;
    [Header("船体5")]
    public Sprite ship_5;
    [Header("船体6")]
    public Sprite ship_6;
    [Header("小型炮塔1")]
    public Sprite Turret_S_1;
    [Header("小型炮塔2")]
    public Sprite Turret_S_2;
    [Header("小型炮塔3")]
    public Sprite Turret_S_3;
    [Header("中型炮塔1")]
    public Sprite Turret_M_1;
    [Header("中型炮塔2")]
    public Sprite Turret_M_2;
    [Header("中型炮塔3")]
    public Sprite Turret_M_3;
    [Header("大型炮塔1")]
    public Sprite Turret_L_1;
    [Header("大型炮塔2")]
    public Sprite Turret_L_2;
    [Header("大型炮塔3")]
    public Sprite Turret_L_3;
    [Header("动能炮弹1")]
    public Sprite Kinteic_1;
    [Header("动能炮弹2")]
    public Sprite Kinteic_2;
    [Header("机枪子弹1")]
    public Sprite Multiple_Cannon_1;
    [Header("机枪子弹2")]
    public Sprite Multiple_Cannon_2;
    [Header("导弹1")]
    public Sprite Missle_1;
    [Header("导弹2")]
    public Sprite Missle_2;
    [Header("小型炮塔例")]
    public Sprite Turret_E_S;
    [Header("中型炮塔例")]
    public Sprite Turret_E_M;
    [Header("大型炮塔例")]
    public Sprite Turret_E_L;

    public static GameObject player;
    
    private void Awake()
    {
        d = this;
        gameState = GUIManager.States.Init;
    }

    public static GameObject CreateShip(int type = 1,string tag="Ship",List<int> num=null, string name="ship")
    {
        //船基本配置
        GameObject ship = new GameObject();
        ship.tag = tag;
        ship.name = name;
        SpriteRenderer spriteR = ship.AddComponent<SpriteRenderer>();
        //船体贴图
        switch(type)
        {
            case 0:
                spriteR.sprite = d.ship_0;
                break;
            case 1:
                spriteR.sprite = d.ship_1;
                break;
            case 2:
                spriteR.sprite = d.ship_2;
                break;
            case 3:
                spriteR.sprite = d.ship_4;
                break;
            case 4:
                spriteR.sprite = d.ship_5;
                break;
            case 5:
                spriteR.sprite = d.ship_6;
                break;
        }
        spriteR.sortingOrder = 0;
        if(tag=="Player"||tag=="Ship")//区分在游戏还是装配界面
        {
            //安装炮塔
            if (num == null)//加载默认配置
            {
                if (type < ShipTurretConf.Length)//防止越界
                {
                    Vector3[] conf = ShipTurretConf[type];
                    if (conf != null)//火箭没炮塔是null
                    {
                        GameObject[] turrets = new GameObject[conf.Length];
                        for (int i = 0; i < conf.Length; i++)
                        {
                            turrets[i] = CreateTurret(ShipTurretType[type][i]);
                            turrets[i].transform.parent = ship.transform;
                            turrets[i].transform.Translate(conf[i]);
                            if (conf[i][1] < 0)
                            {
                                turrets[i].transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                            }
                        }
                    }
                    else//配置里没有炮塔
                    {
                        
                    }
                }//越界没有else
            }
            else//有客制炮塔
            {
                if (type < ShipTurretConf.Length)//防止越界
                {
                    Vector3[] conf = ShipTurretConf[type];
                    if (conf != null)//火箭没炮塔是null
                    {
                        GameObject[] turrets = new GameObject[conf.Length];
                        for (int i = 0; i < conf.Length; i++)
                        {
                            if(save.Turrets[i]<9)
                            {
                                turrets[i] = CreateTurret(save.Turrets[i]);
                                turrets[i].transform.parent = ship.transform;
                                turrets[i].transform.Translate(conf[i]);
                                if (conf[i][1] < 0)
                                {
                                    turrets[i].transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                                }
                            }
                        }
                    }
                }
            }
            ship.AddComponent<RocketMove>().HP = d.ShipHP[type];
            switch(type)
            {
                case 0:
                    ship.GetComponent<RocketMove>().angle = 45;
                    break;
                default:
                    break;
            }
            ship.transform.localScale = new Vector3(3, 3);
            ship.AddComponent<CapsuleCollider2D>();
            Rigidbody2D r = ship.AddComponent<Rigidbody2D>();
            r.gravityScale = 0.0f;
            r.useAutoMass = true;
        }
        if(tag=="Outfit")//在装配界面
        {
            switch (type)
            {
                case 0:
                    //ship.GetComponent<RocketMove>().angle = 45;
                    break;
                default:
                    break;
            }
            
            if (type < ShipTurretConf.Length)//防止越界
            {

                Vector3[] conf = ShipTurretConf[type];
                if (conf != null)//火箭没炮塔是null
                {
                    GameObject[] turrets = new GameObject[conf.Length];
                    for (int i = 0; i < conf.Length; i++)
                    {
                        //安装炮塔
                        if (num == null)
                        {
                            switch (ShipTurretType[type][i])
                            {
                                case 0://默认炮塔0说明是小炮塔
                                    turrets[i] = CreateTurret(9, 0, "Outfit");
                                    turrets[i].transform.parent = ship.transform;
                                    turrets[i].transform.Translate(conf[i]);
                                    break;
                                case 3://以此类推
                                    turrets[i] = CreateTurret(10, 0, "Outfit");
                                    turrets[i].transform.parent = ship.transform;
                                    turrets[i].transform.Translate(conf[i]);
                                    break;
                                case 6:
                                    turrets[i] = CreateTurret(11, 0, "Outfit");
                                    turrets[i].transform.parent = ship.transform;
                                    turrets[i].transform.Translate(conf[i]);
                                    break;
                                default:
                                    break;
                            }
                            turrets[i].GetComponent<OutfitTurretsCom>().seq = i;
                        }
                        else
                        {
                            turrets[i] = CreateTurret(save.Turrets[i],i,"Outfit");
                            turrets[i].transform.parent = ship.transform;
                            turrets[i].transform.Translate(conf[i]);
                            if (conf[i][1] < 0)
                            {
                                turrets[i].transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                            }
                            turrets[i].GetComponent<OutfitTurretsCom>().seq = i;
                        }
                        
                    }
                }
                else//配置里没有炮塔
                {

                }
            }//越界没有else
            ship.transform.localScale += new Vector3(2, 2);
        }
        return ship;
    }
    public Sprite GetTurretSpriteFromNumber(int num)
    {
        Sprite sp=new Sprite();
        switch (num)
        {
            case 0:
                sp = d.Turret_S_1;
                break;
            case 1:
                sp = d.Turret_S_2;
                break;
            case 2:
                sp = d.Turret_S_3;
                break;
            case 3:
                sp = d.Turret_M_1;
                break;
            case 4:
                sp = d.Turret_M_2;
                break;
            case 5:
                sp = d.Turret_M_3;
                break;
            case 6:
                sp = d.Turret_L_1;
                break;
            case 7:
                sp = d.Turret_L_2;
                break;
            case 8:
                sp = d.Turret_L_3;
                break;
            case 9:
                sp = d.Turret_E_S;
                break;
            case 10:
                sp = d.Turret_E_M;
                break;
            case 11:
                sp = d.Turret_E_L;
                break;

        }
        return sp;
    }
    public static GameObject CreateTurret(int num = 0,int seq = 0,string usetype = "Game")
    {
        GameObject p = new GameObject("Turret",typeof(SpriteRenderer));
        p.tag = "Turret";
        var sprite = p.GetComponent<SpriteRenderer>();
        switch(num)
        {
            case 0:
                sprite.sprite = d.Turret_S_1;
                break;
            case 1:
                sprite.sprite = d.Turret_S_2;
                break;
            case 2:
                sprite.sprite = d.Turret_S_3;
                break;
            case 3:
                sprite.sprite = d.Turret_M_1;
                break;
            case 4:
                sprite.sprite = d.Turret_M_2;
                break;
            case 5:
                sprite.sprite = d.Turret_M_3;
                break;
            case 6:
                sprite.sprite = d.Turret_L_1;
                break;
            case 7:
                sprite.sprite = d.Turret_L_2;
                break;
            case 8:
                sprite.sprite = d.Turret_L_3;
                break;
            case 9:
                    sprite.sprite = d.Turret_E_S;
                break;
            case 10:
                    sprite.sprite = d.Turret_E_M;
                break;
            case 11:
                    sprite.sprite = d.Turret_E_L;
                break;
        }
        sprite.sortingOrder = 2;
        if(usetype == "Game")
        {
            p.AddComponent<TurretAI>();
            p.GetComponent<TurretAI>().setType(num);
        }
        else if(usetype == "Outfit")
        {
            p.AddComponent<OutfitTurretsCom>().seq=seq;
            p.GetComponent<OutfitTurretsCom>().type = num;
            p.AddComponent<CapsuleCollider2D>();
        }
        return p;
    }

    
    public static GameObject CreateProjectile(int type, GameObject target=null, GameObject mother=null)
    {
        GameObject p = new GameObject("Projectile", typeof(SpriteRenderer));
        var sprite = p.GetComponent<SpriteRenderer>();
        switch(type)
        {
            case 0:
                sprite.sprite = d.Kinteic_1;
                break;
            case 1:
                sprite.sprite = d.Kinteic_2;
                break;
            case 2:
                sprite.sprite = d.Multiple_Cannon_1;
                break;
            case 3:
                sprite.sprite = d.Multiple_Cannon_2;
                break;
            case 4:
                sprite.sprite = d.Missle_1;
                break;
            case 5:
                sprite.sprite = d.Missle_2;
                break;
        }
        sprite.sortingOrder = 1;
        p.transform.localScale += new Vector3(2, 2);
        var c = p.AddComponent<CapsuleCollider2D>();
        Rigidbody2D r = p.AddComponent<Rigidbody2D>();
        r.gravityScale = 0.0f;
        r.useAutoMass=true;
        var a = p.AddComponent<ProjectileMove>();
        a.target = target;
        a.mother = mother;
        a.ammotype = type;
        if(GameProcess.shells!=null)
        {
            GameProcess.shells.Add(p);
        }
        return p;
    }


    private GameObject CreateShipGUI(int HP)
    {
        GameObject shipGUI = new GameObject("shipGUI",typeof(MeshRenderer));
        shipGUI.GetComponent<MeshRenderer>().sortingOrder = 3;
        var t = shipGUI.AddComponent<TextMesh>();
        t.text = HP.ToString();
        t.fontSize = 40;
        return shipGUI;
    }
    public List<GameObject> CreateShipStatus()
    {
        List<GameObject> l = new List<GameObject>();
        foreach (GameObject ship in GameProcess.ships)
        {
            GameObject shipGUI = CreateShipGUI(ship.GetComponent<RocketMove>().HP);
            shipGUI.GetComponent<Transform>().position = ship.transform.position+Vector3.up*12;
            l.Add(shipGUI);
        }
        return l;
    }
    public static void UpdateShipStatus()
    {
        if(GameProcess.shipGUIs!=null)
        {
            while(GameProcess.shipGUIs.Count>GameProcess.ships.Count)
            {
                Destroy(GameProcess.shipGUIs[GameProcess.shipGUIs.Count - 1]);
                GameProcess.shipGUIs.RemoveAt(GameProcess.shipGUIs.Count-1);
            }
            if(GameProcess.shipGUIs.Count<GameProcess.ships.Count)
            {
                foreach(GameObject g in GameProcess.shipGUIs)
                {
                    Destroy(g);
                }
                GameProcess.shipGUIs = d.CreateShipStatus();
            }
            for(int i=0;i<GameProcess.ships.Count;i++)
            {
                GameProcess.shipGUIs[i].GetComponent<TextMesh>().text = GameProcess.ships[i].GetComponent<RocketMove>().HP.ToString();
                GameProcess.shipGUIs[i].transform.position = GameProcess.ships[i].transform.position + new Vector3(-3, 15);
            }
        }

    }

    public int GetTurretSize(int shipType,int turretSeq)
    {
        int result = -1;
        switch(ShipTurretType[shipType][turretSeq])
        {
            case 0:
                result = 0;
                break;
            case 3:
                result = 3;
                break;
            case 6:
                result = 6;
                break;
            default:
                break;
        }
        return result;
    }
    // Use this for initialization
    void Start () {
        //炮台相对船中心的位置
        ShipTurretConf = new Vector3[6][];
        ShipTurretConf[0] =null;
        //ShipTurretConf[0][0] = new Vector3(0, 0);
        ShipTurretConf[1] = new Vector3[1];
        ShipTurretConf[1][0] = new Vector3(0, 0);
        ShipTurretConf[2] = new Vector3[2];
        ShipTurretConf[2][0] = new Vector3(0, 2);
        ShipTurretConf[2][1] = new Vector3(0, -2);
        ShipTurretConf[3] = new Vector3[3];
        ShipTurretConf[3][0] = new Vector3(0, 3.2f);
        ShipTurretConf[3][1] = new Vector3(0, 0.9f);
        ShipTurretConf[3][2] = new Vector3(0, -2);
        ShipTurretConf[4] = new Vector3[4];
        ShipTurretConf[4][0] = new Vector3(0, 4);
        ShipTurretConf[4][1] = new Vector3(0, 2);
        ShipTurretConf[4][2] = new Vector3(0, -1.34f);
        ShipTurretConf[4][3] = new Vector3(0, -4.19f);
        ShipTurretConf[5] = new Vector3[4];
        ShipTurretConf[5][0] = new Vector3(0, 3.9f);
        ShipTurretConf[5][1] = new Vector3(0, 0.83f);
        ShipTurretConf[5][2] = new Vector3(0, -1.7f);
        ShipTurretConf[5][3] = new Vector3(0, -4);

        //炮台的默认类型
        ShipTurretType = new int[6][];
        ShipTurretType[0] = null;
        ShipTurretType[1] = new int[1];
        ShipTurretType[1][0] = 0;
        ShipTurretType[2] = new int[2];
        ShipTurretType[2][0] = 3;
        ShipTurretType[2][1] = 0;
        ShipTurretType[3] = new int[3];
        ShipTurretType[3][0] = 3;
        ShipTurretType[3][1] = 3;
        ShipTurretType[3][2] = 6;
        ShipTurretType[4] = new int[4];
        ShipTurretType[4][0] = 6;
        ShipTurretType[4][1] = 6;
        ShipTurretType[4][2] = 0;
        ShipTurretType[4][3] = 6;
        ShipTurretType[5] = new int[4];
        ShipTurretType[5][0] = 6;
        ShipTurretType[5][1] = 6;
        ShipTurretType[5][2] = 3;
        ShipTurretType[5][3] = 6;
        var camera = Camera.main;//获取相机组件
        float orthographicSize = camera.orthographicSize;
        float aspectRatio = Screen.width * 1.0f / Screen.height;
        float cameraHeight = orthographicSize * 2;//unit高度
        //float cameraWidth = cameraHeight * aspectRatio;//unit宽度
        left = -orthographicSize * aspectRatio;
        right = orthographicSize * aspectRatio;
        up = orthographicSize;
        down = -orthographicSize;

        //gameState = GUIManager.States.MainScreen;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
