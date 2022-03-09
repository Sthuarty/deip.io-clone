using System.Collections.Generic;
using System.Linq;
using BehaviorTree;
using UnityEngine;

public class CheckEnemyInFOVRange : Node {
    private Transform _transform;
    private static int _enemyLayerMask = 1 << 6;


    public CheckEnemyInFOVRange(Transform transform) => _transform = transform;

    public override NodeState Evaluate() {
        object target = GetData("target");

        if (target == null) {
            List<Collider2D> colliders = Physics2D.OverlapCircleAll(_transform.position, NPCEnemyBT.distanceToStartChasing, _enemyLayerMask).ToList();

            //  Como o enimigo tem a mesma layer (Character) das outras personagens, ele acaba entrando na lista, então o retiramos
            foreach (Collider2D collider in colliders.ToList())
                if (GameObject.ReferenceEquals(collider.transform, _transform)) colliders.Remove(collider);

            if (colliders.Count > 0) {
                parent.parent.SetData("target", colliders[0].transform);
                NPCEnemyBT.isChasing = true;

                state = NodeState.Success;
                return state;
            }

            state = NodeState.Failure;
            return state;
        }

        if (!NPCEnemyBT.isChasing)
            ClearData("target");

        state = NodeState.Success;
        return state;
    }
}
