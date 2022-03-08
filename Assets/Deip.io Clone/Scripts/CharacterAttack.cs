using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Character))]
public class CharacterAttack : MonoBehaviourPun, IPunObservable {
    public List<GunScriptableObject> guns = new List<GunScriptableObject>();
    private int _currentGunIndex = 0;

    private Dictionary<int, int> ammo = new Dictionary<int, int>();    //  key: gunIndex  |  value: ammo quantity
    private Character _character;
    private Gun _gun;
    private float _attackCullDown, _nextAttackTime;

#if UNITY_EDITOR
    [SerializeField] private bool m_Debug;
#endif


    private void Awake() {
        _character = GetComponent<Character>();
        SetInitialLoadout();
    }

    private void SetInitialLoadout() {
        ammo.Add(0, 30);
        ammo.Add(1, 20);
        ammo.Add(2, 10);

        ChangeGun(guns[_currentGunIndex]);
    }

    #region CHANGE GUN
    public void ChangeGunHandler(GunScriptableObject gun) {
        if (PhotonNetwork.IsConnected) _character.PhotonView.RPC("RPC_ChangeGun", RpcTarget.All, (int)guns.IndexOf(gun));
        else ChangeGun(gun);
    }

    [PunRPC] public void RPC_ChangeGun(int gunIndex) => ChangeGun(guns[gunIndex]);

    private void ChangeGun(GunScriptableObject gun) {
        _character.CurrentGun = gun;
        _currentGunIndex = guns.IndexOf(gun);

        foreach (Transform child in transform)
            Destroy(child.gameObject);

        GameObject gunGO = Instantiate(gun.prefab, transform);
        _attackCullDown = gun.attackCullDown;
        _gun = gunGO.GetComponent<Gun>();
    }
    #endregion

    #region SHOOT
    public void ShootInputEvent(InputAction.CallbackContext context) { if (context.performed) ShootHandler(); }

    private void ShootHandler() {
        if (CanShoot) {
            if (PhotonNetwork.IsConnected) _character.PhotonView.RPC("RPC_GunFire", RpcTarget.All);
            else _gun.Fire();

            ammo[_currentGunIndex]--;
            _nextAttackTime = Time.time + _attackCullDown;
        }
    }

    [PunRPC] public void RPC_GunFire() => _gun.Fire();
    #endregion

    public void IncreaseAmmo(int value) => ammo[_currentGunIndex] += value;

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
                ChangeGunHandler(guns[value]);

#if UNITY_EDITOR
            if (m_Debug) Debug.Log($"Deip.io  |  CharacterAttack.cs  |  OnPhotonSerializeView()  |  READING  |  value -> {value}");
#endif
        }
    }
}
