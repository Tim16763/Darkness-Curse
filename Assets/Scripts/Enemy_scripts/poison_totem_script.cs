using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;

namespace inp_
{
    public class poison_totem_script : MonoBehaviour
    {
        // Start is called before the first frame update
        public float R;
        public int N;
        public int M;
        public int cooldown;
        private int cooldownStorage;
        public Vector3 center;
        public int HP = 50;
        public GameObject ded_state;
        public Slider HP_bar;
        public GameObject player;
        public TextMeshProUGUI text;
        public NavMeshAgent navMeshAgent;

        public ParticleSystem particle;

        public clear_curse horn;

        public int dmg_reducion;

        public int StoreWeaponDmg;

        void Start()
        {
            player = GameObject.Find("Player");
            StoreWeaponDmg = player.GetComponentInChildren<Weapon_damage>().damage;
            center = transform.position;
            cooldownStorage = 0;
         
            StartCoroutine(Stack());
            navMeshAgent.updateRotation = false;
        }

        private void Update()
        {
            
            Relocate();
            if (dmg_reducion > 0)
            {
                text.gameObject.SetActive(true); 
            }
            else
            {
                text.gameObject.SetActive(false);   
            }
        }

        // Update is called once per frame

        private void HP_upd(int damage)
        {
            HP -= damage;
            HP_bar.value -= damage;
            if (HP <= 0)
            {
                
                dmg_reducion = 0;
                text.gameObject.SetActive(false);
                horn.DETONATE();
                player.GetComponentInChildren<Weapon_damage>().damage_storage = StoreWeaponDmg;
                player.GetComponent<PlayerManager>().KillCount++;
                GameObject.Destroy(gameObject);
            }
        }
        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Hero_weapon")
            {
                if (other.GetComponentInParent<PlayerLocomotion>().animator.anim.GetBool("Is_attacking"))
                {
                    
                    HP_upd(other.GetComponent<Weapon_damage>().damage);
                    other.GetComponent<Weapon_damage>().damage = 0;
                }
            }

        }
        void Relocate()
        {
            if (Vector3.Distance(transform.position, player.transform.position) < M && cooldownStorage == 0)
            {
                Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * R;
                // Debug.Log(Convert.ToInt32(Vector3.Distance(gameObject.transform.position, player.transform.position)) < M);
                
                navMeshAgent.gameObject.transform.SetPositionAndRotation(new Vector3(center.x + randomCircle.x, center.y, center.z + randomCircle.y), transform.rotation);
                cooldownStorage = cooldown;
                StartCoroutine(CooldownTick());
            }
            else if (Vector3.Distance(transform.position, player.transform.position) < R)
            {
                if (navMeshAgent.velocity == Vector3.zero)
                {
                    Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * R;
                    
                    navMeshAgent.SetDestination(new Vector3(center.x + randomCircle.x, 3.879961f, center.z + randomCircle.y));
                }
            }
        }
        

        public void CheckRadius(float radius, bool is_in_radius = false)
        {

            if (Vector3.Distance(center, player.transform.position) < radius)
            {
                if (!is_in_radius)
                {
                    StartCoroutine(Stack());
                }
                is_in_radius = true;

            }
            else
            {
                is_in_radius = false;
            }
        }

        private IEnumerator CooldownTick()
        {
            while (cooldownStorage != 0)
            {
                yield return new WaitForSeconds(1);
                cooldownStorage--;

            }


        }
        private IEnumerator Stack()
        {
            while (gameObject.active)
            {
                yield return new WaitForSeconds(N);
                if (Vector3.Distance(transform.position, player.transform.position) < R)
                {
                    
                    if (player.GetComponentInChildren<Weapon_damage>().damage_storage > 2 )
                    {

                        player.GetComponentInChildren<Weapon_damage>().damage_storage --;

                        dmg_reducion++;
                        if (Vector3.Distance(transform.position,player.transform.position) < R / 2)
                        {
                            if (player.GetComponentInChildren<Weapon_damage>().damage_storage > 1)
                            {
                                player.GetComponentInChildren<Weapon_damage>().damage_storage--;
                                dmg_reducion++;
                                
                            }
                        }
                        text.text =  "-" + dmg_reducion;
                        particle.Play();
                    }
                    Debug.Log("Stack"); 
                }
                else
                {
                    yield return null;
                }
            }

        }
    }
}
