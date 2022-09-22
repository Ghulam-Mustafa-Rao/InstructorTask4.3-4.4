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
    public List<Vector3> enemyMovepoints;
    public Vector3 destination;
    public Vector3 preDestination;
    public float bombThrowSpeed;
    public bool collisionDone = false;
    public bool bombsAway = false;
    public GameObject bomb;
    public GameObject bombSpwanerLocation;
    public GameObject bombsOnTopOfPlayerHead;
    public float firstRadius;
    public float secondRadius;
    Collider[] colliders;
    public bool changeDestination = true;
    bool ExplodeMineCalled = false;
    float startTime = 0;
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

        bombsOnTopOfPlayerHead.SetActive(false);
        //if(transform.position - player.transform.position )
        startTime += Time.deltaTime;
        if (!bombsAway && startTime > 5)
        {
            colliders = Physics.OverlapSphere(transform.position, firstRadius);
            foreach (Collider hit in colliders)
            {
                //hit.isTrigger = true;
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    if (hit.gameObject.CompareTag("Player"))
                    {
                        bombsOnTopOfPlayerHead.SetActive(true);
                        break;
                    }
                }
            }

            colliders = Physics.OverlapSphere(transform.position, secondRadius);
            foreach (Collider hit in colliders)
            {
                //hit.isTrigger = true;
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {

                    if (hit.gameObject.CompareTag("Player"))
                    {
                        ThrowBomb();
                        break;
                    }
                }
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("stickyBomb") && collisionDone == false)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            collisionDone = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            collision.gameObject.transform.SetParent(this.gameObject.transform);
            collision.gameObject.transform.position = this.gameObject.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("mineBomb"))
        {
            if (!ExplodeMineCalled)
                StartCoroutine(ExplodeMine(other.gameObject));

        }
    }
    IEnumerator ChangeDestination()
    {
        while (true)
        {
            if (changeDestination)
            {
                preDestination = destination;
                if (Random.Range(0, 3) == 1)
                {
                    destination = player.transform.position;
                }
                else
                {
                    while (preDestination == destination)
                    {
                        destination = enemyMovepoints[Random.Range(0, enemyMovepoints.Count)];
                    }
                }
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

    void ThrowBomb()
    {
        transform.LookAt(player.transform.position);
        destination = player.transform.position;
        changeDestination = false;
        GameObject ob = Instantiate(bomb, bombSpwanerLocation.transform.position, bomb.transform.rotation);
        ob.GetComponent<Rigidbody>().AddForce(focalPoint.transform.forward * bombThrowSpeed, ForceMode.Impulse);
        bombsAway = true;
        StartCoroutine(SetBombsAwayFalse());

    }

    IEnumerator SetBombsAwayFalse()
    {

        changeDestination = true;
        yield return new WaitForSeconds(3f);
        bombsAway = false;
    }

    IEnumerator ExplodeMine(GameObject other)
    {
        ExplodeMineCalled = true;
        other.gameObject.transform.Find("Explosion").GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.5f);
        Destroy(other.gameObject);
        Destroy(this.gameObject);
    }
}
