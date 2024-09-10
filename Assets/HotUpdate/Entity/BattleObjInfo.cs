using HotUpdate.GameFrameWork.Module;
using UnityEngine;

namespace HotUpdate.Entity
{
    public class BattleObjInfo
    {
        public Vector2 Pos { get; set; }//raw column
        public BrickType Type { get; set; }
        public GameObject Obj { get; set; }
        public bool IsBlock = false;
        public long BlockEndTime = 0;
        public enum BrickType
        {
            None = 0,
            Red = 1,
            Huang = 2,
            Zi = 3,
            Lan = 4,
            Cheng = 5,
            Max,
        }

        public Vector2 Up => Pos + new Vector2(-1, 0);
        public Vector2 Down => Pos + new Vector2(1, 0);
        public Vector2 Left => Pos + new Vector2(0, -1);
        public Vector2 Right => Pos + new Vector2(0, 1);



        public void SetEffect(bool active)
        {
            Obj.transform.Find("Effect").gameObject.SetActive(active);
        }

        public void UpdateSprite()
        {
            int index = (int)Type;
            Obj.transform.Find("Icon").localPosition = Vector3.zero;
            Obj.transform.Find("Score").gameObject.SetActive(index == 0);
            if (index == 0)
            {
                Obj.transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = null;
                return;
            }
            Obj.transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = UIManager.Instance.BrickAtlas[index.ToString()];
            UpdateBlock();
        }

        public void UpdateBlock()
        {
            Obj.transform.Find("Block").gameObject.SetActive(IsBlock);
        }

        public void Clear()
        {
            Type = BrickType.None;
            SetEffect(true);
            UpdateSprite();
        }

        public void GenerateType()
        {
            Type = (BattleObjInfo.BrickType)Random.Range(1, (int)BattleObjInfo.BrickType.Max);
            SetEffect(false);
        }

        public void GenerateTypeWithOutType(BrickType type)
        {
            Type = type;

        }

        public void SetSelect(bool active)
        {
            Obj.transform.Find("Select").gameObject.SetActive(active);
        }

        public BattleObjInfo(Vector2 pos, BrickType type,GameObject obj)
        {
            Pos = pos;
            Type = type;
            Obj = obj;
        }
    }
}