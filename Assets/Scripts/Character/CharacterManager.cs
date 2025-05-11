using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace ADD
{
    public class CharacterManager : NetworkBehaviour
    {
        public CharacterController characterController;
        
        CharacterNetworkManager characterNetworkManager;
        
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
            
            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }

        protected virtual void Update()
        {
            // IF THIS CHARACTER IS BERING CONTROLLED BY THE OWNER, THEN ASSIGN ITS NETWORK POSITION TO THE POSTION OF THIS CHARACTER
            if (IsOwner)
            {
                characterNetworkManager.NetworkPosition.Value = transform.position;
                characterNetworkManager.NetworkRotation.Value = transform.rotation;
            }
            // IF THIS CHARACTER IS BEING CONTROLLED FROM ELSE WHERE, THEN ASSIGN ITS POSITION HERE LOCALLY BY THE POSITION OF ITS NETWORK TRANSFORM
            else
            {
                // Position
                transform.position = Vector3.SmoothDamp
                    (transform.position,
                        characterNetworkManager.NetworkPosition.Value,
                        ref characterNetworkManager.networkPositionVelocity,
                        characterNetworkManager.networkPositionSmoothTime);
                // Rotation
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    characterNetworkManager.NetworkRotation.Value,
                    characterNetworkManager.networkRotationSmoothTime);
            }
        }
    }
}