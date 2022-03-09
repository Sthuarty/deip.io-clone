using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(CharacterMovement))]
public class CharacterPlayer : Character {
    public CharacterAttack Attack => _attack;

    private PlayerInput _playerInput;
    private CharacterMovement _movement;
    private CharacterLook _look;
    private CharacterAttack _attack;

    public event CommonTypes.EventWithIntParameter HealthChangedEvent;
    public event CommonTypes.EventWithIntParameter ScoreChangedEvent;


    public override void Awake() {
        base.Awake();

        _playerInput = GetComponent<PlayerInput>();
        _movement = GetComponent<CharacterMovement>();
        _look = GetComponent<CharacterLook>();
        _attack = GetComponent<CharacterAttack>();

        if (PhotonNetwork.IsConnected && !PhotonView.IsMine) {
            _playerInput.enabled = false;
            _movement.enabled = false;
            _look.enabled = false;
            _attack.enabled = false;
        }
    }

    private void Start() {
        HealthChangedEvent?.Invoke((int)_health);
        ScoreChangedEvent?.Invoke(_score);
    }

    public override void IncreaseAmmo(int value) => _attack.IncreaseAmmo(value);

    public override void TakeDamage(Character whoDamaged) {
        base.TakeDamage(whoDamaged);
        HealthChangedEvent?.Invoke((int)_health);
    }

    public override void IncreaseScore(int value) {
        base.IncreaseScore(value);
        ScoreChangedEvent?.Invoke(_score);
    }
}
