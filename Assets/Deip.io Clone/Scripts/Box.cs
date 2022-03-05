using UnityEngine;

public class Box : MonoBehaviour {
    [SerializeField] private int m_MaxCondition = 40;
    [SerializeField] private int m_AmmoOnBreak = 10;
    [SerializeField] private int m_ScoreOnBreak = 10;

    [SerializeField] private int m_Condition;


    private void Awake() {
        m_Condition = m_MaxCondition;
    }

    private void OnParticleCollision(GameObject other) {
        if (string.Equals(other.tag, "Bullet"))
            TakeDamage(other.GetComponentInParent<Character>());
    }

    private void TakeDamage(Character whoDamaged) {
        m_Condition -= whoDamaged.CurrentGun.damageAmount;
        if (m_Condition <= 0) Break(whoDamaged);
    }

    private void Break(Character whoBroke) {
        whoBroke.IncreaseScore(m_ScoreOnBreak);
        whoBroke.IncreaseAmmo(m_AmmoOnBreak);
        Destroy(this.gameObject);
    }
}
