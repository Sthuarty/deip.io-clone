using CommonTypes;
using Photon.Pun;

public class ConnectToServer : MonoBehaviourPunCallbacks {

    public event Event OnJoinedLobbyEvent;

    public void Initialize() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() {
        OnJoinedLobbyEvent?.Invoke();
    }
}
