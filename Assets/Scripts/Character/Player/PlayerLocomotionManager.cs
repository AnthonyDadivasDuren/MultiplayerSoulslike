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
        private Vector3 targetRotationDirection;
        [SerializeField] float walkingSpeed = 2f;
        [SerializeField] float runningSpeed = 5f;
        [SerializeField] float rotationSpeed = 15f;

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
            HandleRotation();
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

        private void HandleRotation()
        {
            Vector3 targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection +
                                      PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation =
                Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
    }

}