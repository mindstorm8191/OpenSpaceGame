using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TouchingDamage : MonoBehaviour
{
    public Text healthBar;
    public Image dmgScreen;
    private float hp;
    private float timeOut;

    private float GetHP() => hp = float.Parse(healthBar.transform.name);
    private void SetHP(float newHP)
    {
        hp = newHP;
        healthBar.transform.name = newHP.ToString();
        healthBar.text = hp.ToString();
    }

    private void Start()
    {
        GetHP();
        timeOut = 0f;

        Color c1 = dmgScreen.color;
        c1.a = 0f;
        dmgScreen.color = c1;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy1") && hp > 0f && timeOut <= 0f)
        {
            timeOut = 10f;
            SetHP(hp - 10f);


            StartCoroutine(OpacityTransition());
        }

        if (timeOut > 0f)
            timeOut -= .25f;
    }

    public IEnumerator OpacityTransition(float result = .5f)
    {
        while (dmgScreen.color.a < result)
        {
            Color c = dmgScreen.color;
            c.a += result / 15f;
            dmgScreen.color = c;
            yield return new WaitForSeconds(result / 20f);
        }

        Color c1 = dmgScreen.color;
        c1.a = 0f;
        dmgScreen.color = c1;
    }
}
