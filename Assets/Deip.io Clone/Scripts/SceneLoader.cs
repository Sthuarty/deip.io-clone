using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

///<sumary>
/// Lembrar de adicionar as cenas no Scenes do Build Settings
///</sumary>
public class SceneLoader : Singleton<SceneLoader> {
    [SerializeField] private Animator m_Transition;
    [SerializeField] private float m_TransitionTime = 1f;

    public string CurrentSceneName => SceneManager.GetActiveScene().name;


    public void LoadSceneWithFadeEffect(string sceneName, bool photonLoad = false) => StartCoroutine(LoadSceneWithFadeEffectCoroutine(sceneName, photonLoad));
    private IEnumerator LoadSceneWithFadeEffectCoroutine(string sceneName, bool photonLoad = false) {
        if (m_Transition != null) {
            m_Transition.SetTrigger("FadeIn");
            yield return new WaitForSeconds(m_TransitionTime);

            if (photonLoad)
                PhotonNetwork.LoadLevel(sceneName);
            else
                SceneManager.LoadScene(sceneName);

            m_Transition.SetTrigger("FadeOut");
        }
    }
}
