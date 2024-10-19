using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupSelection : MonoBehaviour
{

    public Action<string> OnClickImport { get; set; }
    public string SelectedValue
    {
        get
        {
            var dropDown = GetComponent<Dropdown>();
           return dropDown.options[dropDown.value].text;
        }
    }

    public void UpdateSetupList(List<string> setupList)
    {
        var dropDown = GetComponent<Dropdown>();
        dropDown.ClearOptions();
        dropDown.AddOptions(setupList);
    }
    private void Start()
    {
        var dropDown = GetComponent<Dropdown>();
        dropDown.AddOptions( new List<string>() {"Loading.."});
    }

    public void OnSelectSetup()
    {
     
       // var dropDown = GetComponent<Dropdown>();
     
    }
}