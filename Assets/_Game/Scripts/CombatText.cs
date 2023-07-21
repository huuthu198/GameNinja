using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatText : MonoBehaviour
{
    [SerializeField] Text textHp;

   public void OnInit(float damage)
    {
        textHp.text = damage.ToString();
        Invoke(nameof(OnDespawn), 0.5f);
    }
    public void OnDespawn()
    {
        Destroy(gameObject);
    }
}
