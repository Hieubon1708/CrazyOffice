using UnityEngine;

public class EnemyBone : MonoBehaviour
{
    public Transform[] olds;
    public Transform[] news;

    public bool isHead;
    public bool isBody;

    const int indexHead = 7;
    const int indexBody = 111;

    public void LateUpdate()
    {
        for (int i = 0; i < news.Length; i++)
        {
            news[i].position = olds[i].position;
            news[i].rotation = olds[i].rotation;
        }
    }
}
