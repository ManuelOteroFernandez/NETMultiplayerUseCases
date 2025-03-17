using Unity.Netcode.Samples.MultiplayerUseCases.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Unity.Netcode.Samples.MultiplayerUseCases.Proximity
{
    /// <summary>
    /// Manages the color of a Networked object
    /// </summary>
    public class ColorPlayer : NetworkBehaviour
    {
        NetworkVariable<Color32> m_NetworkedColor = new NetworkVariable<Color32>();
        Material m_Material;

        void Awake()
        {
            m_Material = GetComponent<Renderer>().material;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsClient)
            {
                ServerChangeColorRpc();
                m_NetworkedColor.OnValueChanged += OnClientColorChanged;
                OnClientColorChanged(m_Material.color, m_NetworkedColor.Value);
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (IsClient)
            {
                m_NetworkedColor.OnValueChanged -= OnClientColorChanged;
            }
        }


        [Rpc(SendTo.Server)]
        void ServerChangeColorRpc()
        {
            m_NetworkedColor.Value = MultiplayerUseCasesUtilities.GetRandomColor();
        }

        void OnClientColorChanged(Color32 previousColor, Color32 newColor)
        {
            m_Material.color = newColor;
        }

    }
}
