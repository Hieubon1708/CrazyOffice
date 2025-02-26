using UnityEngine;

public class EnemyBoneHandler : MonoBehaviour
{
    bool isCollision;

    public void OnCollisionEnter(Collision collision)
    {
        if (isCollision) return;
        isCollision = true;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            Debug.Log(collision.rigidbody.velocity);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            isCollision = false;
        }
    }
}
