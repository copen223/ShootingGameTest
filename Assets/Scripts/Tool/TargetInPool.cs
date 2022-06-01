using System;
using UnityEngine;
namespace Tool
{
    public abstract class TargetInPool:MonoBehaviour
    {
        public abstract void OnReset();

        protected void OnEndUsing()  
        {
            gameObject.SetActive(false);
            OnEndUsingEvent?.Invoke(this);
        }

        public event Action<TargetInPool> OnEndUsingEvent;

    }
}