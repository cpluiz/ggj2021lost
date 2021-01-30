using System;
using System.Collections;
using System.Collections.Generic;
using RDG;
using Pathfinding;
using TMPro;
using UnityEngine;

public class SetPlayerTarget : MonoBehaviour{
    [Range(0,1)]
    public float minDistance = 0.2f;
    [Header("Objeto a ser referenciado")]
    public Transform targetPoint;
    [Header("AI controller")]
    public AIDestinationSetter targetController;
    [SerializeField]
    private InteractableTarget interactableObject;

    [SerializeField] private GameObject textArea;
    [SerializeField] private TextMeshPro textToShow;
    //private Animator robotAnimator;
    void Start(){
        targetController = GetComponent<AIDestinationSetter>();
        targetController.enabled = false;
        interactableObject = null;
        textArea.SetActive(false);
        //robotAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetMouseButton(0)){
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;
            targetPoint.position = Camera.main.ScreenToWorldPoint(mousePosition);
            targetController.target = targetPoint;
            targetController.enabled = true;
            interactableObject = null;
            //robotAnimator.SetBool("Walking", true);
            transform.localScale = new Vector3(Math.Sign(targetPoint.position.x - transform.position.x), 1, 1);
            textArea.transform.localScale = transform.localScale = new Vector3(Math.Sign(targetPoint.position.x - transform.position.x), 1, 1);
            RaycastHit2D hit = Physics2D.Raycast(targetPoint.position, -Vector2.up, 0.05f);
            if (hit.collider != null){
                if (hit.transform.gameObject.GetComponent<InteractableTarget>()){
                    interactableObject = hit.transform.gameObject.GetComponent<InteractableTarget>();
                }
            }
        }

        float newSize = (4 - transform.position.y);
        //transform.localScale = new Vector3(Math.Abs(newSize), newSize);
    }

    private void FixedUpdate(){
        if (Vector2.Distance(transform.position, targetPoint.position) <= minDistance){
            targetController.enabled = false;
            //robotAnimator.SetBool("Walking", false);
            if (interactableObject != null){
                Vibration.Vibrate(100, 100);
                textToShow.text = TextController.getString("TEXT_EXAMPLE_1");
                textArea.SetActive(true);
                interactableObject = null;
                StartCoroutine(nameof(HideTextArea));
            }
        }
    }

    public IEnumerator HideTextArea(){
        yield return new WaitForSeconds(5);
        textArea.SetActive(false);
    }
}
