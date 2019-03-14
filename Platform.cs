using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour
{

    //public GameObject player;
    //bool ablejumpdown = false;
    //bool jumpingdown = false;
    // Use this for initialization
    void Start()
    {
       // player = GameObject.Find("Warrior");
    }

    // Update is called once per frame
    void Update()
    {
       // if (ablejumpdown)
       // {
       //
       //     if (Input.GetKeyDown(KeyCode.S)&& player.GetComponent<PlayerControl>().PlayerRB.velocity.y == 0)
       //     {
       //         Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
       //         jumpingdown = true;
       //     }
       // }
    }





    void OnTriggerEnter2D(Collider2D coll)
    {

       // if (coll.tag == "CheckGroundStick" || player.GetComponent<PlayerControl>().PlayerRB.velocity.y == 0)
       // {
       //
       //     ablejumpdown = true;
       //     //if (Input.GetKeyDown(KeyCode.S))
       //     //{
       //     //    Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
       //     //    print("1");
       //     //}
       // }
    }

    void OnTriggerStay2D(Collider2D coll)
    {

       //if (coll.tag == "CheckGroundStick" || player.GetComponent<PlayerControl>().PlayerRB.velocity.y == 0)
       //{
       //
       //    ablejumpdown = true;
       //
       //    //if (Input.GetKeyDown(KeyCode.S))
       //    //{
       //    //    Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
       //    //    print("2");
       //    //
       //    //}
       //}
    }
    void OnTriggerExit2D(Collider2D coll)
    {

      // if (coll.tag == "CheckGroundStick")
      // {
      //     if (jumpingdown)
      //     {
      //         Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), false);
      //         ablejumpdown = false;
      //     }
      // }
    }
}
