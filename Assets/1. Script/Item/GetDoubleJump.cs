using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDoubleJump : MonoBehaviour
{
    public GameObject PanelManager;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("TriggerOn");
            transform.GetComponent<Animator>().SetBool("Oncollision", true);

            //get skill
            other.GetComponent<PlayerMovement>().doubleJumpSkill = true;
            
            PanelManager.SendMessage("ShowDoubleJumpPanel");
            StartCoroutine(DestroyItem());
        }
    }

    IEnumerator DestroyItem()
	{
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
