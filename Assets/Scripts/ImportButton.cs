using UnityEngine;

public class ImportButton : MonoBehaviour
{
    // Start is called before the first frame update
    

    public void OnClickImport()
    {
        var setupSelectionComponent = GetComponentInParent<SetupSelection>();

        setupSelectionComponent.OnClickImport?.Invoke(setupSelectionComponent.SelectedValue);
        
        Debug.Log($"Importing {setupSelectionComponent.SelectedValue}..");
    }
}