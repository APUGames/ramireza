using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickSFX;
    [SerializeField] int coinValue = 100;
    // Start is called before the first frame update
   private void OnTriggerEnter2D(Collider2D collision)
	{
        FindObjectOfType<GameSession>().ProcessPlayerScore(coinValue);

        AudioSource.PlayClipAtPoint(coinPickSFX, Camera.main.transform.position);

        Destroy(gameObject); 
	}
    
}
