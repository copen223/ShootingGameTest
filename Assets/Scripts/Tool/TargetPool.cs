using System.Collections.Generic;
using UnityEngine;
namespace Tool
{
    public class TargetPool<T>
    where T:TargetInPool
    {
        private T prefab;
        private Transform parent;
        public TargetPool(T targetPrefab,Transform _parent)
        {
            prefab = targetPrefab;
            parent = _parent;
        }

        private readonly List<T> waitToUseTargets = new List<T>();
        private List<T> usingTargets = new List<T>();

        public T GetActiveTarget()
        {
            if (waitToUseTargets.Count <= 0)
            {
                var newTarget = Object.Instantiate(prefab, parent);
                newTarget.gameObject.SetActive(false);
                waitToUseTargets.Add(newTarget);
            }

            var target = waitToUseTargets[0];
            usingTargets.Add(target);
            waitToUseTargets.RemoveAt(0);
            
            target.OnEndUsingEvent += OnTargetOverCallback;
            target.OnReset();
            return target;
        }

        private void OnTargetOverCallback(TargetInPool target)
        {
            waitToUseTargets.Add(target as T);
            usingTargets.Remove(target as T);
            
            target.OnEndUsingEvent -= OnTargetOverCallback;
        }
        
    }
}