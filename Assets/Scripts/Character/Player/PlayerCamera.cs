using UnityEngine;



namespace ADD
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header("References")]
        public static PlayerCamera instance;
        public PlayerManager player;       
        public Camera cameraObject;
        [SerializeField] Transform cameraPivotTransform;
        
        
        // CHANGE THESE TO TWEAK CAMERA PERFORMANCE
        [Header("Camera Settings")] 
        private float cameraSmoothSpeed = 1f;   // THE BIGGER THIS NUMBER, THE LONGER FOR THE CAMERA TO REACH ITS POSITION DURING MOVEMENT
        [SerializeField] float leftAndRightRotationSpeed = 220;
        [SerializeField] float upAndDownRotationSpeed = 220;
        [SerializeField] private float minimumPivot = -30;  // THE LOWEST POINT YOU ARE ABLE TO LOOK DOWN
        [SerializeField] private float maximumPivot = 60;   // THE HIGHEST POINT YOU ARE ABLE TO LOOK UP
        
        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        
    

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
        }

        public void HandleAllCameraActions()
        {
            if (player != null)
            {
                // FOLLOW THE PLAYER
                HandleFollowTarget();
                
                // ROTATE AROUND THE PLAYER
                HandleRotations();
                // COLLIDE WITH OBJECTS
            }
        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotations()
        {
            // IF LOCKED ON, FORCE ROTATION TOWARDS TARGET
            // ELSE ROTATE REGULARLY
            
            // ROTATE LEFT AND RIGHT BASED ON THE HORIZONTAL CAMERA MOVEMENT INPUT  
            leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput) * leftAndRightRotationSpeed * Time.deltaTime;
            // ROTATE UP AND DOWN BASED ON THE VERTICAL CAMERA MOVEMENT INPUT 
            upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput) * upAndDownRotationSpeed * Time.deltaTime;
            // CLAMPT THE UP AND DOWN LOOK ANGLE BETWEEN AND MAX VALUE
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);
            
            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;
            
            // ROTATE THIS GAME OBJECT LEFT AND RIGHT
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;
            
            // ROTATE THE PIVOT GAMEOBJECT UP AND DOWN
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
            
        }
    }
}