using UnityEngine;
using System.Collections;

public class EarthquakeScript : MonoBehaviour
{
    public GameObject player;
    BoxCollider2D Collider;
    float stunDuration;
    float duration;


    // Use this for initialization
    void Start ()
    {
        player = GameObject.Find("Warrior");
        Collider = GetComponent<BoxCollider2D>();
        stunDuration = 5.0f;
        duration = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (player.GetComponent<PlayerControl>().Hp <= 0)
            Destroy(gameObject);

        duration += Time.deltaTime;

        if (duration >= 3.0f)
        {
            if (Collider.IsTouching(player.GetComponent<BoxCollider2D>()))
            {
                player.GetComponent<PlayerControl>().Stun(stunDuration);
                player.GetComponent<PlayerControl>().TakeDamage(15.0f, PlayerControl.WUXING.Earth, gameObject);
            }
            Destroy(gameObject);
        }
	}
}
