using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class HealerAgent : Agent
{
    public enum HealTask
    {
        HealAttacker,
        HealTower,
        Retreat,
        Wait,
    }

    public HealTask healTask;
    private int healAmount = 2;
    public float healRange = 1;
    public float healTimer;
    public bool isAvaliable = true;
    private Vector3 initialPos;
    public int towerIndex;

    // Start is called before the first frame update
    void Start()
    {
        healTask = HealTask.Wait;
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
        switch (healTask)
        {
            case HealTask.HealAttacker:
                if (target == null)
                {
                    healTask = HealTask.Wait;
                    break;
                }

                if (inRange(target.transform.position, healRange))
                {
                    healTimer += Time.deltaTime;

                    if (healTimer >= 3)
                    {
                        target.GetComponent<Agent>().health += healAmount;
                        healTimer = 0;
                        Debug.Log("Heal");
                    }

                    //If Attacker is Fully Healed, Switch
                    if (target.GetComponent<Agent>().health >= target.GetComponent<Agent>().maxHealth)
                    {
                        target.GetComponent<Agent>().health = target.GetComponent<Agent>().maxHealth;
                        target = null;
                        healTask = HealTask.Wait;
                        isAvaliable = true;
                        healTimer = 0;
                    }

                }
                else
                {
                    healTimer = 0;
                }
                break;
            case HealTask.HealTower:
                if (inRange(target.transform.position, healRange))
                {
                    if (target == null)
                    {
                        healTask = HealTask.Wait;
                        break;
                    }

                    if (inRange(target.transform.position, healRange))
                    {
                        healTimer += Time.deltaTime;

                        if (healTimer >= 3)
                        {
                            Blackboard.instance.blueTowerHps[towerIndex] += healAmount;
                            Blackboard.instance.blueTowerHpText[towerIndex].text = Blackboard.instance.blueTowerHps[towerIndex].ToString();
                            healTimer = 0;
                            Debug.Log("Heal");
                        }

                        //If Attacker is Fully Healed, Switch
                        if (Blackboard.instance.blueTowerHps[towerIndex] >= Blackboard.instance.towerCriticalHealth)
                        {
                            target = null;
                            healTask = HealTask.Retreat;
                            MoveTo(initialPos);
                            isAvaliable = true;
                            healTimer = 0;
                        }

                    }
                }
                break;
            case HealTask.Retreat:
                if (inRange(initialPos, stopRange))
                {
                    healTask = HealTask.Wait;
                }
                break;
            case HealTask.Wait:
                break;
            default:
                break;
        }
    
    }

    public override void RedUpdate()
    {
        switch (healTask)
        {
            case HealTask.HealAttacker:
                if (target == null)
                {
                    healTask = HealTask.Wait;
                    break;
                }

                if (inRange(target.transform.position, healRange))
                {
                    healTimer += Time.deltaTime;

                    if (healTimer >= 3)
                    {
                        target.GetComponent<Agent>().health += healAmount;
                        healTimer = 0;
                        Debug.Log("Heal");
                    }

                    //If Attacker is Fully Healed, Switch
                    if (target.GetComponent<Agent>().health >= target.GetComponent<Agent>().maxHealth)
                    {
                        target.GetComponent<Agent>().health = target.GetComponent<Agent>().maxHealth;
                        target = null;
                        healTask = HealTask.Wait;
                        isAvaliable = true;
                        healTimer = 0;
                    }

                }
                else
                {
                    healTimer = 0;
                }
                break;
            case HealTask.HealTower:
                if (inRange(target.transform.position, healRange))
                {
                    if (target == null)
                    {
                        healTask = HealTask.Wait;
                        break;
                    }

                    if (inRange(target.transform.position, healRange))
                    {
                        healTimer += Time.deltaTime;

                        if (healTimer >= 3)
                        {
                            Blackboard.instance.redTowerHps[towerIndex] += healAmount;
                            Blackboard.instance.redTowerHpText[towerIndex].text = Blackboard.instance.redTowerHps[towerIndex].ToString();
                            healTimer = 0;
                            Debug.Log("Heal");
                        }

                        //If Attacker is Fully Healed, Switch
                        if (Blackboard.instance.redTowerHps[towerIndex] >= Blackboard.instance.towerCriticalHealth)
                        {
                            target = null;
                            healTask = HealTask.Retreat;
                            MoveTo(initialPos);
                            isAvaliable = true;
                            healTimer = 0;
                        }

                    }
                }
                break;
            case HealTask.Retreat:
                if (inRange(initialPos, stopRange))
                {
                    healTask = HealTask.Wait;
                }
                break;
            case HealTask.Wait:
                break;
            default:
                break;
        }
    }
}
