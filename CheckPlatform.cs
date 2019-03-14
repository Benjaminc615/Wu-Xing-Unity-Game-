using UnityEngine;
using System.Collections;

public class CheckPlatform : MonoBehaviour
{

    public GameObject platform;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter(Collider player)
    {
        //make the parent platform ignore the jumper
        if (player.tag == "Player")
        {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            if (Input.GetKeyDown(KeyCode.S))
            {
                Physics.IgnoreCollision(player, platform.GetComponent<BoxCollider>());

            }
        }
    }

    void OnTriggerExit(Collider player)
    {
        //reset jumper's layer to something that the platform collides with
        //just in case we wanted to jump throgh this one


        //re-enable collision between jumper and parent platform, so we can stand on top again
        //gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

        Physics.IgnoreCollision(player, platform.GetComponent<BoxCollider>(), false);
    }
}
