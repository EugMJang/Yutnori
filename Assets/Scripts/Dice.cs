﻿using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour {

    private GameObject descriptionText;

    // Array of dice sides sprites to load from Resources folder
    private Sprite[] diceSides;

    // Reference to sprite renderer to change sprites
    private SpriteRenderer rend;

	// Use this for initialization
	private void Start () {

        // Assign Renderer component
        rend = GetComponent<SpriteRenderer>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");

        descriptionText = GameObject.Find("DescriptionText");
	}
	
    // If you left click over the dice then RollTheDice coroutine is started
    private void OnMouseDown()
    {
        StartCoroutine("RollTheDice");
    }

    // Returns the side that corresponds with the right probability.
    private int returnSide (int diceValue) {
        if (diceValue >= 1 && diceValue <= 72) {
            return 1;
        }
        else if (diceValue >= 73 && diceValue <= 288) {
            return 2;
        }
        else if (diceValue >= 289 && diceValue <= 504) {
            return 3;
        }
        else if (diceValue >= 505 && diceValue <= 585) {
            return 4;
        }
        else if (diceValue >= 586 && diceValue <= 601) {
            return 5;
        }
        else if (diceValue >= 602 && diceValue <= 625) {
            return -1;
        }
        else {
            throw new System.ArgumentException("Invalid die value");
        }
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice()
    {
        if (turnController.currentPlayer.GetComponent<Player>().canRoll) {
            turnController.currentPlayer.GetComponent<Player>().canRoll = false;

            // Variable to contain random dice side number.
            // It needs to be assigned. Let it be 0 initially
            int randomDiceSide = 0;

            // Final side or value that dice reads in the end of coroutine
            int finalSide = 0;

            // Loop to switch dice sides ramdomly
            // before final side appears. 20 itterations here.
            for (int i = 0; i <= 20; i++)
            {
                // Pick up random value from 0 to 5 (All inclusive)
                randomDiceSide = UnityEngine.Random.Range(1, 625);

                // Set sprite to upper face of dice from array according to random value
                if (returnSide(randomDiceSide) == -1) {
                    rend.sprite = diceSides[5];
                } else {
                    rend.sprite = diceSides[returnSide(randomDiceSide) - 1];
                }
                // Pause before next itteration
                yield return new WaitForSeconds(0.05f);
            }

            // Assigning final side so you can use this value later in your game
            // for player movement for example
            finalSide = returnSide(randomDiceSide);

            if (finalSide == 4 || finalSide == 5) {
                turnController.currentPlayer.GetComponent<Player>().canRoll = true;
                turnController.currentPlayer.GetComponent<Player>().numMoves += 1;
            }
            
            if (finalSide == -1 && !turnController.currentPlayer.GetComponent<Player>().canBackOne()) {
                turnController.switchTurns();
                descriptionText.GetComponent<Text>().text = "You weren't able to move!";
            }
            else{
                MoveScript.add(finalSide);
            }
        }
    }

    void Update() {
        ///*
        if (Input.GetKey(KeyCode.Alpha1)) {
            MoveScript.add(1);
        } else if (Input.GetKey(KeyCode.Alpha2)) {
            MoveScript.add(2);
        } else if (Input.GetKey(KeyCode.Alpha3)) {
            MoveScript.add(3);
        } else if (Input.GetKey(KeyCode.Alpha4)) {
            MoveScript.add(4);
        } else if (Input.GetKey(KeyCode.Alpha5)) {
            MoveScript.add(5);
        } else if (Input.GetKey(KeyCode.Alpha6)) {
            MoveScript.add(-1);
        }
        //*/
    }
}

