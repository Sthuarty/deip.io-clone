using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(CharacterMovement))]
public class Character : MonoBehaviour {
    [HideInInspector] public bool isDead;
    [HideInInspector] public PhotonView photonView;

    private PlayerInput m_PlayerInput;
    private CharacterMovement m_Movement;
    private CharacterLook m_Look;
    private CharacterAttack m_Attack;
    [SerializeField] private int m_Life = 100;


    private void Awake() {
        photonView = GetComponent<PhotonView>();
        m_PlayerInput = GetComponent<PlayerInput>();
        m_Movement = GetComponent<CharacterMovement>();
        m_Look = GetComponent<CharacterLook>();
        m_Attack = GetComponent<CharacterAttack>();

        if (!photonView.IsMine) {
            m_PlayerInput.enabled = false;
            m_Movement.enabled = false;
            m_Look.enabled = false;
            m_Attack.enabled = false;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void TakeDamage(int value) {
        m_Life -= value;
    }

    private void OnParticleCollision(GameObject other) {
        if (string.Equals(other.tag, "Bullet")) {
            TakeDamage(other.GetComponentInParent<Cannon>().damage);
        }
    }
}
