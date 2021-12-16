using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Arbiter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If Towers are Destroyed End the Game
        if (Blackboard.instance.redTower.Count == 0)
        {
            Blackboard.instance.blueWins.SetActive(true);
        }
        else if (Blackboard.instance.blueTower.Count == 0)
        {
            Blackboard.instance.redWins.SetActive(true);
        }
        else
        {
            //Run each Units Update if They Exist
            if (Blackboard.instance.blueAttackers.Count > 0)
            {
                BlueUpdate();
            }

            if (Blackboard.instance.blueDefenders.Count > 0)
            {
                BlueDefenderUpdate();
            }

            if (Blackboard.instance.blueHealers.Count > 0)
            {
                BlueHealerUpdate();
            }

            if (Blackboard.instance.redAttackers.Count > 0)
            {
                RedUpdate();
            }

            if (Blackboard.instance.redDefenders.Count > 0)
            {
                RedDefenderUpdate();
            }

            if (Blackboard.instance.redHealers.Count > 0)
            {
                RedHealerUpdate();
            }
        }

    }

    /// <summary>
    /// Helper Function to Return the Index of the Nearest Object in the List
    /// </summary>
    /// <param name="unit">Current start unit</param>
    /// <param name="destinations">List of possible destinations</param>
    /// <returns>Index of nearest object in list</returns>
    public int nearestIndex(GameObject unit, List<GameObject> destinations)
    {
        float shortestDistance = Mathf.Infinity;
        int index = 0;

        for (int i = 0; i < destinations.Count; i++)
        {
            //The Woderful Distance Formula
            float distance = Mathf.Sqrt(Mathf.Pow((unit.transform.position.x - destinations[i].transform.position.x), 2)
                + Mathf.Pow((unit.transform.position.y - destinations[i].transform.position.y), 2));

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                index = i;
            }
        }

        return index;
    }

    /// <summary>
    /// Helper Function to Return the Index of the Nearest Object in the List
    /// </summary>
    /// <param name="unit">Current start unit</param>
    /// <param name="destinations">List of possible destinations</param>
    /// <returns>Index of nearest object in list</returns>
    public int nearestIndex(GameObject unit, List<Agent> destinations)
    {
        float shortestDistance = Mathf.Infinity;
        int index = 0;

        for (int i = 0; i < destinations.Count; i++)
        {
            //The Woderful Distance Formula
            float distance = Mathf.Sqrt(Mathf.Pow((unit.transform.position.x - destinations[i].transform.position.x), 2)
                + Mathf.Pow((unit.transform.position.y - destinations[i].transform.position.y), 2));

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                index = i;
            }
        }

        return index;
    }
    /// <summary>
    /// Helper Function to Return the Index of the Nearest Object in the List
    /// </summary>
    /// <param name="unit">Current start unit</param>
    /// <param name="destinations">List of possible destinations</param>
    /// <returns>Index of nearest object in list</returns>
    public int nearestIndex(GameObject unit, List<HealerAgent> destinations)
    {
        float shortestDistance = Mathf.Infinity;
        int index = 0;

        for (int i = 0; i < destinations.Count; i++)
        {
            //The Woderful Distance Formula
            float distance = Mathf.Sqrt(Mathf.Pow((unit.transform.position.x - destinations[i].transform.position.x), 2)
                + Mathf.Pow((unit.transform.position.y - destinations[i].transform.position.y), 2));

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                index = i;
            }
        }

        return index;
    }

    public int nearestIndex(Vector3 unit, List<Agent> destinations)
    {
        float shortestDistance = Mathf.Infinity;
        int index = 0;

        for (int i = 0; i < destinations.Count; i++)
        {
            //The Woderful Distance Formula
            float distance = Mathf.Sqrt(Mathf.Pow((unit.x - destinations[i].transform.position.x), 2)
                + Mathf.Pow((unit.y - destinations[i].transform.position.y), 2));

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                index = i;
            }
        }

        return index;
    }

    /// <summary>
    /// Helper Function to Return the Two Nearest Agents For a Tower
    /// </summary>
    /// <param name="tower"></param>
    /// <param name="attackers"></param>
    /// <returns></returns>
    public List<Agent> nearestAgents(GameObject tower, List<Agent> attackers)
    {
        
        List<Agent> defenders = new List<Agent>();

        float shortestDistance = Mathf.Infinity;
        float secondShortestDistance = Mathf.Infinity;

        int index = 0;
        int index2 = 0;

        for (int i = 0; i < attackers.Count; i++)
        {
            //The Wonderful Distance Formula
            float distance = Mathf.Sqrt(Mathf.Pow((tower.transform.position.x - attackers[i].transform.position.x), 2)
                + Mathf.Pow((tower.transform.position.y - attackers[i].transform.position.y), 2));

            if (distance < secondShortestDistance)
            {
                if (distance < shortestDistance)
                {
                    secondShortestDistance = shortestDistance;
                    shortestDistance = distance;
                    index = i;
                }
                else
                {
                    secondShortestDistance = distance;
                    index2 = i;
                }

            }
        }

        defenders.Add(attackers[index]);
        defenders.Add(attackers[index2]);

        return defenders;
    }

    public void requestHealing(Agent agent)
    {
        //Find All Avaliable Healers
        List<HealerAgent> avaliableHealers = new List<HealerAgent>();

        if (agent.isBlue)
        {
            foreach (HealerAgent h in Blackboard.instance.blueHealers)
            {
                if (h.isAvaliable)
                {
                    avaliableHealers.Add(h);
                }
            }
        }
        else
        {
            foreach (HealerAgent h in Blackboard.instance.redHealers)
            {
                if (h.isAvaliable)
                {
                    avaliableHealers.Add(h);
                }
            }
        }

        //If None Are Avaliable, Return
        if (avaliableHealers.Count <= 0) { return; }

        //Find Nearest Avaliable Healer
        int temp = nearestIndex(agent.gameObject, avaliableHealers);

        agent.target = avaliableHealers[temp].gameObject;
        avaliableHealers[temp].isAvaliable = false;
        avaliableHealers[temp].target = agent.gameObject;
        avaliableHealers[temp].healTask = HealerAgent.HealTask.HealAttacker;
        agent.MoveTo(avaliableHealers[temp].transform.position);
        agent.task = Agent.Task.Retreat;
    }

    public void BlueUpdate()
    {

        //If an Attack Unit is Waiting Send It After An Enemy Tower
        foreach (Agent a in Blackboard.instance.blueAttackers)
        {
            if (a.task == Agent.Task.Wait)
            {
                //Find Nearest Tower to Unit
                int tower = nearestIndex(a.gameObject, Blackboard.instance.redTower);

                a.MoveTo(Blackboard.instance.redTower[tower].transform.position);
                a.target = Blackboard.instance.redTower[tower];
                a.targetIndex = tower;
                a.task = Agent.Task.MoveToTower;
            }
        }


        //Attack Enemies that are Nearby
        foreach (Agent a in Blackboard.instance.blueAttackers)
        {
            if (a.task != Agent.Task.AttackEnemy && a.enemiesNearby.Count > 0)
            {
                a.MoveTo(a.enemiesNearby[0].transform.position);
                a.target = a.enemiesNearby[0].gameObject;
                a.targetIndex = 0;
                a.task = Agent.Task.MoveToEnemy;
            }
        }

        //First Priority Should Be Defending Towers
        //If a Tower's Health is Below The Check Limit, Send Two of the Attackers to Defend It
        //Choose by nearest Attackers

        for (int i = 0; i < Blackboard.instance.blueTowerHps.Count; i++)
        {
            if (Blackboard.instance.blueTowerHps[i] < Blackboard.instance.blueTowerDefendHps[i])
            {
                //Find nearest two attackers
                float num = Mathf.Round(Blackboard.instance.blueAttackers.Count / 2);

                List<Agent> defenders = nearestAgents(Blackboard.instance.blueTower[i], Blackboard.instance.blueAttackers);

                foreach (Agent a in defenders)
                {
                    a.MoveTo(Blackboard.instance.blueTower[i].transform.position);
                    a.target = Blackboard.instance.blueTower[i];
                    a.targetIndex = i;
                    a.task = Agent.Task.DefendTower;
                }

                break;

            }
        }

        //If Enemy is in Critical Health, it retreats to its side to get healing if a healer is avaliable
        foreach (Agent a in Blackboard.instance.blueAttackers)
        {

            if (a.health <= Blackboard.instance.criticalHealth && a.task != Agent.Task.Retreat)
            {
                requestHealing(a);
            }
        }

        //If Any Type of Enemy is Low, Retreat and Summon
        bool summoning = false;
        foreach (Agent a in Blackboard.instance.blueAttackers)
        {
            if (a.task == Agent.Task.Summon)
            {
                summoning = true;
            }
        }

        //Attackers
        if (Blackboard.instance.blueAttackers.Count < 3 && !summoning)
        {
            int temp = nearestIndex(Blackboard.instance.blueSummonPos, Blackboard.instance.blueAttackers);
            Blackboard.instance.blueAttackers[temp].task = Agent.Task.Summon;
            Blackboard.instance.blueAttackers[temp].typeToSummon = "attacker";
            Blackboard.instance.blueAttackers[temp].MoveTo(Blackboard.instance.blueSummonPos);
        }
        //Defenders
        else if (Blackboard.instance.blueDefenders.Count < 2 && !summoning)
        {
            int temp = nearestIndex(Blackboard.instance.blueSummonPos, Blackboard.instance.blueAttackers);
            Blackboard.instance.blueAttackers[temp].task = Agent.Task.Summon;
            Blackboard.instance.blueAttackers[temp].typeToSummon = "defender";
            Blackboard.instance.blueAttackers[temp].MoveTo(Blackboard.instance.blueSummonPos);
        }
        //Healers
        else if (Blackboard.instance.blueHealers.Count < 1 && !summoning)
        {
            int temp = nearestIndex(Blackboard.instance.blueSummonPos, Blackboard.instance.blueAttackers);
            Blackboard.instance.blueAttackers[temp].task = Agent.Task.Summon;
            Blackboard.instance.blueAttackers[temp].typeToSummon = "healer";
            Blackboard.instance.blueAttackers[temp].MoveTo(Blackboard.instance.blueSummonPos);
        }
    }

    /// <summary>
    /// Update Red Agents with Instructions. Red will proritize attack enemies over towers.
    /// </summary>
    public void RedUpdate()
    {
        //If there are no enemies to attack, attack the towers
        if (Blackboard.instance.blueAttackers.Count <= 0)
        {
            //Send Each Unit to the Nearest Tower to Attack
            foreach (Agent a in Blackboard.instance.redAttackers)
            {
                if (a.task != Agent.Task.AttackTower && a.task != Agent.Task.Retreat)
                {

                    //Find Nearest Tower to Unit
                    int tower = nearestIndex(a.gameObject, Blackboard.instance.blueTower);

                    //Send Unit
                    a.MoveTo(Blackboard.instance.blueTower[tower].transform.position);
                    a.target = Blackboard.instance.blueTower[tower];
                    a.targetIndex = tower;
                    a.task = Agent.Task.MoveToTower;
                }
                
            }
        }
        else
        {
            //Attack Existing Enemies 
            foreach (Agent a in Blackboard.instance.redAttackers)
            {
                if (a.task != Agent.Task.AttackEnemy && a.task != Agent.Task.Retreat)
                {
                    int enemy = nearestIndex(a.gameObject, Blackboard.instance.blueAttackers);

                    a.MoveTo(Blackboard.instance.blueAttackers[enemy].transform.position);
                    a.target = Blackboard.instance.blueAttackers[enemy].gameObject;
                    a.targetIndex = enemy;
                    a.task = Agent.Task.MoveToEnemy;
                }
            }

            //First Priority Should Be Defending Towers
            //If a Tower's Health is Below Defend Check, Send 2 of the Attackers to Defend It
            //Choose by nearest Attackers
            for (int i = 0; i < Blackboard.instance.redTowerHps.Count; i++)
            {
                if (Blackboard.instance.redTowerHps[i] < Blackboard.instance.redTowerDefendHps[i]) //&& Blackboard.instance.redTowersBeingAttacked[i])
                {
                    //Find nearest two attackers
                    List<Agent> defenders = nearestAgents(Blackboard.instance.redTower[i], Blackboard.instance.redAttackers);

                    foreach (Agent a in defenders)
                    {
                        a.MoveTo(Blackboard.instance.redTower[i].transform.position);
                        a.target = Blackboard.instance.redTower[i];
                        a.targetIndex = i;
                        a.task = Agent.Task.DefendTower;
                    }

                    break;

                }
            }
        }

        //If Enemy is in Critical Health, it retreats to its side to get healing if a healer is avaliable
        foreach (Agent a in Blackboard.instance.redAttackers)
        {

            if (a.health <= Blackboard.instance.criticalHealth && a.task != Agent.Task.Retreat)
            {
                requestHealing(a);
            }
        }

        //If Any Type of Enemy is Low, Retreat and Summon
        bool summoning = false;
        foreach (Agent a in Blackboard.instance.redAttackers)
        {
            if (a.task == Agent.Task.Summon)
            {
                summoning = true;
            }
        }

        //Attackers
        if (Blackboard.instance.redAttackers.Count < 3 && !summoning)
        {
            int temp = nearestIndex(Blackboard.instance.redSummonPos, Blackboard.instance.redAttackers);
            Blackboard.instance.redAttackers[temp].task = Agent.Task.Summon;
            Blackboard.instance.redAttackers[temp].typeToSummon = "attacker";
            Blackboard.instance.redAttackers[temp].MoveTo(Blackboard.instance.redSummonPos);
        }
        //Defenders
        else if (Blackboard.instance.redDefenders.Count < 2 && !summoning)
        {
            int temp = nearestIndex(Blackboard.instance.redSummonPos, Blackboard.instance.redAttackers);
            Blackboard.instance.redAttackers[temp].task = Agent.Task.Summon;
            Blackboard.instance.redAttackers[temp].typeToSummon = "defender";
            Blackboard.instance.redAttackers[temp].MoveTo(Blackboard.instance.redSummonPos);
        }
        //Healers
        else if (Blackboard.instance.redHealers.Count < 1 && !summoning)
        {
            int temp = nearestIndex(Blackboard.instance.redSummonPos, Blackboard.instance.redAttackers);
            Blackboard.instance.redAttackers[temp].task = Agent.Task.Summon;
            Blackboard.instance.redAttackers[temp].typeToSummon = "healer";
            Blackboard.instance.redAttackers[temp].MoveTo(Blackboard.instance.redSummonPos);
        }

    }

    public void RedHealerUpdate()
    {
        //Check Tower's Health, if any are below the healing threshold, heal them
        //At half health send an avaliable healer
        //At quarter health send all avaliable healers

        for (int i = 0; i < Blackboard.instance.redTowerHps.Count; i++)
        {
            if (Blackboard.instance.redTowerHps[i] < Blackboard.instance.towerSuperCriticalHealth)
            {
                //Send All Healers
                foreach (HealerAgent a in Blackboard.instance.redHealers)
                {                 
                    a.MoveTo(Blackboard.instance.redTower[i].transform.position);
                    a.healTask = HealerAgent.HealTask.HealTower;
                    a.target = Blackboard.instance.redTower[i];
                    a.towerIndex = i;
                    a.isAvaliable = false;
                }
            }
        }

        //If towers are healed, return to initial position

    }

    public void BlueHealerUpdate()
    {
        //Check Tower's Health, if any are below the healing threshold, heal them
        //At half health send an avaliable healer
        //At quarter health send all avaliable healers

        for (int i = 0; i < Blackboard.instance.blueTowerHps.Count; i++)
        {
            if (Blackboard.instance.blueTowerHps[i] < Blackboard.instance.towerSuperCriticalHealth)
            {
                //Send All Healers
                foreach (HealerAgent a in Blackboard.instance.blueHealers)
                {
                    a.MoveTo(Blackboard.instance.blueTower[i].transform.position);
                    a.healTask = HealerAgent.HealTask.HealTower;
                    a.target = Blackboard.instance.blueTower[i];
                    a.towerIndex = i;
                    a.isAvaliable = false;
                }
            }
        }

        //If towers are healed, return to initial position

    }

    public void RedDefenderUpdate()
    {
        //If a Tower is Being Attacked, send all avaliable defenders
        for (int i = 0; i < Blackboard.instance.redTowerHps.Count; i++)
        {
            if (Blackboard.instance.redTowerHps[i] < Blackboard.instance.redTowerPrevHps[i])
            {
                foreach (DefenderAgent d in Blackboard.instance.redDefenders)
                {
                    if (d.isAvaliable)
                    {
                        d.MoveTo(Blackboard.instance.redTower[i].transform.position);
                        d.isAvaliable = false;
                        d.defendTask = DefenderAgent.DefendTask.DefendTower;
                        d.towerIndex = i;
                    }

                }

                Blackboard.instance.redTowerPrevHps[i] = Blackboard.instance.redTowerHps[i];
            }
        }



        //Check if any enemies are near the defenders and if so attack them
        foreach (DefenderAgent d in Blackboard.instance.redDefenders)
        {
            if (d.enemiesNearby.Count > 0 && d.defendTask != DefenderAgent.DefendTask.AttackEnemy && d.isAvaliable)
            {
                d.defendTask = DefenderAgent.DefendTask.AttackEnemy;
                d.isAvaliable = false;
                d.target = d.enemiesNearby[0].gameObject;
                d.MoveTo(d.enemiesNearby[0].transform.position);
            }
        }

    }

    public void BlueDefenderUpdate()
    {
        //If a Tower is Being Attacked, send all avaliable defenders
        for (int i = 0; i < Blackboard.instance.blueTowerHps.Count; i++)
        {
            if (Blackboard.instance.blueTowerHps[i] < Blackboard.instance.blueTowerPrevHps[i])
            {
                foreach (DefenderAgent d in Blackboard.instance.blueDefenders)
                {
                    if (d.isAvaliable)
                    {
                        d.MoveTo(Blackboard.instance.blueTower[i].transform.position);
                        d.isAvaliable = false;
                        d.defendTask = DefenderAgent.DefendTask.DefendTower;
                        d.towerIndex = i;
                    }

                }

                Blackboard.instance.blueTowerPrevHps[i] = Blackboard.instance.blueTowerHps[i];
            }
        }



        //Check if any enemies are near the defenders and if so attack them
        foreach (DefenderAgent d in Blackboard.instance.blueDefenders)
        {
            if (d.enemiesNearby.Count > 0 && d.defendTask != DefenderAgent.DefendTask.AttackEnemy && d.isAvaliable)
            {
                d.defendTask = DefenderAgent.DefendTask.AttackEnemy;
                d.isAvaliable = false;
                d.target = d.enemiesNearby[0].gameObject;
                d.MoveTo(d.enemiesNearby[0].transform.position);
            }
        }

    }
}
