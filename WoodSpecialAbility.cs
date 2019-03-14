using UnityEngine;
using System.Collections;

public class WoodSpecialAbility : MonoBehaviour
{


    [SerializeField]
    GameObject WoodSpecialEff;

    float Curr_radius = 0f;
    Vector2 Curr_scale = new Vector2(0, 0);

    float Max_radius = 1.25f;
    public float Growing_rate = 1f;
    // bool IsAttached = false;
    public bool Growing = false;
    // Use this for initialization
    void Start()
    {
        gameObject.transform.localScale = Curr_scale;
    }

    // Update is called once per frame
    void Update()
    {


    }

    void FixedUpdate()
    {
        if (Growing)
        {
            if (Curr_radius < Max_radius)
            {
                Curr_radius += Growing_rate * Time.deltaTime;
                gameObject.transform.localScale = new Vector3(55 * Curr_radius, 55 * Curr_radius, 1);
                
            }
            else
            {
                Growing = false;
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy" || col.tag == "MeleeEnemy" || col.tag == "RangedEnemy" || col.tag == "MiniBoss" || col.tag == "Boss")
        {
            //if (Vector3.Distance(col.transform.position, gameObject.transform.position) - gameObject.GetComponent<CircleCollider2D>().radius<  -0.05f)
            //{
            //
            GameObject g = GameObject.Instantiate(WoodSpecialEff) as GameObject;
            //g.transform.parent = col.transform;
            g.GetComponent<WoodSpecialEff>().Enemy = col.transform;
            //}
        }

    }
}
