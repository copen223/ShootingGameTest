using System.Runtime.InteropServices;
using ActorModule;
using ShootModule.Gun;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }
        }

        [SerializeField] private Color element_Normal;
        [SerializeField] private Color element_Fire;
        [SerializeField] private Color element_Ice;
        [SerializeField] private Color element_Thunder;

        public Color GetElementColor(DamageInfo.ElementType type)
        {
            switch (type)
            {
                case DamageInfo.ElementType.Normal:
                    return element_Normal;
                case DamageInfo.ElementType.Ice:
                    return element_Ice;
                case DamageInfo.ElementType.Thunder:
                    return element_Thunder;
                case DamageInfo.ElementType.Fire:
                    return element_Fire;
            }
            return Color.black;
        }


        public Transform bulletParent;

        [SerializeField] private AmmoItem ammoPrefab;
        [SerializeField] private Transform ammoParent;
        public void CreatAmmoAt(DamageInfo.ElementType type, Vector2 position)
        {
            var ammo = Instantiate(ammoPrefab,ammoParent);
            ammo.transform.position = position;
            ammo.elementType = type;
            ammo.num = 6;
        }
        
        public enum ItemType
        {
            Ammo,
            MedKit
        }

    }
}
