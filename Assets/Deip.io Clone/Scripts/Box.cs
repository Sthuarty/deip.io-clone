using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class Box : MonoBehaviour, IDamageable {
    [SerializeField] private int _maxCondition = 40;
    [SerializeField] private int _ammoOnBreak = 10;
    [SerializeField] private int _scoreOnBreak = 10;

    [SerializeField] private int _condition;

    [SerializeField] private List<GunScriptableObject> _guns;



    private void Awake() {
        _condition = _maxCondition;
        if (PhotonNetwork.IsConnected)
            DontDestroyOnLoad(this.gameObject);
    }

    private void OnParticleCollision(GameObject other) {
        if (string.Equals(other.tag, "Bullet"))
            TakeDamage(other.GetComponentInParent<Character>());
    }

    public void TakeDamage(Character whoDamaged) {
        _condition -= whoDamaged.CurrentGun.damageAmount;
        if (_condition <= 0) Break(whoDamaged);
    }

    private void Break(Character whoBroke) {
        whoBroke.IncreaseScore(_scoreOnBreak);
        whoBroke.IncreaseAmmo(_ammoOnBreak);

        CharacterPlayer playerChar = whoBroke.GetComponent<CharacterPlayer>();
        if (playerChar != null)
            DropLoot(to: playerChar);

        if (PhotonNetwork.IsConnected && GetComponent<PhotonView>().IsMine) PhotonNetwork.Destroy(gameObject);
        else if (!PhotonNetwork.IsConnected) Destroy(this.gameObject);
    }

    private void DropLoot(CharacterPlayer to) {
        foreach (GunScriptableObject gun in _guns.ToList()) {
            if (to.Attack.Guns.Contains(gun))
                _guns.Remove(gun);
        }

        foreach (GunScriptableObject gun in _guns) {
            float randomValue = UnityEngine.Random.Range(0, 101);
            if (randomValue <= gun.chanceToDropOnBoxLoot) {
                to.Attack.AddGun(gun);
                return;
            }
        }
    }
}
