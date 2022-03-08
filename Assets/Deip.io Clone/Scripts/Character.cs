using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(CharacterMovement))]
public class Character : MonoBehaviourPun, IPunObservable {
    [HideInInspector] public bool isDead;
    public CharacterAttack Attack => m_Attack;
    public PhotonView PhotonView => m_PhotonView;

    private PhotonView m_PhotonView;
    private PlayerInput m_PlayerInput;
    private CharacterMovement m_Movement;
    private CharacterLook m_Look;
    private CharacterAttack m_Attack;
    private Color m_RandomColor;

    [SerializeField] private int m_Life = 100;
    [SerializeField] private int m_Score = 0;


    private void Awake() {
        m_PhotonView = GetComponent<PhotonView>();
        m_PlayerInput = GetComponent<PlayerInput>();
        m_Movement = GetComponent<CharacterMovement>();
        m_Look = GetComponent<CharacterLook>();
        m_Attack = GetComponent<CharacterAttack>();

        if (PhotonNetwork.IsConnected) {
            if (m_PhotonView.IsMine)
                ChangeThePlayerColorToARandomColor();
            else {
                m_PlayerInput.enabled = false;
                m_Movement.enabled = false;
                m_Look.enabled = false;
                m_Attack.enabled = false;
            }

            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void ChangeThePlayerColorToARandomColor() {
        m_RandomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        GetComponent<SpriteRenderer>().color = m_RandomColor;
    }

    private void TakeDamage(int value) {
        m_Life -= value;
    }

    public void IncreaseScore(int value) {
        m_Score += value;
    }

    private void OnParticleCollision(GameObject other) {
        if (string.Equals(other.tag, "Bullet")) {
            Character character = other.GetComponentInParent<Character>();
            if (character) TakeDamage(character.Attack.CurrentGun.damageAmount);

            NPCEnemy enemy = other.GetComponentInParent<NPCEnemy>();
            if (enemy) TakeDamage(enemy.CurrentGun.damageAmount);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            Vector3 colorVector = ColorToVector3(m_RandomColor);
            stream.SendNext(colorVector);
        } else {
            Vector3 colorVector = (Vector3)stream.ReceiveNext();
            Color _color = Vector3ToColor(colorVector);
            if (GetComponent<SpriteRenderer>().color != _color)
                GetComponent<SpriteRenderer>().color = _color;
        }
    }

    private Vector3 ColorToVector3(Color color) => new Vector3(color.r, color.g, color.b);
    private Color Vector3ToColor(Vector3 vector3) => new Color(vector3.x, vector3.y, vector3.z);
}
