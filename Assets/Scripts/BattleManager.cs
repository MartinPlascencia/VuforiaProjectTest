using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{

    [SerializeField]
    private List<Fighter> fighters = new List<Fighter>();
    [SerializeField]
    private int requiredFighters = 2;
    [SerializeField]
    private float secondsBetweenAttacks = 1f;
    [SerializeField]
    private float secondsToStartBattle = 1f;
    [SerializeField]
    public UnityEvent onBattleStart;
    [SerializeField]
    private UnityEvent onBattleStop;
    private int currentFighterIndex = 0;
    private bool isBattleActive = false;
    public void AddFighter(Fighter fighter)
    {
        fighters.Add(fighter);
        CheckFighters();
    }
    public void RemoveFighter(Fighter fighter)
    {
        fighters.Remove(fighter);
        CheckFighters();
    }

    private void CheckFighters()
    {
        if (fighters.Count < requiredFighters)
        {
            StopBattle();
        } 
        else
        {
            StartBattle();
        }
    }

    private void StartBattle()
    {
        isBattleActive = true;
        onBattleStart?.Invoke();
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {

        currentFighterIndex = Random.Range(0, fighters.Count);
        Fighter attacker = fighters[currentFighterIndex];
        Fighter defender;
        do
        {
            currentFighterIndex = Random.Range(0, fighters.Count);
            defender = fighters[currentFighterIndex];
        } while (attacker == defender);

        attacker.Attack();
        float damage = attacker.GetDamage();
        defender.GetComponent<Health>().TakeDamage(damage);

        yield return new WaitForSeconds(secondsBetweenAttacks);
        if (defender.GetComponent<Health>().CurrentHealth > 0)
        {
            StartCoroutine(Attack());
        } else
        {
            StopBattle();
        }
    }

    private void StopBattle()
    {
        StopCoroutine(Attack());
        onBattleStop?.Invoke();
    }
}
