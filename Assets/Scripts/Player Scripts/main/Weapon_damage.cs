using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace inp_
{
    public class Weapon_damage : MonoBehaviour
    {
        public int damage;
        public int damage_storage;
        public bool is_in_attack_mode = false;
        
        public Animator anim;

        public void Start()
        {
            anim = GetComponentInParent<Animator>();
            damage_storage = damage;
        }
        
        public void dmg_update()
        {
            if (anim.GetBool("Is_attacking"))
            {
                damage = damage_storage;
            }
            else
            {
                damage = 0;
            }
        }
    }
}
