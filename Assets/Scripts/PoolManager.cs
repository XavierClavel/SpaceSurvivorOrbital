using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolManager
{

}


public abstract class Pool<T> where T : Object
{
    public T prefab;
    protected Stack<T> stack = new Stack<T>();
    public T get()
    {
        if (stack.Count == 0)
        {
            return GameObject.Instantiate(prefab);
        }
        else return stack.Pop();
    }

    public abstract T get(Vector3 position);

    public void push(T newObject)
    {
        stack.Push(newObject);
    }
}

public class GameObjectPool : Pool<GameObject>
{
    public override GameObject get(Vector3 position)
    {
        if (stack.Count == 0)
        {
            return GameObject.Instantiate(prefab, position, Quaternion.identity);
        }
        else
        {
            GameObject instance = stack.Pop();
            instance.transform.position = position;
            instance.SetActive(true);
            return instance;
        }
    }
}

public class ComponentPool : Pool<Component>
{
    public override Component get(Vector3 position)
    {
        if (stack.Count == 0)
        {
            return GameObject.Instantiate(prefab, position, Quaternion.identity);
        }
        else
        {
            ParticleSystem instance = (ParticleSystem)stack.Pop();
            instance.transform.position = position;
            instance.gameObject.SetActive(true);
            return instance;
        }
    }
}

public class GameObjectTimedPool
{
    private float lifetime;
    protected GameObjectPool pool;

    public GameObjectTimedPool(GameObject prefab, float lifetime)
    {
        pool = new GameObjectPool();
        pool.prefab = prefab;
        this.lifetime = lifetime;
    }

    public GameObject get(Vector3 position)
    {
        GameObject instance = pool.get(position);
        instance.SetActive(true);
        return instance;
    }

    IEnumerator WaitAndRetrieve(GameObject target)
    {
        yield return lifetime;
        target.SetActive(false);
        pool.push(target);
    }

}

public class ParticleSystemTimedPool
{
    protected ComponentPool pool;
    WaitForSeconds wait;

    public ParticleSystemTimedPool(ParticleSystem prefab, float lifetime)
    {
        pool = new ComponentPool();
        pool.prefab = prefab;
        wait = Helpers.GetWait(lifetime);
    }

    public ParticleSystem get(Vector3 position)
    {
        ParticleSystem instance = (ParticleSystem)pool.get(position);
        instance.gameObject.SetActive(true);
        Orchestrator.context.StartCoroutine(WaitAndRetrieve(instance));
        return instance;
    }

    IEnumerator WaitAndRetrieve(ParticleSystem target)
    {
        yield return wait;
        target.gameObject.SetActive(false);
        pool.push(target);
    }

}

public class EventPool<T>
{

}