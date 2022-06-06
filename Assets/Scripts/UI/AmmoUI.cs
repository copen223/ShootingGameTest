using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AmmoUI : MonoBehaviour
    {
        [SerializeField]private List<AmmoItemUI> ammoUIs = new List<AmmoItemUI>();
        private int index = 0;
        
        public void SelectNextAmmo()
        {
            index++;
            index = index >= ammoUIs.Count ? 0 : index;

            foreach (var ui in ammoUIs)
            {
                ui.SetThisAmmo(false);
            }
            
            ammoUIs[index].SetThisAmmo(true);
        }
        public void SelectLastAmmo()
        {
            index--;
            index = index < 0 ? ammoUIs.Count - 1 : index;

            foreach (var ui in ammoUIs)
            {
                ui.SetThisAmmo(false);
            }
            
            ammoUIs[index].SetThisAmmo(true);
        }

        public void SelectAmmo(int index_Ammo)
        {
            foreach (var ui in ammoUIs)
            {
                ui.SetThisAmmo(false);
            }

            index = index_Ammo;
            ammoUIs[index_Ammo].SetThisAmmo(true);
        }
    }


    
}
