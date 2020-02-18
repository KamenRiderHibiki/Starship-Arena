using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour {
    public GameObject target;
    public GameObject mother;
    private float speed = 1.0f;
    private float MoveTimer;
    private float MoveTimerMax;
    private Vector2 size;
    public Quaternion Heading;
    public int ammotype;
    
    // Use this for initialization
    void Start () {
        MoveTimerMax = 0.02f;
        MoveTimer = MoveTimerMax;
        size = GetComponent<Renderer>().bounds.size;
        //ammotype = 0;
    }
	
	// Update is called once per frame
	void Update () {
        MoveTimer += Time.deltaTime;
        Forward();
        if (MoveTimer > MoveTimerMax)
        {
            MoveTimer -= MoveTimerMax;
            
        }
        //越界处理
        Vector3 Pos = transform.position;
        if(Pos.x<GameData.d.left-size.x||Pos.x>GameData.d.right||Pos.y>GameData.d.up+size.y||Pos.y<GameData.d.down)
        {
            Destroy(transform.gameObject);
        }
        //给导弹上追踪
        if(ammotype>=4)
        {
            if(target!=null)
            {
                Vector3 distance = target.transform.position - this.transform.position;//到目标的向量
                float angle = Vector3.Angle(distance, Vector3.right);              ///< 计算旋转角度
                distance = Vector3.Normalize(distance);                           ///< 向量规范化
                float dot = Vector3.Dot(distance, Vector3.up);                  ///< 判断是否Vector3.right在同一方向
                if (dot < 0)
                    angle = 360 - angle;
                var AtanTarget = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, AtanTarget - 90), Time.deltaTime);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Projectile")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if(other.gameObject!=mother&&other.gameObject.tag=="Ship"|| other.gameObject != mother && other.gameObject.tag=="Player")
        {
            switch(ammotype)
            {
                case 0:
                    other.gameObject.GetComponent<RocketMove>().HP -= 30;
                    break;
                case 1:
                    other.gameObject.GetComponent<RocketMove>().HP -= 30;
                    break;
                case 2:
                    other.gameObject.GetComponent<RocketMove>().HP -= 15;
                    break;
                case 3:
                    other.gameObject.GetComponent<RocketMove>().HP -= 15;
                    break;
                case 4:
                    other.gameObject.GetComponent<RocketMove>().HP -= 30;
                    break;
                case 5:
                    break;
            }
            
            Destroy(gameObject);
        }
        else
        {

        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        //if (other.gameObject.name == "Cube")
        {
            //Destroy(other.gameObject);
        }
        if(other.gameObject==mother.gameObject)
        {
            transform.rotation = Heading;
        }
    }
    void Forward()
    {
        //MoveTimer += Time.deltaTime;
        if (MoveTimer >= MoveTimerMax)
        {
            //transform.Translate(Vector3.up * Time.deltaTime);
            transform.Translate(Vector3.up);
        }
    }
}
