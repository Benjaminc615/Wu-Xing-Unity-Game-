using UnityEngine;
using System.Collections;

public class WaterSpecialAbility : MonoBehaviour
{

    public GameObject player;

    Transform tran;
    float timer;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Warrior");
        tran = gameObject.transform;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        tran.position = player.transform.position;
        timer += Time.deltaTime;
        liftingfog();
        if (player.GetComponent<PlayerControl>().wuXing != PlayerControl.WUXING.Water)
        {
            Destroy(gameObject);
            player.GetComponent<PlayerControl>().isdodge = false;
        }

    }

    void liftingfog()
    {
        if (gameObject.GetComponent<ParticleSystem>().maxParticles > 0)
        {
            if (timer >= 1)
            {
                gameObject.GetComponent<ParticleSystem>().maxParticles -= 10;
                timer = 0;
            }
        }
        else
        {
            Destroy(gameObject);
            player.GetComponent<PlayerControl>().isdodge = false;
        }
    }
}
