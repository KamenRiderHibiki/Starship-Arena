using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class TurretAI : MonoBehaviour {
    private float targetAngle, AtanTarget;
    private float reloadTime;
    private float firespeed;
    private Vector3 targetVec;
    GameObject target;
    private int type;
    private int ammotype;
    
    public void setType(int type = 0)
    {
        this.type = type;
        switch(type)
        {
            case 0:
                firespeed = 3.0f;
                ammotype = 0;
                break;
            case 1:
                firespeed = 1.5f;
                ammotype = 2;
                break;
            case 2:
                firespeed = 3.3f;
                ammotype = 4;
                break;
            case 3:
                firespeed = 2.0f;
                ammotype = 0;
                break;
            case 4:
                firespeed = 1.0f;
                ammotype = 2;
                break;
            case 5:
                firespeed = 2.2f;
                ammotype = 4;
                break;
            case 6:
                firespeed = 1.0f;
                ammotype = 0;
                break;
            case 7:
                firespeed = 0.5f;
                ammotype = 2;
                break;
            case 8:
                firespeed = 1.1f;
                ammotype = 4;
                break;
        }
    }
    // Use this for initialization
    void Start () {
        CSDNRotation();
        reloadTime = 0.0f;
        //firespeed = 3.0f;
        target = null;
	}
	
	// Update is called once per frame
	void Update () {
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, AtanTarget-90), Time.deltaTime);
        CSDNRotation();
        reloadTime += Time.deltaTime;
        if(reloadTime>firespeed)
        {
            GameObject shell =  GameData.CreateProjectile(ammotype,target,transform.parent.gameObject);
            
            shell.transform.position = transform.position;
            shell.GetComponent<ProjectileMove>().Heading =  shell.transform.rotation = Quaternion.AngleAxis(targetAngle-90, Vector3.forward);
            
            reloadTime -= firespeed;
        }
    }

    void CSDNRotation()
    {
        List<GameObject> s = GameProcess.ships;//获取目标列表
        target = null;//重置目标
        Vector3 distanceMin = new Vector3(float.MaxValue / 2, float.MaxValue / 2);//先填一个大数字
        foreach (GameObject o in s)
        {
            if(o==null)
            {
                break;
            }
            if (o != this.transform.parent.gameObject)
            {
                Vector3 distance = o.transform.position - transform.position;
                //如果距离更小就把目标变成这个敌人
                if (distance.magnitude < distanceMin.magnitude)
                {
                    distanceMin = distance;
                    target = o;
                }
            }
            else
            {
                //是自己就什么都不做
            }
        }
        //如果没敌人就转回默认位置
        if (distanceMin.magnitude > float.MaxValue / 4)
        {
            if (transform.localScale.y > 0)
            {
                distanceMin = Vector3.up;
            }
            else
            {
                distanceMin = Vector3.down;
            }
        }
        float angle = Vector3.Angle(distanceMin, Vector3.right);              ///< 计算旋转角度
        distanceMin = Vector3.Normalize(distanceMin);                           ///< 向量规范化
        float dot = Vector3.Dot(distanceMin, Vector3.up);                  ///< 判断是否Vector3.right在同一方向
        if (dot < 0)
            angle = 360 - angle;

        targetAngle = angle;
        targetVec = new Vector3(0, 0, angle);
        ///< 补充点1： 通过Atan2与方向向量的两条边可以计算出转向的角度，通过计算结果可以看到targetAngle与-AtanTarget相加正好是360°，即二者都指向同一方向。具体使用场景需要根据具体需求分析。
        AtanTarget = Mathf.Atan2(distanceMin.y, distanceMin.x) * Mathf.Rad2Deg;
        ///< 补充点2： 使用欧拉角来控制物体的旋转
        //arrow.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, angle);

        
    }
    //public void 
}
