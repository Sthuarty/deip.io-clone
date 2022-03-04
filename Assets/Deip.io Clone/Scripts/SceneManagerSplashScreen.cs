using UnityEngine;

public class SceneManagerSplashScreen : MonoBehaviour {
    [SerializeField] private PhotonManager m_PhotonManager;


    private void OnEnable() => m_PhotonManager.OnInitializeEvent += HasPhotonInitialized;
    private void OnDisable() => m_PhotonManager.OnInitializeEvent -= HasPhotonInitialized;

    private void Start() {
        m_PhotonManager.Initialize();
    }

    private void HasPhotonInitialized() {
        SceneLoader.Instance.LoadSceneWithFadeEffect("Menu", true);
    }
}
