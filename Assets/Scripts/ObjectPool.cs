using UnityEngine;
using System.Collections.Generic;

public class ObjectPool {

    private int capacity = 100;
    private Dictionary<string, Stack<GameObject>> pools = new Dictionary<string, Stack<GameObject>>();

    public ObjectPool(int capacity = 100) {
        this.capacity = capacity;
    }

    public GameObject instance(GameObject prefab) {
        return instance(prefab, Vector3.zero, Quaternion.identity);
    }

    public GameObject instance(GameObject prefab, Vector3 position) {
        return instance(prefab, position, Quaternion.identity);
    }

    public GameObject instance(GameObject prefab, Vector3 position, Quaternion rotation) {

        //request pool
        var pool = requestPool(prefab.name);

        //pop object from pool
        GameObject instance = null;
        if (pool.Count > 0) {
            instance = pool.Pop();
        }

        //new instance
        else {
            instance = GameObject.Instantiate(prefab);
        }

        //return
        instance.transform.position = position;
        instance.transform.rotation = rotation;
        instance.name = prefab.name;
        instance.SetActive(true);

        return instance;
    }

    public void destroy(GameObject instance) {

        //request pool
        var pool = requestPool(instance.name);

        //reserve
        if (pool.Count < capacity) {
            instance.SetActive(false);
            pool.Push(instance);
        }

        //destroy
        else {
            GameObject.Destroy(instance);
        }

    }

    public void clear() {

        //destroy objects
        foreach (var i in pools) {
            var pool = i.Value;
            while (pool.Count > 0) {
                GameObject.Destroy(pool.Pop());
            }
        }

        //clear pools
        pools.Clear();

    }

    //get pool per object name

    Stack<GameObject> requestPool(string name) {
        Stack<GameObject> pool;
        if (!pools.TryGetValue(name, out pool)) {
            pool = new Stack<GameObject>(capacity);
            pools.Add(name, pool);
        }
        return pool;
    }

}
