using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Character))]
public class CharacterAttack : MonoBehaviour {
    private Character m_Character;
    private float m_NextAttackTime = 0f;

    [SerializeField] private float m_AttackCullDown = 0.5f;
    [SerializeField] private ParticleSystem m_ParticleSystem;


    private void Awake() {
        m_Character = GetComponent<Character>();
        m_ParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    [PunRPC]
    public void RPC_Shoot() {
        Shoot();
    }

    private void Shoot() {
        m_ParticleSystem.Play();
        m_NextAttackTime = Time.time + m_AttackCullDown;
    }

    public void ShootInputEvent(InputAction.CallbackContext context) {
        if (context.performed) {
            if (!m_Character.isDead && Time.time >= m_NextAttackTime) {
                if (PhotonNetwork.IsConnected) {
                    if (m_Character.photonView.IsMine)
                        m_Character.photonView.RPC("RPC_Shoot", RpcTarget.All);
                } else
                    Shoot();
            }
        }
    }
}
