using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoBoss : MonoBehaviour
{
    public GameObject gameobject;
    public GameObject Player;
    public GameObject Camera;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Go to Boss");
            gameobject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        gameobject.SetActive(false);
    }

    public void BossRoom()
    {
        //transform position of player
        Player.transform.position = new Vector2(38.5f,3.5f);

        //change BGM
        Camera.GetComponent<BGMmanager>().PlayBGM("boss");

        //make camera wide sight
        Camera.GetComponent<Camera>().orthographicSize += 1;
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
