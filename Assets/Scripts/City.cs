using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class City : MonoBehaviour
{
    public int money;
    public int day;
    public int curPopulation;
    public int curJobs;
    public int curFood;
    public int maxPopulation;
    public int maxJobs;
    public int incomePerJob;
    public Text statsText;
    public List<Building> buildings = new List<Building>();

    public static City instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateStatText();
    }

    public void OnPlaceBuilding(Building building)
    {
        money -= building.preset.cost;
        curPopulation += building.preset.population;
        curJobs += building.preset.jobs;
        curFood += building.preset.food;
        buildings.Add(building);
        UpdateStatText();
    }

    public void OnRemoveBuilding(Building building)
    {
        
        maxPopulation -= building.preset.population;
        maxJobs -= building.preset.jobs;
        buildings.Remove(building);
        Destroy(building.gameObject);
        UpdateStatText();
    }

    void UpdateStatText()
    {
        statsText.text = string.Format("Day: {0} \nMoney: {1} \nPop: {2} / {3} \nJobs:{4} /{5} \nFood: {6}", new object[7] { day, money, curPopulation, maxPopulation, curJobs, maxJobs, curFood });

    }

    public void EndTurn()
    {
        day++;
        CalculateMoney();
        CalculatePopulation();
        CalculateJobs();
        CalculateFood();
        UpdateStatText();


    }

    void CalculateMoney()
    {
        money += curJobs * incomePerJob;
        foreach (Building building in buildings)
            money -= building.preset.costPerTurn;

    }
    void CalculatePopulation()
    {
        if(curFood>=curPopulation && curPopulation< maxPopulation)
        {
            curFood -= curPopulation / 4;
            curPopulation = Mathf.Min(curPopulation + (curFood / 4), maxPopulation);
        }
        else if(curFood<curPopulation)
        {
            curPopulation = curFood;
        }

    }
    void CalculateJobs()
    {
        curJobs = Mathf.Min(curPopulation, maxJobs);

    }

    void CalculateFood()
    {
        curFood = 0;
        foreach (Building building in buildings)
            curFood += building.preset.food;

    }

}
