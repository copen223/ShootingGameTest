using System.Collections.Generic;
using ShootModule.Gun;
using UnityEngine;

namespace ActorModule
{
    public class DamageInfo
    {
        public DamageInfo(Bullet _sourseBullet,params BeHitPoint[] _beHitPoints)
        {
            type = _sourseBullet.DamageType;
            damage = _sourseBullet.Damage;
            BeHitPoints.Clear();
            foreach (var behit in _beHitPoints)
            {
                BeHitPoints.Add(behit);
            }

            sourceActor = _sourseBullet.SourceActor;
            sourceBullet = _sourseBullet;
            damagePos = _sourseBullet.transform.position;
        }
        
        public enum ElementType
        {
            Fire,
            Thunder,
            Ice,
            Normal
        }

        public ElementType type;
        public float damage;
        public List<BeHitPoint> BeHitPoints = new List<BeHitPoint>();
        public ActorMono sourceActor;
        public Bullet sourceBullet;
        public Vector2 damagePos;

        public float GetDamageMultiply(ElementType hit, ElementType behit)
        {
            float multiply = 10;
            
            switch (hit)
            {
                case ElementType.Fire:
                    if (behit == ElementType.Ice)
                        multiply *= 2f;
                    if (behit == ElementType.Thunder)
                        multiply /= 2f;
                    break;
                case ElementType.Thunder:
                    if (behit == ElementType.Fire)
                        multiply *= 2f;
                    if (behit == ElementType.Ice)
                        multiply /= 2f;
                    break;
                case ElementType.Ice:
                    if (behit == ElementType.Thunder)
                        multiply *= 2f;
                    if (behit == ElementType.Fire)
                        multiply /= 2f;
                    break;
                case ElementType.Normal:
                    break;
            }

            return multiply;
        }

        /// <summary>
        /// 获得元素增伤计算的乘值列表
        /// </summary>
        /// <returns></returns>
        public List<float> GetDamageMultipliesByElementCheck()
        {
            List<float> multiplies = new List<float>();
            foreach (var beHitPoint in BeHitPoints)
            {
                float mul = GetDamageMultiply(type, beHitPoint.Element);
                multiplies.Add(mul);
            }

            return multiplies;
        }

    }
}