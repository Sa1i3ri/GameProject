using CardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSwordCard : MonoBehaviour
{
    public bool isUpgraded;

    public static UpgradeSwordCard Instance;



    private void Awake()
    {
        this.isUpgraded = false;

        if (Instance == null)
        {
            Instance = this;
        }

    }
    //Éý¼¶½£
    public void UpGradeSword()
    {
        if (isUpgraded == false && BowCardManager.Instance.BowNum > 0 && SwordCardManager.Instance.SwordNum > 0)
        {//Éý¼¶½£
            BowCardManager.Instance.UseCard(false);
            GetComponent<Image>().color = Color.gray;
            this.isUpgraded = true;

            SwordCardManager.Instance.getUpdated();

        }

        

    }
    //½µ¼¶½£
    public void DownGradeSword()
    {
        if (isUpgraded == true)
        {
            this.isUpgraded = false;
            GetComponent<Image>().color = new Color(0, 222, 0, 134);
        }

    }





}
