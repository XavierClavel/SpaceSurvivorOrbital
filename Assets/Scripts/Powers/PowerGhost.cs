using UnityEngine;

public class PowerGhost : Power, IEnnemyListener
{
    [SerializeField] private Ghost ghost;
    [SerializeField] private Shockwave ghostShockwave;

    [SerializeField] private int spawnsEvery = 2;
    private int spawnCounter = 0;

    private ComponentPool<Ghost> poolGhosts;
    private ComponentPool<Shockwave> poolShockwaves;
    
    private static PowerGhost instance;

    
    public override void Setup(PlayerData _)
    {
        base.Setup(_);
        Ennemy.registerListener(this);
        poolGhosts = new ComponentPool<Ghost>(ghost);
        poolShockwaves = new ComponentPool<Shockwave>(ghostShockwave);
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
    
    public static Shockwave SpawnShockwave(Vector2 position)
    {
        return instance.poolShockwaves.get(position);
    }
    
    public static void recallShockwave(Shockwave shockwaveToRecall)
    {
        instance.poolShockwaves.recall(shockwaveToRecall);
    }

}
