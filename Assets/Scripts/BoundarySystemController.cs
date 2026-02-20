using UnityEngine;

public class BoundarySystemController : MonoBehaviour
{
    [SerializeField]
    private Transform LeftWall;
    [SerializeField]
    private Transform RightWall;
    [SerializeField]
    private Transform TopWall;
    [SerializeField]
    private Transform BottomWall;

    [SerializeField]
    private float distToWall;

    [SerializeField]
    public int wallNum = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(PlayerMASTER.Instance.transform.position.x > (RightWall.position.x - 5) && wallNum == 1)
        {
            PlayerMASTER.Instance.transform.position = new Vector3(LeftWall.position.x + distToWall,PlayerMASTER.Instance.transform.position.y,PlayerMASTER.Instance.transform.position.z);
        } 
        else if (PlayerMASTER.Instance.transform.position.x < (LeftWall.position.x + 5) && wallNum == 2)
        {
            PlayerMASTER.Instance.transform.position = new Vector3(RightWall.position.x - distToWall,PlayerMASTER.Instance.transform.position.y,PlayerMASTER.Instance.transform.position.z);   
        }
        else if (PlayerMASTER.Instance.transform.position.y > (TopWall.position.y - 5) && wallNum == 3)
        {
            PlayerMASTER.Instance.transform.position = new Vector3(PlayerMASTER.Instance.transform.position.x,BottomWall.position.y + distToWall,PlayerMASTER.Instance.transform.position.z);   
        }
        else if (PlayerMASTER.Instance.transform.position.y < (BottomWall.position.y + 5) && wallNum == 4)
        {
            PlayerMASTER.Instance.transform.position = new Vector3(PlayerMASTER.Instance.transform.position.x,TopWall.position.y - distToWall,PlayerMASTER.Instance.transform.position.z);   
        }
    }
}
