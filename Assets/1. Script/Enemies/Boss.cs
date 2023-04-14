using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.

	public float life = 10;

	private bool facingRight = true;

	public float speed = 5f; 

	public bool isInvincible = false;
	private bool isHitted = false;

	public GameObject enemy;
	private float distToPlayer;
	private float distToPlayerY;
	public float meleeDist = 1.5f;
	public float rangeDist = 5f;
	public float dmgValue = 4;


	private float randomDecision = 0;
	private bool doOnceDecision = true;
	private bool endDecision = false;
	private Animator anim;
	public GameObject PanelManager;
	public GameObject musicPlayer;

	void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{

		if (life <= 0)
		{
			PanelManager.SendMessage("ShowEndingPanel");
			musicPlayer.GetComponent<BGMmanager>().PlayBGM("end");
			StartCoroutine(DestroyEnemy());
		}

		else if (enemy != null) 
		{
			if (!isHitted)
			{
				distToPlayer = enemy.transform.position.x - transform.position.x;
				distToPlayerY = enemy.transform.position.y - transform.position.y;
				//action consider distance
				if (Mathf.Abs(distToPlayer) < 0.1f)
				{
					GetComponent<Rigidbody2D>().velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
					anim.SetBool("IsWaiting", true);
				}
				else if (Mathf.Abs(distToPlayer) > 0.1f && Mathf.Abs(distToPlayer) < meleeDist && Mathf.Abs(distToPlayerY) < 2f)
				{
					GetComponent<Rigidbody2D>().velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
					if ((distToPlayer > 0f && transform.localScale.x < 0f) || (distToPlayer < 0f && transform.localScale.x > 0f)) 
						Flip();
				}
				else if (Mathf.Abs(distToPlayer) > meleeDist && Mathf.Abs(distToPlayer) < rangeDist)
				{
					anim.SetBool("IsWaiting", false);
					m_Rigidbody2D.velocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * speed, m_Rigidbody2D.velocity.y);
				}
				else
				{
					if (!endDecision)
					{
						if ((distToPlayer > 0f && transform.localScale.x < 0f) || (distToPlayer < 0f && transform.localScale.x > 0f)) 
							Flip();
						//Pattern
						if (randomDecision < 0.3f)
							Run();
						else if (randomDecision >= 0.3f && randomDecision < 0.9f)
							Jump();
						else
							Idle();
					}
					else
					{
						endDecision = false;
					}
				}
			}
			else if (isHitted)
			{
				if ((distToPlayer > 0f && transform.localScale.x > 0f) || (distToPlayer < 0f && transform.localScale.x < 0f))
				{
					Flip();
				}
			}
		}

		if (transform.localScale.x * m_Rigidbody2D.velocity.x > 0 && !m_FacingRight && life > 0)
		{
			// ... flip the player.
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (transform.localScale.x * m_Rigidbody2D.velocity.x < 0 && m_FacingRight && life > 0)
		{
			// ... flip the player.
			Flip();
		}
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player" && life > 0)
		{
			collision.gameObject.GetComponent<CharacterController2D>().ApplyDamage(2f, transform.position);
		}
	}

	void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	public void ApplyDamage(float damage)
	{
		if (!isInvincible)
		{
			float direction = damage / Mathf.Abs(damage);
			damage = Mathf.Abs(damage);
			anim.SetBool("Hit", true);
			life -= damage;
			transform.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
			transform.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction * 300f, 100f)); 
			StartCoroutine(HitTime());
		}
	}
	

	public void Run()
	{
		anim.SetBool("IsWaiting", false);
		m_Rigidbody2D.velocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * speed, m_Rigidbody2D.velocity.y);
		if (doOnceDecision)
			StartCoroutine(NextDecision(0.5f));
	}
	public void Jump()
	{
		Vector3 targetVelocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * speed, m_Rigidbody2D.velocity.y);
		Vector3 velocity = Vector3.zero;
		m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, 0.05f);
		if (doOnceDecision)
		{
			anim.SetBool("IsWaiting", false);
			m_Rigidbody2D.AddForce(new Vector2(0f, 300f));
			StartCoroutine(NextDecision(1f));
		}
	}

	public void Idle()
	{
		m_Rigidbody2D.velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
		if (doOnceDecision)
		{
			anim.SetBool("IsWaiting", true);
			StartCoroutine(NextDecision(1f));
		}
	}

	public void EndDecision()
	{
		randomDecision = Random.Range(0.0f, 1.0f); 
		endDecision = true;
	}

	IEnumerator HitTime()
	{
		isInvincible = true;
		isHitted = true;
		yield return new WaitForSeconds(0.1f);
		isHitted = false;
		isInvincible = false;
	}
	IEnumerator NextDecision(float time)
	{
		doOnceDecision = false;
		yield return new WaitForSeconds(time);
		EndDecision();
		doOnceDecision = true;
		anim.SetBool("IsWaiting", false);
	}

	IEnumerator DestroyEnemy()
	{
		transform.GetComponent<Animator>().SetBool("IsDead", true);
		yield return new WaitForSeconds(0.25f);
		m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
	}
}
