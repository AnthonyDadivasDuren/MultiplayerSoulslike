using UnityEngine;
using Unity.Netcode;

namespace ADD
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        [Header("Position")] public NetworkVariable<Vector3> NetworkPosition =
            new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);

        public NetworkVariable<Quaternion> NetworkRotation = new NetworkVariable<Quaternion>(Quaternion.identity,
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public Vector3 networkPositionVelocity;
        public float networkPositionSmoothTime = 0.1f;
        public float networkRotationSmoothTime = 0.1f;
    }
}