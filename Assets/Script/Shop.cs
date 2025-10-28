using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements.Experimental;
using System.Collections;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public TextMeshProUGUI skillBuyCost;
    public TextMeshProUGUI myCost;
    public GameObject arrow;
    public TextMeshProUGUI arrowText;
    public TextMeshProUGUI readyText;
    public SpriteRenderer Tree;
    public Sprite[] TreeSprites;
    public CanvasGroup alphaScreen;
    public float readying;
    public int selectIndex;

    public int[] buyCost = { 5, 10, 12, 20, 25, 35, 50 };

    public int skillIndexV;
    public int skillIndexH;

    private void Awake()
    {
        StartCoroutine(ShopStart());
    }

    void Update()
    {
        SetDes();
        myCost.text = "보유 PP : " + DB.instance.perfectCount.ToString();

        for (int i = 6; i >= 0; i--)
        {
            if (DB.instance.isBuy[i])
            {
                Tree.sprite = TreeSprites[i];
                break;
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            readying += Time.deltaTime * 200f;
        }
        else
        {
            if (readying > 0)
            {
                readying -= Time.deltaTime * 300f;
            }
        }

        if (readying <= 100)
        {
            readyText.text = "[ D ] 키를 눌러 출발하세요 - □ □ □";
        }
        else if (readying <= 200)
        {
            readyText.text = "[ D ] 키를 눌러 출발하세요 - ■ □ □";
        }
        else if (readying <= 300)
        {
            readyText.text = "[ D ] 키를 눌러 출발하세요 - ■ ■ □";
        }
        else if (readying <= 400)
        {
            readyText.text = "[ D ] 키를 눌러 출발하세요 - ■ ■ ■";
        }
        else if (readying <= 500)
        {
            readyText.color = Color.white;
            readyText.text = "출발합니다.";
            readying = 3000f;
            StartCoroutine(GameStart());
        }

        switch (skillIndexV)
        {
            case 0:
                arrow.transform.localPosition = new Vector3(arrow.transform.localPosition.x, -156f, arrow.transform.localPosition.z);
                arrowText.color = Color.white;
                break;
            case 1:
                arrow.transform.localPosition = new Vector3(arrow.transform.localPosition.x, -80f, arrow.transform.localPosition.z);
                if (skillIndexH == 0) arrowText.color = Color.skyBlue; else arrowText.color = Color.softRed;
                break;
            case 2:
                arrow.transform.localPosition = new Vector3(arrow.transform.localPosition.x, -20f, arrow.transform.localPosition.z);
                if (skillIndexH == 0) arrowText.color = Color.skyBlue; else arrowText.color = Color.softRed;
                break;
            case 3:
                arrow.transform.localPosition = new Vector3(arrow.transform.localPosition.x, 90f, arrow.transform.localPosition.z);
                if (skillIndexH == 0) arrowText.color = Color.skyBlue; else arrowText.color = Color.softRed;
                break;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            BuySkill(selectIndex);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            skillIndexV--;
            if (skillIndexV < 0)
                skillIndexV = 3;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            skillIndexV++;
            if (skillIndexV > 3)
                skillIndexV = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (skillIndexV != 0)
            {
                if (skillIndexH > 0)
                {
                    skillIndexH--;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (skillIndexV != 0)
            {
                if (skillIndexH < 1)
                {
                    skillIndexH++;
                }
            }
        }
    }

    IEnumerator ShopStart()
    {
        alphaScreen.alpha = 1f;
        for (int i = 0; i < 40; i++)
        {
            alphaScreen.alpha -= 0.025f;
            yield return new WaitForSeconds(0.03f);
        }
    }

    IEnumerator GameStart()
    {
        AsyncOperation asyncOp;
        
        asyncOp = SceneManager.LoadSceneAsync(2);
        asyncOp.allowSceneActivation = false;

        alphaScreen.alpha = 0f;

        for (int i = 0; i < 40; i++)
        {
            alphaScreen.alpha += 0.025f;
            yield return new WaitForSeconds(0.03f);
        }

        asyncOp.allowSceneActivation = true;
    }
        
        void SetDes()
    {
        switch (skillIndexV)
        {
            case 0:
                skillName.text = "완벽한 성공";
                skillDescription.text = "피해를 입지 않고 스테이지를 클리어 시\n1초의 추가 시간을 획득합니다.";
                skillBuyCost.text = "구매 비용 : 5 PP";
                selectIndex = 0;
                if (DB.instance.isBuy[selectIndex])
                {
                    skillBuyCost.text = "구매 완료";
                }
                break;
            case 1:
                if (skillIndexH == 0)
                {
                    skillName.text = "안정적인 출발";
                    skillDescription.text = "시작 시간이 <color=#4AA8D8>5초</color> 증가합니다.";
                    skillBuyCost.text = "구매 비용 : 10 PP";
                    selectIndex = 1;
                    if (DB.instance.isBuy[selectIndex])
                    {
                        skillBuyCost.text = "구매 완료";
                    }
                }
                else
                {
                    skillName.text = "연속 돌파";
                    skillDescription.text = "완벽한 성공의 추가 시간을\n<color=#CE0018>0.5초</color> 증가시킨다.";
                    skillBuyCost.text = "구매 비용 : 12 PP";
                    selectIndex = 2;
                    if (DB.instance.isBuy[selectIndex])
                    {
                        skillBuyCost.text = "구매 완료";
                    }
                }
                break;
            case 2:
                if (skillIndexH == 0)
                {
                    skillName.text = "기초 훈련";
                    skillDescription.text = "스테이지 돌파 시간이\n<color=#4AA8D8>1초</color> 증가합니다.";
                    skillBuyCost.text = "구매 비용 : 20 PP";
                    selectIndex = 3;
                    if (DB.instance.isBuy[selectIndex])
                    {
                        skillBuyCost.text = "구매 완료";
                    }
                }
                else
                {
                    skillName.text = "고강도 훈련";
                    skillDescription.text = "다음 레벨까지 필요한\n스테이지의 수가 <color=#CE0018>2</color> 감소합니다.";
                    skillBuyCost.text = "구매 비용 : 25 PP";
                    selectIndex = 4;
                    if (DB.instance.isBuy[selectIndex])
                    {
                        skillBuyCost.text = "구매 완료";
                    }
                }
                break;
            case 3:
                if (skillIndexH == 0)
                {
                    skillName.text = "시간 압축";
                    skillDescription.text = "마지막 스테이지에서 ♥ 가\n<color=#4AA8D8>7초의</color> 시간으로 교환됩니다.";
                    skillBuyCost.text = "구매 비용 : 35 PP";
                    selectIndex = 5;
                    if (DB.instance.isBuy[selectIndex])
                    {
                        skillBuyCost.text = "구매 완료";
                    }
                }
                else
                {
                    skillName.text = "진실";
                    skillDescription.text = "더 이상 마지막 스테이지에서\n죽어도 <color=#CE0018>생명을 소모하지 않습니다</color>";
                    skillBuyCost.text = "구매 비용 : 50 PP";
                    selectIndex = 6;
                    if (DB.instance.isBuy[selectIndex])
                    {
                        skillBuyCost.text = "구매 완료";
                    }
                }
                break;
        }
    }

    void BuySkill(int selectIndex)
    {
        if (DB.instance.perfectCount >= buyCost[selectIndex] && !DB.instance.isBuy[selectIndex])
        {
            DB.instance.perfectCount -= buyCost[selectIndex];
            DB.instance.isBuy[selectIndex] = true;
        }
    }
}
