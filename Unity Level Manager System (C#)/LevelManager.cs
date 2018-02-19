/*Level Manager.cs
 * The level manager allows the creation of levels
 * and the abilities the manage those levels
 * as well as the UI to show the levels
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    //arrays for counting levels/playlists
    Level[] levels = new Level[1];
    Playlist[] playLists = new Playlist[1];

    //keeps track of # of levels
    public int count = 0;

    //object that gets spawned (set in unity menu)
    public Transform InstansiateMe;
    //object that gets spawned on (set in unity menu)
    public GameObject theParent;

    //needed to delete copys of Levelmanager, since dontdestroyonload creates copys
    private static bool spawned = false;

    //bool for editmode
    public bool isEditMode = false;


    //a short cut that keeps track of level currently being look at (kinda like a pointer)
    public int levelRef;

    //awake runs only when object with this script is created
    private void Awake() {

        //this is the check to see if the LevelManager spawning is a copy or not
        if (!spawned) {
            spawned = true;
            DontDestroyOnLoad(transform.gameObject);
        } else {
            DestroyImmediate(gameObject);
        }

        //the new way to have a method that runs everytime a scene loads
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    //the method that runs every time a scene loads
    //this is mainly used to ensure that sliders dont get reset during scene changes
    void onSceneLoaded(Scene scene, LoadSceneMode mode) {
        if(scene.name == "Settings") {

            //if you were in edit mode enter edit mode and load up a levels setting
            if (isEditMode) {
                exitEnterEdit(true);
                editHelper(levelRef);
            }

            //since buttons lose refernces to this class I have to add buttons that need it here
            Button create = findObj("YesButon").GetComponent<Button>();
            create.onClick.AddListener(() => getAndCreate());
            findObj("CreateLevelConfirmationPanel").SetActive(false);
            theParent = findObj("findMe");
            drawUI(levels);
        }
        if(scene.name == "Calibration") {
            //if in edit mode show the calibration for the level you are editing
            if(isEditMode) {
                findObj("Calibration Slider").GetComponent<Slider>().value = levels[levelRef].calibrate;
            }
        }
    }


   
    //adds a level to the level array 
    //also does quality of life changes such as clearing a textfeild for the name
    public void getAndCreate() {
        addLevel(new Level(count+1));
        findObj("InputField").GetComponent<InputField>().text = "";
        findObj("CreateLevelConfirmationPanel").SetActive(false);
        deleteUI();
        drawUI(levels);

    }

    //update level is called when you exit edit mode saving your changes
    public void updateLevel (int index) {
        levels[index].Levelupdate();
    }

    //return a level based on its index
    public Level getLevel(int index) {
        return levels[index];
    }

    //this basically tells the Playersetting class to run the given levels atributes insted of the defaults
    public void playLevel(int index) {
        levelRef = index;
        PlayerSettings settings = GameObject.Find("PlayerSettings").GetComponent<PlayerSettings>();
        PlayerSettings.customLevel = true;
        SceneManager.LoadScene(1);
        settings.playALevel(getLevel(index));
        

    }

    //adds a level to an array
    public void addLevel(Level newLevel) {
        if ( count != 0 ) {
            Level[] temp = new Level[levels.Length + 1];
            for (int i = 0; i < levels.Length; i++) {
                temp[i] = levels[i];
            }
            temp[temp.Length - 1] = newLevel;
            levels = temp;
            temp = null;
            count++;
        } else {
            levels[0] = newLevel;
            count++;
        }
    }
    //removes a Level from an array
    public void removeLevel(int index) {
        if(count == 1) {
            levels[0] = null;
            count--;
        } else {
            Level[] temp = new Level[levels.Length - 1];
            for (int i = 0; i < temp.Length; i++) {
                if (i >= index) {
                    temp[i] = levels[i + 1];
                } else {
                    temp[i] = levels[i];
                }
            }
            levels = temp;
            temp = null;
            count--;
        }
       
    }

    // Use this for initialization
    public void drawUI( Level[] array) {
        if(array[0] != null){
            for (int i = 1; i < array.Length + 1; i++) {
                //manages the sliders content box's size
                theParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (90 * i) + (5 + i));

                //go is each level
                var go = Instantiate(InstansiateMe, theParent.transform.position, transform.rotation);

                //find the levels name so it can draw it
                go.name = "level " + i;
                go.transform.SetParent(theParent.transform, false);
                go.Find("LvlName").GetComponent<UnityEngine.UI.Text>().text = array[i - 1].name;

                //temp int is the current place in the array
                int tempInt = i - 1;
                //creates buttons and specific actions for each of the levels buttons
                Button delete = go.Find("Delete").GetComponent<Button>();
                delete.onClick.AddListener(() => removeLevel(tempInt));
                delete.onClick.AddListener(() => deleteUI());
                delete.onClick.AddListener(() => drawUI(array));
                Button edit = go.Find("Edit").GetComponent<Button>();
                edit.onClick.AddListener(() => editMode(tempInt));
                Button play = go.Find("Play").GetComponent<Button>();
                play.onClick.AddListener(() => playLevel(tempInt));
            }
        }
        
    }
    //method called by button
    //sets level ref that calls other functions
    private void editMode(int level) {
        levelRef = level;
        exitEnterEdit(true);
        editHelper(level);
        

    }

    //this basically hide/shows buttons that are usable in edit mode.
    private void exitEnterEdit( bool edit) {
        findObj("LevelSettingsButton").gameObject.SetActive(!edit);
        findObj("AddLevelButton").gameObject.SetActive(!edit);
        findObj("MyLevelsButton").gameObject.SetActive(!edit);
        findObj("BackButton").gameObject.SetActive(!edit);
        findObj("DisplayLevelsPanel").gameObject.SetActive(!edit);
        findObj("CalibrateButton").gameObject.SetActive(!edit);
        findObj("EditUI").gameObject.SetActive(edit);
        isEditMode = edit;
    }

    //this is the method thats called to set the sliders properlly
    //and to create a button to save and exit
    private void editHelper(int index) {
        findObj("SS_Speed").GetComponent<Slider>().value = levels[index].speed;
        findObj("SS_Complexity").GetComponent<Slider>().value = levels[index].complexity;
        findObj("Spawn_Frequency").GetComponent<Slider>().value = levels[index].spawnFreq;
        findObj("Beam_Width").GetComponent<Slider>().value = levels[index].beamWidth;
        Button save = findObj("DoneEdit").GetComponent<Button>();
        save.onClick.AddListener(() => updateLevel(levelRef));
        save.onClick.AddListener(() => exitEnterEdit(false));
    }

    //deletes the levels showing used to update the list
    public void deleteUI() {
        if(theParent != null) {
            foreach (Transform child in theParent.transform) {
                Destroy(child.gameObject);
            }            
        }
    }

    //my search object since GameObject.find doesnt work on inactives this does
    public GameObject findObj(string name) {

        Transform[] trans = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trans) {
            if (t.gameObject.name == name) {
                return t.gameObject;
            }
        }
        return null;
    }

   



}
