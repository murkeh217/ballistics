using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class EnemyController: MonoBehaviour {
    [SerializeField] float timeToChangeDirection;

    [SerializeField] Rigidbody rb;
    // Use this for initialization
    public void Start () {
        ChangeDirection();

        rb = GetComponent<Rigidbody>();
    }
         
    // Update is called once per frame
    public void Update () {
        timeToChangeDirection -= Time.deltaTime;
     
        if (timeToChangeDirection <= 0) {
            ChangeDirection();
        }
     
        rb.velocity = transform.forward * 2;
    }
     
    IEnumerator Respawn(float timeToDespawn, float timeToRespawn) {
        yield return new WaitForSeconds(timeToDespawn);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(timeToRespawn);
        gameObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //StartCoroutine(Respawn(5f,7f));
    }

    private void ChangeDirection() {
        float angle = Random.Range(0f, 360f);
        Quaternion quat = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector3 newUp = quat * Vector3.up;
        newUp.z = 0;
        newUp.Normalize();
        transform.forward = newUp;
        timeToChangeDirection = 1.5f;
    }
}