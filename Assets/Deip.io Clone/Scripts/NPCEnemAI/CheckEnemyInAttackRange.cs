using BehaviorTree;
using UnityEngine;

public class CheckEnemyInAttackRange : Node {
    private Transform _transform;


    public CheckEnemyInAttackRange(Transform transform) => _transform = transform;

    public override NodeState Evaluate() {
        Transform target = (Transform)GetData("target");

        if (target != null) {
            float distanceToEnemy = Vector2.SqrMagnitude(target.position - _transform.position);

            if (distanceToEnemy <= NPCEnemyBT.distanceToStartAttacking) {
                state = NodeState.Success;
                return state;
            }
        }

        state = NodeState.Failure;
        return state;
    }
}
