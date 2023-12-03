using UnityEngine;

public class PowerGhost : Power, IEnnemyListener
{
    [SerializeField] private Ghost ghost;

    [SerializeField] private int spawnsEvery = 2;
    private int spawnCounter = 0;

    private ComponentPool<Ghost> poolGhosts;
    private static PowerGhost instance;

    
    public override void Setup(PlayerData _)
    {
        base.Setup(_);
        Ennemy.registerListener(this);
        poolGhosts = new ComponentPool<Ghost>(ghost);
        instance = this;
    }

    private void OnDestroy()
    {
        Ennemy.unregisterListener(this);
    }

    public void onEnnemyDeath(Vector2 position)
    {
        spawnCounter++;
        if (spawnCounter < spawnsEvery) return;
        spawnCounter = 0;
        SpawnGhost(position);
    }
    private void SpawnGhost(Vector2 position)
    {
        Ghost newGhost = poolGhosts.get(position);
        newGhost.Setup(fullStats);
    }

    public static void recallGhost(Ghost ghostToRecall)
    {
        instance.poolGhosts.recall(ghostToRecall);
    }

}
