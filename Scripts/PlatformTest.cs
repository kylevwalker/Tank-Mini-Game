using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTest : MonoBehaviour
{

    private Vector3 A;
    private float moveSpeed = 2f;
    private int direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        A = new Vector3(3, 1, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x >= 7 || transform.position.x <= 0)
        {
            direction *= -1;
        }
        
        Vector3 target = new Vector3(4, 0, 0) * direction;
        transform.position = Vector3.MoveTowards(transform.position, A + target, moveSpeed * Time.deltaTime);
    }
}
