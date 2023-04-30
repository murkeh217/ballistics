using System.Collections;
using System.Numerics;
using UnityEngine;
using EZCameraShake;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody _RB;
    [SerializeField] float waitTime = 2f;

    [SerializeField] GameObject sparkyExplosion;
    [SerializeField] GameObject etherealExplosion;
    [SerializeField] GameObject smokyExplosion;

    [SerializeField] float radius = 50f;
    [SerializeField] float power = 100f;

    [SerializeField] float delay = 0.8f;
    
    [SerializeField] GameObject target;
    [SerializeField] float manuverability = 0.05f;
    public static float speed=40;
    [SerializeField] float minSpeed = 20;

    Quaternion previousRotation;
    Vector3 previousTargetPosition;

    [SerializeField] float life = 5;
//setup pool capacity
    ObjectPool pool = new ObjectPool(10);

    private void Awake()
    {
        _RB = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        life -= Time.deltaTime;
        
        previousRotation = transform.rotation;
        AdjustAngle();
        SpeedAdjustment();
        if (target)
        {
            previousTargetPosition = target.transform.position;
        }

        waitTime -= Time.deltaTime;
        
        if (waitTime <= 0)
        {
            Vector3 smoothedDelta = Vector3.MoveTowards(transform.position, target.transform.position, Time.fixedDeltaTime * speed);
            _RB.MovePosition(smoothedDelta);
        }
        

    }

    void AdjustAngle()
    {
        if (target)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.forward, target.transform.position - transform.position), manuverability);
        }

        if (Vector3.Distance(transform.position, target.transform.position)<0.5)
        {
            manuverability = 1;
        }

    }

    void SpeedAdjustment()
    {
        if ( target && target.transform.position == previousTargetPosition && transform.rotation != previousRotation)
        {
            if (speed > minSpeed)
            {
                speed=speed*0.99f;
            }
        }
    }

    public void AssignTarget (GameObject newTarget)
    {
        target = newTarget;
    }

  
//pause before hitting, good effect
    IEnumerator HitStop()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(1.2f);
        Time.timeScale = 1f;
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(delay);
                //deactivate to pool
        pool.destroy(gameObject);
    }
    


    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.name == "2")
        {
            StartCoroutine(HitStop());
            
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
            }
            CameraShaker.Instance.ShakeOnce(1f, 1f, 1f, 1f);
            //activate from pool
            pool.instance(smokyExplosion, transform.position, transform.rotation);
            StartCoroutine(WaitAndDestroy());
        }

        if (other.collider.name == "1")
        {
            CameraShaker.Instance.ShakeOnce(2f, 2f, 0.5f, 0.5f);
            StartCoroutine(HitStop());
            StartCoroutine(WaitAndDestroy());
            pool.instance(sparkyExplosion, transform.position, transform.rotation);
            StartCoroutine(WaitAndDestroy());

        }

        if (other.collider.name == "0")
        {
            StartCoroutine(HitStop());
            pool.instance(etherealExplosion, transform.position, transform.rotation);
            StartCoroutine(WaitAndDestroy());

        }
        
        if(other.gameObject.CompareTag("Land"))
        {
            pool.destroy(gameObject);
        }
    }
}
