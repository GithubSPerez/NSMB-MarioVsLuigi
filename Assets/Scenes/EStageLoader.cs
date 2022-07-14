using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

//[CreateAssetMenu(fileName = "ELoader", menuName = "Edited Stage Loader")]
public class EStageLoader : MonoBehaviour
{
    //public static MainMenuManager Instance;
    public TMP_Dropdown levelDropdown;
    public List<string> editedStages = new List<string> {
        "(E) Nightland",
        "(E) Mario 85",
        "(E) McChomk Land"
    };
    //bool addedToList = false;
    // Start is called before the first frame update
    
    void Start()
    {
        //print(Instance.levelCameraPositions[0]);
    }
    // Update is called once per frame
    void Update()
    {
        if (levelDropdown.options[levelDropdown.options.Count - 1].text != editedStages[editedStages.Count - 1])
        {
            levelDropdown.AddOptions(editedStages);
            //addedToList = true;
            print(levelDropdown.options[levelDropdown.options.Count - 1].text);
        }
    }
}
