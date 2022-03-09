using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(CharacterMovement))]
public class Character : MonoBehaviourPun, IPunObservable, IDamageable {
    [HideInInspector] public bool IsDead => _health <= 0;

    [SerializeField] protected float _health = 100.0f;
    [SerializeField] protected int _score = 0;
    [SerializeField] protected int _scoreOnBreak = 10;

    public GunScriptableObject CurrentGun;

    public PhotonView PhotonView => _photonView;
    private PhotonView _photonView;

    private Color _randomColor;



    public virtual void Awake() {
        _photonView = GetComponent<PhotonView>();

        if (PhotonNetwork.IsConnected) {
            if (_photonView.IsMine) ChangeThePlayerColorToARandomColor();
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public virtual void TakeDamage(Character whoDamaged) {
        _health -= whoDamaged.CurrentGun.damageAmount;
    }

    public virtual void IncreaseScore(int value) {
        _score += value;
    }

    public virtual void IncreaseAmmo(int value) { }

    private void OnParticleCollision(GameObject other) {
        if (string.Equals(other.tag, "Bullet")) {
            Character character = other.GetComponentInParent<Character>();
            if (character) TakeDamage(whoDamaged: character);
        }
    }

    private void ChangeThePlayerColorToARandomColor() {
        _randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        GetComponent<SpriteRenderer>().color = _randomColor;
    }

    private Vector3 ColorToVector3(Color color) => new Vector3(color.r, color.g, color.b);
    private Color Vector3ToColor(Vector3 vector3) => new Color(vector3.x, vector3.y, vector3.z);

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            Vector3 colorVector = ColorToVector3(_randomColor);
            stream.SendNext(colorVector);
        } else {
            Vector3 colorVector = (Vector3)stream.ReceiveNext();
            Color _color = Vector3ToColor(colorVector);
            if (GetComponent<SpriteRenderer>().color != _color)
                GetComponent<SpriteRenderer>().color = _color;
        }
    }
}
