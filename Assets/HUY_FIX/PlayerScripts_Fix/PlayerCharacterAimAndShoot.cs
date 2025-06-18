using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerCharacterAimAndShoot : MonoBehaviour
{
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float recoilForce = 5f;

    [Header("Ammo Settings")]
    [SerializeField] private int maxAmmo = 6;
    [SerializeField] private float reloadTime = 2f;

    private int currentAmmo;
    private bool isReloading;

    private Vector2 worldPosition;
    private Vector2 direction;
    private float angle;

    private Coroutine reloadCoroutine; //Luu so luong dan

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentAmmo = maxAmmo;
        UIBulletCaculator(0);
    }

    private void Update()
    {
        HandleGunRotation();
        if (!isReloading)
        {
            HandleGunShooting();
        }
    }

    private void HandleGunRotation()
    {
        float facingDirection = GetComponentInParent<Transform>().localScale.x;
        //Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (worldPosition - (Vector2)gun.transform.position).normalized;
        gun.transform.right = direction;

        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Vector3 localScale = new Vector3(1f, 1f, 1f);
        if (angle > 90 || angle < -90)
        {
            localScale.y = -1f;
        }
        else
        {
            localScale.x = 1f;
        }

        gun.transform.localScale = new Vector3(facingDirection, localScale.y, localScale.z);
    }

    private void HandleGunShooting()
    {
        // if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("atk");
            if (currentAmmo > 0)
            {
                GameObject bulletInst = Instantiate(bullet, shootingPoint.position, gun.transform.rotation);
                bulletInst.GetComponent<BulletsScript>().direction = direction;

                currentAmmo--;
                Debug.Log("Đạn còn lại: " + currentAmmo);

                UIBulletCaculator(0);

                if (direction.y < -0.7f)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(-direction.normalized * recoilForce, ForceMode2D.Impulse);
                }

                if (currentAmmo <= 0)
                {
                    reloadCoroutine = StartCoroutine(Reload());
                }
            }
            else
            {
                Debug.Log("Hết đạn!");
            }
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        if (ReloadUI.instance != null)
            ReloadUI.instance.ShowReloadText();

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;

       if (ReloadUI.instance != null)
            ReloadUI.instance.HideReloadText();

        UIBulletCaculator(0);
    }

    public void UIBulletCaculator(int amount)
    {
        currentAmmo += amount;
        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);

        Debug.Log(currentAmmo + "/" + maxAmmo);

        if (UIBullet.instance != null)
        {
            float ratio = (float)currentAmmo / maxAmmo;
            UIBullet.instance.SetValue(ratio);
        }
    }


    public void CancelReload()
    {
        if (isReloading && reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            isReloading = false;
            Debug.Log("Đã huỷ nạp đạn do nhặt thêm đạn!");
        }
    }

}
