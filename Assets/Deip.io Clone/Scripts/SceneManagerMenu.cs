using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagerMenu : MonoBehaviour {
    const string playerNamePrefKey = "PlayerName";

    [SerializeField] private PhotonManager m_PhotonManager;
    [SerializeField] private TMP_InputField m_NameInputField;
    [SerializeField] private Button m_JoinButton;


    private void OnEnable() => m_PhotonManager.OnJoinRoomEvent += HasJoinedRoom;
    private void OnDisable() => m_PhotonManager.OnJoinRoomEvent -= HasJoinedRoom;

    private void Start() {
        if (PlayerPrefs.HasKey(playerNamePrefKey))
            m_NameInputField.text = PlayerPrefs.GetString(playerNamePrefKey);

        m_JoinButton.onClick.AddListener(StartGame);
    }

    public void StartGame() {
        string playerName = m_NameInputField.text;
        if (!string.IsNullOrEmpty(playerName) && !string.IsNullOrWhiteSpace(playerName)) {
            PlayerPrefs.SetString(playerNamePrefKey, playerName);
            m_PhotonManager.SetPlayerName(playerName);
        }
        m_PhotonManager.JoinSomeRoom();
    }

    private void HasJoinedRoom() {
        SceneLoader.Instance.LoadSceneWithFadeEffect("Game", true);
    }
}
