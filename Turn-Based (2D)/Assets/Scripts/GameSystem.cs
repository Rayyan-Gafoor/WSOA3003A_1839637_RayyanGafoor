using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStates
{
    Start,
    PlayerTurn,
    EnemyTurn,
    Win,
    Lose
}

public class GameSystem : MonoBehaviour
{
    public GameStates State;
    public GameObject Player;
    public GameObject Enemy;

    public Transform PlayerBattleStation;
    public Transform EnemyBattleStation;

    CharacterData PlayerData;
    CharacterData EnemyData;

    public Text dialogueText;
    public Text BuffText;
    public Text BuffApplied;
    public string Buff;

    public Text PlayerCharacterName;
    public Text PlayerCharacterStats;
    public Text PlayerCharacterInfo;

    public Text EnemyCharacterName;
    public Text EnemyCharacterStats;
    public Text EnemyCharacterInfo;



    // Start is called before the first frame update
    void Start()
    {
        State = GameStates.Start;
        StartCoroutine(BattleSetUp());

    }

    //game set up 
    IEnumerator BattleSetUp()
    {
        yield return new WaitForSeconds(2f);
        GameObject PlayerGameObject = Instantiate(Player, PlayerBattleStation);
        PlayerData = PlayerGameObject.GetComponent<CharacterData>();

        GameObject EnemyGameObject = Instantiate(Enemy, EnemyBattleStation);
        EnemyData = EnemyGameObject.GetComponent<CharacterData>();

        dialogueText.text = "A wild " + EnemyData.Name + " approaches";
        BuffText.text = "No Buff Active";
        BuffApplied.text = "No Buff Applied";
        //player stats bar
        PlayerCharacterName.text = "Player: " + PlayerData.Name;
        PlayerCharacterStats.text = "ATT:" + PlayerData.Attack + "  DEF:" + PlayerData.Defence + "  Health:" + PlayerData.CurrentHealth + "/" + PlayerData.Health + " ENG:" + PlayerData.current_ENG + "/" + PlayerData.ENG +" INT:" +PlayerData.INT;
        PlayerCharacterInfo.text = "Character Info: " + PlayerData.STRS_WEAK;
        //enemy stats bar
        EnemyCharacterName.text = "Enemy: " + EnemyData.Name;
        EnemyCharacterStats.text = "ATT:" + EnemyData.Attack + "  DEF:" + EnemyData.Defence + "  Health:" + EnemyData.CurrentHealth + "/" + EnemyData.Health + " INT:" + EnemyData.INT;
        EnemyCharacterInfo.text = "Character Info: " + EnemyData.STRS_WEAK;

        yield return new WaitForSeconds(2f);

        State = GameStates.PlayerTurn;
        PlayerTurn();
    }

    //Attack Function ...deals damage to enemy
    IEnumerator PlayerAttack()
    {
        bool isDead = EnemyData.PlayerAttacks(PlayerData.Attack);


        if (Buff == "Aether Buff")
        {
            BuffApplied.text = "Life Gain Has Been Used";
            Debug.Log("Aether buff has been used");
            PlayerData.CurrentHealth = PlayerData.CurrentHealth + EnemyData.CurrentHealth * (0.3f);
        }
        if (Buff == "Necro Buff")
        {
            BuffApplied.text = "Life Steal has been used";
            Debug.Log("Necro buff activated- Life Steal has been used");
            PlayerData.CurrentHealth = PlayerData.CurrentHealth + 5f;
            EnemyData.CurrentHealth = EnemyData.CurrentHealth - 5f;
        }
        if (Buff == "Aero Buff")
        {
            BuffApplied.text = "Energy Recharge";
            Debug.Log("Aero buff activated- Energy Recharge");
            PlayerData.current_ENG = PlayerData.current_ENG + 1f;
            
        }
        if (Buff == "Pyro Buff")
        {
            BuffApplied.text = "Vaporized is used";
            Debug.Log("Pyro buff activated- Vapourized is used");
            EnemyData.CurrentHealth = EnemyData.CurrentHealth - EnemyData.current_ENG * (0.3f);
        }
        if (Buff == "Hydro Buff")
        {
            BuffApplied.text = "Heal has been used";
            Debug.Log("Hydro buff activated- Heal has been used");
            PlayerData.CurrentHealth = PlayerData.CurrentHealth + 5f;
        }
        if (Buff == "Cryo Buff")
        {
            BuffApplied.text = "Freeze has been used";
            Debug.Log("Cryo buff activated- Freeze has been used");

            EnemyData.CurrentHealth = EnemyData.CurrentHealth - 2f;
            StartCoroutine(PlayerStun());
        }
        if (Buff == "Electro Buff")
        {
            BuffApplied.text = "Shock has been used";
            Debug.Log("Electro buff activated- Shock has been used");

            EnemyData.CurrentHealth = EnemyData.CurrentHealth - 6f;
            StartCoroutine(PlayerStun());
        }
        if (Buff == "Geo Buff")
        {
            BuffApplied.text = "Stun has been used";
            Debug.Log("Geo buff activated- Stun has been used");

            PlayerData.Defence = PlayerData.Defence + PlayerData.Defence * (0.1f);
            StartCoroutine(PlayerStun());
        }
        if (Buff == "Dendro Buff")
        {
            BuffApplied.text = "Heal has been used";
            Debug.Log("Dendro buff activated- Heal has been used");
            PlayerData.CurrentHealth = PlayerData.CurrentHealth + 2f;
        }
        

        EnemyCharacterStats.text = "ATT:" + EnemyData.Attack + "  DEF:" + EnemyData.Defence + "  Health:" + EnemyData.CurrentHealth + "/" + EnemyData.Health + " INT:" + EnemyData.INT;
        PlayerCharacterStats.text = "ATT:" + PlayerData.Attack + "  DEF:" + PlayerData.Defence + "  Health:" + PlayerData.CurrentHealth + "/" + PlayerData.Health + " ENG:" + PlayerData.current_ENG + "/" + PlayerData.ENG + " INT:" + PlayerData.INT;
        dialogueText.text = PlayerData.Name + "attacked " + EnemyData.Name;
        Debug.Log("player attacked");
        Debug.Log(Buff);

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            State = GameStates.Win;
            EndGame();
        }
        else
        {
            State = GameStates.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }
    IEnumerator PlayerSpecial()
    {

        bool isDead = EnemyData.SpecialAttack(PlayerData.Attack, PlayerData.Element, EnemyData.Element, PlayerData.Elemental_Damage, PlayerData.current_ENG);

        PlayerData.current_ENG = PlayerData.current_ENG - 1f;
        EnemyCharacterStats.text = "ATT:" + EnemyData.Attack + "  DEF:" + EnemyData.Defence + "  Health:" + EnemyData.CurrentHealth + "/" + EnemyData.Health + " INT:" + EnemyData.INT;
        PlayerCharacterStats.text = "ATT:" + PlayerData.Attack + "  DEF:" + PlayerData.Defence + "  Health:" + PlayerData.CurrentHealth + "/" + PlayerData.Health + " ENG:" + PlayerData.current_ENG + "/" + PlayerData.ENG + " INT:" + PlayerData.INT;
        dialogueText.text = PlayerData.Name + " used Special Attack on " + EnemyData.Name;
        Debug.Log("player special attacked");

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            State = GameStates.Win;
            EndGame();
        }
        else
        {
            State = GameStates.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal()
    {
        PlayerData.HealCharacter(PlayerData.INT);

        PlayerCharacterStats.text = "ATT:" + PlayerData.Attack + "  DEF:" + PlayerData.Defence + "  Health:" + PlayerData.CurrentHealth + "/" + PlayerData.Health + " ENG:" + PlayerData.current_ENG + "/" + PlayerData.ENG + " INT:" + PlayerData.INT;
        dialogueText.text = PlayerData.Name + "Used Heal... ";
        Debug.Log("player Heal");

        yield return new WaitForSeconds(2f);


        State = GameStates.EnemyTurn;
        StartCoroutine(EnemyTurn());
    }
    IEnumerator PlayerStun()
    {
        bool isStunned = EnemyData.Stunned(PlayerData.INT, EnemyData.INT);
        EnemyCharacterStats.text = "ATT:" + EnemyData.Attack + "  DEF:" + EnemyData.Defence + "  Health:" + EnemyData.CurrentHealth + "/" + EnemyData.Health + " INT:" + EnemyData.INT;

        dialogueText.text = PlayerData.Name + " tries to stun " + EnemyData.Name;
        Debug.Log("player Stun");

        yield return new WaitForSeconds(2f);

        if (isStunned)
        {
            dialogueText.text = EnemyData.Name + " is Stunned ";

            yield return new WaitForSeconds(3f);

            State = GameStates.PlayerTurn;
            PlayerTurn();
        }
        else
        {
            dialogueText.text = EnemyData.Name + " was not Stunned ";
            yield return new WaitForSeconds(3f);
            StartCoroutine(EnemyTurn());
        }
    }

    //Player Turn State
    void PlayerTurn()
    {
        dialogueText.text = " Choose An Action: ";
    }

    //Attack Button... Call Attack Function
    public void OnAttack()
    {
        if (State != GameStates.PlayerTurn)
        {
            return;
        }

        StartCoroutine(PlayerAttack());
    }
    public void OnSpecial()
    {
        if (State != GameStates.PlayerTurn)
        {
            return;
        }

        StartCoroutine(PlayerSpecial());
    }
    //Heal Button ... Call Heal Function
    public void OnHeal()
    {
        if (State != GameStates.PlayerTurn)
        {
            return;
        }

        StartCoroutine(PlayerHeal());
    }
    //Stun Button... Call Stun Function
    public void OnStun()
    {

        if (State != GameStates.PlayerTurn)
        {
            return;
        }

        StartCoroutine(PlayerStun());
    }



    //Enemy Turn State...
    IEnumerator EnemyTurn()
    {
        dialogueText.text = EnemyData.Name + "Attacks " + PlayerData.Name;

        yield return new WaitForSeconds(1f);

        bool isDead = PlayerData.EnemyAttacks(EnemyData.Attack);
        PlayerCharacterStats.text = "ATT:" + PlayerData.Attack + "  DEF:" + PlayerData.Defence + "  Health:" + PlayerData.CurrentHealth + "/" + PlayerData.Health + " ENG:" + PlayerData.current_ENG + "/" + PlayerData.ENG + " INT:" + PlayerData.INT;
        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            State = GameStates.Lose;
            EndGame();
        }
        else
        {
            State = GameStates.PlayerTurn;
            PlayerTurn();
        }
    }

    //End Game Function
     void EndGame()
    {
        if (State == GameStates.Win)
        {
            dialogueText.text = "You Won! ";
            BuffText.text = "a new Game will begin in shortly";
            

        }
        else if (State == GameStates.Lose)
        {
            dialogueText.text = "OH wow, would you look at that, you LOST :(";
            BuffText.text = "a new Game will begin in shortly";
        }
        
        StartCoroutine(BattleSetUp());
    }

    public void OnAetherBuff()
    {

        if (State != GameStates.PlayerTurn)
        {
            return;
        }
        Buff = "Aether Buff";
        BuffText.text = Buff + " Is Active";
        // StartCoroutine(PlayerStun());
    }
    public void OnNecroBuff()
    {

        if (State != GameStates.PlayerTurn)
        {
            return;
        }
        Buff = "Necro Buff";
        BuffText.text = Buff + " Is Active";
        // StartCoroutine(PlayerStun());
    }
    public void OnAeroBuff()
    {

        if (State != GameStates.PlayerTurn)
        {
            return;
        }
        Buff = "Aero Buff";
        BuffText.text = Buff + " Is Active";
        // StartCoroutine(PlayerStun());
    }
    public void OnPyroBuff()
    {

        if (State != GameStates.PlayerTurn)
        {
            return;
        }
        Buff = "Pyro Buff";
        BuffText.text = Buff + " Is Active";
        // StartCoroutine(PlayerStun());
    }
    public void OnHydroBuff()
    {

        if (State != GameStates.PlayerTurn)
        {
            return;
        }
        Buff = "Hydro Buff";
        BuffText.text = Buff + " Is Active";
        // StartCoroutine(PlayerStun());
    }
    public void OnElectroBuff()
    {

        if (State != GameStates.PlayerTurn)
        {
            return;
        }
        Buff = "Electro Buff";
        BuffText.text = Buff + " Is Active";
        // StartCoroutine(PlayerStun());

    }
    public void OnCryoBuff()
    {

        if (State != GameStates.PlayerTurn)
        {
            return;
        }
        Buff = "Cryo Buff";
        BuffText.text = Buff + " Is Active";
        // StartCoroutine(PlayerStun());
    }
    public void OnDendroBuff()
    {

        if (State != GameStates.PlayerTurn)
        {
            return;
        }
        Buff = "Dendro Buff";
        BuffText.text = Buff + " Is Active";
        // StartCoroutine(PlayerStun());
    }
    public void OnGeoBuff()
    {

        if (State != GameStates.PlayerTurn)
        {
            return;
        }
        Buff = "Geo Buff";
        BuffText.text = Buff + " Is Active";
        // StartCoroutine(PlayerStun());
    }
}
