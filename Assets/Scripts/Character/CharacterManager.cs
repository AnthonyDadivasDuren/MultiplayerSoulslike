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
                characterNetworkManager.Networkposition.Value = transform.position;
            }
            // IF THIS CHARACTER IS BEING CONTROLLED FROM ELSE WHERE, THEN ASSIGN ITS POSITION HERE LOCALLY BY THE POSITION OF ITS NETWORK TRANSFORM
            else
            {
                transform.position = Vector3.SmoothDamp
                    (transform.position,
                        characterNetworkManager.Networkposition.Value,
                        ref characterNetworkManager.networkPositionVelocity,
                        characterNetworkManager.networkPositionSmoothTime);
            }
        }
    }
}