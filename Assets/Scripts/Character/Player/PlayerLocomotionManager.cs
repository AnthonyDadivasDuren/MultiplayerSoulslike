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

        override protected void Awake()
        {
            base.Awake();
            
            // DO MORE STUFF ONLY FOR THE PLAYER
            player = GetComponent<PlayerManager>();
        }
        
        public void handleAllMovement()
        {
            //  GROUNDED MOVEMENT
            //  AERIAL MOVEMENT
          
        }
        
        private void HandleGroundedMovement()
        {
            moveDirection = 
        }
    }
}