using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Target
{

    public int money { get; set; }

}

public class MoneySystem : MonoBehaviour
{

    //-----------------------------------
    //@GRG - Money System
    //-----------------------------------
    [SerializeField] private int startingAmount = 500;
    [SerializeField] private int moneyPerTurn = 500;
    [SerializeField] private float timeToFill = 2f;

    [SerializeField] private TextMeshProUGUI playerMoneyDisplay;

    public Target player, enemyAI;

    public static MoneySystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }


        //Create two instances of Tagret class
        player = new Target();
        enemyAI = new Target();

        //Set player and AI money to the starting amount
        player.money = enemyAI.money = startingAmount;

    }

    void Start()
    {
        
    }

    void Update()
    {

        DisplayPlayerMoney(player.money);
        
    }

    
    void DisplayPlayerMoney(int amount)
    {

        if (amount >= 1000)
        {

            //Use of String Builder to avoid garbage collectionGora
            playerMoneyDisplay.text = ((float) amount / 1000).ToString("0.0") + "K" + " €";

        }

        else
        {

            playerMoneyDisplay.text = amount.ToString() + " €";

        }

    }

    public void GiveTakeMoney(int amount, Target target)
    {
        //Starts a coroutine to smoothly add money
        StartCoroutine(ChangeValueSmoothly(amount, target));
    }

    private IEnumerator ChangeValueSmoothly(int amount, Target target)
    {
        //Calculates the amount of money that must be given each frame
        //(1/60 of a second) so adding money always lasts the same time.

        int moneyPerFrame = (int)(amount / timeToFill * 0.016f);

        while (Mathf.Abs(amount) > 0)
        {

            if (Mathf.Abs(moneyPerFrame) > Mathf.Abs(amount))                           
            {    
                //If the amount of money that should be given this frame
                //is higher than the reminder amount, give reminder instead

                target.money += amount;
                amount -= amount;

            }

            else
            {

                //Subtract from the total amount, add to the target's money
                amount -= moneyPerFrame;
                target.money += moneyPerFrame;
                             
            }

            //Wait a frame
            yield return new WaitForSeconds(0.016f);                

        }

    }

    public int MoneyPerTurn()
    {

        return moneyPerTurn;

    }
}
