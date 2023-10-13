using CardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordCardManager : MonoBehaviour
{
    public static SwordCardManager Instance;



    //卡牌数量
    public int SwordNum = 5;

    //卡牌文本
    [SerializeField] private Text SwordText;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }



    public SwordCardManager(Text text)
    {
        this.SwordText = text;
    }

    public void PickUpCard(Card card)
    {
        if (card.getName().Equals("Sword"))
        {
            this.SwordNum++;

        }
        updateText();

    }
    //isRelease：是否要释放技能
    public void UseCard(bool isRelease)
    {


        Button button = GetComponent<Button>();
        if (button.tag == "SwordCard")
        {
            Debug.Log("sword --");
            if (this.SwordNum > 0)
            {
                this.SwordNum--;
                if (isRelease)
                {

                    useSword(UpgradeSwordCard.Instance.isUpgraded);

                    
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
        this.SwordText.text = this.SwordNum.ToString();
    }

    //使用剑的效果，参数为是否为升级剑
    private void useSword(bool isUpgraded)
    {
        if (isUpgraded)
        {//是升级剑
            GetComponent<Image>().color = new Color(255, 255, 255);
            UpgradeSwordCard.Instance.DownGradeSword();
            //TODO

        }
        else
        {//是普通剑
            Player.Instance.player_state = Player.Player_State.Sword;
         //需要补上动画
        }
        return;
    }



}
