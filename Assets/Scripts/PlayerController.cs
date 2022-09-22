using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float plSpeed = 0;
    public float maxSpeed = 20;
    public Animator playerAnimator;
    public Rigidbody plRigidbody;
    public GameObject focalPoint;
    public float plRotateSpeed;
    public GameObject[] bombs;
    public GameObject mine;
    public GameObject[] bombsOnTopOfPlayerHead;
    public GameObject bombSpwanerLocation;
    public float bombThrowSpeed;
    public int stickyBombleft = 0;
    public Slider healthBar;
    public float sprintSpeed = 0;
    float verticalInput = 0;
    float horizontalInput = 0;
    bool bombSelected = false;


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
        if (plRigidbody.velocity.magnitude > (maxSpeed + sprintSpeed))
        {
            plRigidbody.velocity = Vector3.ClampMagnitude(plRigidbody.velocity, maxSpeed);
        }
        if (plRigidbody.velocity.magnitude < (-maxSpeed - sprintSpeed))
        {
            plRigidbody.velocity = Vector3.ClampMagnitude(plRigidbody.velocity, -maxSpeed);
        }


        //stop player if user dont want to move player
        if (verticalInput == 0)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            //playerAnimator.SetBool("Static_b",false);
            playerAnimator.SetFloat("Speed_f", 0);
            sprintSpeed = 0;
            plSpeed = 1;
        }
        else
        {
            //playerAnimator.SetBool("Static_b", true);
            playerAnimator.SetFloat("Speed_f", 0.26f);
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                sprintSpeed = maxSpeed * 3;
                plSpeed = 3;
                playerAnimator.SetFloat("Speed_f", 0.6f);
            }
            else
            {
                plSpeed = 1;
                sprintSpeed = 0;
            }
        }

        //Throw bomb if selected
        if (Input.GetKeyDown(KeyCode.Space) && bombSelected)
        {
            ThrowBomb();
        }

        //selecte bomb
        if (Input.GetKeyDown(KeyCode.F))
        {
            bombSelected = true;
            SelectBomb();
        }

        //place Mines if mines are available
        if (Input.GetKeyDown(KeyCode.G) && GameManager.gameManager.minesCount > 0)
        {
            GameManager.gameManager.minesCount--;
            ThrowMine();
        }

        if (GameManager.gameManager.gameOver)
        {
            StartCoroutine(PerformDeath());
        }

        healthBar.value = GameManager.gameManager.playerLives;
    }


    private void OnTriggerEnter(Collider other)
    {
        //give three stikey bombs on collision with stickybomb powerup
        if (other.gameObject.CompareTag("stickyBomb"))
        {
            Destroy(other.gameObject);
            GameManager.gameManager.bombType = BombType.stickyBomb;
            stickyBombleft = 3;
            GameManager.gameManager.multiBombConuter = 0;
        }
        //give multibomb for 10 seconds on collision with multiBomb powerup
        if (other.gameObject.CompareTag("multiBomb"))
        {
            Destroy(other.gameObject);
            GameManager.gameManager.bombType = BombType.multiBomb;
            GameManager.gameManager.multiBombConuter = 0;
        }

        //give 1 mine on collision with multiBomb powerup
        if (other.gameObject.CompareTag("mine"))
        {
            Destroy(other.gameObject);
            GameManager.gameManager.bombType = BombType.multiBomb;
            GameManager.gameManager.multiBombConuter = 0;
        }

        //give one life on collision with health powerup
        if (other.gameObject.CompareTag("health"))
        {
            Destroy(other.gameObject);
            //max health can be 4
            GameManager.gameManager.playerLives = 4;


        }

    }


    void ThrowMine()
    {
        Instantiate(mine, transform.position, mine.transform.rotation);
    }

    void ThrowBomb()
    {
        GameObject bomb = bombs[0];
        bombSelected = false;
        playerAnimator.SetInteger("WeaponType_int", 10);
        HideAllBombs();
        playerAnimator.SetInteger("WeaponType_int", 0);
        //Debug.LogError(GameManager.gameManager.bombType);
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
            else
            {
                bombSelected = true;
                SelectBomb();
            }
        }
        if (GameManager.gameManager.bombType == BombType.multiBomb)
        {
            bomb = Instantiate(bombs[2], bombSpwanerLocation.transform.position, bombs[0].transform.rotation);

            if (GameManager.gameManager.bombType == BombType.multiBomb)
            {
                bombSelected = true;
                SelectBomb();
            }


        }
        //bomb.transform.position =new Vector3( bombSpwanerLocation.transform.position.x-1.68f, bombSpwanerLocation.transform.position.y, bombSpwanerLocation.transform.position.z);
        //bomb.transform.LookAt(focalPoint.transform);
        bomb.GetComponent<Rigidbody>().AddForce(focalPoint.transform.forward * bombThrowSpeed, ForceMode.Impulse);


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

    IEnumerator PerformDeath()
    {
        playerAnimator.SetInteger("DeathType_int", 1);
        playerAnimator.SetBool(" Death_b", true);
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0;
        GameManager.gameManager.gameOverText.text = "Game Over";
    }

}
