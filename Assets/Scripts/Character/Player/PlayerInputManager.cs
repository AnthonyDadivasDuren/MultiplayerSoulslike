using System;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace ADD
{


    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;
        
        public PlayerManager player;

        
        PlayerControls playerControls;

        [Header("PLAYER MOVEMENT INPUT")]
        [SerializeField] Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;
        
        [Header("CAMERA MOVEMENT INPUT")]
        [SerializeField] Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            // WHEN THE SCENE CHANGES, RUN THIS LOGIC
            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            // IF WE ARE LOADING THE WORLD SCENE, ENABLE OUR PLAYER CONTROLS
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
            }
            // OTHERWISE, WE MUST BE AT THE MENU SCENE, SO DISABLE OUR PLAYER CONTROLS
            // THIS IS SO THE PLAYER CANT MOVE AROUND IF IT ENTERS THINGS LIKE A CHARACTER CREATION MENU ETC.
            else
            {
                instance.enabled = false;
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            // IF WE DESTROY THIS OBJECT, UNSUBSCRIBE FROM THE SCENE CHANGE EVENT
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                //ONLY ALLOWS PLAYER CONTROLS WHEN APPLICATION IS FOCUSED
                if (focus)
                {

                    playerControls.Enable();
                }
                //STOPS PLAYER CONTROLS WHEN APPLICATION IS MINIMIZED ( i.e. LOST FOCUS)
                else
                {

                    playerControls.Disable();
                }
            }
        }

        private void Update()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
        }

        private void HandlePlayerMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            // RETURNS THE ABSOLUTE NUMBER, (Meaning number without the negative sign, so its always positive)
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            // CLAMP THE VALUES, SO THEY ARE 0, 0.5 OR 1 (meaning 3 different states and no inbetweens)
            if (moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }
            
            // WHY DO PASS 0 ON THE HORIZONTAL? BECAUSE ONLY WANT NON-STRAFING MOVEMENT
            // USE THE HORIZONTAL WHEN STRAFING OR LOCKED ON
            
            if (player == null) { return; }
            
            // IF NOT LOCKED ON, ONLY USE THE MOVE AMOUNT
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
            
            // IF LOCKED ON PASS THE HORIZONTAL VALUE AS WELL

        }

        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }
    }

}