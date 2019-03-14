using UnityEngine;
using System.Collections;

public class StreamToRight : MonoBehaviour
{

    public GameObject player;
    BoxCollider2D Collider;

    // Use this for initialization
    void Start()
    {
        //player = GameObject.Find("Warrior");
        Collider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
    
        if (Collider.IsTouching(player.GetComponent<Collider2D>()))
        {
            print("2");

            if (Input.GetAxisRaw("Horizontal") > 0.1f)
            {
                print("2");

                player.GetComponent<PlayerControl>().MovingSpeed = 6f;
            }
            else if (Input.GetAxisRaw("Horizontal") < -0.1f)
            {
                print("2");

                player.GetComponent<PlayerControl>().MovingSpeed = 1f;
            }
            else //if (Input.GetAxisRaw("Horizontal") == 0)
            {
                player.transform.Translate(Vector2.right * 1 * Time.deltaTime);
            }


        }
        

    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            player.GetComponent<PlayerControl>().MovingSpeed = 3.0f;
        }
    }
}
