using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject focalPoint;
    public GameObject player;
    public NavMeshAgent agent;
    public Animator enemyAnimator;
    public Vector3[] enemyMovepoints;
    public Vector3 destination;
    public Vector3 preDestination;
    private void Awake()
    {
        player = GameManager.gameManager.player;
        enemyMovepoints = GameManager.gameManager.enemyMovepoints;
        StartCoroutine(ChangeDestination());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(player.transform.position);
        enemyAnimator.SetFloat("Speed_f", 0.26f);
        agent.SetDestination(destination);

        Debug.LogError("Enemy position = " + transform.position.x + " , " + transform.position.y + " , " + transform.position.z + " , ");
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("stickyBomb"))
        {
            collision.gameObject.transform.SetParent(transform);
        }
        if (collision.gameObject.CompareTag("mineBomb"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("bombExploded"))
        {
            //kill Enemy
        }
    }
    IEnumerator ChangeDestination()
    {
        while(true)
        {
            preDestination = destination;
            while (preDestination == destination)
            {
                destination = enemyMovepoints[Random.Range(0, enemyMovepoints.Length)];
            }

            yield return new WaitForSeconds(1.5f);
        } 
    }

    void ThrowBomb()
    {
        transform.LookAt(player.transform.position);
    }
}
