using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal otherPortal;
    public Material material;

    public Transform portalCollider;
    private Transform renderSurface;
    private Camera myCamera;

    private GameObject player;
    private PortalTeleport portalTeleport;
    private PortalCamera portalCamera;

    private void Awake()
    {
        myCamera = gameObject.transform.Find("PortalCamera").GetComponent<Camera>();
        renderSurface = gameObject.transform.Find("RenderSurface");
        portalCollider = gameObject.transform.Find("Collider");

        player = GameObject.FindGameObjectWithTag("Player");

        portalTeleport = portalCollider.GetComponent<PortalTeleport>();
        portalTeleport.player = player.transform;
        portalTeleport.receiver = otherPortal.portalCollider;

        portalCamera = myCamera.GetComponent<PortalCamera>();
        portalCamera.playerCamera = player.GetComponentInChildren<Camera>().transform;
        
    }
}
