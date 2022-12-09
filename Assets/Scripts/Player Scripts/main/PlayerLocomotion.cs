using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace inp_
{
    public class PlayerLocomotion : MonoBehaviour
    {
        Transform cameraObject;
        inputHandler input;
        public Vector3 moveDirect;

        public HP_managment hp;
        public Transform myTr;

        public Weapon_damage weapon_;

        public AnimatorHandler animator;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        public SphereCollider hit_trigger;

        public float movement_speed = 5;

        public float rotationSpeed = 10;

        public AudioSource footsteps;
        public AudioSource SwordSound;

        public GameObject pause;
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            input = GetComponent<inputHandler>();
            animator = GetComponentInChildren<AnimatorHandler>();
            cameraObject = Camera.main.transform;
            myTr = transform;
            animator.Initialize();
        }

        public void OnDestroy()
        {
            footsteps.Stop();
            SwordSound.mute = true;
        }

        public void Update()
        {
            float delta = Time.deltaTime;

            input.TickInput(delta);
            HandleMovement(delta);
            HandleRollingAndSprinting(delta);
            HandleAttack(delta);
            HandlePause(delta);
            Audio(delta);
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = input.move;

            targetDir += cameraObject.forward * input.vertical;
            targetDir += cameraObject.right * input.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
            {
                targetDir = myTr.forward;
            }

            float rs = rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTr.rotation, tr, rs * delta);

            myTr.rotation = targetRotation;
        }

        public void HandleMovement(float delta)
        {
            if (input.isInteracting || animator.anim.GetBool("Is_attacking"))
            {
                return;
            }
            moveDirect = cameraObject.forward * input.vertical;
            moveDirect += cameraObject.right * input.horizontal;

            moveDirect.Normalize();
            moveDirect.y = 0;

            float speed = movement_speed;
            moveDirect *= speed;

            Vector3 projectedVel = Vector3.ProjectOnPlane(moveDirect, normalVector);
            rigidbody.velocity = projectedVel;

            animator.UpdateAnimatorValues(input.move, 0);

            if (animator.can_rotate)
            {
                HandleRotation(delta);
            }
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (animator.anim.GetBool("is_interacting"))
            {
                return;
            }

            if (input.roll_flag && hp.stamina >= 10)
            {

               // if (input.move > 0)
                
                moveDirect = cameraObject.forward;
                //moveDirect += cameraObject.right;
                    
                animator.PlayTargetAnimation("RollForward", true);
                StartCoroutine(TimerForInter(0.94f,"is_interacting"));
                    
                moveDirect.y = 0;
                animator.anim.SetBool("is_unded",true);
                Quaternion rollRotation = Quaternion.LookRotation(moveDirect);
                myTr.rotation = rollRotation;
                //rigidbody.AddForce(cameraObject.forward * 2, ForceMode.Impulse);
                rigidbody.AddForce(moveDirect * 2, ForceMode.Impulse);
                hp.stamina -= 10;
                    
                
                
               
            }
        }

        public void HandlePause(float delta)
        {
            if (input.plalyerControls.Interactivity.Pause.triggered)
            {
                pause.SetActive(true);
                
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            
        }

        public void Resume()
        {
            pause.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
        }

        public void HandleAttack(float delta)
        {
            if (animator.anim.GetBool("Is_attacking")) 
            {
                return;
            }
            if (hp.stamina >= 5 && input.attack_flag)
            {
                rigidbody.velocity = Vector3.zero;
                Vector3 rotate = cameraObject.forward;
                rotate.y = 0;
                myTr.rotation = Quaternion.LookRotation(rotate);
                animator.PlayTargetAnimation("MeleeAttack_TwoHanded",false);
                StartCoroutine(HandleAttackAudio());
                hp.stamina -= 5;

                animator.anim.SetBool("Is_attacking", true);
                StartCoroutine(TimerForInter(1f,"Is_attacking"));
                weapon_.dmg_update();
            }
        }

        public IEnumerator TimerForInter(float sec,string boolean)
        {
            yield return new WaitForSeconds(sec);
            animator.anim.SetBool(boolean,false);
        }

        public IEnumerator HandleAttackAudio()
        {
            yield return new WaitForSeconds(0.34f);
            
            SwordSound.Play();
        }


        
        #endregion

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Enemy_weapon" && !animator.anim.GetBool("is_unded"))
            {
                hp.HP -= other.GetComponent<Weapon_damage>().damage;
                other.GetComponent<Weapon_damage>().damage = 0;
            }
        }

        public void Audio(float delta)
        {
            if (input.move > 0 && footsteps.mute && !input.isInteracting)
            {
                footsteps.mute = false;
            }
            else if (input.move <= 0 || input.isInteracting)
            {
                footsteps.mute=true;
            }
        }
    }
}