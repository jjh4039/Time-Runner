using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public TextMeshProUGUI skillBuyCost;
    public TextMeshProUGUI myCost;

    public int skillIndexV;
    public int skillIndexH;

    void Update()
    {
        myCost.text = "º¸À¯ PP : " + DB.instance.perfectCount.ToString();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            skillIndexV--;
            if (skillIndexV < 0)
                skillIndexV = 3;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            skillIndexV++;
            if (skillIndexV > 3)
                skillIndexV = 0;
        }

    }
}
