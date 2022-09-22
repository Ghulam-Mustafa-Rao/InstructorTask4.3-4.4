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
    bool makeVelocityZero;
    WaitForSeconds exploadWait = new WaitForSeconds(1.5f);
    // Start is called before the first frame update
    void Start()
    {
        /* if (gameObject.CompareTag("stickyBomb"))
         {
             Destroy(gameObject.GetComponent<Rigidbody>());
         }*/
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
            if (!explodeCalled && !gameObject.CompareTag("mineBomb"))
                StartCoroutine(Explode());
        }


    }

    IEnumerator Explode()
    {
        explodeCalled = true;
        yield return exploadWait;
        GameObject parentOfStickyBomb = null;
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
                        GameManager.gameManager.playerLives--;
                        
                        //Destroy(hit.gameObject);
                        continue;
                    }
                    if (hit.gameObject.tag != "Bomb" && hit.gameObject.tag != "stickyBomb")
                        hit.gameObject.tag = "objectToDestroy";

                    //Debug.LogError(hit.gameObject.tag);
                    if (hit.gameObject.CompareTag("Enemy") || hit.gameObject.CompareTag("objectToDestroy"))
                    {
                        if (hit.gameObject != this.gameObject.transform.parent)
                        {
                            Destroy(hit.gameObject);
                        }
                        else
                        {
                            parentOfStickyBomb = hit.gameObject;
                        }
                    }

                    //hit.gameObject.tag = "bombExploded";
                    //rb.AddExplosionForce(power, transform.position, radius, 1.0F, ForceMode.Impulse);
                }
                else
                {
                    if (hit.gameObject.CompareTag("Enemy"))
                    {
                        Destroy(hit.gameObject);
                    }
                }

            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        //Debug.LogError("Aya3");
        explosionEffect.Play();
        if (parentOfStickyBomb != null)
            Destroy(parentOfStickyBomb);
        yield return new WaitForSeconds(0.5f);
        GameManager.gameManager.explosionBombAway = false;
        Destroy(this.gameObject);
    }


    void DestroyRigidbody()
    {

    }
}
