using System.Collections.Generic;
using ActorModule.Monster;
using ShootModule.Gun;
using UnityEngine;

namespace ActorModule
{
    public class DamageInfo
    {
        public DamageInfo(Bullet _sourseBullet,BeHitPoint _beHitPoint)
        {
            type = _sourseBullet.DamageType;
            damage = _sourseBullet.Damage;
            beHitPoint = _beHitPoint;

            sourceActor = _sourseBullet.SourceActor;
            sourceBullet = _sourseBullet;
            damagePos = _sourseBullet.transform.position;

            float damage_Point = damage * GetDamageMultiply_Element(type, beHitPoint.Element);
            finalDamage = damage_Point * GetDamageMultiply_Weakness(beHitPoint);
        }

        public DamageInfo(DamageBody damageBody, BeHitPoint _behitPoint)
        {
            type = damageBody.damageElementType;
            damage = damageBody.damage;
            beHitPoint = _behitPoint;

            sourceActor = damageBody.sourceActor;
            sourceDamageBody = damageBody;
            damagePos = sourceDamageBody.transform.position;
            
            float damage_Point = damage * GetDamageMultiply_Element(type, beHitPoint.Element);
            finalDamage = damage_Point * GetDamageMultiply_Weakness(beHitPoint);
        }
        
        public enum ElementType
        {
            Fire,
            Thunder,
            Ice,
            Normal
        }

        public ElementType type;
        /// <summary>
        /// 基础伤害值
        /// </summary>
        public float damage;
        /// <summary>
        /// 最终伤害值
        /// </summary>
        public float finalDamage;
        public BeHitPoint beHitPoint;
        public ActorMono sourceActor;
        public Bullet sourceBullet;
        public DamageBody sourceDamageBody;
        public Vector2 damagePos;

        public int ifBeHitWeakElement { get; private set; }
        
        /// <summary>
        /// 元素伤害的乘值
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="behit"></param>
        /// <returns></returns>
        private float GetDamageMultiply_Element(ElementType hit, ElementType behit)
        {
            float multiply = 10;
            
            switch (hit)
            {
                case ElementType.Fire:
                    if (behit == ElementType.Thunder)
                    {
                        multiply *= 4f;
                        ifBeHitWeakElement = 1;
                    }
                    else if  (behit == ElementType.Ice)
                    {
                        multiply /= 2f;
                        ifBeHitWeakElement = -1;
                    }
                    else
                    {
                        ifBeHitWeakElement = 0;
                    }
                    break;
                
                case ElementType.Thunder:
                    if (behit == ElementType.Ice)
                    {
                        multiply *= 4f;
                        ifBeHitWeakElement = 1;
                    }
                    else if  (behit == ElementType.Fire)
                    {
                        multiply /= 2f;
                        ifBeHitWeakElement = -1;
                    }
                    else
                    {
                        ifBeHitWeakElement = 0;    
                    } 
                    break;
                
                case ElementType.Ice:
                    if (behit == ElementType.Fire)
                    {
                        multiply *= 4f;
                        ifBeHitWeakElement = 1;
                    }
                    else if (behit == ElementType.Thunder)
                    {
                        multiply /= 2f;
                        ifBeHitWeakElement = -1;
                    }
                    else
                    {
                        ifBeHitWeakElement = 0;
                    }

                    break;
                
                case ElementType.Normal:
                    ifBeHitWeakElement = 0;
                    break;
            }

            return multiply;
        }

        public float GetDamageMultiply_Weakness(BeHitPoint behit)
        {
            switch (behit.Type)
            {
                case BeHitPoint.BehitType.Normal:
                    break;
                case BeHitPoint.BehitType.Tough:
                    return 0.25f;
                    break;
                case BeHitPoint.BehitType.Weakness:
                    return 4f;
                    break;
            }

            return 1;
        }
        
        
        /*/// <summary>
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
        }*/

    }
}