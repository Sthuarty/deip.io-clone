using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Character))]
public class CharacterAttack : MonoBehaviourPun, IPunObservable {
    public List<GunScriptableObject> Guns => _guns;
    [SerializeField] private List<GunScriptableObject> _guns = new List<GunScriptableObject>();
    private int _currentGunIndex = 0;
    private Gun _gunController;
    private List<GunScriptableObject> _allGameGuns = new List<GunScriptableObject>();


    private Dictionary<int, int> ammo = new Dictionary<int, int>();    //  key: gunIndex  |  value: ammo quantity
    private CharacterPlayer _character;
    private float _attackCullDown, _nextAttackTime;

    public event CommonTypes.EventWithIntParameter AmmoChangedEvent;
    public event CommonTypes.Event NewGunObtainedEvent;

#if UNITY_EDITOR
    [SerializeField] private bool m_Debug;
#endif


    private void Awake() {
        _character = GetComponent<CharacterPlayer>();
        _guns.Add(_character.CurrentGun);
        _gunController = GetComponentInChildren<Gun>();
        _allGameGuns = SceneManagerGame.Instance.allGunsList;

        //  Set Initial Loadout
        ammo.Add(0, 30);
        ammo.Add(1, 20);
        ammo.Add(2, 10);
    }

    private void Start() => ChangeGun(_character.CurrentGun);

    #region CHANGE GUN
    public void ChangeGunHandler(GunScriptableObject gun) {
        if (PhotonNetwork.IsConnected && _character.PhotonView.IsMine) _character.PhotonView.RPC("RPC_ChangeGun", RpcTarget.All, (int)_allGameGuns.IndexOf(gun));
        else ChangeGun(gun);
    }

    [PunRPC] public void RPC_ChangeGun(int gunIndex) => ChangeGun(_allGameGuns[gunIndex]);

    private void ChangeGun(GunScriptableObject gun) {
        _character.CurrentGun = gun;
        _currentGunIndex = _allGameGuns.IndexOf(gun);

        foreach (Transform child in transform)
            Destroy(child.gameObject);

        GameObject gunGO = Instantiate(gun.prefab, transform);
        _attackCullDown = gun.attackCullDown;
        _gunController = gunGO.GetComponent<Gun>();
        AmmoChangedEvent?.Invoke(ammo[_currentGunIndex]);
    }
    #endregion

    #region SHOOT
    public void ShootInputEvent(InputAction.CallbackContext context) { if (context.performed) ShootHandler(); }

    private void ShootHandler() {
        if (CanShoot) {
            if (PhotonNetwork.IsConnected) _character.PhotonView.RPC("RPC_GunFire", RpcTarget.All);
            else _gunController.Fire();

            ammo[_currentGunIndex]--;
            AmmoChangedEvent?.Invoke(ammo[_currentGunIndex]);
            _nextAttackTime = Time.time + _attackCullDown;
        }
    }

    [PunRPC] public void RPC_GunFire() => _gunController.Fire();
    #endregion

    public void IncreaseAmmo(int value) {
        ammo[_currentGunIndex] += value;
        AmmoChangedEvent?.Invoke(ammo[_currentGunIndex]);
    }

    public void AddGun(GunScriptableObject gun) {
        _guns.Add(gun);
        NewGunObtainedEvent?.Invoke();
    }

    private bool CanShoot => !_character.IsDead && HasAmmo && !IsInCullDownTime;

    private bool HasAmmo => ammo[_currentGunIndex] > 0;

    public bool IsInCullDownTime => Time.time < _nextAttackTime;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(_currentGunIndex);
#if UNITY_EDITOR
            if (m_Debug) Debug.Log($"Deip.io  |  CharacterAttack.cs  |  OnPhotonSerializeView()  |  WRINTING  |  m_CurrentGunIndex -> {_currentGunIndex}");
#endif
        } else {
            int value = (int)stream.ReceiveNext();
            if (value != _currentGunIndex)
                ChangeGunHandler(_allGameGuns[value]);

#if UNITY_EDITOR
            if (m_Debug) Debug.Log($"Deip.io  |  CharacterAttack.cs  |  OnPhotonSerializeView()  |  READING  |  value -> {value}");
#endif
        }
    }
}
