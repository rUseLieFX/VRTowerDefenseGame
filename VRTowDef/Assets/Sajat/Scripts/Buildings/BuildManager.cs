using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using TMPro;
using TMPro.EditorUtilities;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    [SerializeField] GameObject[] allBuildings;
    [SerializeField] GameObject selectedBuildingGO = null;
    [SerializeField] Building selectedBuilding = null;
    [SerializeField] TMP_Text bruh; //IDEIGLENES - mutatja az UI-n a p�nzt.
    [SerializeField] int money;
    public GameObject SelectedBuilding {  get { return selectedBuildingGO; } }
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        bruh.text = money.ToString();
    }

    public void Build(BuildableGrid grid)
    {
        if (selectedBuildingGO == null) //Elad�s van kiv�lasztva.
        {
            if (grid.hasBuilding)
            {
                grid.currentBuilding.Sell();
                Money(grid.currentBuilding.cost / 2);
                grid.hasBuilding = false;
            }
            else
            {
                Debug.Log("Elad�s van kiv�lasztva, de nincs mit eladni!");
            }
            return;
        }

        //Teh�t nem elad�s van kiv�lasztva...

        if (grid.hasBuilding) 
        {
            Debug.Log("Itt m�r van �p�let, nem tudok ide �p�tkezni!");
            return;
        }

        //Teh�t nincs �p�let m�g a mez�n...

        if (selectedBuilding.cost <= money) // Ha van el�g p�nzed..
        {
            Money(-selectedBuilding.cost);
            grid.currentBuilding = Instantiate(selectedBuildingGO, grid.transform.position, Quaternion.identity).GetComponent<Building>();
            grid.hasBuilding = true;

            return;
        }
        Debug.Log("Nincs el�g p�nzed!");
    }
    public void ChangeSelectedBuilding(int id)
    {
        if (id == 0) //A 0-s ID az elad�s.
        {
            selectedBuildingGO = null;
            selectedBuilding = null;
            return;
        }
        selectedBuildingGO = allBuildings[id-1];
        selectedBuilding = selectedBuildingGO.GetComponent<Building>();
    }

    public void Money(int bonus)
    {
        money += bonus;
        bruh.text = money.ToString();
    }

    public int Money()
    {
        return money;
    }
}
