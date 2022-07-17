using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    Unit yondUnit;
    Unit enemyUnit;

    public BattleState state;

    public GameObject yondPrefab;
    public GameObject enemyPrefab;

    public Transform yondStation;
    public Transform enemyStation;

    public BattleHUD yondHUD;
    public BattleHUD enemyHUD;

    private int damage;
    private int score;
    //private int money;
    private int num_rolls;
    public Yacht yacht_board;
    public Dice[] dice_sprites;

    private int bonus_val = 0;
    private bool short_stun;
    private bool long_stun_turn_1;
    private bool long_stun_turn_2;

    private string attack;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        yacht_board = new Yacht();
        //dice_set = new Dice[5];
        long_stun_turn_1 = false; // this needs to be set up here as unlike all the others we need to check it before setting it each turn

        SetupBattle();
    }

    void Update()
    {  
        for(int i = 0; i < 5; i++)
        {
            dice_sprites[i].UpdateHelper(yacht_board.dice.dice_set[i]);

        }
    }

    void SetupBattle() {
        GameObject yondGO = Instantiate(yondPrefab, yondStation);
        GameObject enemyGO = Instantiate(enemyPrefab, enemyStation);

        yondUnit = yondGO.GetComponent<Unit>();
        enemyUnit = enemyGO.GetComponent<Unit>();

        yondHUD.SetHUD(yondUnit);
        enemyHUD.SetHUD(enemyUnit);

        num_rolls = 2;

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn() {
        
    }

    public void onRollClick()
    {
        if(state!= BattleState.PLAYERTURN || num_rolls<=0){
            return;
        }
        else
        {
            yacht_board.dice.roll_selected_dice(yacht_board.dice.active);
        }
    }
    public void onAttackButton() {
        if (state != BattleState.PLAYERTURN) {
            return;
        }
        else {
            if(! yacht_board.available_choices["5"])
            {
                attack = "5";
                PlayerAttack();
            }
        }
    }
    public void onDiceClick(int die_num)
    {
        if(state!= BattleState.PLAYERTURN || num_rolls<=0){
            return;
        }
        dice_sprites[die_num].active = ! dice_sprites[die_num].active;
        yacht_board.dice.active[die_num]  = ! yacht_board.dice.active[die_num];
    }
    void PlayerAttack() {

        damage = 0;
        //money = 0;
        short_stun = false;
        long_stun_turn_2 = false;

        if (long_stun_turn_1 == true) {
            long_stun_turn_1 = false;
            long_stun_turn_2 = true;
        }

        /*
            Outcomes:
            - A base score of 1-6: Direct damage
            - 4 of a kind: Bonus die
            - Small straight: 1 turn stun
            - Large straight: 2 turn stun
            - Full House: Steal money B)
            - Yacht: Hit by a yacht (1 turn stun + extra money)
        */
        
        attack = "5";
        score = 35;

        if(string.Equals(attack,"1")) {
            score = yacht_board.score_one();
            damage = score + 1;
        }
        else if (string.Equals(attack,"2")) {
            score = yacht_board.score_two();
            damage = score + 2;
        }
        else if (string.Equals(attack,"3")) {
            score = yacht_board.score_three();
            damage = score + 3;
        }
        else if (string.Equals(attack,"4")) {
            score = yacht_board.score_four();
            damage = score + 4;
        }
        else if (string.Equals(attack,"5")) {
            damage = score + 5;
            score = yacht_board.score_five();
        }
        else if (string.Equals(attack,"6")) {
            damage = score + 6;
            score = yacht_board.score_six();
        }
        else if (string.Equals(attack,"4_of_a_kind")) {
            int bonus;
            score = yacht_board.score_4kind(out bonus);
            bonus_val = bonus;
            damage = score;
        }
        else if (string.Equals(attack,"short_straight")) {
            score = yacht_board.score_smstr8();
            damage = score;
            short_stun = true;
        }
        else if (string.Equals(attack,"long_straight")) {
            score = yacht_board.score_lgstr8();
            damage = score;
            long_stun_turn_1 = true;
        }
        else if (string.Equals(attack,"full_house")) {
            score = yacht_board.score_fhouse();
            damage = score;
        }
        else if (string.Equals(attack,"yacht")) {
            score = yacht_board.score_yacht();
            damage = score;
            short_stun = true;
        }
        if(bonus_val!= 0)
        {
            damage = damage + bonus_val;
            bonus_val = 0;
        }

        bool isDead = enemyUnit.TakeDamage(damage);

        enemyHUD.SetHP(enemyUnit.currentHP);

        if(isDead) {
            state = BattleState.WON;
            EndBattle();
        }
        else {
            if(short_stun || long_stun_turn_1 || long_stun_turn_2) {
                PlayerTurn();
            }
            else {
                state = BattleState.ENEMYTURN;
                EnemyTurn();
            } 
        }
    }

    void EnemyTurn() {

        damage = 0;

        for (int i = 0; i < 6; i++) {
            damage += Random.Range(1,7);
        }

        bool isDead = yondUnit.TakeDamage(damage);

        yondHUD.SetHP(yondUnit.currentHP);

        if(isDead) {
            state = BattleState.LOST;
            EndBattle();
        }
        else {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle() {
        if(state == BattleState.LOST) {

        }
        else {

        }
    }
}
