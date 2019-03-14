using UnityEngine;
using System.Collections;

public class StreamToLeft : MonoBehaviour
{

    public GameObject player;
    BoxCollider2D Collider;
    public bool left, right;

    // Use this for initialization
    void Start()
    {
        //   player = GameObject.Find("Warrior");
        Collider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerControl>().wuXing != PlayerControl.WUXING.Water)
            if (Collider.IsTouching(player.GetComponent<Collider2D>()))
            {
                print("1");
                if (Input.GetAxisRaw("Horizontal") > 0.1f)
                {
                    if (left)
                        player.GetComponent<PlayerControl>().MovingSpeed = 1;
                    if (right)
                        player.GetComponent<PlayerControl>().MovingSpeed = 6;
                }
                else if (Input.GetAxisRaw("Horizontal") < -0.1f)
                {
                    if (left)
                        player.GetComponent<PlayerControl>().MovingSpeed = 6;
                    if (right)
                        player.GetComponent<PlayerControl>().MovingSpeed = 1;
                }
                else //if (Input.GetAxisRaw("Horizontal") == 0)
                {
                    if (left)
                        player.transform.Translate(Vector2.left * 1 * Time.deltaTime);
                    if (right)
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
