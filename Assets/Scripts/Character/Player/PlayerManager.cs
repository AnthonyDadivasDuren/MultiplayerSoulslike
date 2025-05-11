using UnityEngine;

namespace ADD
{
    public class PlayerManager : CharacterManager
    {
        PlayerLocomotionManager playerLocomotionManager;
        override protected void Awake()
        {
            base.Awake();
            
            // DO MORE STUFF ONLY FOR THE PLAYER
            
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }
        override protected void Update()
        {
            base.Update();
            
            // IF NOT OWNER OF THIS GAME OBJECT, THEN CAN'T CONTROL OR EDIT IT
            if (!IsOwner) { return;}
            
            // HANDLE MOVEMENT
            playerLocomotionManager.handleAllMovement();
        }
    }
}