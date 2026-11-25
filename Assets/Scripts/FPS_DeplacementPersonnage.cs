using UnityEngine;
using UnityEngine.InputSystem;

public class FPS_DeplacementPersonnage : MonoBehaviour
{
    [SerializeField] InputAction deplacementInput;
    [SerializeField] InputAction sautInput;
    [SerializeField] InputAction accelerationInput;

    [SerializeField] float vitesseDeplacement = 7.5f;
    [SerializeField] float multiplicateurCourse = 2.2f;
    [SerializeField] float hauteurSaut = 2f;
    [SerializeField] float gravite = -9.81f;

    float velociteVerticale;
    Vector3 deplacement;
    CharacterController characterController;
    Animator animator;
    bool estEnCourse = false;

    void OnEnable()
    {
        deplacementInput.Enable();
        sautInput.Enable();
        accelerationInput.Enable();
    }

    void OnDisable()
    {
        deplacementInput.Disable();
        sautInput.Disable();
        accelerationInput.Disable();
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (characterController.isGrounded && sautInput.triggered)
        {
            velociteVerticale = Mathf.Sqrt(hauteurSaut * -2f * gravite);
        }

        if (characterController.isGrounded && velociteVerticale < 0)
        {
            velociteVerticale = -2f;
        }
        else
        {
            velociteVerticale += gravite * Time.deltaTime;
        }

        deplacement = deplacementInput.ReadValue<Vector2>();
        estEnCourse = accelerationInput.IsPressed();

        Vector3 mouvement = transform.right * deplacement.x + transform.forward * deplacement.y;
        float vitesseActuelle = estEnCourse ? vitesseDeplacement * multiplicateurCourse : vitesseDeplacement;

        characterController.Move(mouvement * vitesseActuelle * Time.deltaTime + Vector3.up * velociteVerticale * Time.deltaTime);

        if (animator != null)
        {
            float vitesseAnim = mouvement.magnitude * (estEnCourse ? multiplicateurCourse : 1f);
            animator.SetFloat("vitesse", vitesseAnim);
        }
    }
}