using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    [SerializeField]
    private GameObject[] instructions;

    [SerializeField]
    private Transform placeholderPosition;

    private int currentIndex = -1;
    private Stack<GameObject> previousInstructions = new Stack<GameObject>();


    public void ShowNextInstruction()
    {
        Debug.Log("NEXT");

        // Deactivate the current instruction if it exists
        if (currentIndex >= 0 && currentIndex < instructions.Length)
        {
            if (instructions[currentIndex] != null)
                instructions[currentIndex].SetActive(false);
        }

        // Increment index and show next instruction
        currentIndex = (currentIndex + 1) % instructions.Length;
        GameObject newInstruction = Instantiate(instructions[currentIndex], placeholderPosition.position, Quaternion.identity);
        newInstruction.SetActive(true);

        // Push the new instruction onto the stack
        previousInstructions.Push(newInstruction);
        Debug.Log("NEXT Stack Count: " + previousInstructions.Count);
        Debug.Log("NEXT Current Top: " + previousInstructions.Peek().name);
    }

    public void ShowPreviousInstruction()
    {
        Debug.Log("PREVIOUS");
        Debug.Log("PREVIOUS Stack Count: " + previousInstructions.Count);

        // If there's only one instruction in the stack, remove it and deactivate the current instruction
        if (previousInstructions.Count == 1)
        {
            GameObject currentInstruction = previousInstructions.Pop();
            Destroy(currentInstruction); // Remove the current instruction from the scene

            // Deactivate the current instruction
            if (currentIndex >= 0 && currentIndex < instructions.Length)
            {
                if (instructions[currentIndex] != null)
                    instructions[currentIndex].SetActive(false);
            }
        }
        else if (previousInstructions.Count > 1)
        {
            // Pop the current instruction from the stack
            GameObject currentInstruction = previousInstructions.Pop();
            Destroy(currentInstruction); // Remove the current instruction from the scene

            // Pop the previous instruction from the stack and show it
            GameObject prevInstruction = previousInstructions.Peek();
            prevInstruction.transform.position = placeholderPosition.position;
            prevInstruction.SetActive(true);
            Debug.Log("PREVIOUS prevInstruction object Name :" + prevInstruction.name);
        }
        else
        {
            Debug.LogWarning("No previous instruction available.");
        }
    }
}





