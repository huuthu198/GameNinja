
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image m_Image;
    [SerializeField] private float hp;
    [SerializeField] private float maxHP;
    private void Update()
    {
        m_Image.fillAmount = Mathf.Lerp(m_Image.fillAmount,hp/maxHP,Time.deltaTime*5f);
    }
    public void OnInit( float maxHP)
    {
        this.maxHP = maxHP;
        hp = maxHP;
        m_Image.fillAmount = 1;
    }
    public void SetNewHp(float hp)

    {
        this.hp = hp;
       
    }
}
