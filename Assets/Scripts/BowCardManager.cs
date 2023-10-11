using CardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowCardManager: MonoBehaviour
{
    public static BowCardManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }
    //卡牌数量
    public int BowNum = 5;

    //卡牌文本
    [SerializeField]private Text BowText;

    public BowCardManager(Text text)
    {
        this.BowText = text;
    }

    public void PickUpCard(Card card)
    {
        if (card.getName().Equals("Bow"))
        {
            this.BowNum++;

        }
        updateText();

    }
    //isRelease：是否要释放技能
    public void UseCard(bool isRelease)
    {


        Button button = GetComponent<Button>();
        if (button.tag == "BowCard")
        {
            Debug.Log("bow --");
            if (this.BowNum > 0)
            {
                this.BowNum--;
                if (isRelease)
                {
                    useBow(UpgradeBowCard.Instance.isUpgraded);

                }

            }

        }

        updateText();


    }

    public void getUpdated()
    {
        GetComponent<Image>().color = Color.blue;
    }

    public void updateText()
    {
        this.BowText.text = this.BowNum.ToString();
    }

    //使用弓的效果
    private void useBow(bool isUpgraded)
    {
        if (isUpgraded)
        {//是升级弓
            GetComponent<Image>().color = new Color(255, 255, 255);
            UpgradeBowCard.Instance.DownGradeBow();
            //TODO
        }
        else
        {//是普通弓
            //TODO
        }
        return;
    }
}
