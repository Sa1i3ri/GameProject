using CardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombCardManager : MonoBehaviour
{
    public static BombCardManager Instance;
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
    [SerializeField] public int BombNum = 0;

    //卡牌文本
    [SerializeField] private Text BombText;

    public BombCardManager(Text text)
    {
        this.BombText = text;
    }

    public void PickUpCard()
    {
        audioSource.clip = pick_sound;
        audioSource.Play();
        this.BombNum++;
        updateText();

    }
    //isRelease：是否要释放技能
    public void UseCard(bool isRelease)
    {
        if (Player.Instance.player_state == Player.Player_State.Bomb)
        {
            return;
        }

        Button button = GetComponent<Button>();
        if (button.tag == "BombCard")
        {
            Debug.Log("bomb --");
            if (this.BombNum > 0)
            {
                this.BombNum--;

                audioSource.clip = select_sound;
                audioSource.Play();




                if (isRelease)
                {
                    if (Player.Instance.player_state == Player.Player_State.Sword || Player.Instance.player_state == Player.Player_State.SwordPlus)
                    {
                        SwordCardManager.Instance.SwordNum++;
                        SwordCardManager.Instance.updateText();
                    }

                    if (Player.Instance.player_state == Player.Player_State.Bow || Player.Instance.player_state == Player.Player_State.BowPlus)
                    {
                        BowCardManager.Instance.BowNum += 1;
                        BowCardManager.Instance.updateText();
                    }
                    useBomb();

                }

            }

        }

        updateText();


    }


    public void updateText()
    {
        this.BombText.text = this.BombNum.ToString();
    }

    //使用摧毁城的效果
    private void useBomb()
    {


        Player.Instance.player_state = Player.Player_State.Bomb;
        Player.Instance.setAnimeOn("BombOn");

        return;
    }
}
