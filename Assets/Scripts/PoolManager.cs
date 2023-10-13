using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolManager
{
    static Dictionary<GameObject, ComponentPool> dict = new Dictionary<GameObject, ComponentPool>();
    public static void register(Component target, ComponentPool pool)
    {
        dict[target.gameObject] = pool;
    }
    public static void recall(Component target)
    {
        dict[target.gameObject].recall(target);
        dict.Remove(target.gameObject);
    }

    public static void reset()
    {
        dict = new Dictionary<GameObject, ComponentPool>();
    }
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
            Component instance = (Component)stack.Pop();
            instance.transform.position = position;
            instance.gameObject.SetActive(true);
            return instance;
        }
    }

    public void recall(Component target)
    {
        target.gameObject.SetActive(false);
        push(target);
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

public class ComponentPool<T> where T : Component
{
    protected ComponentPool pool;
    WaitForSeconds wait = null;

    public ComponentPool<T> setTimer(float lifetime)
    {
        wait = Helpers.GetWait(lifetime);
        return this;
    }

    public ComponentPool(T prefab)
    {
        pool = new ComponentPool();
        pool.prefab = prefab;
    }

    public T get(Vector3 position)
    {
        T instance = (T)pool.get(position);
        instance.gameObject.SetActive(true);
        if (wait != null) Orchestrator.context.StartCoroutine(WaitAndRetrieve(instance));
        else PoolManager.register(instance, pool);
        return instance;
    }

    IEnumerator WaitAndRetrieve(T target)
    {
        yield return wait;
        target.gameObject.SetActive(false);
        pool.push(target);
    }

}

public class EventPool<T>
{

}