using Photon.Pun;
using UnityEngine;

public class NPCEnemy : Character {
    [SerializeField] private GunScriptableObject _gun;
    [SerializeField] private int _life = 100;
    [SerializeField] private float _attackCullDown = 0.5f;
    private float _nextAttackTime;
    private PhotonView _photonView;
    private Gun m_Gun;


    public override void Awake() {
        base.Awake();
        CurrentGun = _gun;
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

    public override void TakeDamage(Character whoDamaged) {
        _life -= whoDamaged.CurrentGun.damageAmount;
        if (_life <= 0) Die(whoDamaged);
    }

    private void Die(Character whoKilled) {
        whoKilled.IncreaseScore(_scoreOnBreak);
        Destroy(this.gameObject);
    }
}
