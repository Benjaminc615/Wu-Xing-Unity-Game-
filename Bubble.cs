using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {

    public GameObject player;
    Collider2D Collider;
    float timer;
    // Use this for initialization
    void Start()
    {
        StartCoroutine("DestroyBubble");
        Collider = gameObject.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Collider.IsTouching(player.GetComponent<Collider2D>()))
        {
                player.GetComponent<PlayerControl>().MovingSpeed = 1;
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            player.GetComponent<PlayerControl>().MovingSpeed = 3.0f;
        }
    }

    IEnumerator DestroyBubble()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
