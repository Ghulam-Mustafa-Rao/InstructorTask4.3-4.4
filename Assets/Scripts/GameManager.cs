using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public List<Vector3> enemyMovepoints;
    public BombType bombType;
    public GameObject floor;
    public bool explosionBombAway = false;
    public bool gameStarted = false;
    public TextMeshProUGUI waveNoText;
    public float multiBombConuter = 0;
    public int playerLives = 3;
    public GameObject infoPanel;
    public TextMeshProUGUI livesText;
    public Image currentBombimage;
    public TextMeshProUGUI minesAvailableText;
    public TextMeshProUGUI gameOverText;
    public bool gameOver = false;

    public int minesCount = 0;
    private void Awake()
    {
        if (gameManager == null)
            gameManager = this;

        bombType = BombType.explosiveBomb;
    }
    // Start is called before the first frame update
    void Start()
    {
        infoPanel.SetActive(true);
        waveNoText.gameObject.SetActive(true);

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

        if (multiBombConuter > 10)
        {
            bombType = BombType.explosiveBomb;
            multiBombConuter = 0;
        }

        livesText.text = "Lives : " + playerLives;
        minesAvailableText.text = "Mines Available : " + minesCount;
        if (playerLives <= 0)
        {
            playerLives = 0;
            gameOver = true;
        }

        switch (bombType)
        {
            case BombType.explosiveBomb:
                {
                    currentBombimage.color = new Color(149, 149, 149, 255);
                }
                break;
            case BombType.multiBomb:
                {
                    currentBombimage.color = Color.red;
                }
                break;
            default:
                {
                    currentBombimage.color = Color.green;
                }
                break;
        }
    }

    void DisableWaveNoTextAndStartGame()
    {
        waveNoText.gameObject.SetActive(false);
        gameStarted = true;
    }

    public void StartButtonClicked()
    {
        infoPanel.SetActive(false);
        Invoke("DisableWaveNoTextAndStartGame", 2f);
    }

}
