using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitTurretsCom : MonoBehaviour {

    public int seq;
    public int type;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnMouseDown()
    {
        if(this.gameObject!=GUIManager.showTurret)
        {
            //清除之前的显示炮塔
            if (GUIManager.showTurret != null)
            {
                Destroy(GUIManager.showTurret);
            }
            //显示炮塔改成自己
            GUIManager.showTurret = GameData.CreateTurret(type, seq, "Outfit");
            GUIManager.showTurret.transform.position = new Vector3(40, 21);
            GUIManager.showTurret.transform.localScale = new Vector3(10, 10);
            //改写追踪数据
            GUIManager.ChangeShipOutfitState(seq, type);
        }
    }
}
