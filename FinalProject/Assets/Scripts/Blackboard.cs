using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackboard : MonoBehaviour
{
    //Singleton Instance
    public static Blackboard instance;

    //Shared Variables
    public List<GameObject> redTower;
    public List<GameObject> blueTower;
    public List<Text> redTowerHpText;
    public List<Text> blueTowerHpText;
    public List<int> redTowerHps;
    public List<int> blueTowerHps;
    public List<int> redTowerPrevHps;
    public List<int> blueTowerPrevHps;
    public List<int> redTowerDefendHps;
    public List<int> blueTowerDefendHps;

    public List<Agent> blueAttackers;
    public List<Agent> redAttackers;
    public List<HealerAgent> blueHealers;
    public List<HealerAgent> redHealers;
    public List<DefenderAgent> blueDefenders;
    public List<DefenderAgent> redDefenders;


    public GameObject redPrefab;
    public GameObject bluePrefab;
    public GameObject redHealerPrefab;
    public GameObject blueHealerPrefab;
    public GameObject redDefenderPrefab;
    public GameObject blueDefenderPrefab;
    public GameObject blueWins;
    public GameObject redWins;

    public int criticalHealth = 3;
    public int towerCriticalHealth = 50;
    public int towerSuperCriticalHealth = 25;
    public Vector3 redSummonPos;
    public Vector3 blueSummonPos;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    //Spawning Methods
    public void SpawnRed()
    {
        float x = Random.Range(-7, 7.1f);

        GameObject temp = Instantiate(redPrefab, new Vector3(x, 2.5f, 0), Quaternion.identity);
        redAttackers.Add(temp.GetComponent<Agent>());
    }

    public void SpawnBlue()
    {
        float x = Random.Range(-7, 7.1f);

        GameObject temp = Instantiate(bluePrefab, new Vector3(x, -2.5f, 0), Quaternion.identity);
        blueAttackers.Add(temp.GetComponent<Agent>());
    }

    public void SpawnRedHealer()
    {
        float x = Random.Range(-7, 7.1f);

        GameObject temp = Instantiate(redHealerPrefab, new Vector3(x, 2.5f, 0), Quaternion.identity);
        redHealers.Add(temp.GetComponent<HealerAgent>());
    }

    public void SpawnBlueHealer()
    {
        float x = Random.Range(-7, 7.1f);

        GameObject temp = Instantiate(blueHealerPrefab, new Vector3(x, -2.5f, 0), Quaternion.identity);
        blueHealers.Add(temp.GetComponent<HealerAgent>());
    }

    public void SpawnRedDefender()
    {
        float x = Random.Range(-7, 7.1f);

        GameObject temp = Instantiate(redDefenderPrefab, new Vector3(x, 2.5f, 0), Quaternion.identity);
        redDefenders.Add(temp.GetComponent<DefenderAgent>());
    }

    public void SpawnBlueDefender()
    {
        float x = Random.Range(-7, 7.1f);

        GameObject temp = Instantiate(blueDefenderPrefab, new Vector3(x, -2.5f, 0), Quaternion.identity);
        blueDefenders.Add(temp.GetComponent<DefenderAgent>());
    }
}
