using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using HotUpdate.Entity;
using UnityEngine;
using EventType = HotUpdate.GameFrameWork.MoudleDef.EventType;
using Random = UnityEngine.Random;
using DG.Tweening;

namespace HotUpdate.GameFrameWork.Module
{
    public class BattleManager : BaseManager<BattleManager>
    {
        public BattleObjInfo curSelectInfo;
        public Dictionary<Vector2, BattleObjInfo> ObjInfos = new Dictionary<Vector2, BattleObjInfo>();
        private GameObject _brickTrans;
        private const int Row = 9;
        private const int Column = 9;
        private GameObject _brick;
        public int Score = 0;
        private bool _isDroping = false;
        public enum BrickType
        {
            None,
            Red,
            Huang,
            Zi,
            End
        }
        public void Init()
        {
            LoadBrickAsset();
            _brickTrans = new GameObject
            {
                name = "BrickRoot"
            };
            _brickTrans.layer = 7;
        }
        
        public bool IsCanExchange(BattleObjInfo info)
        {
            if (info.Type == curSelectInfo.Type || info.Pos == curSelectInfo.Pos || info.Type == BattleObjInfo.BrickType.None)
            {
                return false;
            }
            else
            {
                if (curSelectInfo.Pos.x == info.Pos.x)
                {
                    return Math.Abs(info.Pos.y - curSelectInfo.Pos.y) <= 1;
                }
                else if(curSelectInfo.Pos.y == info.Pos.y)
                {
                    return Math.Abs(info.Pos.x - curSelectInfo.Pos.x) <= 1;
                }
            }
            return false;
        }

        private void Clear()
        {
            if (_isDroping)
            {
                return;
            }
            List<BattleObjInfo> clearInfos = new List<BattleObjInfo>();
            int sumScore = 0;
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    BattleObjInfo curInfo = ObjInfos[new Vector2(i, j)];
                    List<BattleObjInfo> matchInfos = new List<BattleObjInfo> { curInfo };
                    for (int x = i+1; x < Row; x++)
                    {
                        BattleObjInfo nextInfo = ObjInfos[new Vector2(x,j)];
                        if (nextInfo != null && nextInfo.Type == curInfo.Type)
                        {
                            matchInfos.Add(nextInfo);
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (matchInfos.Count >= 3)
                    {
                        sumScore += 1;
                        EventManager.Instance.TriggerEvent(EventType.AddScore,null);
                        foreach (BattleObjInfo info in matchInfos)
                        {
                            clearInfos.Add(info);
                        }
                    }
                    matchInfos.Clear();
                    matchInfos.Add(curInfo);
                    // 检查纵向匹配
                    for (int y = j + 1; y < Column; y++)
                    {
                        BattleObjInfo nextInfo = ObjInfos[new Vector2(i, y)];
                        if (nextInfo != null && curInfo.Type == nextInfo.Type)
                        {
                            matchInfos.Add(nextInfo);
                        }
                        else
                        {
                            break;
                        }
                    }

                    // 如果匹配的方块数量大于等于3，标记为可消除
                    if (matchInfos.Count >= 3)
                    {
                        sumScore += 1;
                        EventManager.Instance.TriggerEvent(EventType.AddScore,null);
                        foreach (BattleObjInfo info in matchInfos)
                        {
                            clearInfos.Add(info);
                        }
                    }
                }
            }

            Score += sumScore;
            MoveDown(clearInfos);
            // FreshChest();
        }


        private void MoveDown(List<BattleObjInfo>clearInfos)
        {
            _isDroping = true;
            foreach (BattleObjInfo info in clearInfos)
            {
                info.Clear();
            }

            if (clearInfos.TrueForAll(t=>t.Pos.x == 0))
            {
                FreshChest();
                return;
            }
            Dictionary<Vector2, Vector2> relations = new Dictionary<Vector2, Vector2>();
            for (int col = 0; col < Column; col++)
            {
                // 从底部向上遍历每一行
                for (int row = Row-1; row >= 0; row--)
                {
                    Vector2 curPos = new Vector2(row, col);
                    // 如果当前位置为空，向上搜索直到找到一个非空位置
                    if (ObjInfos[curPos].Type == BattleObjInfo.BrickType.None)
                    {
                        for (int i = row - 1; i >= 0; i--)
                        {
                            Vector2 calculatePos = new Vector2(i, col);
                            // 如果找到了非空位置，将其物品移动到当前位置，并清空原位置
                            if (ObjInfos[calculatePos].Type != BattleObjInfo.BrickType.None)
                            {
                                ObjInfos[curPos].Type = ObjInfos[calculatePos].Type;
                                ObjInfos[calculatePos].Type = BattleObjInfo.BrickType.None;
                                relations.Add(curPos,calculatePos);
                                break;
                            }
                        }
                    }
                }
            }
            int count = relations.Count;
            if (count == 0)
            {
                FreshChest();
                return;
            }
            Debug.Log(count);
            foreach (KeyValuePair<Vector2,Vector2> relation in relations)
            {
                ObjInfos[relation.Value].Obj.transform.Find("Icon").DOLocalMove(new Vector3(0, -(relation.Key.x-relation.Value.x), 0),2.5f).SetSpeedBased().SetEase(Ease.Linear).onComplete = ()=>
                {
                    ObjInfos[relation.Value].Obj.transform.Find("Icon").localPosition = Vector3.zero;
                    ObjInfos[relation.Key].UpdateSprite();
                    count--;
                    if (count == 0)
                    {
                        FreshChest();
                    }
                };
            }
        }

        private void FreshChest()
        {
            _isDroping = false;
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    Vector2 pos = new Vector2(i, j);
                    BattleObjInfo info = ObjInfos[pos];
                    if (info.Type == BattleObjInfo.BrickType.None)
                    {
                        info.GenerateType();
                    }
                    info.UpdateSprite();
                }
            }
        }
        private async void StartCheckClear()
        {
            await CheckClear();
        }
        
        private async UniTask CheckClear()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                Clear();
            }
        }
        public void LoadBattleMap()
        {
            ResourceManager.Instance.LoadPrefabByAssetType(ResourceManager.AssetType.None,"BattleMap1", (obj) =>
            {
                GameObject gameObject = GameObject.Instantiate(obj);
            });
            InitBricks();
            StartCheckClear();
            // Clear();
        }

        private void LoadBrickAsset()
        {
            ResourceManager.Instance.LoadPrefabByAssetType(ResourceManager.AssetType.BattlePrefab,"1", (obj) =>
            {
                _brick = obj;
            });
        }

        private void InitBricks()
        {
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    BattleObjInfo info;
                    int index = Random.Range(1, (int)BrickType.End);
                    GameObject obj = GameObject.Instantiate(_brick, _brickTrans.transform, true);
                    obj.name = $"Brick({j},{i})";
                    info = new BattleObjInfo(new Vector2(j, i), (BattleObjInfo.BrickType)index,obj);
                    obj.transform.position = new Vector3((float)(i * 1 - (float)(Row / 2f)), (float)(3.5-j));
                    info.UpdateSprite();
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
                    AddDataToDic(info);
                }
            }
        }
        
        public void AddDataToDic(BattleObjInfo info)
        {
            if (info!= null && !ObjInfos.ContainsKey(info.Pos))
            {
                ObjInfos.Add(info.Pos,info);
            }
        }

        public void Exchange(BattleObjInfo info)
        {
            Debug.Log("交换");
            curSelectInfo.SetSelect(false);
            if (info != null && ObjInfos.ContainsKey(info.Pos))
            {
                info.SetSelect(false);
                (curSelectInfo.Type, info.Type) = (info.Type, curSelectInfo.Type);
                curSelectInfo = null;
                FreshChest();
                Clear();
            }
        }
    }
}