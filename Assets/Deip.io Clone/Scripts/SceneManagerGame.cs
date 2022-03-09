using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagerGame : MonoBehaviour {
    [SerializeField] private GameObject _playerCharacterPrefab;
    [SerializeField] private GameObject _npcEnemyPrefab;
    [SerializeField] private int _npcEnemyCount = 10;
    [SerializeField] private GameObject _boxPrefab;
    [SerializeField] private int _boxCount = 10;

    [Header("Instantiate Positions Limits")]
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;
    Vector2 RandomPosition => new Vector2(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY));


    [Header("UI Settings")]
    [SerializeField] private GameObject _uiPanelGun;
    [SerializeField] private GameObject _uiButtonGunPrefab;
    [SerializeField] private TMP_Text _uiTextHealth;
    [SerializeField] private TMP_Text _uiTextAmmo;
    [SerializeField] private TMP_Text _uiTextScore;
    [SerializeField] private UIShowHide _uiPanelGameOver;
    [SerializeField] private TMP_Text _uiTextGameOverScore;


    [Space(10)]
    [SerializeField] private Transform[] _waypoints;

    private CharacterPlayer _playerCharacter;
    private PhotonManager _photonManager;

    private List<NPCEnemyBT> _enemies = new List<NPCEnemyBT>();


    private void Awake() {
        _photonManager = FindObjectOfType<PhotonManager>();
        if (_photonManager == null) {
            Debug.LogError("SceneManagerGame.cs  |  Awake()  ->  The variable photonManager is NULL!");
            return;
        }
    }

    private void OnEnable() => _photonManager.OnQuitEvent += Quit;
    private void OnDisable() => _photonManager.OnQuitEvent -= Quit;

    private void Start() {
        if (!PhotonNetwork.IsConnected || (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)) {
            InstantiateNPCEnemies();
            InstantiateBoxes();
        }

        InstantiateCharacter();
        ConfigureUI();
    }

    private GameObject InstantiateHandler(GameObject prefab, Vector2 position, bool roomObject = false) {
        return PhotonNetwork.IsConnected
                    ? roomObject
                        ? PhotonNetwork.InstantiateRoomObject(prefab.name, position, Quaternion.identity)
                        : PhotonNetwork.Instantiate(prefab.name, position, Quaternion.identity)
                    : Instantiate(prefab, (Vector3)position, Quaternion.identity);
    }

    private void InstantiateCharacter() {
        GameObject character = InstantiateHandler(_playerCharacterPrefab, RandomPosition);
        _playerCharacter = character.GetComponent<CharacterPlayer>();
        Camera.main.GetComponent<CameraFollow>().target = character.transform;

        _playerCharacter.HealthChangedEvent += UpdateHealthUI;
        _playerCharacter.Attack.AmmoChangedEvent += UpdateAmmoUI;
        _playerCharacter.ScoreChangedEvent += UpdateScoreUI;
        _playerCharacter.CharacterDiedEvent += GameOver;
    }

    private void InstantiateNPCEnemies() {
        for (int i = 0; i < _npcEnemyCount; i++)
            InstantiateHandler(_npcEnemyPrefab, RandomPosition, roomObject: true);
    }

    private void InstantiateBoxes() {
        for (int i = 0; i < _boxCount; i++)
            InstantiateHandler(_boxPrefab, RandomPosition, roomObject: true);
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

    private void UpdateScoreUI(int value) => _uiTextScore.text = value.ToString();
    private void UpdateAmmoUI(int value) => _uiTextAmmo.text = value.ToString();
    private void UpdateHealthUI(int value) => _uiTextHealth.text = value.ToString();

    private void GameOver() {
        _uiTextGameOverScore.text = _uiTextScore.text;
        _uiPanelGameOver.Show();
    }

    public void QuitHandler() {  //  Chamada nos botões da UI
        if (PhotonNetwork.IsConnected) _photonManager.Quit();
        else Quit();
    }

    private void Quit() => SceneLoader.Instance.LoadSceneWithFadeEffect("SplashScreen", true);

    private void OnDestroy() {
        _playerCharacter.HealthChangedEvent -= UpdateHealthUI;
        _playerCharacter.Attack.AmmoChangedEvent -= UpdateAmmoUI;
        _playerCharacter.ScoreChangedEvent -= UpdateScoreUI;
    }
}
