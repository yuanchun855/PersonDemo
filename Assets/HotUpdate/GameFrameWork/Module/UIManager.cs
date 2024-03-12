using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HotUpdate.GameFrameWork.Module
{
    public enum UIGroup
    {
        Top = 0,
        Normal = 1,
        Buttom = 2,
    }
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance => instance;
        private Dictionary<string, GameObject> uiWindows = new Dictionary<string, GameObject>();
        protected Dictionary<string, UIWindow> UIWindowsObject = new Dictionary<string, UIWindow>();
        protected Stack<UIWindow> UIWindowsStack = new Stack<UIWindow>();
        public GameObject TopRoot;
        public GameObject NormalRoot;
        public GameObject ButtomRoot;
        
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        

        public delegate void LoadWindowSuccess<T>(T win);

        public void LoadWindow<T>(LoadWindowSuccess<T> loadSuccess = null) where T : UIWindow
        {
            string winName = typeof(T).Name;
            if (!UIWindowsObject.ContainsKey(winName))
            {
                ResourceManager.Instance.LoadPrefabByAssetType(ResourceManager.AssetType.Window, winName, (obj) =>
                {
                    GameObject viewObj = Instantiate(obj, transform);
                    viewObj.name = typeof(T).Name;
                    T win = viewObj.GetComponent<T>();
                    UIWindowsObject.Add(winName, win);
                    if (winName != "WindowMain")
                    {
                        UIWindowsStack.Push(win);
                    }

                    win.OnOpen();
                    for (int i = 0; i < UIWindowsStack.Count; i++)
                    {
                        if (i == 0)
                        {
                            UIWindowsStack.ElementAt(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            UIWindowsStack.ElementAt(i).gameObject.SetActive(false);
                        }
                    }

                    loadSuccess?.Invoke(win);
                });
            }
        }

        public void CloseWindow<T>() where T : UIWindow
        {
            string winName = typeof(T).Name;
            if (UIWindowsObject.ContainsKey(winName))
            {
                UIWindowsStack.Pop();
                UIWindowsObject[winName].OnClose();
                Destroy(UIWindowsObject[winName].gameObject);
                UIWindowsObject.Remove(winName);
            }

            UIWindowsStack.TryPeek(out UIWindow uiWindow);
            if (uiWindow != null)
            {
                uiWindow.gameObject.SetActive(true);
            }
        }

        public UIWindow GetCurWindow()
        {
            return UIWindowsStack.Pop();
        }

        public void CloseCurWindow()
        {
            UIWindow uiWindow = GetCurWindow();
            string winName = uiWindow.name;
            UIWindowsObject[winName].OnClose();
            Destroy(UIWindowsObject[winName].gameObject);
            UIWindowsObject.Remove(winName);
            UIWindowsStack.TryPeek(out UIWindow win);
            if (win != null)
            {
                win.gameObject.SetActive(true);
            }
        }

        protected T InstantiateView<T>() where T : UIWindow
        {
            string winName = typeof(T).Name;
            T view = null;
            ResourceManager.Instance.LoadPrefabByAssetType(ResourceManager.AssetType.Window,winName, (obj) =>
            {
                GameObject viewObj = Instantiate(obj, transform);
                viewObj.name = typeof(T).Name;
                view = viewObj.GetComponent<T>();
            });
            return view;
        }

        public void Init()
        {
        }

        public void OpenPopWindow(string info)
        {
            // LoadWindow<WindowPop>((win) => { win.Open(info); });
        }
    }
}