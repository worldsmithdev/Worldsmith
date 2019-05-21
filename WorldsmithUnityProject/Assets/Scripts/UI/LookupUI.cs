using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LookupUI : MonoBehaviour
{
    // Handles UI and functionality for Places - Lookup (World 2 Sub 1)

    public TMP_InputField searchField;
    public TMP_Dropdown combine1;
    public TMP_Dropdown combine2;
    public TMP_Dropdown combine3;
    public TMP_Dropdown combine4;
    public TMP_Dropdown combine5;
    public TMP_Dropdown combine6;
    public TMP_Dropdown compare1;
    public TMP_Dropdown compare2;
    public TMP_Dropdown compare3;
    public TMP_Dropdown compare4;
    public TMP_Dropdown compare5;
    public TMP_Dropdown compare6;
    public TMP_Dropdown value1;
    public TMP_Dropdown value2;
    public TMP_Dropdown value3;
    public TMP_Dropdown value4;
    public TMP_Dropdown value5;
    public TMP_Dropdown value6; 

    public List<TMP_Dropdown> blueDropdownsList;
    public List<TMP_Dropdown> redDropdownsList;
    public List<TMP_Dropdown> greenDropdownsList;

    bool passedBlue1 = false;
    bool passedBlue2 = false;
    bool passedBlue3 = false;
    bool passedRed1 = false;
    bool passedRed2 = false;
    bool passedGreen1 = false;

    bool noBlueSelected = true;
    bool noRedSelected = true;
    bool noGreenSelected = true;

    List<Location> tempBlueLocList = new List<Location>();
    List<Location> tempRedLocList = new List<Location>(); 
    List<Location> tempGreenLocList = new List<Location>(); 
    List<Location> locationSearchList = new List<Location>();


    private void Start()
    { 
        foreach (TMP_Dropdown dropdown in blueDropdownsList)        
            dropdown.onValueChanged.AddListener(delegate { ValueChangedHandler(); });        
        foreach (TMP_Dropdown dropdown in redDropdownsList)        
            dropdown.onValueChanged.AddListener(delegate { ValueChangedHandler(); });        
        foreach (TMP_Dropdown dropdown in greenDropdownsList)        
            dropdown.onValueChanged.AddListener(delegate { ValueChangedHandler(); });
    }
    public void SearchForLocation()
    {
        ContainerController.Instance.ToggleHighlights0();
        string str = searchField.text;
        foreach (Location loc in WorldController.Instance.GetWorld().locationList)
            if (str == loc.elementID.ToLower() || str == loc.elementID)
                locationSearchList.Add(loc);
        foreach (Location loc in locationSearchList)
            ContainerController.Instance.HighlightLocationRed(loc);

        locationSearchList = new List<Location>();
    }
    void Update()
    {
        if (searchField.isFocused && searchField.text != "" && Input.GetKey(KeyCode.Return))
        {
            foreach (Location loc in WorldController.Instance.GetWorld().locationList)
            {
                if (loc.elementID == searchField.text || loc.elementID.ToLower() == searchField.text)
                {
                    LocationController.Instance.SetSelectedLocation(loc);
                    CameraController.Instance.CenterCameraOnSelectedLocation();
                }
                // Add multi-location qualifiers here - description variable given as example
                else if (loc.description == searchField.text || loc.description.ToLower() == searchField.text )
                {
                    locationSearchList.Add(loc);
                }
            }
            SearchForLocation();
        }
    }
       
    private void ValueChangedHandler()
    { 
        UpdateHighlights();
    } 

    public void UpdateHighlights()
    {
        ResetHighlights();

        foreach (Location   loc in WorldController.Instance.GetWorld().locationList)
        {
            //BLUE
            if (combine1.value == 0)            
                passedBlue1 = true;            
            else
            { 
                passedBlue1 = CheckIfDropdownMatches(loc, combine1, compare1, value1);
                noBlueSelected = false;
            }
            if (combine2.value == 0)            
                passedBlue2 = true;            
            else
            {
                passedBlue2 = CheckIfDropdownMatches(loc,  combine2, compare2, value2);
                noBlueSelected = false;
            }
            if (combine3.value == 0)            
                passedBlue3 = true;            
            else
            {
                passedBlue3 = CheckIfDropdownMatches(loc,  combine3, compare3, value3);
                noBlueSelected = false;
            }
            //RED
            if (combine4.value == 0)            
                passedRed1 = true;            
            else
            {
                passedRed1 = CheckIfDropdownMatches(loc,  combine4, compare4, value4);
                noRedSelected = false;
            }
            if (combine5.value == 0)            
                passedRed2 = true;            
            else
            {
                passedRed2 = CheckIfDropdownMatches(loc,  combine5, compare5, value5);
                noRedSelected = false;
            }
            //GREEN
            if (combine6.value == 0)            
                passedGreen1 = true;
            else
            {
                passedGreen1 = CheckIfDropdownMatches(loc,  combine6, compare6, value6);
                noGreenSelected = false;
            } 

            if (passedBlue1 && passedBlue2 && passedBlue3)            
                tempBlueLocList.Add(loc);   
            if (passedRed1 && passedRed2)        
                tempRedLocList.Add(loc);
            if (passedGreen1)
                tempGreenLocList.Add(loc);

        }
        foreach (Location loc in tempBlueLocList)        
            if (noBlueSelected == false)            
                ContainerController.Instance.HighlightLocationBlue(loc);
        foreach (Location loc in tempRedLocList)
            if (noRedSelected == false)
                ContainerController.Instance.HighlightLocationRed(loc);
        foreach (Location loc in tempGreenLocList)
            if (noGreenSelected == false)
                ContainerController.Instance.HighlightLocationGreen(loc); 

        passedBlue1 = false;
        passedBlue2 = false;
        passedBlue3 = false;
        passedRed1 = false;
        passedRed2 = false; 
        passedGreen1 = false; 
        noBlueSelected = true;
        noRedSelected = true; 
        noGreenSelected = true; 
    } 

        public void ResetHighlights()
    {
        ContainerController.Instance.ToggleHighlights0();
        tempBlueLocList = new List<Location>();
        tempRedLocList = new List<Location>();
        tempGreenLocList = new List<Location>();
        passedBlue1 = false;
        passedBlue2 = false;
        passedBlue3 = false;
        passedRed1 = false;
        passedRed2 = false;  
        passedGreen1 = false;  
        noBlueSelected = true;
        noRedSelected = true;
        noGreenSelected = true;
    }

    public void ClearBlue()
    {
        foreach (TMP_Dropdown dropdown in blueDropdownsList)        
            dropdown.value = 0;        
        UpdateHighlights();
    }
    public void ClearRed()
    {
        foreach (TMP_Dropdown dropdown in redDropdownsList)        
            dropdown.value = 0;        
        UpdateHighlights();
    }
    public void ClearGreen()
    {
        foreach (TMP_Dropdown dropdown in greenDropdownsList)
            dropdown.value = 0;
        UpdateHighlights();
    }
     

    public bool CheckIfDropdownMatches(Location loc, TMP_Dropdown combineDropdown, TMP_Dropdown compareDropdown, TMP_Dropdown valueDropdown)
    {

        bool tempBool  = false;
        if (combineDropdown.value == 1)  // Variable 1
        {
            if (compareDropdown.value == 0) // EQUALS            
                if (loc.culturalType == valueDropdown.value)
                    tempBool = true;          
            if (compareDropdown.value == 1) // DOES NOT EQUAL            
                if (loc.culturalType != valueDropdown.value)
                    tempBool = true;            
            if (compareDropdown.value == 2) // GREATER THAN            
                if (loc.culturalType > valueDropdown.value)
                    tempBool = true;            
            if (compareDropdown.value < 3) // SMALLER THAN            
                if (loc.culturalType == valueDropdown.value)
                    tempBool = true;            
        }
        if (combineDropdown.value == 2)  // Variable 2
        {
            if (compareDropdown.value == 0) // EQUALS            
                if (loc.politicalType == valueDropdown.value)
                    tempBool = true;
            if (compareDropdown.value == 1) // DOES NOT EQUAL            
                if (loc.politicalType != valueDropdown.value)
                    tempBool = true;
            if (compareDropdown.value == 2) // GREATER THAN            
                if (loc.politicalType > valueDropdown.value)
                    tempBool = true;
            if (compareDropdown.value < 3) // SMALLER THAN            
                if (loc.politicalType == valueDropdown.value)
                    tempBool = true;
        }      
        if (combineDropdown.value == 3)  // Variable 3
        {
            if (compareDropdown.value == 0) // EQUALS            
                if (loc.economicType == valueDropdown.value)
                    tempBool = true;
            if (compareDropdown.value == 1) // DOES NOT EQUAL            
                if (loc.economicType != valueDropdown.value)
                    tempBool = true;
            if (compareDropdown.value == 2) // GREATER THAN            
                if (loc.economicType > valueDropdown.value)
                    tempBool = true;
            if (compareDropdown.value < 3) // SMALLER THAN            
                if (loc.economicType == valueDropdown.value)
                    tempBool = true;
        }    
       
        return tempBool;
    }
     

}
