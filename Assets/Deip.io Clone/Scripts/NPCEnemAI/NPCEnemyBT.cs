using System.Collections.Generic;
using BehaviorTree;
public class NPCEnemyBT : Tree {
    public static float speed = 2f;
    public static float distanceToStartChasing = 5.0f;
    public static float distanceToStopChasing = 200.0f;
    public static float distanceToAttack = 75.0f;
    public static float distanceClosestToTheEnemy = 10f;
    public static bool isChasing;


    protected override Node SetupTree() {
        Node root = new Selector(new List<Node> {
            new Sequence( new List<Node> {
                new CheckEnemyInAttackRange(transform),
                new TaskAttack(transform),
            }),

            new Sequence( new List<Node> {
                new CheckEnemyInFOVRange(transform),
                new TaskChase(transform),
            }),

            new TaskPatrol(transform),
        });

        return root;
    }
}
