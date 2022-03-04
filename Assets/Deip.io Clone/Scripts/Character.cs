using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(CharacterMovement))]
public class Character : MonoBehaviourPunCallbacks {
    private PhotonView m_PhotonView;
    private CharacterMovement m_movement;


    private void Awake() {
        m_PhotonView = GetComponent<PhotonView>();
        m_movement = GetComponent<CharacterMovement>();

        if (!m_PhotonView.IsMine)
            m_movement.enabled = false;

        DontDestroyOnLoad(this.gameObject);
    }
}
