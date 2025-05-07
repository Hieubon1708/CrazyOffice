using UnityEngine;

public class Bone : MonoBehaviour
{
    public Transform center;
    public ParticleSystem par;

    private void OnEnable()
    {
        par.Play();

        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();

        for (int i = 0; i < rbs.Length; i++)
        {
            rbs[i].AddExplosionForce(3, center.position, 5, 2.5f, ForceMode.Impulse);
        }
    }
}
