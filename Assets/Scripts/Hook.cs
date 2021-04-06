using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hook : MonoBehaviour
{
    //public Camera MainCamera; //be sure to assign this in the inspector to your main camera
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;


    public Transform HookedTransform;
    private Camera MainCamera;

    private int length, strength, FishCount;

    private Collider2D col;
    private bool canMove = true;

    //List<Fish>

    private Tweener CameraTeween;


    private void Awake() {
        MainCamera = Camera.main;
        col = GetComponent<Collider2D>();
        //list<fish>
    }
    // Start is called before the first frame update
    void Start() {

        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
        //objectHeight = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2

    }


    private void LateUpdate() {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
        //viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
        transform.position = viewPos; //new Vector3(viewPos.x, transform.position.y, transform.position.z);
    }
    // Update is called once per frame
    void Update()   {
        MovingHook();



        if (Input.GetMouseButtonUp(0)) {
            StartFishing();
        }
    }

    void MovingHook() {
        if (canMove && Input.GetMouseButton(0)) {
            Vector3 vector = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = transform.position;
            position.x = vector.x;
            transform.position = position;

        }
    }

    public void StartFishing() {
        length = -50; // idlemanager
        strength = 3;
        FishCount = 0;
        float time = (-length) * 0.1f;

        CameraTeween = MainCamera.transform.DOMoveY(length, 1 + time * 0.25f, false).OnUpdate(delegate
        {
            if (MainCamera.transform.position.y <= -11)
                transform.SetParent(MainCamera.transform);
        }).OnComplete(delegate
        {
            col.enabled = true;
            CameraTeween = MainCamera.transform.DOMoveY(0, time * 5, false).OnUpdate(delegate
            {
                if (MainCamera.transform.position.y >= -25f)
                    StopFishing();
            });
        });

        col.enabled = false;
        canMove = true;

    }

    void StopFishing() {

        canMove = false;
        CameraTeween.Kill(false);
        CameraTeween = MainCamera.transform.DOMoveY(0, 2, false).OnUpdate(delegate {
            if (MainCamera.transform.position.y >= -11) {
                transform.SetParent(null);
                transform.position = new Vector2(transform.position.x, -6);
            }
        }).OnComplete(delegate {
            //transform.position = Vector2.down * 15;
            //
            col.enabled = true;
        });
    }
}
