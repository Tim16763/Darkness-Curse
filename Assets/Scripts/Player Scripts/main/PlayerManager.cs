using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace inp_
{
    public class PlayerManager : MonoBehaviour
    {
        // Start is called before the first frame update
        inputHandler input;
        Animator anim;
        public int KillCount = 0;
        public int MaxKillCount = 11;

        public GameObject T1;
        public GameObject T2;
        public GameObject T3;

        public GameObject goal;
        public GameObject EndGoal;
        
        public IEnumerator Tutorial()
        {
            yield return new WaitForSeconds(3);
            T1.SetActive(false);
            T2.SetActive(true);
            yield return new WaitForSeconds(3);
            T2.SetActive(false);
            T3.SetActive(true);
            yield return new WaitForSeconds(3);
            T3.SetActive(false);
            goal.SetActive(true);
        }

        private void Start()
        {
            input = GetComponent<inputHandler>();
            anim = GetComponentInChildren<Animator>();
            StartCoroutine(Tutorial());
        }

        // Update is called once per frame
        void Update()
        {
            input.isInteracting = anim.GetBool("is_interacting");
            input.roll_flag = false;
            input.attack_flag = false;
            if (KillCount == MaxKillCount)
            {
                goal.SetActive(false);
                EndGoal.SetActive(true);
            }

        }
    }
}
