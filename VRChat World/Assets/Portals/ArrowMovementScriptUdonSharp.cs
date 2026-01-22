
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ArrowMovementScriptUdonSharp : UdonSharpBehaviour
{

    public float rotationSpeed;
    public AnimationCurve curvelt;
    public bool bounce;
    private float startY;

    private void Start()
    {
        startY = transform.position.y;
    }

    public void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        if (bounce == true)
        {
            transform.position = new Vector3(transform.position.x, startY + curvelt.Evaluate((Time.time % curvelt.length)), transform.position.z);
        }
    }
}
