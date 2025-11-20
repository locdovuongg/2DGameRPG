using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Destructible : MonoBehaviour
{
   [SerializeField] private GameObject destroyVFX;
   private void OnTriggerEnter2D(Collider2D other)
   {
       if (other.gameObject.GetComponent<DamageSource>())
       {
            GetComponent<PickUpSpawner>().DropItems();
           Instantiate(destroyVFX, transform.position, Quaternion.identity);
           Destroy(gameObject);
       }
   }

}
