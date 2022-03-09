using BehaviorTree;
using UnityEngine;

public class CheckEnemyInAttackRange : Node {
    private Transform _transform;


    public CheckEnemyInAttackRange(Transform transform) => _transform = transform;

    public override NodeState Evaluate() {
        Transform target = (Transform)GetData("target");

        if (target != null && target.gameObject.activeInHierarchy) {
            float distanceToEnemy = Vector2.SqrMagnitude(target.position - _transform.position);

            if (distanceToEnemy > NPCEnemyBT.distanceToStopChasing)
                NPCEnemyBT.isChasing = false;

            if (distanceToEnemy <= NPCEnemyBT.distanceToAttack) {
                state = NodeState.Success;
                return state;
            }
        } else
            NPCEnemyBT.isChasing = false;

        state = NodeState.Failure;
        return state;
    }
}
