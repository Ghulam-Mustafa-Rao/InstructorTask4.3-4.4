using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BombType
{
    explosiveBomb,
    stickyBomb,
    multiBomb,
    mines
}

public class GameManager : MonoBehaviour
{

    public static GameManager gameManager;

    public int waveNo = 1;
    public GameObject player;
    public Vector3[] enemyMovepoints;
    public BombType bombType;
    public GameObject floor;
    public bool explosionBombAway = false;
    public bool gameStarted = false;
    public TextMeshProUGUI waveNoText;
    public float multiBombConuter = 0;
    public int playerLives = 3;
    private void Awake()
    {
        if (gameManager == null)
            gameManager = this;

        bombType = BombType.explosiveBomb;
    }
    // Start is called before the first frame update
    void Start()
    {
        waveNoText.gameObject.SetActive(true);
        Invoke("DisableWaveNoTextAndStartGame", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        floor.tag = "Floor";
        waveNoText.text = "Wave No : " + waveNo;

        if (bombType == BombType.multiBomb)
        {
            multiBombConuter += Time.deltaTime;
        }
        else
        {
            multiBombConuter = 0;
        }

        if (multiBombConuter > 10 )
        {
            bombType = BombType.explosiveBomb;
            multiBombConuter = 0;
        }



    }

    void DisableWaveNoTextAndStartGame()
    {
        waveNoText.gameObject.SetActive(false);
        gameStarted = true;
    }

    
}
