using System.Collections.Generic;
using UnityEngine;

public class LevelManager
{
    //Initiliazed and used in playerMovement script.
    GameObject player;
    playerMovement playerScript;
    List<int> pollinatedFlowers;
    int flowerPollen;
    public static Level lvl_1 = new Level(Levels.lvl_1_Obs, Levels.lvl_1_Map, Levels.lvl_1_Flowers, 1);
    public static int currentLevel;

    public LevelManager(int levelNumber) {
        currentLevel = levelNumber;
        
        flowerPollen = -1;
        pollinatedFlowers = new List<int>();

        if(player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        playerScript = player.GetComponent<playerMovement>();
    }

    public void onPlayerMove() {
        int index = flowerIndex();
        //If the player is on a flower and the bee's current pollen is not from that flower, set the bee's pollen to that flower

        if (index > -1 && index != flowerPollen) {
            pollinatedFlowers.Add(index);
            flowerPollen = index;
        }

        if (pollinatedFlowers.Count == Levels.lvl_1_Flowers.Count) {
            //Do something
            // PlayerPrefs.SetInt("Level_1", 1);
            // auto to next level or go to level select?
        }
    }

    int flowerIndex() {
        //If the player is on a flower, return its index. Otherwise, return -1

        return Levels.lvl_1_Flowers.IndexOf(playerScript.getLocation());
    }

    Level getCurrentLevel() {
        //TODO 
        //Get current level from somewhere

        return new Level();
    }
}
