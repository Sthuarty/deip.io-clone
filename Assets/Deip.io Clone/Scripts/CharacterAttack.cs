using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Character))]
public class CharacterAttack : MonoBehaviourPun, IPunObservable {
    public GunScriptableObject CurrentGun => guns[m_CurrentGunIndex];
    public List<GunScriptableObject> guns = new List<GunScriptableObject>();

    private Dictionary<int, int> ammo = new Dictionary<int, int>();    //  key: gunIndex  |  value: ammo quantity
    private Character m_Character;
    private Gun m_Gun;
    private int m_CurrentGunIndex = 0;
    private float m_AttackCullDown, m_NextAttackTime;

#if UNITY_EDITOR
    [SerializeField] private bool m_Debug;
#endif


    private void Awake() {
        m_Character = GetComponent<Character>();
        SetInitialLoadout();
    }

    private void SetInitialLoadout() {
        ammo.Add(0, 30);
        ammo.Add(1, 20);
        ammo.Add(2, 10);

        ChangeGun(guns[m_CurrentGunIndex]);
    }

    #region CHANGE GUN
    public void ChangeGunHandler(GunScriptableObject gun) {
        if (PhotonNetwork.IsConnected) m_Character.PhotonView.RPC("RPC_ChangeGun", RpcTarget.All, (int)guns.IndexOf(gun));
        else ChangeGun(gun);
    }

    [PunRPC] public void RPC_ChangeGun(int gunIndex) => ChangeGun(guns[gunIndex]);

    private void ChangeGun(GunScriptableObject gun) {
        m_CurrentGunIndex = guns.IndexOf(gun);

        foreach (Transform child in transform)
            Destroy(child.gameObject);

        GameObject gunGO = Instantiate(gun.prefab, transform);
        m_AttackCullDown = gun.attackCullDown;
        m_Gun = gunGO.GetComponent<Gun>();
    }
    #endregion

    #region SHOOT
    public void ShootInputEvent(InputAction.CallbackContext context) { if (context.performed) ShootHandler(); }

    private void ShootHandler() {
        if (CanShoot) {
            if (PhotonNetwork.IsConnected) m_Character.PhotonView.RPC("RPC_GunFire", RpcTarget.All);
            else m_Gun.Fire();

            ammo[m_CurrentGunIndex]--;
            m_NextAttackTime = Time.time + m_AttackCullDown;
        }
    }

    [PunRPC] public void RPC_GunFire() => m_Gun.Fire();
    #endregion

    public void IncreaseAmmo(int value) => ammo[m_CurrentGunIndex] += value;

    private bool CanShoot => !m_Character.isDead && HasAmmo && !IsInCullDownTime;

    private bool HasAmmo => ammo[m_CurrentGunIndex] > 0;

    public bool IsInCullDownTime => Time.time < m_NextAttackTime;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(m_CurrentGunIndex);
#if UNITY_EDITOR
            if (m_Debug) Debug.Log($"Deip.io  |  CharacterAttack.cs  |  OnPhotonSerializeView()  |  WRINTING  |  m_CurrentGunIndex -> {m_CurrentGunIndex}");
#endif
        } else {
            int value = (int)stream.ReceiveNext();
            if (value != m_CurrentGunIndex)
                ChangeGunHandler(guns[value]);

#if UNITY_EDITOR
            if (m_Debug) Debug.Log($"Deip.io  |  CharacterAttack.cs  |  OnPhotonSerializeView()  |  READING  |  value -> {value}");
#endif
        }
    }
}
