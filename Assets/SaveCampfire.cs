using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace inp_
{
    public class SaveCampfire : MonoBehaviour
    {
        public PlayerManager playerManager;
        public GameObject player;
        public GameObject show;
        public HP_managment HP_;
        public int Stage;
        private void Start()
        {
            player = GameObject.Find("Player");
            playerManager = player.GetComponent<PlayerManager>();
            HP_ = player.GetComponent <HP_managment>();
            Stage = 0;
        }

        public void SceneStageUpdate()
        {
            if (Stage == 0)
            {

                HP_.Ded();
            }
            else
            {
                HP_.NotQuietDed();
                show.GetComponent<Ghost_Guardian>().HP = 200 ;
                show.GetComponent<Ghost_Guardian>().HP_bar.value = 200;
                player.transform.position = transform.position;
            }
            
        }

        public void Update()
        {
            if (HP_.HP <= 0)
            {
                SceneStageUpdate();
            }
        }

        public void Save()
        {
            StartCoroutine(RestoreHealth(playerManager.gameObject.GetComponent<HP_managment>()));
            Stage = 1;
            show.SetActive(true);
        }

        public IEnumerator RestoreHealth(HP_managment hP_Managment)
        {
            while (hP_Managment.HP < 100)
            {
                yield return new WaitForSeconds(0.1f);
                hP_Managment.HP++;
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.name == "Player")
            {
                if (playerManager.KillCount == playerManager.MaxKillCount && Stage == 0)
                {
                    Save();
                    
                }
            }
        }
    }
}
