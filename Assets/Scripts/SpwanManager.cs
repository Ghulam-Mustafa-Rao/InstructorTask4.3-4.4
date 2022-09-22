using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpwanManager : MonoBehaviour
{

    public float xBound;
    public float yBound;
    public float zBound;

    public GameObject[] enemiesPrefabs;
    public GameObject enemyHolder;
    bool invokeCalled = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //spwan enemies if all enemies are destroyed
        if (enemyHolder.transform.childCount == 0)
        {
            if (GameManager.gameManager.gameStarted)
            {
                invokeCalled = false;
                SpwanEnemies();
            }
            else if(!invokeCalled)
            {
                if (!GameManager.gameManager.infoPanel.active)
                {
                    invokeCalled = true;
                    GameManager.gameManager.waveNoText.gameObject.SetActive(true);
                    GameManager.gameManager.Invoke("DisableWaveNoTextAndStartGame", 2f);
                }
                
            }
        }
    }

    void SpwanEnemies()
    {
        
        for (int i = 0; i < GameManager.gameManager.waveNo; i++)
        {
            GameObject enemyToSpwan = enemiesPrefabs[Random.Range(0, enemiesPrefabs.Length)];
            Instantiate(enemyToSpwan, GenerateRandomLocation(), enemyToSpwan.transform.rotation).transform.SetParent(enemyHolder.transform);
        }
        GameManager.gameManager.waveNo++;
        GameManager.gameManager.gameStarted = false;
    }
    Vector3 GenerateRandomLocation()
    {
        return new Vector3(Random.Range(-xBound, xBound + 1), yBound, Random.Range(-zBound, zBound + 1));
    }
}
