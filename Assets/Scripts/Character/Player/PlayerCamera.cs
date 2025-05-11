using UnityEngine;
using UnityEngine.Serialization;


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
        [SerializeField] float minimumPivot = -30;  // THE LOWEST POINT YOU ARE ABLE TO LOOK DOWN
        [SerializeField] float maximumPivot = 60;   // THE HIGHEST POINT YOU ARE ABLE TO LOOK UP
        [SerializeField] float cameraCollisionRadius = 0.2f; 
        [SerializeField] LayerMask collideWithLayers;
        
        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 camerObjectPosition;    //  USED FOR CAMERA COLLISIONS (MOVES THE CAMERA OBJECT TO THIS POSITION UPON COLLIDING) 
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPosition;  // VALUES USED FOR CAMERA COLLISIONS  
        private float targetCameraZPosition;   // VALUES USED FOR CAMERA COLLISIONS 
        
    

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
            cameraZPosition = cameraObject.transform.localPosition.z;
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
                HandleCollisions();
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

        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            
            // DIRECTION FOR COLLISION CHECK
            RaycastHit hit;
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position; 
            direction.Normalize();
            
            // CHECK IF THERE IS AN OBJECT IN FRONT OF THE DESIRED DIRECTION ^ (SEE ABOVE)
            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction,out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {
                // IF THERE IS, WE GET OUR DISTANCE FROM IT 
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                // THEN EQUATE THE TARGETS Z POSITION TO THE FOLLOWING
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }
            
            // IF TARGET POSITION IS LESS THAN THE COLLISION RADIUS, SUBSTRACT THE COLLISION RADIUS (MAKING IT SNAP BACK)
            if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }
            
            // THEN APPLY FINAL POSITION USING A LERP OVER A TIME OF 0.2F
            camerObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = camerObjectPosition;
        }
    }
}