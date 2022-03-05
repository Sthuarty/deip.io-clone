
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SceneManagerGame : MonoBehaviourPunCallbacks {
    [SerializeField] private GameObject m_CharacterPrefab;
    [Header("Player Start Position")]
    [SerializeField] private float m_MinX;
    [SerializeField] private float m_MaxX;
    [SerializeField] private float m_MinY;
    [SerializeField] private float m_MaxY;


    private void Start() {
        InstantiateCharacter();

        if (PhotonNetwork.IsMasterClient)
            Debug.Log($"Start -> IsMasterClient: {PhotonNetwork.IsMasterClient}", gameObject);
    }

    private void InstantiateCharacter() {
        Vector2 randomPosition = new Vector2(Random.Range(m_MinX, m_MaxX), Random.Range(m_MinY, m_MaxY));
        GameObject character = PhotonNetwork.Instantiate(m_CharacterPrefab.name, randomPosition, Quaternion.identity);

        Color randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        character.GetComponent<SpriteRenderer>().color = randomColor;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        Debug.Log($"OnPlayerEnteredRoom() {newPlayer.NickName}", gameObject);
    }
}
