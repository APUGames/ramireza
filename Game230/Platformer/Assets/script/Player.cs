using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	// Start is called before the first frame update
	Rigidbody2D playerCharacter;
	Animator playerAnimator;
	CapsuleCollider2D playerBodyCollider;
	BoxCollider2D playerFeetCollider;
	//CircleCollider2D playerHeadCollider;

	[SerializeField] float runSpeed = 5.0f;
	[SerializeField] float jumpSpeed = 5.0f;
	[SerializeField] float climbSpeed = 5.0f;
	[SerializeField] Vector2 deathSeq = new Vector2(25f, 25f);
	float gravityScaleAtStart;

	[SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip deathSound;
    AudioSource audioSource;
	private bool jump;

	bool isAlive = true;

	void Start()
	{
		playerCharacter = GetComponent<Rigidbody2D>();
		playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
		playerFeetCollider = GetComponent<BoxCollider2D>();
		//playerHeadCollider = GetComponent<CircleCollider2D>();

		audioSource = GetComponent<AudioSource>();

		gravityScaleAtStart = playerCharacter.gravityScale;
	}
    

	// Update is called once per frame
	void Update()
	{
		if (!isAlive)
		{
			return;
		}

		Run();
		FlipSprite();
		Jump();
		Climb();
        //DetectCeiling();
        Die();
	}

	private void Run()
	{
		float hMovement = Input.GetAxis("Horizontal");
		Vector2 runVelocity = new Vector2(hMovement * runSpeed, playerCharacter.velocity.y);
		playerCharacter.velocity = runVelocity;
        playerAnimator.SetBool("run", true);
        bool hSpeed = Mathf.Abs(playerCharacter.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("run", hSpeed);

        //print(runVelocity);
	}

	private void FlipSprite()
	{
		bool hMovement = Mathf.Abs(playerCharacter.velocity.x) > Mathf.Epsilon;
		if (hMovement)
		{
			transform.localScale = new Vector2(Mathf.Sign(playerCharacter.velocity.x), 1f);
		}
	}

    private void Jump()
    {
        if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Foreground")))
        {
            return;
        }
        if(Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocity = new Vector2(0.0f, jumpSpeed);
            playerCharacter.velocity += jumpVelocity;

        if (jump)
            {
                audioSource.PlayOneShot(jumpSound, 0.7f);
            }
        }
    }

    private void Climb()
    {
        if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            playerAnimator.SetBool("climb", false);
            playerCharacter.gravityScale = gravityScaleAtStart;
            return;
        }
        float vMovement = Input.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(playerCharacter.velocity.x, vMovement * climbSpeed);
        playerCharacter.velocity = climbVelocity;

        playerCharacter.gravityScale = 0.0f; 

        bool vSpeed = Mathf.Abs(playerCharacter.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("climb", vSpeed);
          
    }
       
       
      
	/*private void DetectCeiling()
	{
		if (!playerHeadCollider.IsTouchingLayers(LayerMask.GetMask("Foreground")))
		{
			jump = true;
		}
		else
		{
			jump = false;
		}
	} */

	private void Die()
	{
		if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
		{
            audioSource.PlayOneShot(deathSound, 0.7f);
			isAlive = false;
			playerAnimator.SetTrigger("die");
			GetComponent<Rigidbody2D>().velocity = deathSeq;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
		}
	} 
}
