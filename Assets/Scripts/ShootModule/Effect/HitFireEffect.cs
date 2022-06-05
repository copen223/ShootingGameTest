using Tool;
using UnityEngine;

namespace ShootModule.Effect
{
    public class HitFireEffect : TargetInPool
    {
        [SerializeField] private Animator animator;

        private void Update()
        {
        }

        public override void OnReset()
        {
            gameObject.SetActive(true);
            animator.Play($"HitFire");
        }
    }
}
