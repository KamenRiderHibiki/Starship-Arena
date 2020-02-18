//Belong to:
//Version:0.1.0
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey;
using CodeMonkey.Utils;

enum towardEdge
{
    Tup,
    Tdown,
    Tleft,
    Tright,
    Tnone
}

public class RocketMove : MonoBehaviour {
    [SerializeField]
    public float angle;//火箭角度
    public int HP;
    private float heading = 0.0f;
    private float destination = 0.0f;
    private float speed = 0.5f;
    private float orthographicSize;//获取size
    private float aspectRatio;//纵横比
    private Vector2 size;//图片大小
    private float left, right, up, down;
    private towardEdge te;
    private shipStates state;
    private float MoveTimer;
    private float MoveTimerMax;

    void Awake()
    {
        MoveTimerMax = 0.06f;
        MoveTimer = MoveTimerMax;
        state = shipStates.Live;
    }
    void Start () {
        GameObject gameObject = GameObject.Find("Main Camera");//获取主相机
        //var camera = Camera.main;
        var camera = gameObject.GetComponent<Camera>();//获取相机组件
        orthographicSize = camera.orthographicSize;
        aspectRatio = Screen.width * 1.0f / Screen.height;
        float cameraHeight = orthographicSize * 2;//unit高度
        float cameraWidth = cameraHeight * aspectRatio;//unit宽度
        size = GetComponent<Renderer>().bounds.size;
        left = -orthographicSize * aspectRatio + size.x / 2;
        right = orthographicSize * aspectRatio - size.x / 2;
        up = orthographicSize - size.y / 2;
        down = -orthographicSize + size.y / 2;
        te = towardEdge.Tnone;
        //HP在data设定
    }
	
	// Update is called once per frame
	void Update () {
        switch (state)
        {
           case shipStates.Live:
                MoveTimer += Time.deltaTime;
                Vector3 p = gameObject.transform.position;//获取位置
                float l = p.x - left;
                float r = right - p.x;
                float u = up - p.y;
                float d = p.y - down;
                int theta = ((int)heading / 90) % 4;//朝向象限
                float a1, a2;
                switch (theta)
                {
                    case 0://0-89°
                        a1 = l / Mathf.Tan(Mathf.Deg2Rad * heading);
                        if (a1 > u)
                        {
                            //Debug.Log("向上");
                            te = towardEdge.Tup;
                            a2 = u / Mathf.Cos(Mathf.Deg2Rad * heading);
                        }
                        else
                        {
                            //Debug.Log("向左");
                            te = towardEdge.Tleft;
                            a2 = l / Mathf.Cos(Mathf.Deg2Rad * (90.0f - heading));
                        }
                        break;
                    case 1:
                        a1 = l / Mathf.Tan(Mathf.Deg2Rad * (180.0f - heading));
                        if (a1 > d)
                        {
                            //Debug.Log("向下");
                            te = towardEdge.Tdown;
                            a2 = d / Mathf.Cos(Mathf.Deg2Rad * (180.0f - heading));
                        }
                        else
                        {
                            //Debug.Log("向左");
                            te = towardEdge.Tleft;
                            a2 = l / Mathf.Cos(Mathf.Deg2Rad * (heading - 90.0f));
                        }
                        break;
                    case 2:
                        a1 = r / Mathf.Tan(Mathf.Deg2Rad * (heading - 180.0f));
                        if (a1 > d)
                        {
                            //Debug.Log("向下");
                            te = towardEdge.Tdown;
                            a2 = d / Mathf.Cos(Mathf.Deg2Rad * (heading - 180.0f));
                        }
                        else
                        {
                            //Debug.Log("向右");
                            te = towardEdge.Tright;
                            a2 = r / Mathf.Cos(Mathf.Deg2Rad * (270.0f - heading));
                        }
                        break;
                    case 3:
                        a1 = r / Mathf.Tan(Mathf.Deg2Rad * (360.0f - heading));
                        if (a1 > u)
                        {
                            //Debug.Log("向上");
                            te = towardEdge.Tup;
                            a2 = u / Mathf.Cos(Mathf.Deg2Rad * (360.0f - heading));
                        }
                        else
                        {
                            //Debug.Log("向右");
                            te = towardEdge.Tright;
                            a2 = r / Mathf.Cos(Mathf.Deg2Rad * (heading - 270.0f));
                        }
                        break;
                    default:
                        a2 = 0.0f;
                        break;
                }
                if (a2 < size.x * 2)
                {
                    switch (te)
                    {
                        case towardEdge.Tup:
                            if (theta == 0)
                            {
                                heading = ++heading > 360 ? ++heading % 360 : ++heading;
                            }
                            else//=3
                            {
                                heading = --heading < 0 ? --heading + 360 : --heading;
                            }
                            break;
                        case towardEdge.Tdown:
                            if (theta == 2)
                            {
                                heading = ++heading > 360 ? ++heading % 360 : ++heading;
                            }
                            else//=1
                            {
                                heading = --heading < 0 ? --heading + 360 : --heading;
                            }
                            break;
                        case towardEdge.Tleft:
                            if (theta == 1)
                            {
                                heading = ++heading > 360 ? ++heading % 360 : ++heading;
                            }
                            else//=0
                            {
                                heading = --heading < 0 ? --heading + 360 : --heading;
                            }
                            break;
                        case towardEdge.Tright:
                            if (theta == 3)
                            {
                                heading = ++heading > 360 ? ++heading % 360 : ++heading;
                            }
                            else//=2
                            {
                                heading = --heading < 0 ? --heading + 360 : --heading;
                            }
                            break;
                        default:
                            break;
                    }
                }
                Forward();
                //实现点击移动
                if (Input.GetMouseButtonDown(0))
                {

                }
                
                if (Input.GetKey(KeyCode.W))
                {
                    //speed += 0.1f;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    //speed -= 0.1f;
                    //if (speed < 0)
                        //speed = 0;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    if(MoveTimer>MoveTimerMax && gameObject.tag== "Player")
                        heading = --heading < 0 ? --heading + 360 : --heading;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    if (MoveTimer > MoveTimerMax && gameObject.tag == "Player")
                        heading = ++heading > 360 ? ++heading % 360 : ++heading;
                }
                if (MoveTimer > MoveTimerMax)
                    MoveTimer -= MoveTimerMax;
                //限制在边界内
                p = gameObject.transform.position;
                if (p.y > up)
                {
                    p.y = up;
                }
                if (p.y < down)
                {
                    p.y = down;
                }
                if (p.x < left)
                {
                    p.x = left;
                }
                if (p.x > right)
                {
                    p.x = right;
                }
                gameObject.transform.position = p;
                transform.rotation = Quaternion.AngleAxis(heading + angle, Vector3.forward);
                if(HP<=0)
                {
                    GameObject.Find("EventSystem").GetComponent<GameProcess>().Endgame(this.gameObject);
                }
                break;
           case shipStates.Dead:
                if(this.tag!="shipStates")
                {

                }
                else
                {

                }
               break;
           case shipStates.Pause:
               break;
        }

        
    }
    void Forward()
    {
        //MoveTimer += Time.deltaTime;
        if (MoveTimer >= MoveTimerMax)
        {
            transform.Translate(speed * 0.1f * new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle)));
        }
    }
    float r2deg(float rad)
    {
        return rad * Mathf.PI / 180.0f;
    }

    public void PauseShip()
    {
        //state = shipStates.Pause;
    }

    public void ResumeShip()
    {
        //state = shipStates.Live;
    }
    void OnMouseDown()
    {      

        //CMDebug.TextPopupMouse("Sprite Clicked");
    }
}
