using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace inp_ {

    
    public class Exit_from_location : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject Congratulations;
        public GameObject IO;
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Player")
            {
                if (other.GetComponent<PlayerManager>().KillCount == other.GetComponent<PlayerManager>().MaxKillCount && IO.GetComponent<Ghost_Guardian>().HP <= 0)
                {
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Congratulations.SetActive(true);
                }
            }
        }
    }
}
