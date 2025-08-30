using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class Password : MonoBehaviour
{
    public Image[] numberPad;
    public TextMeshProUGUI myPasswordText;
    public TextMeshProUGUI cPasswordText;
    public CanvasGroup numberPadsAlpha;
    public int password;
    public int loadpassword;
    public bool isPasswordinput;
    public GameObject[] door;

    IEnumerator CheckPassword()
    {
        isPasswordinput = false;
        numberPadsAlpha.alpha = 0f;
        myPasswordText.text = "";
        cPasswordText.text = "";
        password = Random.Range(1000, 10000);
        loadpassword = password;

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < 100; i++)
        {
            numberPadsAlpha.alpha += 0.02f;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.5f);

        isPasswordinput = true;

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
    }

    IEnumerator FinalPassword()
    {
        int passwordAsInt;
        for (int i = 0; i < 8; i++)
        {
            if (door[i] != null)
                door[i].SetActive(false);
        }

        door[0] = GameObject.Find("D1");
        door[1] = GameObject.Find("D2");
        door[2] = GameObject.Find("D3");
        door[3] = GameObject.Find("D4");
        door[4] = GameObject.Find("D5");
        door[5] = GameObject.Find("D6");
        door[6] = GameObject.Find("D7");
        door[7] = GameObject.Find("D8");

        if (int.TryParse(myPasswordText.text, out passwordAsInt))
        {
            string correctColor = "#00FF00"; // ������ ���� ���� �ڵ� (�ʷϻ�)
            string wrongColor = "#FF0000";   // ������ ���� ���� �ڵ� (������)
            int doorindex = 0;
            // ���ο� �ؽ�Ʈ�� ������ ����
            string newPasswordText = "";

            // ��й�ȣ�� �� �ڸ����� ���մϴ�.
            for (int i = 1000; i >= 1; i /= 10)
            { 
                // �Է°��� ������ �ش� �ڸ��� ���ڸ� �����ɴϴ�.
                int inputDigit = (passwordAsInt / i) % 10;
                int correctDigit = (loadpassword / i) % 10;

                // ���ڰ� ��ġ�ϴ��� Ȯ���մϴ�.
                if (inputDigit == correctDigit)
                {
               
                    // ��ġ�ϸ� �ʷϻ� �±׸� �߰��մϴ�.
                    newPasswordText += $"<color={correctColor}>{inputDigit}</color>";
                    DoorOpen(doorindex);
                }
                else
                {
                    // ��ġ���� ������ ������ �±׸� �߰��մϴ�.
                    newPasswordText += $"<color={wrongColor}>{inputDigit}</color>";
                    DoorClose(doorindex);
                }
                cPasswordText.text = newPasswordText;
                doorindex++;
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(1f);
            myPasswordText.text = "";
            cPasswordText.text = "";
        }
    }

    void DoorOpen(int index)
    {
        door[index].SetActive(false);
    }

    void DoorClose(int index)
    {
        door[index + 4].SetActive(false);
    }
}
