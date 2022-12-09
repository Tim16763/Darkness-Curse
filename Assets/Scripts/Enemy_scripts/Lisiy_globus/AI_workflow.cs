using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace inp_
{
    public class AI_workflow : MonoBehaviour
    {
        public NavMeshAgent agent;
        private AudioSource sword_swipe;
        public GameObject player;
        public Animator anim;
        public int HP = 50;
        private bool is_running = false;
        public GameObject ded_state;
        public Weapon_damage dmg;

        public float hit_window = 0.5f;

        public Weapon_damage my_dmg;
        public Slider HP_bar;

        public void Start()
        {
            player = GameObject.Find("Player");
            HP_bar.maxValue = HP;
            agent.speed = 3;
            sword_swipe = GetComponent<AudioSource>();
            my_dmg = GetComponentInChildren<Weapon_damage>();
        }

        public IEnumerator HitWindow(float wind)
        {
            yield return new WaitForSeconds(0.30f);
            sword_swipe.Stop();
            yield return new WaitForSeconds(wind+0.68f);
            anim.SetBool("Is_attacking", false);
            transform.LookAt(player.transform);
            agent.Resume();
            agent.SetDestination(player.transform.position);
        }

        public void Update()
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= 1.5f && !anim.GetBool("Is_attacking"))
            {
                
               // else
               // {
                    agent.Stop();
                    
                    anim.SetFloat("Blend", 0);
                    
                    is_running = false;
                    
                    
                    anim.SetBool("Is_attacking", true);
                    StartCoroutine(HitWindow(hit_window));
                    my_dmg.dmg_update();
                    anim.Play("MeleeAttack_TwoHanded");
                    sword_swipe.PlayDelayed(0.30f);
               // }
            }
            else if (Vector3.Distance(transform.position, player.transform.position) > 2 && Vector3.Distance(transform.position, player.transform.position) < 10 && !anim.GetBool("Is_attacking"))
            {
                my_dmg.dmg_update();
                if (!is_running)
                {
                    anim.SetFloat("Blend", 1);
                    is_running = true;
                    agent.Resume();
                    agent.SetDestination(player.transform.position);
                }
                agent.SetDestination(player.transform.position);

            }
            if (agent.remainingDistance <= 0.2f)
            {
                is_running=false;
                anim.SetFloat("Blend",0);
            }
            if (player.GetComponent<HP_managment>().HP <= 0)
            {
                GameObject.Destroy(gameObject);
            }
        }

        private void HP_upd(int damage)
        {
            HP -= damage;
            HP_bar.value -= damage;
            if (HP <= 0)
            {
                Instantiate(ded_state,transform.position,transform.rotation);
                player.GetComponent<PlayerManager>().KillCount++;
                gameObject.SetActive(false);
            }
        }
        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Hero_weapon")
            {
                if (other.GetComponentInParent<PlayerLocomotion>().animator.anim.GetBool("Is_attacking"))
                {
                    dmg = other.GetComponent<Weapon_damage>();
                    HP_upd(dmg.damage);
                    other.GetComponent<Weapon_damage>().damage = 0;
                }
            }

        }

    }
}
