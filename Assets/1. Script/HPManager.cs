using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{

    public Text HPText;
    //public float HP;
    public GameObject Player;
    private float life;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        life = Player.GetComponent<CharacterController2D>().life;
        if(life<0)
            life = 0;
        HPText.text = life.ToString();
    }
}
