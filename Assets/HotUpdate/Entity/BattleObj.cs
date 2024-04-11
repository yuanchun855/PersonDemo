using System;
using HotUpdate.GameFrameWork.Module;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HotUpdate.Entity
{
    public class BattleObj: MonoBehaviour
    {
        public BattleObjInfo BattleObjInfo;

        private void Start()
        {
            transform.Find("Select").gameObject.SetActive(false);
        }

        public void PointClick()
        {
            if (BattleManager.Instance.ObjInfos[BattleObjInfo.Pos].IsBlock)
            {
                return;
            }
            if (BattleManager.Instance.curSelectInfo == null)
            {
                BattleObjInfo.SetSelect(true);
                BattleManager.Instance.curSelectInfo = BattleObjInfo;
            }
            else
            {
                if (BattleManager.Instance.IsCanExchange(BattleObjInfo))
                {
                    BattleManager.Instance.Exchange(BattleObjInfo);
                }
                else
                {
                    BattleManager.Instance.curSelectInfo = BattleObjInfo;
                }
            }
        }
       
    }
}