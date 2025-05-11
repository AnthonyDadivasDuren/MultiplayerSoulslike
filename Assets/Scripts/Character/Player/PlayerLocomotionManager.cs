using UnityEngine;

namespace ADD
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerManager player;
        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;
        
        private Vector3 moveDirection;
        
        [SerializeField] float walkingSpeed = 2f;
        [SerializeField] float runningSpeed = 5f;

        override protected void Awake()
        {
            base.Awake();
            
            // DO MORE STUFF ONLY FOR THE PLAYER
            player = GetComponent<PlayerManager>();
        }
        
        public void handleAllMovement()
        {
            //  GROUNDED MOVEMENT
            HandleGroundedMovement();
            //  AERIAL MOVEMENT
          
        }

        private void GetVerticalAndHorizontalInputs()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            
            // CLAMP THE MOVEMENTS
            
        }
        
        private void HandleGroundedMovement()
        {
            GetVerticalAndHorizontalInputs();
            
            //  MOVE DIRECTION IS BASED ON THE CAMERAS FACING PERSPECTIVE & MOVEMENT INPUTS
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if (PlayerInputManager.instance.moveAmount > 0.5f)
            {
                // MOVE AT A RUNNING SPEED
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.instance.moveAmount <= 0.5f)
            {
                // MOVE AT A WALKING SPEED
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
           
        }
    }
}