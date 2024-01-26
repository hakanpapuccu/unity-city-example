using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPlacement : MonoBehaviour
{
    private bool currentlyPlacing;
    private bool currentlyBulldozering;


    private BuildingPreset curBuildingPreset;

    private float indicatorUpdateTime = 0.05f;
    private float lastUpdateTime;
    private Vector3 curIndicatorPos;

    public GameObject placementIndicator;
    public GameObject bulldozerIndicator;
    public GameObject ErrorScreen;
    public Text ErrorText;


    public void BeginNewBuildingPlacement(BuildingPreset preset)
    {
        if(City.instance.money<preset.cost)
        {
            CancelBuildingPlacement();
            ErrorScreen.SetActive(true);
            ErrorText.text = "Your money is not enough to build this building.";
        }

        else
        {
            currentlyPlacing = true;
            curBuildingPreset = preset;
            placementIndicator.SetActive(true);
        }

        
    }

    public void ErrorOkButton()
    {
        ErrorScreen.SetActive(false);
    }

    void CancelBuildingPlacement()
    {
        currentlyPlacing = false;
        placementIndicator.SetActive(false);
    }

    public void ToggleBulldoze()
    {
        currentlyBulldozering = !currentlyBulldozering;
        bulldozerIndicator.SetActive(currentlyBulldozering);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CancelBuildingPlacement();


        if(Time.time-lastUpdateTime> indicatorUpdateTime)
        {
            lastUpdateTime = Time.time;


            curIndicatorPos = Selector.instance.GetCurTilePosition();


            if (currentlyPlacing)
                placementIndicator.transform.position = curIndicatorPos;
            else if (currentlyBulldozering)
                bulldozerIndicator.transform.position = curIndicatorPos;

        }

        if(Input.GetMouseButtonDown(0) && currentlyPlacing)
        {
            PlaceBuilding();
        } else if(Input.GetMouseButtonDown(0) && currentlyBulldozering)
        {
            Bulldoze();
        }
    }

    void PlaceBuilding()
    {
        GameObject buildingObj = Instantiate(curBuildingPreset.prefab, curIndicatorPos, Quaternion.identity);
        City.instance.OnPlaceBuilding(buildingObj.GetComponent<Building>());
        CancelBuildingPlacement();
    }

    void Bulldoze()
    {
        Building buildingToDestroy = City.instance.buildings.Find(x => x.transform.position == curIndicatorPos);
        if(buildingToDestroy!=null)
        {
            City.instance.OnRemoveBuilding(buildingToDestroy);
        }

    }





}
