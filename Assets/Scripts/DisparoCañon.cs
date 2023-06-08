using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoCañon : MonoBehaviour
{
    [SerializeField] Transform PosicionDisparo;
    [SerializeField] GameObject bullet;
    GameObject objectBullet;
    [SerializeField] Camera cam;
    private Vector3 objetivo;
    public float bulletModifier;
    Vector2 direction;

    [SerializeField] private GameObject point;
    private GameObject[] pointsList;
    [SerializeField] private int pointsCount;
    [SerializeField] private float spaceBetween;
    // Start is called before the first frame update
    void Start()
    {
        pointsList = new GameObject[pointsCount];
        for (int i = 0; i < pointsCount; i++)
        {
            pointsList[i] = Instantiate(point, PosicionDisparo.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 launchePosition = transform.position;

        direction = mousePosition - launchePosition;

        transform.right = direction;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            objectBullet=Instantiate(bullet, PosicionDisparo.position, Quaternion.identity);
            objectBullet.GetComponent<Rigidbody>().velocity = transform.right * bulletModifier;
        }
        for (int i = 0; i < pointsCount; i++)
        {
            pointsList[i].transform.position = CurrentPosition(i * spaceBetween);
            pointsList[i].transform.SetParent(transform);
        }
    }
    void FollowMouse()
    {
        objetivo = cam.ScreenToWorldPoint(Input.mousePosition);

        float angulosRadianes =Mathf.Atan2(objetivo.y-transform.position.y,objetivo.x-transform.position.x);
        float angulosGrados = (180/Mathf.PI)*angulosRadianes-90;
        transform.rotation= Quaternion.Euler(0,0, angulosGrados);
    }
    private Vector2 CurrentPosition(float t)
    {
        return (Vector2)PosicionDisparo.position + (direction.normalized * bulletModifier * t) + (Vector2)(0.5f * Physics.gravity * (t * t));
    }
}
