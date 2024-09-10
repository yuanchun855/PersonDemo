using HotUpdate.GameFrameWork.Module;
using HotUpdate.GameFrameWork.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotUpdate.GameFrameWork.Windows
{
    public class Window_Start: UIWindow
    {
        private Button _enterBattle; 
        public void Open()
        {
            BattleManager.Instance.Init();
        }

        protected override void OnAddListener()
        {
            base.OnAddListener();
            _enterBattle.onClick.AddListener(EnterBattle);
        }

        public void EnterBattle()
        {
            Debug.Log("333333333333333333333333");
            UIManager.Instance.LoadWindow<Window_BattleUI>((win => win.Open()));
            BattleManager.Instance.LoadBattleMap();
        }

        protected override void OnRemoveListener()
        {
            base.OnRemoveListener();
            _enterBattle.onClick.RemoveAllListeners();
        }

        public override void OnOpen()
        {
            _enterBattle = GetCommon<Button>("EnterBattle");
            base.OnOpen();
        }
    }
}