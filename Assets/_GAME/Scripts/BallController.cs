using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private int count;
    [SerializeField] private TextMeshProUGUI countText;

    public void TakeDamage(int damage)
    {
        count-=damage;
        if (count < 0)
            Debug.Log("bitti");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Damage(this);
            Debug.Log("çarptý");
        }
    }

}
