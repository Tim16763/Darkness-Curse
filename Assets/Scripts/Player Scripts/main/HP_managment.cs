using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


namespace inp_
{
    public class HP_managment : MonoBehaviour
    {
        public int HP = 100;
        private float hp_value;
        public int stamina = 50;

        public inputHandler input;

        public GameObject DeathScreen;

        public Slider hp_bar;
        public Slider stamina_bar;

        //public CustomPostProcessVolumeComponent component;
        //public Volume volume;
        //public Vignette Vignette;
        public TextMeshProUGUI text;

        public TextMeshProUGUI curse_counter;
        public int curse;


        private void Start()
        {
            input = GetComponent<inputHandler>();
            StartCoroutine("StaminaRegen");
        }
        private void Update()
        {
            HP_update();
            Stamina_update();
            
            
        }

        public void Ded()
        {
            DeathScreen.SetActive(true);
            
            GameObject.Destroy(gameObject.GetComponent<PlayerLocomotion>());
            
            if (input.plalyerControls.Interactivity.Restart.triggered)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.UnloadScene(1);
                SceneManager.LoadScene(1);
                
            }
        }

        public void NotQuietDed()
        {
            DeathScreen.SetActive(true);
            
            if (input.plalyerControls.Interactivity.Restart.triggered)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                HP = 100;
                DeathScreen.SetActive(false);
                
            }
        }
        private void HP_update()
        {

            hp_value = HP / 100f;
            hp_bar.value = hp_value;
            text.text = HP.ToString();
          //  volume.profile.TryGet<Vignette>(out Vignette);
          //  Vignette.intensity.Override(1 - (HP / 100f));
        }
        private void Stamina_update()
        {
            stamina_bar.value = stamina;
        }
        public IEnumerator StaminaRegen()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                if (stamina < 50)
                {
                    stamina++;
                }
            }
        }
    }

    
}
