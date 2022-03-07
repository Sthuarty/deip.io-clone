using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagerGame : MonoBehaviour {
    [SerializeField] private GameObject m_UIPanelGun;
    [SerializeField] private GameObject m_UIButtonGun;
    [SerializeField] private GameObject m_CharacterPrefab;
    [Header("Player Start Position")]
    [SerializeField] private float m_MinX;
    [SerializeField] private float m_MaxX;
    [SerializeField] private float m_MinY;
    [SerializeField] private float m_MaxY;

    private Character m_Character;

    private PhotonManager photonManager;


    private void Awake() {
        photonManager = FindObjectOfType<PhotonManager>();
        if (!photonManager) Debug.LogError("SceneManagerGame.cs  |  Awake()  ->  The variable photonManager is NULL!");
    }

    private void OnEnable() => photonManager.OnQuitEvent += HasPhotonQuited;
    private void OnDisable() => photonManager.OnQuitEvent -= HasPhotonQuited;

    private void Start() {
        InstantiateCharacter();
        ConfigureUI();
    }

    private void ConfigureUI() {
        foreach (Transform child in m_UIPanelGun.transform)
            Destroy(child);

        foreach (GunScriptableObject gun in m_Character.Attack.guns) {
            GameObject uiButtonGO = Instantiate(m_UIButtonGun, m_UIPanelGun.transform);
            uiButtonGO.GetComponentInChildren<Button>().onClick.AddListener(() => m_Character.Attack.ChangeGunHandler(gun));
            uiButtonGO.GetComponentInChildren<TMP_Text>().text = gun.gunName;
        }
    }

    private void InstantiateCharacter() {
        Vector2 randomPosition = new Vector2(Random.Range(m_MinX, m_MaxX), Random.Range(m_MinY, m_MaxY));

        GameObject character = PhotonNetwork.IsConnected
            ? PhotonNetwork.Instantiate(m_CharacterPrefab.name, randomPosition, Quaternion.identity)
            : Instantiate(m_CharacterPrefab, (Vector3)randomPosition, Quaternion.identity);

        m_Character = character.GetComponent<Character>();
    }

    public void Quit() {  //  Chamada nos botões da UI
        photonManager.Quit();
    }

    private void HasPhotonQuited() {
        SceneLoader.Instance.LoadSceneWithFadeEffect("SplashScreen", true);
    }
}
