using UnityEngine;

public class Boss : MonoBehaviour
{
    int hp = 6;
    int startHp;

    BossHealth bossHealth;

    private void Awake()
    {
        bossHealth = GetComponentInChildren<BossHealth>();
        startHp = hp;
    }

    public void SubtractHp()
    {
        if (hp <= 0) return;
        hp -= 1;
        bossHealth.SubtractHp(startHp, startHp - hp);
    }
}
