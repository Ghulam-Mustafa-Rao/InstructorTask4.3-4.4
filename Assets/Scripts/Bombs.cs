using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : MonoBehaviour
{

    public BombType bombType;
    public float radius;
    public float power;
    public bool explodeCalled = false;
    public ParticleSystem explosionEffect;
    float counter = 0;
    WaitForSeconds exploadWait = new WaitForSeconds(1.5f);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (!explodeCalled && counter > 1f && !gameObject.CompareTag("mineBomb"))
        {
            StartCoroutine(Explode());
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.LogError("Aya1");
            if (!explodeCalled && !gameObject.CompareTag("mineBomb"))
                StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        explodeCalled = true;
        yield return exploadWait;
        Debug.LogError("Aya2");
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        try
        {
            foreach (Collider hit in colliders)
            {
                //hit.isTrigger = true;
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    
                    if (hit.gameObject.CompareTag("Player"))
                    {
                        Destroy(hit.gameObject);
                        continue;
                    }
                    hit.gameObject.tag = "objectToDestroy";
                    if ( hit.gameObject.CompareTag("Enemy") || hit.gameObject.CompareTag("objectToDestroy"))
                    {
                        GameManager.gameManager.playerLives--;
                    }
                    
                    //hit.gameObject.tag = "bombExploded";
                    //rb.AddExplosionForce(power, transform.position, radius, 1.0F, ForceMode.Impulse);
                }

            }
        }
        catch (System.Exception)
        {
            throw;
        }
        Debug.LogError("Aya3");
        explosionEffect.Play();
        yield return new WaitForSeconds(0.5f);
        GameManager.gameManager.explosionBombAway = false;
        Destroy(this.gameObject);
    }

}
