using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float per;//관통수치

    public void Init(float damage, int per)
    {
        this.damage = damage;
        this.per = per;
    }
}
