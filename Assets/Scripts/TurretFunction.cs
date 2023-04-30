using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;

public class TurretFunction : MonoBehaviour
{
    public static float fireInterval = 2;
    [SerializeField] GameObject[] targets;
    [SerializeField] GameObject closestTarget;
    [SerializeField] float range=40;
    [SerializeField] string target_tag;
    [SerializeField] float timeToNextFire;

    // Start is called before the first frame update
    void Start()
    {
        timeToNextFire = fireInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToNextFire > 0)
        {
            timeToNextFire -= Time.deltaTime;
        }
        else
        {
            timeToNextFire = fireInterval;

            FindClosestTarget();

            if (closestTarget && Vector3.Distance(closestTarget.transform.position, transform.position) < range)
            {
                LaunchMissile();
            }


        }



    }

    void FindClosestTarget()
    {
        targets = GameObject.FindGameObjectsWithTag(target_tag);

        if (targets.Length == 0)
        {
            return;
        }

        closestTarget = targets[0];
        for(int i = 0; i < targets.Length; i++)
        {
            if (Vector3.Distance(transform.position, targets[i].transform.position) < Vector3.Distance(transform.position, closestTarget.transform.position))
            {
                closestTarget = targets[i];
            }
        }

    }

    void LaunchMissile()
    {
        GetComponent<AutoTurret>().LaunchMissile(closestTarget);
    }
}