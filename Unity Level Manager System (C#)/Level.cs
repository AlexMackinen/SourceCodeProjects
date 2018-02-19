/*Level.cs
 * Level just hosts all atributes that constitutes a game session
 * To use in a differnt context you need to include those application atributes
 * in order to work with level manager this class is responsible for getting the data
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The level class contains variables the level uses to variy the difficulty and game feel.
public class Level  {
    public float speed;
    public float complexity;
    public float spawnFreq;
    public float beamWidth;
    public float calibrate;
    public string name;

    //better contructer
    //adds to LevelManager being more independent
    //count is only used to name unnamed levels

    public Level(int count) {
        
        //pulls settings from playersettings
        speed = PlayerSettings.SpaceShipSpeed;
        complexity = PlayerSettings.SpaceShipMovement;
        spawnFreq = PlayerSettings.SpawnFrequency;
        beamWidth = PlayerSettings.BeamWidth;
        calibrate = PlayerSettings.calibrationSize;

        //trys to find a name for the level if no name is found go with untitiled + i
        GameObject lvlName = GameObject.Find("Name") as GameObject;
        if(lvlName.GetComponent<UnityEngine.UI.Text>().text == "") {
            name = "Untitled " + count;
        } else {
            name = lvlName.GetComponent<UnityEngine.UI.Text>().text;
        }
        


    }
    //level update
    //resets atributes with new values
    //not a constructer used for editing levels
    public void Levelupdate() {
        speed = PlayerSettings.SpaceShipSpeed;
        complexity = PlayerSettings.SpaceShipMovement;
        spawnFreq = PlayerSettings.SpawnFrequency;
        beamWidth = PlayerSettings.BeamWidth;
        calibrate = PlayerSettings.calibrationSize;
    }

}

