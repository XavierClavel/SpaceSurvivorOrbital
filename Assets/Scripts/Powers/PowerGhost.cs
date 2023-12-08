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
    
    private static bool isShockwaveEnabled;
    private static int shockwaveDamage;
    private static float shockwaveMaxRange;
    private static status shockwaveElement;

    
    public override void Setup(PlayerData _)
    {
        base.Setup(_);
        isShockwaveEnabled = _.generic.boolA;
        shockwaveMaxRange = _.generic.floatA;
        shockwaveDamage = _.generic.intA;
        shockwaveElement = _.generic.elementA;
        
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
    
    public static void SpawnShockwave(Vector2 position)
    {
        Shockwave shockwaveGhost = instance.poolShockwaves.get(position);
        shockwaveGhost.transform.localScale = Vector3.zero;
        shockwaveGhost.Setup(shockwaveMaxRange, shockwaveDamage, shockwaveElement, 0);
        shockwaveGhost.setRecallMethod(delegate
        {
            PowerGhost.recallShockwave(shockwaveGhost);
            
        });
        shockwaveGhost.doShockwave(true);
    }
    
    public static void recallShockwave(Shockwave shockwaveToRecall)
    {
        Debug.Log("shockwave recalled");
        instance.poolShockwaves.recall(shockwaveToRecall);
    }

}
