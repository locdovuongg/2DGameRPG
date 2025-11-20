using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goldCoin;
    [SerializeField] private GameObject healthGlobe;
    [SerializeField] private GameObject staminaGlobe;

    public void DropItems()
    {
        int randomNum = Random.Range(1, 4);  
        switch (randomNum)
        {
            case 1: // health
                Instantiate(healthGlobe, transform.position, Quaternion.identity);
                break;

            case 2: // stamina
                Instantiate(staminaGlobe, transform.position, Quaternion.identity);
                break;

            case 3: // gold
                int amount = Random.Range(1, 4);
                for (int i = 0; i < amount; i++)
                {
                    Instantiate(goldCoin, transform.position, Quaternion.identity);
                }
                break;
        }
    }
}
