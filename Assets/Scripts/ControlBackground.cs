using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBackground : MonoBehaviour
{
    [SerializeField] GameObject[] backgrounds;

    [SerializeField] float moveDistance = 10f;

    [SerializeField] GameObject followTarget;

    float totalMovedDistance = 0f;
    float initialReferenceHorizontalPosition;

    int numberOfTimesOfMoveDistance = 0;

    //int previousIndexMoveDistance0 = -1;
    //int previousIndexMoveDistance1 = 0;

    int[] previousIndexMoveDistance;

    private void Awake()
    {
        initialReferenceHorizontalPosition = followTarget.transform.position.x;

        previousIndexMoveDistance = new int[backgrounds.Length];

        for(int i=0; i<backgrounds.Length; i++)
        {
            previousIndexMoveDistance[i] = (1 + i) - backgrounds.Length;
        }
    }


    private void Update()
    {
        totalMovedDistance = (-followTarget.transform.position.x) - initialReferenceHorizontalPosition;

        numberOfTimesOfMoveDistance = (int)(totalMovedDistance / moveDistance);


        for(int i=0; i<backgrounds.Length; i++)
        {
            if(numberOfTimesOfMoveDistance >= previousIndexMoveDistance[i] + backgrounds.Length)
            {
                backgrounds[i].transform.localPosition = new Vector3(backgrounds[i].transform.localPosition.x + backgrounds.Length * moveDistance, backgrounds[i].transform.localPosition.y, backgrounds[i].transform.localPosition.z);
                previousIndexMoveDistance[i] = numberOfTimesOfMoveDistance;
            }            
        }

        //if (numberOfTimesOfMoveDistance >= previousIndexMoveDistance0 + 2)
        //{
        //    background0.transform.position = new Vector3(background0.transform.position.x + 2 * moveDistance, background0.transform.position.y, background0.transform.position.z);
        //    previousIndexMoveDistance0 = numberOfTimesOfMoveDistance;
        //}

        //if (numberOfTimesOfMoveDistance >= previousIndexMoveDistance1 + 2)
        //{
        //    background1.transform.position = new Vector3(background1.transform.position.x + 2 * moveDistance, background1.transform.position.y, background1.transform.position.z);
        //    previousIndexMoveDistance1 = numberOfTimesOfMoveDistance;
        //}


        
    }
}
