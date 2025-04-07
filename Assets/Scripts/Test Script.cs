using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject[] cubes; 
    public float speed;


    void Start()
    {
    }
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movementVector = new Vector3(moveHorizontal, 0, moveVertical);

        foreach (GameObject obj in cubes)
        {
            obj.transform.position += movementVector * speed * Time.deltaTime;

            if (obj.transform.position.x > 5)
            {
                obj.transform.position = new Vector3(5, obj.transform.position.y, obj.transform.position.z);
            }
            else if (obj.transform.position.x < -5)
            {
                obj.transform.position = new Vector3(-5, obj.transform.position.y, obj.transform.position.z);
            }

            // Ограничиваем по оси Z
            if (obj.transform.position.z > 5)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 5);
            }
            else if (obj.transform.position.z < -5)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, -5);
            }
        }
    }
}
