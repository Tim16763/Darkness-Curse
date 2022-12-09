using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace inp_
{
    public class inputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float move;
        public float mouseX;
        public float mouseY;

        public bool b_input;
        public bool attack_inp;
        public bool roll_flag;
        public bool attack_flag;

        public bool isInteracting;

        public PlalyerControls plalyerControls;
        CameraHandler cameraHandler;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            cameraHandler = CameraHandler.singleton;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            float delta = Time.deltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, mouseX / 2, mouseY / 2);
            }
        }

        public void OnEnable()
        {
            if (plalyerControls == null)
            {
                plalyerControls = new PlalyerControls();
                plalyerControls.PlayerSpaceMovement.Movement.performed += plalyerControls => movementInput = plalyerControls.ReadValue<Vector2>();
                plalyerControls.PlayerSpaceMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            }

            plalyerControls.Enable();
        }

        private void OnDisable()
        {
            plalyerControls.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            move = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleRollInput(float delta)
        {
            b_input = plalyerControls.PlayerActions.Roll.triggered;// == UnityEngine.InputSystem.InputActionPhase.Started;
            //Debug.Log(b_input);
            if (b_input)
            {
                roll_flag = true;
               // Debug.Log(b_input);
            }
        }
        private void HandleAttackInput(float delta)
        {
            attack_inp = plalyerControls.Combat.Main_attack.triggered;
           
            if (attack_inp)
            {
                attack_flag = true;
            }
            
        }
    }
}
