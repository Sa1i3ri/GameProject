using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    [System.Serializable]
    public class Card
    {
        protected string name; // 卡牌名称
        protected int level; // 卡牌等级

        public string getName()
        {
            return this.name;
        }

        public int getLevel()
        {
            return this.level;
        }


    }


    public class SwordCard : Card
    {
        public SwordCard()
        {
            this.name = "Sword";
            this.level = 1;
        }

        public SwordCard(string name,int level)
        {
            this.name = name;
            this.level = level;
        }
        public SwordCard( int level)
        {
            this.name = "Sword";
            this.level = level;
        }

    }

    public class BowCard : Card
    {
        public BowCard()
        {
            this.name = "Bow";
            this.level = 1;
        }

        public BowCard(string name, int level)
        {
            this.name = name;
            this.level = level;
        }

        public BowCard(int level)
        {
            this.name = "Bow";
            this.level = level;
        }

    }







}
