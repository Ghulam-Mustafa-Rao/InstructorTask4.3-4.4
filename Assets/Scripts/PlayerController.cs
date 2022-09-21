using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float plSpeed = 0;
    public float maxSpeed = 20;
    public Animator playerAnimator;
    public Rigidbody plRigidbody;
    public GameObject focalPoint;
    public float plRotateSpeed;
    public GameObject[] bombs;
    public GameObject[] bombsOnTopOfPlayerHead;
    public GameObject bombSpwanerLocation;
    public float bombThrowSpeed;
    public int stickyBombleft = 0;
    float verticalInput = 0;
    float horizontalInput = 0;


    // Start is called before the first frame update
    void Start()
    {
        HideAllBombs();
    }


    // Update is called once per frame
    void Update()
    {
        //get Inputs
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        //Move player 
        plRigidbody.AddForce(focalPoint.transform.forward * plSpeed * verticalInput, ForceMode.Impulse);
        transform.Rotate(Vector3.up * horizontalInput * Time.deltaTime * plRotateSpeed);


        //Limit speed of player
        if (plRigidbody.velocity.magnitude > maxSpeed)
        {
            plRigidbody.velocity = Vector3.ClampMagnitude(plRigidbody.velocity, maxSpeed);
        }
        if (plRigidbody.velocity.magnitude < -maxSpeed)
        {
            plRigidbody.velocity = Vector3.ClampMagnitude(plRigidbody.velocity, -maxSpeed);
        }


        //stop player if user dont want to move player
        if (verticalInput == 0)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            //playerAnimator.SetBool("Static_b",false);
            playerAnimator.SetFloat("Speed_f", 0);
        }
        else
        {
            //playerAnimator.SetBool("Static_b", true);
            playerAnimator.SetFloat("Speed_f", 0.26f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowBomb();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            SelectBomb();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("stickyBomb"))
        {
            Destroy(other.gameObject);
            GameManager.gameManager.bombType = BombType.stickyBomb;
            stickyBombleft = 3;
            GameManager.gameManager.multiBombConuter = 0;
        }
        if (other.gameObject.CompareTag("multiBomb"))
        {
            Destroy(other.gameObject);
            GameManager.gameManager.bombType = BombType.multiBomb;
            GameManager.gameManager.multiBombConuter = 0;
        }

        if (other.gameObject.CompareTag("health"))
        {
            Destroy(other.gameObject);
            GameManager.gameManager.playerLives++;
            if (GameManager.gameManager.playerLives > 3)
                GameManager.gameManager.playerLives = 3;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bombExploded"))
        {
            //kill Player
            Debug.LogError("Hit by bomb");
        }
    }

    void ThrowBomb()
    {
        GameObject bomb = bombs[0];
        playerAnimator.SetInteger("WeaponType_int", 10);
        HideAllBombs();
        Debug.LogError(GameManager.gameManager.bombType);
        if (GameManager.gameManager.bombType == BombType.explosiveBomb && !GameManager.gameManager.explosionBombAway)
        {
            bomb = Instantiate(bombs[0], bombSpwanerLocation.transform.position, bombs[0].transform.rotation);
            GameManager.gameManager.explosionBombAway = true;
        }
        if (GameManager.gameManager.bombType == BombType.stickyBomb && stickyBombleft > 0)
        {
            bomb = Instantiate(bombs[1], bombSpwanerLocation.transform.position, bombs[0].transform.rotation);
            stickyBombleft--;
            if (stickyBombleft <= 0)
            {
                GameManager.gameManager.bombType = BombType.explosiveBomb;
            }
        }
        if (GameManager.gameManager.bombType == BombType.multiBomb)
        {
            bomb = Instantiate(bombs[2], bombSpwanerLocation.transform.position, bombs[0].transform.rotation);
        }
        //bomb.transform.position =new Vector3( bombSpwanerLocation.transform.position.x-1.68f, bombSpwanerLocation.transform.position.y, bombSpwanerLocation.transform.position.z);
        //bomb.transform.LookAt(focalPoint.transform);
        bomb.GetComponent<Rigidbody>().AddForce(focalPoint.transform.forward * bombThrowSpeed, ForceMode.Impulse);
        playerAnimator.SetInteger("WeaponType_int", 0);
    }

    void SelectBomb()
    {
        HideAllBombs();
        if (GameManager.gameManager.bombType == BombType.explosiveBomb)
        {
            bombsOnTopOfPlayerHead[0].SetActive(true);
        }
        if (GameManager.gameManager.bombType == BombType.stickyBomb)
        {
            bombsOnTopOfPlayerHead[1].SetActive(true);
        }
        if (GameManager.gameManager.bombType == BombType.multiBomb)
        {
            bombsOnTopOfPlayerHead[2].SetActive(true);
        }
    }

    void HideAllBombs()
    {
        foreach (GameObject bomb in bombsOnTopOfPlayerHead)
        {
            bomb.SetActive(false);
        }
    }
}
