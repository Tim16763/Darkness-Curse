using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace inp_
{
    public class clear_curse : MonoBehaviour
    {
        public poison_totem_script Totem;
        

        public void Horn_Relocate()
        {
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * Totem.R;
            
            transform.SetPositionAndRotation(new Vector3(Totem.center.x + randomCircle.x, 4.22f, Totem.center.z + randomCircle.y), transform.rotation);
        }
        // Start is called before the first frame update
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Player")
            {
                ClearCurse();
                Horn_Relocate();
            }
        }

        private void ClearCurse()
        {
            
            GameObject.Find("Player").GetComponentInChildren<Weapon_damage>().damage_storage = 10;
            Totem.dmg_reducion = 0;
            
            
        }

        public void DETONATE()
        {
            GameObject.Destroy(gameObject);
        }
    }
}
