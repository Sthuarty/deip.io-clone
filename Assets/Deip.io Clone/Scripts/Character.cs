using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(CharacterMovement))]
public class Character : MonoBehaviourPunCallbacks {
    private PhotonView m_PhotonView;
    private CharacterMovement m_movement;
    private CharacterLook m_look;


    private void Awake() {
        m_PhotonView = GetComponent<PhotonView>();
        m_movement = GetComponent<CharacterMovement>();
        m_look = GetComponent<CharacterLook>();

        if (!m_PhotonView.IsMine) {
            m_movement.enabled = false;
            m_look.enabled = false;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
