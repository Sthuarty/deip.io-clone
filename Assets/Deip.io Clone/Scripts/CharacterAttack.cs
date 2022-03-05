using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Character))]
public class CharacterAttack : MonoBehaviour {
    /* [HideInInspector] */
    public int ammoCount = 10;
    public GunScriptableObject currentGun;

    private Character m_Character;
    private float m_NextAttackTime = 0f;
    private ParticleSystem m_ParticleSystem;

    [SerializeField] private float m_AttackCullDown = 0.5f;

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
        ammoCount--;
    }

    public void ShootInputEvent(InputAction.CallbackContext context) {
        if (context.performed) {
            if (!m_Character.isDead && ammoCount > 0 && !IsInCullDownTime) {
                if (PhotonNetwork.IsConnected) {
                    if (m_Character.photonView.IsMine)
                        m_Character.photonView.RPC("RPC_Shoot", RpcTarget.All);
                } else
                    Shoot();
            }
        }
    }

    private bool IsInCullDownTime => Time.time < m_NextAttackTime;
}
