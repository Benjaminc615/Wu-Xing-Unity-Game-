using UnityEngine;
using System.Collections;

public class WaterFall : MonoBehaviour
{
    GameObject player;
    BoxCollider2D Collider;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Warrior");
        Collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Collider.IsTouching(player.GetComponent<Collider2D>()))
        {
            if (player.GetComponent<PlayerControl>().wuXing != PlayerControl.WUXING.Water)
                player.GetComponent<Rigidbody2D>().gravityScale = 3.5f;
            print("1");
        }



    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            player.GetComponent<Rigidbody2D>().gravityScale = 2.3f;
        }
    }
}
