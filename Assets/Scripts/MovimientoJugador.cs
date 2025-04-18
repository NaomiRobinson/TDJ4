using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public GameObject jugador;
    public GameObject jugadorEspejado;
    private Rigidbody2D rb;
    private Rigidbody2D rbEspejado;


    [SerializeField] private float velocidadX = 5f;

    private bool gravedadInvertida = false;

    void Start()
    {
        rb = jugador.GetComponent<Rigidbody2D>();
        rbEspejado = jugadorEspejado.GetComponent<Rigidbody2D>();

        rb.gravityScale = 1f;
        rbEspejado.gravityScale = 1f;

    }

    void Update()
    {
        // Movimiento en X (libre y espejado)
        if (Input.GetKey(KeyCode.A))
        {
            rb.linearVelocity = new Vector2(-velocidadX, rb.linearVelocity.y);
            rbEspejado.linearVelocity = new Vector2(velocidadX, rbEspejado.linearVelocity.y);

            SetFlipX(jugador, 1);
            SetFlipX(jugadorEspejado, 1);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.linearVelocity = new Vector2(velocidadX, rb.linearVelocity.y);
            rbEspejado.linearVelocity = new Vector2(-velocidadX, rbEspejado.linearVelocity.y);

            SetFlipX(jugador, -1);
            SetFlipX(jugadorEspejado, -1);
        }
        else
        {
            // Frena en X si no está moviéndose
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            rbEspejado.linearVelocity = new Vector2(0, rbEspejado.linearVelocity.y);
        }

        // Invertir gravedad con W, restaurar con S
        if (Input.GetKeyDown(KeyCode.W))
        {
            InvertirGravedad(true);
            SetFlipY(jugador, -1);
            SetFlipY(jugadorEspejado, -1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            InvertirGravedad(false);
            SetFlipY(jugador, 1);
            SetFlipY(jugadorEspejado, 1);
        }
    }

    void InvertirGravedad(bool invertir)
    {
        gravedadInvertida = invertir;
        float gravedad = invertir ? -1f : 1f;
        rb.gravityScale = gravedad;
        rbEspejado.gravityScale = gravedad;
    }

    void SetFlipX(GameObject obj, int direccion)
    {
        Vector3 scale = obj.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direccion;
        obj.transform.localScale = scale;
    }

    void SetFlipY(GameObject obj, int direccion)
    {
        Vector3 scale = obj.transform.localScale;
        scale.y = Mathf.Abs(scale.y) * direccion;
        obj.transform.localScale = scale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contacto in collision.contacts)
        {
            Vector2 normal = contacto.normal;

            if (Vector2.Angle(normal, Vector2.right) < 10f || Vector2.Angle(normal, Vector2.left) < 10f)
            {
                // Paredes laterales: frenar en X
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                rbEspejado.linearVelocity = new Vector2(0, rbEspejado.linearVelocity.y);
            }
            // No hace falta frenar en Y, lo hace la gravedad + colisión
        }
    }


}