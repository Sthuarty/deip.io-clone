using Photon.Pun;
using TMPro;
using UnityEngine;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks {
    [SerializeField] private TMP_InputField m_InputField;


    public void CreateRoom() => PhotonNetwork.CreateRoom(m_InputField.text);
    public void JoinRoom() => PhotonNetwork.JoinRoom(m_InputField.text);

    public override void OnJoinedRoom() {
        SceneLoader.Instance.LoadSceneWithFadeEffect("Game", true);
    }
}
