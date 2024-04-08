using System;
using System.Collections.Generic;
using HotUpdate.Entity;
using HotUpdate.GameFrameWork.Module;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HotUpdate.Battle
{
    public class GenerateObj: MonoBehaviour
    {
        public GameObject prefab2;
        public GameObject prefab1;
        public GameObject prefab3;
        public static int Row = 9;
        public static int Column = 9;
        public Transform Bricks;
        private List<int> Brick = new List<int>(){ 0, 1, 2, 3 };
        private void Start()
        {
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    GameObject obj = null;
                    BattleObjInfo info = null;
                    int index = Random.Range(1, Brick.Count);
                    switch (index)
                    {
                        case 1:
                            obj = Instantiate(prefab1);
                            break;
                        case 2: 
                            obj = Instantiate(prefab2);
                            break;
                        case 3:
                            obj = Instantiate(prefab3);
                            break;
                    }
                    if (obj!=null)
                    {
                        info = new BattleObjInfo(new Vector2(i, j), (BattleObjInfo.BrickType)index,obj);
                        obj.gameObject.name = $"battleObj({i},{j})";
                        obj.transform.SetParent(Bricks.transform);
                        obj.transform.position = new Vector3((float)(i * 1 - (float)(Row / 2f)), (float)(3.5-j));
                        BattleObj battleObj;
                        if (obj.GetComponent<BattleObj>() == null)
                        {
                            battleObj = obj.AddComponent<BattleObj>();
                            battleObj.BattleObjInfo = info;
                        }
                        else
                        {
                            battleObj = obj.GetComponent<BattleObj>();
                            battleObj.BattleObjInfo = info;
                        }
                    }
                    BattleManager.Instance.AddDataToDic(info);
                }
            }
        }
    }
}