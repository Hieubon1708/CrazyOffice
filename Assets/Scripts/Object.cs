using UnityEngine;

public class Object : MonoBehaviour
{
    bool isCollision;
    Rigidbody rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        transform.localPosition = Vector3.zero;
    }

    public void Throw()
    {
        transform.SetParent(GameController.instance.levelObject.transform);

        rb.isKinematic = false;

        Vector3 dir = PlayerController.instance.transform.position - transform.position;

        rb.velocity = new Vector3(dir.x, dir.y + 7, dir.z);

        SetAngular(10f);
    }

    void SetAngular(float speed)
    {
        Vector3 randomSpinAxis = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
        rb.angularVelocity = randomSpinAxis * speed;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!isCollision && collision.gameObject.CompareTag("Weapon"))
        {
            isCollision = true;

            RaycastHit hit;
            Vector3 dir = transform.position - Vector3.forward - transform.position;
            Physics.Raycast(transform.position, dir * 5, out hit);

            if(hit.collider != null)
            {
                rb.useGravity = false;

                SetAngular(25f);

                Vector3 dirReverse = PlayerController.instance.enemies[0].rbs[0].transform.position - transform.position;

                rb.velocity = dirReverse.normalized * 35;

                Debug.Log("Ok");
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
