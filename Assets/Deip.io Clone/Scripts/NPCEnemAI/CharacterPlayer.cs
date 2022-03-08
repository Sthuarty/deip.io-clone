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

    public override void IncreaseAmmo(int value) => _attack.IncreaseAmmo(value);
}
