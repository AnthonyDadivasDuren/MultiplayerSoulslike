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
            
            // HANDLE MOVEMENT
            playerLocomotionManager.handleAllMovement();
        }
    }
}