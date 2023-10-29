using CardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowCardManager : MonoBehaviour
{
    public static BowCardManager Instance;
    AudioSource audioSource;
    public AudioClip pick_sound;
    public AudioClip select_sound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        audioSource = GetComponent<AudioSource>();
    }
    //卡牌数量
    [SerializeField] public int BowNum = 0;

    //卡牌文本
    [SerializeField] private Text BowText;

    public BowCardManager(Text text)
    {
        this.BowText = text;
    }

    public void PickUpCard()
    {
        audioSource.clip = pick_sound;
        audioSource.Play();
        this.BowNum++;
        updateText();
    }
    //isRelease：是否要释放技能
    public void UseCard(bool isRelease)
    {
        if (Player.Instance.player_state == Player.Player_State.Bow || Player.Instance.player_state == Player.Player_State.BowPlus)
        {
            return;
        }

        Button button = GetComponent<Button>();
        if (button.tag == "BowCard")
        {
            Debug.Log("bow --");
            if (this.BowNum > 0)
            {
                audioSource.clip = select_sound;
                audioSource.Play();



                this.BowNum--;
                if (isRelease)
                {
                    
                    if (Player.Instance.player_state == Player.Player_State.Sword|| Player.Instance.player_state == Player.Player_State.SwordPlus)
                    {
                        SwordCardManager.Instance.SwordNum++;
                        SwordCardManager.Instance.updateText();
                    }

                    if (Player.Instance.player_state == Player.Player_State.Bomb)
                    {
                        BombCardManager.Instance.BombNum += 1;
                        BombCardManager.Instance.updateText();
                    }



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
            Player.Instance.player_state = Player.Player_State.BowPlus;
            Player.Instance.setAnimeOn("U-BowOn");
        }
        else
        {//是普通弓
            Debug.Log("release");
            Player.Instance.player_state = Player.Player_State.Bow;
            Player.Instance.setAnimeOn("BowOn");
        }
        return;
    }
}
