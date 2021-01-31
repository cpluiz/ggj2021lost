using System;
using RDG;
using Pathfinding;
using ScenarySripts;
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
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Rigidbody2D playerBody;

    [Header("Foot targets for sound purposes")] 
    [SerializeField] private Transform[] footReference;
    [SerializeField] private LayerMask groundTypes;
    private bool[] footOnGround;
    void Start(){
        targetController = GetComponent<AIDestinationSetter>();
        targetController.enabled = false;
        interactableObject = null;
        footOnGround = new bool[footReference.Length];
        for (int i = 0; i < footOnGround.Length; i++){
            footOnGround[i] = true;
        }
    }

    // Update is called once per frame
    void Update(){
        if (ScriptableEventController.isInsideSequence){
            StopAllCoroutines();
            playerAnimator.SetBool("Walking", false);
            return;
        }
        if (Input.GetMouseButton(0)){
            CheckTarget();
        }
    }

    private void LateUpdate(){
        CheckDistanceToInteractable();
        CheckFootsteps();
    }

    private void CheckTarget(){
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        targetPoint.position = Camera.main.ScreenToWorldPoint(mousePosition);
        targetController.target = targetPoint;
        targetController.enabled = true;
        interactableObject = null;
        playerAnimator.SetBool("Walking", true);
        transform.localScale = new Vector3(Math.Sign(transform.position.x - targetPoint.position.x), 1, 1);
        textArea.transform.localScale = transform.localScale;
        RaycastHit2D hit = Physics2D.Raycast(targetPoint.position, -Vector2.up, 0.05f);
        if (hit.collider != null){
            if (hit.transform.gameObject.GetComponent<InteractableTarget>()){
                interactableObject = hit.transform.gameObject.GetComponent<InteractableTarget>();
            }
        }
    }

    private void CheckFootsteps(){
        if (!playerAnimator.GetBool("Walking")) return;
        RaycastHit2D hit;
        int index = 0;
        foreach (var foot in footReference){
            hit = Physics2D.CircleCast(foot.position, 0.01f, Vector2.down, 0.01f, groundTypes);
            if (hit.collider != null && !footOnGround[index]){
                footOnGround[index] = true;
                AudioAmbienceController.PlayStepAudio(hit.collider.gameObject.layer);
            }else if(hit.collider == null){
                footOnGround[index] = false;
            }
            index++;
        } 
    }

    private void CheckDistanceToInteractable(){
        if (Math.Abs(transform.position.x - targetPoint.position.x) <= minDistance){
            targetController.enabled = false;
            playerAnimator.SetBool("Walking", false);
            if (interactableObject != null){
                Vibration.Vibrate(100, 100);
                textArea.SetActive(true);
                interactableObject = null;
            }
        }
    }
}
