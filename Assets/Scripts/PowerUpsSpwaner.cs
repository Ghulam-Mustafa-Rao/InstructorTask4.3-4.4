using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsSpwaner : MonoBehaviour
{
    public GameObject[] powerUps;
    public float xBound;
    public float yBound;
    public float zBound;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpwanPowerUps());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpwanPowerUps()
    {
        while(GameManager.gameManager.playerLives > 0)
        {
            yield return new WaitForSeconds(10f);
            int index = Random.Range(0, powerUps.Length);
            Instantiate(powerUps[index], GenerateRandomLocation(), powerUps[index].transform.rotation);
            
        }
    }

    Vector3 GenerateRandomLocation()
    {
        return new Vector3(Random.Range(-xBound, xBound + 1), yBound, Random.Range(-zBound, zBound + 1));
    }
}
