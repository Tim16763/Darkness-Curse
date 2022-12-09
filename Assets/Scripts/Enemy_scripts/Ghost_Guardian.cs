using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace inp_
{
    public class Ghost_Guardian : MonoBehaviour
    {
        public NavMeshAgent agent;
        public GameObject player;
        public Animator anim;
        public int HP = 50;
        private bool is_running = false;
        public GameObject ded_state;
        public Weapon_damage dmg;

        public float hit_window = 0.5f;

        public ParticleSystem RAttack;

        public Weapon_damage my_dmg;
        public Slider HP_bar;

        public void Start()
        {
            player = GameObject.Find("Player");
            HP_bar.maxValue = HP;
            agent.speed = 3;
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            my_dmg = GetComponentInChildren<Weapon_damage>();
        }

        public IEnumerator HitWindow(float wind)
        {
            yield return new WaitForSeconds(wind + 0.98f);
            anim.SetBool("Is_attacking", false);
        }
        public IEnumerator Jump(float timing)
        {
            yield return new WaitForSeconds(timing);
            RAttack.Play();
            anim.SetBool("Is_attacking", false);
            if (!player.GetComponent<PlayerLocomotion>().animator.anim.GetBool("is_unded"))
            {
                if (Vector3.Distance(transform.position, player.transform.position) <= 10)
                {
                    player.GetComponent<HP_managment>().HP -= 20;
                }
            }
          
           
        }

        public void Update()
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= 5 && !anim.GetBool("Is_attacking"))
            {
                agent.Stop();
                anim.SetFloat("Blend", 0);

                is_running = false;

                //.rotation = Quaternion.LookRotation(player.);
                if (Random.Range(0, 5) == 3)
                {
                    
                    anim.SetBool("Is_attacking", true);
                    anim.Play("Jump");
                    StartCoroutine(Jump(1));
                }
                else
                {
                    Vector3 playerPos5 = player.transform.position;
                    playerPos5.y = 0;
                    transform.LookAt(playerPos5);
                    anim.SetBool("Is_attacking", true);
                    StartCoroutine(HitWindow(hit_window));
                    my_dmg.dmg_update();

                    anim.Play("MeleeAttack_TwoHanded");
                }
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
                is_running = false;
                anim.SetFloat("Blend", 0);
            }
        }

        private void HP_upd(int damage)
        {
            HP -= damage;
            HP_bar.value -= damage;
            if (HP <= 0)
            {
                Instantiate(ded_state, transform.position, transform.rotation);
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
