using System;
using HotUpdate.GameFrameWork.Module;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EventType = HotUpdate.GameFrameWork.MoudleDef.EventType;

namespace HotUpdate.GameFrameWork.Windows
{
    public class Window_BattleUI: UIWindow
    {
        private TextMeshProUGUI _score;
        private Button _btn;
        public void Open()
        {
            _score.text = BattleManager.Instance.Score.ToString();
        }
        public override void OnAddListener()
        {
            _btn.onClick.AddListener(() =>
            {
                Debug.Log("#13131313");
            });
            EventManager.Instance.AddListener(EventType.AddScore,FreshScore);
            base.OnAddListener();
        }

        private void FreshScore(object sender, EventArgs e)
        {
            _score.text = BattleManager.Instance.Score.ToString();
        }

        public override void OnRemoveListener()
        {
            base.OnRemoveListener();
            _btn.onClick.RemoveAllListeners();
        }

        public override void OnOpen()
        {
            _score = GetCommon<TextMeshProUGUI>("Score");
            _btn = GetCommon<Button>("Btn");
            base.OnOpen();
        }
    }
}