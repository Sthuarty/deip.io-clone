using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagerGame : MonoBehaviour {
    [SerializeField] private GameObject _uiPanelGun;
    [SerializeField] private GameObject _uiButtonGunPrefab;
    [SerializeField] private GameObject _characterPrefab;
    [Header("Player Start Position")]
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;

    private CharacterPlayer _playerCharacter;

    private PhotonManager photonManager;


    private void Awake() {
        photonManager = FindObjectOfType<PhotonManager>();
        if (photonManager == null) {
            Debug.LogError("SceneManagerGame.cs  |  Awake()  ->  The variable photonManager is NULL!");
            return;
        }
    }

    private void OnEnable() => photonManager.OnQuitEvent += HasPhotonQuited;
    private void OnDisable() => photonManager.OnQuitEvent -= HasPhotonQuited;

    private void Start() {
        InstantiateCharacter();
        ConfigureUI();
    }

    private void InstantiateCharacter() {
        Vector2 randomPosition = new Vector2(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY));

        GameObject character = PhotonNetwork.IsConnected
            ? PhotonNetwork.Instantiate(_characterPrefab.name, randomPosition, Quaternion.identity)
            : Instantiate(_characterPrefab, (Vector3)randomPosition, Quaternion.identity);

        _playerCharacter = character.GetComponent<CharacterPlayer>();
    }

    private void ConfigureUI() {
        foreach (Transform child in _uiPanelGun.transform)
            Destroy(child);

        foreach (GunScriptableObject gun in _playerCharacter.Attack.guns) {
            GameObject uiButtonGO = Instantiate(_uiButtonGunPrefab, _uiPanelGun.transform);
            uiButtonGO.GetComponentInChildren<Button>().onClick.AddListener(() => _playerCharacter.Attack.ChangeGunHandler(gun));
            uiButtonGO.GetComponentInChildren<TMP_Text>().text = gun.gunName;
        }
    }


    public void Quit() {  //  Chamada nos botões da UI
        photonManager.Quit();
    }

    private void HasPhotonQuited() {
        SceneLoader.Instance.LoadSceneWithFadeEffect("SplashScreen", true);
    }
}
