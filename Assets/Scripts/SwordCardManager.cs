using CardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwordCardManager : MonoBehaviour
{
    public static SwordCardManager Instance;
    AudioSource audioSource;
    public AudioClip pick_sound;
    public AudioClip select_sound;


    //卡牌数量
    [SerializeField] public int SwordNum = 0;

    //卡牌文本
    [SerializeField] private Text SwordText;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        audioSource = GetComponent<AudioSource>();
    }



    public SwordCardManager(Text text)
    {
        this.SwordText = text;
    }

    public void PickUpCard()
    {
        audioSource.clip = pick_sound;
        audioSource.Play();
        this.SwordNum++;
        updateText();

    }
    //isRelease：是否要释放技能
    public void UseCard(bool isRelease)
    {
        if (Player.Instance.player_state == Player.Player_State.Sword || Player.Instance.player_state == Player.Player_State.SwordPlus)
        {
            return;
        }



        Button button = GetComponent<Button>();
        if (button.tag == "SwordCard")
        {
            Debug.Log("sword --");
            if (this.SwordNum > 0)
            {
                this.SwordNum--;
                audioSource.clip = select_sound;
                audioSource.Play();



                if (isRelease)
                {
                    if (Player.Instance.player_state == Player.Player_State.Bow || Player.Instance.player_state == Player.Player_State.BowPlus)
                    {
                        BowCardManager.Instance.BowNum += 1;
                        BowCardManager.Instance.updateText();
                    }

                    if (Player.Instance.player_state == Player.Player_State.Bomb)
                    {
                        BombCardManager.Instance.BombNum += 1;
                        BombCardManager.Instance.updateText();
                    }

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

    public  void updateText()
    {
        this.SwordText.text = this.SwordNum.ToString();
    }

    //使用剑的效果，参数为是否为升级剑
    private void useSword(bool isUpgraded)
    {
        if (isUpgraded)
        {//是升级剑
            GetComponent<Image>().color = new Color(255, 255, 255);
            Player.Instance.player_state = Player.Player_State.SwordPlus;
            Player.Instance.setAnimeOn("U-SwordOn");
            UpgradeSwordCard.Instance.DownGradeSword();
        }
        else
        {//是普通剑
            Debug.Log("release");
            Player.Instance.player_state = Player.Player_State.Sword;
            Player.Instance.setAnimeOn("SwordOn");
        }
        return;
    }



}
