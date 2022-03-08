using Photon.Pun;
using UnityEngine;

public class NPCEnemy : MonoBehaviour {
    public GunScriptableObject CurrentGun => _gun;
    [SerializeField] private GunScriptableObject _gun;
    [SerializeField] private int _life = 100;
    [SerializeField] private int _scoreOnBreak = 10;
    [SerializeField] private float _attackCullDown = 0.5f;
    private float _nextAttackTime;
    private PhotonView _photonView;
    private Gun m_Gun;


    private void Awake() {
        _photonView = GetComponent<PhotonView>();
        m_Gun = GetComponentInChildren<Gun>();
    }

    #region SHOOT
    public void ShootHandler() {
        if (!IsInCullDownTime) {
            if (PhotonNetwork.IsConnected) _photonView.RPC("RPC_GunFire", RpcTarget.All);
            else m_Gun.Fire();

            _nextAttackTime = Time.time + _attackCullDown;
        }
    }

    [PunRPC] public void RPC_GunFire() => m_Gun.Fire();

    #endregion

    public bool IsInCullDownTime => Time.time < _nextAttackTime;

    public void TakeDamage(Character whoDamaged) {
        _life -= whoDamaged.Attack.CurrentGun.damageAmount;
        if (_life <= 0) Break(whoDamaged);
    }

    private void Break(Character whoBroke) {
        whoBroke.IncreaseScore(_scoreOnBreak);
        Destroy(this.gameObject);
    }
    /* public bool TakeDamage(int value) {
        _life -= value;
        bool hasDied = _life <= 0;
        if (hasDied) Die();
        return hasDied;
    } */
}
