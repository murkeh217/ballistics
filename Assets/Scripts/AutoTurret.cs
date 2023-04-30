using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurret : MonoBehaviour
{
    [SerializeField] GameObject missilePrefab;
    [SerializeField] GameObject spawnPoint;

    ObjectPool pool = new ObjectPool(10);

    public void LaunchMissile(GameObject target)
    {
        
        if (spawnPoint)
        {
            
            GameObject missile = pool.instance(missilePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);

        }
        else
        {
            GameObject missile = pool.instance(missilePrefab, transform.position, Quaternion.identity);

        }
        missilePrefab.GetComponent<Projectile>().AssignTarget(target);
    }

}