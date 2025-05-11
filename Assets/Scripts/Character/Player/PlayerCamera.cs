using UnityEngine;



namespace ADD
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header("References")]
        public static PlayerCamera instance;
        public PlayerManager player;       
        public Camera cameraObject;
        

        [Header("Camera Settings")] 
        private Vector3 cameraVelocity;
        private float cameraSmoothSpeed = 1f;   // THE BIGGER THIS NUMBER, THE LONGER FOR THE CAMERA TO REACH ITS POSITION DURING MOVEMENT
    

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
                FollowTarget();
                
                // ROTATE AROUND THE PLAYER
                // COLLIDE WITH OBJECTS
            }
        }

        private void FollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }
    }
}