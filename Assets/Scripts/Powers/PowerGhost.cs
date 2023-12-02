using UnityEngine;

public class PowerGhost : Power, IEnnemyListener
{
    [SerializeField] private Ghost ghost;

    [SerializeField] private int spawnsEvery = 2;
    private int spawnCounter = 0;

    
    public override void Setup(PlayerData _)
    {
        base.Setup(_);
        Ennemy.registerListener(this);
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
        Ghost newGhost = Instantiate(ghost);
        newGhost.transform.position = position;
        newGhost.Setup(fullStats);
    }

}
