using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    public abstract T get(Vector3 position, Quaternion rotation);

    public void push(T newObject)
    {
        stack.Push(newObject);
    }
}

public class GameObjectPool : Pool<GameObject>
{
    WaitForSeconds wait = null;

    public GameObjectPool setTimer(float lifetime)
    {
        wait = Helpers.GetWait(lifetime);
        return this;
    }

    public GameObjectPool(GameObject prefab)
    {
        this.prefab = prefab;
    }

    private GameObject getInstance(Vector3 position, Quaternion rotation)
    {
        if (stack.Count == 0)
        {
            return GameObject.Instantiate(prefab, position, rotation);
        }
        else
        {
            GameObject instance = stack.Pop();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.gameObject.SetActive(true);
            return instance;
        }
    }

    public override GameObject get(Vector3 position, Quaternion rotation)
    {
        
        GameObject instance = getInstance(position, rotation);
        instance.gameObject.SetActive(true);
        if (wait != null) Orchestrator.context.StartCoroutine(WaitAndRetrieve(instance));
        return instance;
    }

    

    public GameObject get(Vector3 position)
    {
        GameObject instance = get(position, Quaternion.identity);
        instance.gameObject.SetActive(true);
        if (wait != null) Orchestrator.context.StartCoroutine(WaitAndRetrieve(instance));
        return instance;
    }

    IEnumerator WaitAndRetrieve(GameObject target)
    {
        yield return wait;
        target.gameObject.SetActive(false);
        push(target);
    }
}

public class ComponentPool : Pool<Component>
{
    public override Component get(Vector3 position, Quaternion rotation)
    {
        if (stack.Count == 0)
        {
            return GameObject.Instantiate(prefab, position, rotation);
        }
        else
        {
            Component instance = stack.Pop();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
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
        pool = new GameObjectPool(prefab);
        pool.prefab = prefab;
        this.lifetime = lifetime;
    }

    public GameObject get(Vector3 position)
    {
        return get(position, Quaternion.identity);
    }
    
    public GameObject get(Vector3 position, Quaternion rotation)
    {
        GameObject instance = pool.get(position, rotation);
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
        pool = new ComponentPool
        {
            prefab = prefab
        };
    }

    public void recall(T instance)
    {
        pool.recall(instance);
    }

    public T get(Vector3 position)
    {
        return get(position, Quaternion.identity);
    }
    
    public T get(Vector3 position, Vector3 rotation)
    {
        return get(position, Quaternion.Euler(rotation));
    }
    
    public T get(Vector3 position, Quaternion rotation)
    {
        T instance = (T)pool.get(position, rotation);
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