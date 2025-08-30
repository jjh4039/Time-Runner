using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Password : MonoBehaviour
{
    public Image[] numberPad;
    public TextMeshProUGUI myPasswordText;
    public CanvasGroup numberPadsAlpha;
    public int password;
    public bool isPasswordinput;

    IEnumerator CheckPassword()
    {
        isPasswordinput = true;
        numberPadsAlpha.alpha = 0f;
        password = Random.Range(1000, 10000);
        Debug.Log(password);

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 100; i++)
        {
            numberPadsAlpha.alpha += 0.02f;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 1000; i >= 1; i /= 10)
        {
            int checker = 0;
            checker = password / i;
            password = password % i;
            Debug.Log(checker);

            numberPad[checker].color = new Color(1f, 0.5f, 0.5f, 0.5f);
            yield return new WaitForSeconds(0.4f);
            numberPad[checker].color = new Color(0.53f, 0.53f, 0.53f, 0.5f);
            yield return new WaitForSeconds(0.2f);
        }

        for (int i = 0; i < 100; i++)
        {
            numberPadsAlpha.alpha -= 0.02f;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(3f);

        isPasswordinput = true;
    }
}
