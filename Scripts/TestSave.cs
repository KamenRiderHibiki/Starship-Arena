using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestSave : MonoBehaviour
{

    public static TestSave s;
    /// <summary>
    /// 定义一个测试类
    /// </summary>
    public class SaveClass
    {
        
        public int Type;
        public string Name;
        public List<int> Turrets = new List<int>();
        public int Score;
        public int Progress;
        public SaveClass(int T = 0,List<int> t=null ,int S = 0,int P = 0,string n = "null" )
        {
            Type = T;
            Name = n;
            Turrets = t;
            Score = S;
        }
    }
    void Awake()
    {
        s = this;
    }
    void Start()
    {
        //定义存档路径
        //string dirpath = Application.persistentDataPath + "/Saves";//Pc/Mac电脑 以及android跟Ipad、ipone都可对文件进行任意操作
        //string dirpath = Application.dataPath + "/Saves";
        //创建存档文件夹
        //IOHelper.CreateDirectory(dirpath);
        //定义存档文件路径
        //string filename = dirpath + "/GameData.sav";
        //TestClass t = new TestClass();
        //保存数据
        //IOHelper.SetData(filename, t);
        //读取数据
        //TestClass t1 = (TestClass)IOHelper.GetData(filename, typeof(TestClass));
        //Debug.Log(this.name);
        //Debug.Log(filename);
        //Debug.Log(t1.Name);
        //Debug.Log(t1.Age);
        //Debug.Log(t1.Ints);
        
    }

    /// <summary>
    /// 获取存档数据
    /// </summary>
    public void GetSave()
    {
        SaveClass t1;
        string a = this.name;
        Debug.Log(a);
        string dirpath = Application.dataPath + "/Saves";
        IOHelper.CreateDirectory(dirpath);//此函数处理存在与不存在的问题
        string filename = dirpath +"/error.txt";
        if (a == "Save1")
        {
            filename = dirpath + "/GameData1.sav";
        }
        if (a == "Save2")
        {
            filename = dirpath + "/GameData2.sav";
        }
        if (IOHelper.IsFileExists(filename))
        {
            //Debug.Log("文档存在");
            //读取数据
            t1 = (SaveClass)IOHelper.GetData(filename, typeof(SaveClass));
        }
        else
        {
            List<int> i = new List<int>() { 0 };
            t1 = new SaveClass(0,i,0);
        }
        GameData.savePath = filename;
        GameData.save = t1;
    }

    public void SetSave()
    {
       IOHelper.SetData(GameData.savePath,GameData.save);
    }

    public void DeleteSave(int i)
    {
        bool finish = false;
        string str = null;
        string dirpath = Application.dataPath + "/Saves";
        switch (i)
        {
            case 0:
                str = dirpath + "/GameData1.sav";
                finish = IOHelper.DeleteData(str);
                break;
            case 1:
                str = dirpath + "/GameData2.sav";
                finish = IOHelper.DeleteData(str);
                break;
        }
        if(finish)
        {

        }
    }

}