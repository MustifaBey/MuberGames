using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;

    private Vector3 targetPosition;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Başlangıçta hedef konum mevcut konum olsun
        targetPosition = transform.position;

        // Bileşenleri al
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleInput();
        Move();
    }

    void HandleInput()
    {
        // Mouse referansını al
        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        // Mouse sağ tık basılı tutulursa veya tıklanırsa (Input System)
        if (mouse.rightButton.isPressed)
        {
            // Mouse pozisyonunu al
            Vector2 mouseScreenPos = mouse.position.ReadValue();

            // Mouse pozisyonunu dünya koordinatlarına çevir
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

            // Z eksenini sıfırla (2D oyun için)
            mousePos.z = 0;

            targetPosition = mousePos;
        }
    }

    void Move()
    {
        // Hedefe doğru hareket et
        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Hareketi uygula
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Animasyon kontrolü
            if (animator != null)
            {
                animator.SetBool("IsRunning", true);
            }

            // Karakterin yönünü çevir (opsiyonel)
            if (targetPosition.x < transform.position.x)
            {
                // Sola bakıyor
                if (spriteRenderer != null) spriteRenderer.flipX = true;
                else transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                // Sağa bakıyor
                if (spriteRenderer != null) spriteRenderer.flipX = false;
                else transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            // Durma durumu
            if (animator != null)
            {
                animator.SetBool("IsRunning", false);
            }
        }
    }
}
