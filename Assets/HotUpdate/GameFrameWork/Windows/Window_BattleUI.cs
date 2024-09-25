using System;
using DG.Tweening;
using HotUpdate.GameFrameWork.Module;
using HotUpdate.GameFrameWork.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EventType = HotUpdate.GameFrameWork.MoudleDef.EventType;

namespace HotUpdate.GameFrameWork.Windows
{
    public class Window_BattleUI: UIWindow
    {
        
        private WinView _winView;
        private class WinView
        {
            public readonly TextMeshProUGUI Score;
            public readonly Button ReBuildBtn;
            public readonly Button BtnBomb;
            public readonly Image Fill;
            public readonly TextMeshProUGUI RemainFoot;
            public WinView(UIWindow uiWindow)
            {
                Score = uiWindow.GetCommon<TextMeshProUGUI>("Score");
                ReBuildBtn = uiWindow.GetCommon<Button>("ReBuildBtn");
                BtnBomb = uiWindow.GetCommon<Button>("BtnBomb");
                Fill = uiWindow.GetCommon<Image>("Fill");
                RemainFoot = uiWindow.GetCommon<TextMeshProUGUI>("RemainFoot");
            }
        }
        public void Open()
        {
            _winView.Score.text = BattleManager.Instance.Score.ToString();
            string text = _winView.RemainFoot.text;
            var t = DOTween.To(() => string.Empty, value => _winView.RemainFoot.text = value, text, 3f).SetEase(Ease.Linear);
            //富文本
            t.SetOptions(true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                BattleManager.Instance.ClearRowAndColumn(new Vector2(2,1));
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                BattleManager.Instance.GenerateRandomPos(10);
            }
        }

        protected override void OnAddListener()
        {
            _winView.ReBuildBtn.onClick.AddListener(() =>
            {
                BattleManager.Instance.ReInit();
            });
            _winView.BtnBomb.onClick.AddListener(() =>
            {
                BattleManager.Instance.ClearSomeOne();
            });
            EventManager.Instance.AddListener(EventType.AddScore,FreshScore);
            EventManager.Instance.AddListener(EventType.AttackBoss,FreshBossHp);
            base.OnAddListener();
        }

        private void FreshBossHp(object sender, EventArgs e)
        {
            _winView.Fill.fillAmount = BattleManager.Instance._recordBossHp / 100f;
        }
        private void FreshScore(object sender, EventArgs e)
        {
            _winView.Score.text = BattleManager.Instance.Score.ToString();
        }

        protected override void OnRemoveListener()
        {
            base.OnRemoveListener();
            _winView.ReBuildBtn.onClick.RemoveAllListeners();
            _winView.BtnBomb.onClick.RemoveAllListeners();
            EventManager.Instance.RemoveListener(EventType.AttackBoss,FreshBossHp);
            EventManager.Instance.RemoveListener(EventType.AddScore,FreshScore);
        }

        public override void OnOpen()
        {
            _winView ??= new WinView(this);
            base.OnOpen();
        }
    }
}