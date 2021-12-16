using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DefenderAgent : Agent
{
    public enum DefendTask
    {
        AttackEnemy,
        DefendTower,
        Retreat,
        Wait,
    }

    public DefendTask defendTask;
    public float defendRange = 2;
    public bool isAvaliable = true;

    private Vector3 initialPos;
    public int towerIndex;

    // Start is called before the first frame update
    void Start()
    {
        defendTask = DefendTask.Wait;
        enemiesNearby = new List<Agent>();
        health = maxHealth;
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<TextMeshPro>().text = health.ToString();

        //Quick WASD for Testing
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, .1f, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-.1f, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -.1f, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(.1f, 0, 0);
        }

        //Blue and Red Have Different Targets and Functions
        if (isBlue)
        {
            BlueUpdate();
        }
        else
        {
            RedUpdate();
        }

    }

    public override void BlueUpdate()
    {
        switch (defendTask)
        {
            case DefendTask.AttackEnemy:
                if (target == null)
                {
                    defendTask = DefendTask.Wait;
                    break;
                }

                if (inRange(target.transform.position, defendRange))
                {

                    attackTimer += Time.deltaTime;

                    //Attack
                    if (attackTimer >= 3)
                    {
                        damage = Random.Range(1, 4);

                        target.GetComponent<Agent>().health -= damage;

                        //If Enemy Dies
                        if (target.GetComponent<Agent>().health <= 0)
                        {
                            target.GetComponent<Agent>().OnDeath();

                            if (enemiesNearby.Count <= 0)
                            {
                                MoveTo(initialPos);
                                defendTask = DefendTask.Retreat;
                            }
                        }

                        attackTimer = 0;
                    }
                }
                break;
            case DefendTask.DefendTower:
                if (inRange(Blackboard.instance.blueTower[towerIndex].transform.position, stopRange))
                {
                    transform.DOPause();
                    if (enemiesNearby.Count > 0)
                    {
                        target = enemiesNearby[0].gameObject;
                        defendTask = DefendTask.AttackEnemy;
                    }
                }
                break;
            case DefendTask.Retreat:
                if (inRange(initialPos, stopRange))
                {
                    defendTask = DefendTask.Wait;
                    isAvaliable = true;
                }
                break;
            case DefendTask.Wait:
                break;
            default:
                break;
        }
    }

    public override void RedUpdate()
    {
        switch (defendTask)
        {
            case DefendTask.AttackEnemy:
                if (target == null)
                {
                    defendTask = DefendTask.Wait;
                    break;
                }

                if (inRange(target.transform.position, defendRange))
                {

                    attackTimer += Time.deltaTime;

                    //Attack
                    if (attackTimer >= 3)
                    {
                        damage = Random.Range(1, 4);

                        target.GetComponent<Agent>().health -= damage;

                        //If Enemy Dies
                        if (target.GetComponent<Agent>().health <= 0)
                        {
                            target.GetComponent<Agent>().OnDeath();

                            if (enemiesNearby.Count <= 0)
                            {
                                MoveTo(initialPos);
                                defendTask = DefendTask.Retreat;
                            }
                        }

                        attackTimer = 0;
                    }
                }
                break;
            case DefendTask.DefendTower:
                if (inRange(Blackboard.instance.redTower[towerIndex].transform.position, stopRange))
                {
                    transform.DOPause();
                    if (enemiesNearby.Count > 0)
                    {
                        target = enemiesNearby[0].gameObject;
                        defendTask = DefendTask.AttackEnemy;
                    }
                }
                break;
            case DefendTask.Retreat:
                if (inRange(initialPos, stopRange))
                {
                    defendTask = DefendTask.Wait;
                    isAvaliable = true;
                }
                break;
            case DefendTask.Wait:
                break;
            default:
                break;
        }
    }

}
