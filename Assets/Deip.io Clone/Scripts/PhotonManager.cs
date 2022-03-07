using CommonTypes;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks {
    public event Event OnInitializeEvent;
    public event Event OnJoinRoomEvent;
    public event Event OnQuitEvent;

    private string gameVersion = "1";


    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Initialize() {
        if (!PhotonNetwork.IsConnected) {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        } else
            PhotonNetwork.JoinLobby();
    }

    public void Quit() {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() {
        OnQuitEvent?.Invoke();
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() {
        OnInitializeEvent?.Invoke();
    }

    public void JoinSomeRoom() {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        PhotonNetwork.CreateRoom(null, new RoomOptions());
    }

    public override void OnJoinedRoom() {
        OnJoinRoomEvent?.Invoke();
    }

    public void SetPlayerName(string value) {
        PhotonNetwork.NickName = value;
    }
}
