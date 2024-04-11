/* 
****************************************************
* 文件：BattleManager.cs
* 作者：Dev_Xcy
* 创建时间：2024/04/10 18:22:16 星期三
* 功能：战斗管理器
* 修改：
****************************************************
*/
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HotUpdate.Entity;
using UnityEngine;
using EventType = HotUpdate.GameFrameWork.MoudleDef.EventType;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace HotUpdate.GameFrameWork.Module
{
    public class BattleManager : BaseManager<BattleManager>
    {
        public BattleObjInfo curSelectInfo;
        public Dictionary<Vector2, BattleObjInfo> ObjInfos = new Dictionary<Vector2, BattleObjInfo>();
        private GameObject _brickTrans;
        private const int Row = 9;
        private const int Column = 9;
        private const float _bossHp = 100f;
        public int _recordBossHp = 100;
        private GameObject _brick;
        public int Score = 0;
        private bool _isDroping = false;
        private long ContinueTime = 5;
        private List<BattleObjInfo> _isBlockedInfo = new List<BattleObjInfo>();
        public void Init()
        {
            LoadBrickAsset();
            _brickTrans = new GameObject
            {
                name = "BrickRoot",
                layer = 7
            };
        }

        public void ClearSomeOne()   
        {
            if (curSelectInfo != null)
            {
                curSelectInfo.Clear();
            }
            AfterClearSomeOne(curSelectInfo);
        }

        private Vector2 GetRandomPos()
        {
            int row = Random.Range(0, Row);
            int column = Random.Range(0, Column);
            return new Vector2(row, column);
        }

        private void BlockBrickRandom(int cnt)
        {
            List<Vector2> vector2S = new List<Vector2>();
            while (cnt>0)
            {
                Vector2 vector2 = GetRandomPos();
                if (!vector2S.Contains(vector2) && ObjInfos[vector2].IsBlock==false)    
                {
                    cnt--;
                    vector2S.Add(vector2);
                }
            }

            foreach (var vector2 in vector2S)
            {
                ObjInfos[vector2].IsBlock = true;
                ObjInfos[vector2].BlockEndTime = GetTimeStamp()+ContinueTime;
                _isBlockedInfo.Add(ObjInfos[vector2]);
                ObjInfos[vector2].UpdateBlock();
            }
        }

        private long GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }      
        
        public void GenerateRandomPos(int cnt)
        {
            List<BattleObjInfo> infos = new List<BattleObjInfo>();
            for (int i = 0; i < cnt; i++)
            {
                int row = Random.Range(0, Row);
                int colum = Random.Range(0, Column);
                Vector2 vector2 = new Vector2(row, colum);
                infos.Add(ObjInfos[vector2]);
            }
            BattleObjInfo.BrickType type = (BattleObjInfo.BrickType)Random.Range(0, (int)BattleObjInfo.BrickType.Max);
            foreach (BattleObjInfo info in infos)
            {
                info.GenerateTypeWithOutType(type);
                info.SetEffect(false);
                info.UpdateSprite();
            }
        }

        public void ClearRowAndColumn(Vector2 vector2)
        {
            List<BattleObjInfo> infos = new List<BattleObjInfo>();
            foreach (KeyValuePair<Vector2,BattleObjInfo> keyValuePair in ObjInfos)
            {
                if (keyValuePair.Key.x == vector2.x || keyValuePair.Key.y == vector2.y)
                {
                    infos.Add(keyValuePair.Value);
                }
            }
            MoveDown(infos);
        }
        
        public void ReInit()
        {
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    Vector2 pos = new Vector2(i, j);
                    ObjInfos[pos].GenerateType();
                }
            }
            FreshChest();
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

        private void AfterClearSomeOne(BattleObjInfo info)
        {
            List<BattleObjInfo> clearInfos = new List<BattleObjInfo> { info };
            MoveDown(clearInfos);
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
            _recordBossHp -= sumScore;
            EventManager.Instance.TriggerEvent(EventType.AttackBoss,null);
            MoveDown(clearInfos);
        }


        private void MoveDown(List<BattleObjInfo>clearInfos)
        {
            _isDroping = true;
            SetIsCanClick(false);
            foreach (BattleObjInfo info in clearInfos)
            {
                info.Clear();
            }
            if (clearInfos.TrueForAll(t=>t.Pos.x == 0))
            {
                foreach (BattleObjInfo info in clearInfos)
                {
                    info.SetEffect(true);
                }
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
            Sequence sequence = DOTween.Sequence();
            foreach (KeyValuePair<Vector2,Vector2> relation in relations)
            {
                sequence.Insert(0,ObjInfos[relation.Value].Obj.transform.Find("Icon")
                    .DOLocalMove(new Vector3(0, -(relation.Key.x - relation.Value.x), 0), 1f).SetEase(Ease.Linear).OnComplete((delegate {ObjInfos[relation.Value].Obj.transform.Find("Icon").localPosition = Vector3.zero;
                        ObjInfos[relation.Key].UpdateSprite();})));
            }
            sequence.Play().onComplete = (delegate
            {
                foreach (BattleObjInfo info in clearInfos)
                {
                    info.SetEffect(false);
                }
                SetIsCanClick(true);
                FreshChest();
            });
        }

        private void SetIsCanClick(bool active)
        {
            if (Camera.main != null) Camera.main.GetComponent<Physics2DRaycaster>().enabled = active;
        }
        private void FreshChest()
        {
            SetIsCanClick(false);
            List<BattleObjInfo> infos = new List<BattleObjInfo>();
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    Vector2 pos = new Vector2(i, j);
                    BattleObjInfo info = ObjInfos[pos];
                    if (info.Type == BattleObjInfo.BrickType.None)
                    {
                        infos.Add(info);
                    }
                    else
                    {
                        info.UpdateSprite();
                    }
                }
            }

            int cnt = infos.Count;

            if (cnt <= 0)
            {
                _isDroping = false;
                SetIsCanClick(true);
                return;
            }
            foreach (BattleObjInfo info in infos)
            {
                info.GenerateType();
                info.UpdateSprite();
                info.Obj.transform.Find("Icon").transform.localPosition = new Vector3(0, 1, 0);
                info.Obj.transform.Find("Icon").DOLocalMove(Vector3.zero, 2.5f).SetSpeedBased().SetEase(Ease.Linear).onComplete=
                    () =>
                    {
                        cnt--;
                        if (cnt<=0)
                        {
                            _isDroping = false;
                        }
                    };
            }
            SetIsCanClick(true);
        }
        private async void StartCheckClear()
        {
            await CheckClear();
        }

        private async UniTask BlockPlayer()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(10));
                BlockBrickRandom(4);
            }
        }
        
        private async UniTask CheckBlockTime()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                for (var index = _isBlockedInfo.Count - 1; index >= 0; index--)
                {
                    var num = _isBlockedInfo[index];
                    Debug.Log($"结束时间:{num.BlockEndTime} 系统时间:{GetTimeStamp()}");
                    if (num.BlockEndTime <= GetTimeStamp())
                    {
                        ObjInfos[num.Pos].IsBlock = false;
                        ObjInfos[num.Pos].UpdateBlock();
                        _isBlockedInfo.Remove(num);
                    }
                }
            }
        }
        
        private async UniTask CheckClear()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.5));
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
            BlockPlayer();
            CheckBlockTime();
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
                    int index = Random.Range(1, (int)BattleObjInfo.BrickType.Max);
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
            Ease ease = Ease.InOutBack;
            GameObject curObj = curSelectInfo.Obj.transform.Find("Icon").gameObject;
            GameObject exchangeObj = info.Obj.transform.Find("Icon").gameObject;
            Sequence sequence = DOTween.Sequence();
            if (info.Left == curSelectInfo.Pos)
            {
                Debug.Log("在左边");
                sequence.Insert(0, exchangeObj.transform.DOLocalMove(new Vector3(-1, 0, 0), 0.5f).SetEase(ease));
                sequence.Insert(0, curObj.transform.DOLocalMove(new Vector3(1, 0, 0), 0.5f).SetEase(ease));
                sequence.Play().onComplete = Fresh;
            }
            else if (info.Right == curSelectInfo.Pos)
            {
                Debug.Log("在右边");
                sequence.Insert(0, exchangeObj.transform.DOLocalMove(new Vector3(1, 0, 0), 0.5f).SetEase(ease));
                sequence.Insert(0, curObj.transform.DOLocalMove(new Vector3(-1, 0, 0), 0.5f).SetEase(ease));
                sequence.Play().onComplete = Fresh;
            }
            else if (info.Up == curSelectInfo.Pos)
            {
                Debug.Log("在上面");
                sequence.Insert(0, exchangeObj.transform.DOLocalMove(new Vector3(0, 1, 0), 0.5f).SetEase(ease));
                sequence.Insert(0, curObj.transform.DOLocalMove(new Vector3(0, -1, 0), 0.5f).SetEase(ease));
                sequence.Play().onComplete = Fresh;
            }
            else if(info.Down == curSelectInfo.Pos)
            {
                Debug.Log("在下面");
                sequence.Insert(0, exchangeObj.transform.DOLocalMove(new Vector3(0, -1, 0), 0.5f).SetEase(ease));
                sequence.Insert(0, curObj.transform.DOLocalMove(new Vector3(0, 1, 0), 0.5f).SetEase(ease));
                sequence.Play().onComplete = Fresh;
            }

            void Fresh()
            {
                if (info != null && ObjInfos.ContainsKey(info.Pos))
                {
                    info.SetSelect(false);
                    (curSelectInfo.Type, info.Type) = (info.Type, curSelectInfo.Type);
                    curSelectInfo = null;
                    FreshChest();
                }
            }
        }
    }
}