using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagerGame : MonoBehaviour {
    [SerializeField] private GameObject _playerCharacterPrefab;
    [SerializeField] private GameObject _npcEnemyPrefab;
    [SerializeField] private int _npcEnemyCount = 10;

    [Header("Instantiate Positions Limits")]
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;
    Vector2 RandomPosition => new Vector2(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY));


    [Header("UI Settings")]
    [SerializeField] private GameObject _uiPanelGun;
    [SerializeField] private GameObject _uiButtonGunPrefab;

    [Space(10)]
    [SerializeField] private Transform[] _waypoints;

    private CharacterPlayer _playerCharacter;
    private PhotonManager _photonManager;


    private void Awake() {
        _photonManager = FindObjectOfType<PhotonManager>();
        if (_photonManager == null) {
            Debug.LogError("SceneManagerGame.cs  |  Awake()  ->  The variable photonManager is NULL!");
            return;
        }
    }

    private void OnEnable() => _photonManager.OnQuitEvent += HasPhotonQuited;
    private void OnDisable() => _photonManager.OnQuitEvent -= HasPhotonQuited;

    private void Start() {
        if (PhotonNetwork.IsConnected) {
            if (PhotonNetwork.IsMasterClient) {
                InstantiateNPCEnemies();
                InstantiateBoxes();
            }
        } else {
            InstantiateNPCEnemies();
            InstantiateBoxes();
        }

        InstantiateCharacter();
        ConfigureUI();
    }

    private void InstantiateCharacter() {
        GameObject character = InstantiateHandler(_playerCharacterPrefab, RandomPosition);
        _playerCharacter = character.GetComponent<CharacterPlayer>();
        Camera.main.GetComponent<CameraFollow>().target = character.transform;
    }

    private GameObject InstantiateHandler(GameObject prefab, Vector2 position) {
        return PhotonNetwork.IsConnected
                    ? PhotonNetwork.Instantiate(prefab.name, position, Quaternion.identity)
                    : Instantiate(prefab, (Vector3)position, Quaternion.identity);
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

    GameObject _lastNPCEnemyInstantiated;
    private void InstantiateNPCEnemies() {
        for (int i = 0; i < _npcEnemyCount; i++) {
            _lastNPCEnemyInstantiated = InstantiateHandler(_npcEnemyPrefab, RandomPosition);
            _lastNPCEnemyInstantiated.GetComponent<NPCEnemyBT>().waypoints = _waypoints;
        }
    }

    private void InstantiateBoxes() { }


    public void Quit() {  //  Chamada nos botões da UI
        _photonManager.Quit();
    }

    private void HasPhotonQuited() {
        SceneLoader.Instance.LoadSceneWithFadeEffect("SplashScreen", true);
    }
}
