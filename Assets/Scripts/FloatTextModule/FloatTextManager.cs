using System;
using Tool;
using UnityEngine;

namespace FloatTextModule
{
    public class FloatTextManager : MonoBehaviour
    {
        public static FloatTextManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }
        }


        [SerializeField] private FloatText floatTextTarget;
        [SerializeField] private Transform floatTextParent;
        private TargetPool<FloatText> floatTextPool;

        private void Start()
        {
            floatTextPool = new TargetPool<FloatText>(floatTextTarget,transform);
        }

        private FloatText CreatFloatTextAt(Vector3 position)
        {
            var newFloatText = floatTextPool.GetActiveTarget();
            newFloatText.transform.parent = floatTextParent;
            newFloatText.StartPos = position;
            newFloatText.StartMove();
            return newFloatText;
        }

        public void CreatDamageFloatTextAt(string _text,Vector2 position,Color color,bool ifBig)
        {
            var text = CreatFloatTextAt(position);
            text.SetText(_text,ifBig);
            text.SetColor(color);
        }
        
    }
}
