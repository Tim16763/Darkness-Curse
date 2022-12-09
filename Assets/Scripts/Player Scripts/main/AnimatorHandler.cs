using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace inp_
{
    public class AnimatorHandler : MonoBehaviour
    {
        // Start is called before the first frame update
        public Animator anim;
        public inputHandler input;
        public PlayerLocomotion PlayerLocomotion;
        int vertical;
        int horizontal;
        public bool can_rotate;

        public void Initialize()
        {
            anim = GetComponent<Animator>();
            input = GetComponentInParent<inputHandler>();
            PlayerLocomotion = GetComponentInParent<PlayerLocomotion>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
            
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;

            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else
            {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }


            #endregion

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal,h,0.1f, Time.deltaTime);
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = true;
            anim.SetBool("is_interacting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f);
            
        }
        public void CanRotate()
        {
            can_rotate = true;
        }

        public void StopRotation()
        {
            can_rotate = false;
        }

        private void OnAnimatorMove()
        {
            if (input.isInteracting == false)
            {
                return;
            }
            /*float delta = Time.deltaTime;
            PlayerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            PlayerLocomotion.rigidbody.velocity = velocity;*/
            

        }
    }
}
