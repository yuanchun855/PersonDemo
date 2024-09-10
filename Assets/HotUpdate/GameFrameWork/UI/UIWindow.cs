using System;
using System.Collections.Generic;
using HotUpdate.GameFrameWork.Module;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HotUpdate.GameFrameWork.UI
{
    public class UIWindow : MonoBehaviour
    {
        [HideInInspector] public List<ViewBindItem> ObjList;
        [HideInInspector] public int ObjCount;
        public UIGroup UIGroup;

        private Dictionary<string, Object> mObjDic;
        
        public T GetCommon<T>(string nameKey) where T : Object
        {
            if (mObjDic == null) InitObjDic();
            if (!mObjDic.ContainsKey(nameKey))
            {
                Debug.LogError($"没有绑定此物体:[{nameKey}]");
                return null;
            }

            try
            {
                T t = null;
                if (typeof(T) == typeof(Transform))
                {
                    t = ((GameObject)mObjDic[nameKey]).transform as T;
                }
                else if (typeof(T) == typeof(RectTransform))
                {
                    t = ((GameObject)mObjDic[nameKey]).transform as T;
                }
                else if (typeof(T) == typeof(GameObject))
                {
                    t = (T)mObjDic[nameKey];
                }
                else if (typeof(T) == typeof(Sprite))
                {
                    t = (T)mObjDic[nameKey];
                }
                else if (typeof(T) == typeof(Mesh))
                {
                    t = (T)mObjDic[nameKey];
                }
                else if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)))
                {
                    t = ((GameObject)mObjDic[nameKey]).GetComponent<T>();
                }
                else if (typeof(Behaviour).IsAssignableFrom(typeof(T)))
                {
                    t = ((GameObject)mObjDic[nameKey]).GetComponent<T>();
                }
                else if (typeof(Component).IsAssignableFrom(typeof(T)))
                {
                    t = ((GameObject)mObjDic[nameKey]).GetComponent<T>();
                }
                else if (typeof(T) == typeof(AudioSource))
                {
                    t = ((GameObject)mObjDic[nameKey]).GetComponent<T>();
                }
                else
                {
                    t = (T)mObjDic[nameKey];
                }

                if (t == null)
                {
                    Debug.LogError($"获取obj失败,name:{nameKey}| type:{typeof(T)}");
                }

                return t;
            }
            catch
            {
                Debug.LogError($"强制转换错误,name:{nameKey}| type:{typeof(T)}");
            }

            return null;
        }

        private void InitObjDic() //初始化
        {   
            mObjDic = new Dictionary<string, Object>();
            foreach (var viewBindItem in ObjList)
            {
                if (!mObjDic.ContainsKey(viewBindItem.Name))
                {
                    mObjDic.Add(viewBindItem.Name, viewBindItem.Obj);
                }
                else
                {
                    Debug.LogErrorFormat("ViewObj有重复的Name:{0}", viewBindItem.Name);
                }
            }
        }

        protected virtual void PlayOpenAnim() 
        {   
                
        }
        public virtual void OnOpen()
        {
            PlayOpenAnim();
            OnAddListener();
        }

        public virtual void OnClose()
        {
            OnRemoveListener();
        }

        public virtual void OnUpdate()
        {
            
        }

        protected virtual void OnAddListener()
        {
        }

        protected virtual void OnRemoveListener()
        {
        }

        [System.Serializable]
        public class ViewBindItem
        {
            public string Name;
            public Object Obj;

            public ViewBindItem()
            {
            }

            public ViewBindItem(string name, Object obj)
            {
                this.Name = name;
                this.Obj = obj;
            }
        }

        [System.Serializable]
        public class CurveItem
        {
            public string Name;
            public AnimationCurve Curre;

            public CurveItem()
            {
            }

            public CurveItem(string name, AnimationCurve curve)
            {
                this.Name = name;
                this.Curre = curve;
            }
        }
    }
}