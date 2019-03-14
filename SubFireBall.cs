using UnityEngine;
using System.Collections;

public class SubFireBall : MonoBehaviour {

    private Vector2 direction;
   // [SerializeField]
   // PlayerControl.WUXING wuxing;
    [SerializeField]
    GameObject FireEff;
    void Start()
    {
  //      wuxing = PlayerControl.WUXING.Fire;
        direction = Vector2.right;
        StartCoroutine(WaitToDestroy(2.0F));
    }

    IEnumerator WaitToDestroy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Destroy(gameObject);
    }

    public void setDirection(Vector2 dir)
    {
        direction = dir;
        GetComponent<Rigidbody2D>().velocity = direction * 7.0f;
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));

        transform.LookAt(worldPos, Vector3.forward);
        transform.rotation = Quaternion.Euler(new Vector3(0, 180, transform.rotation.eulerAngles.z + 90));

    }

   // public void setWuXing(PlayerControl.WUXING _wuxing)
   // {
   //     wuxing = _wuxing;
   // }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(direction*Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
        {


            if (other == null)
            {
                return;
            }
            if (other.tag == "Enemy" || other.tag == "MeleeEnemy" || other.tag == "RangedEnemy" || other.tag == "MiniBoss" || other.tag == "Boss")
            {
                print(other.tag);
                other.SendMessage("EffectDamage", 10);
                GameObject g = GameObject.Instantiate(FireEff) as GameObject;
                g.transform.parent = other.transform;
            }
            // print(other.name);
            Destroy(gameObject);

        }
    }
}
