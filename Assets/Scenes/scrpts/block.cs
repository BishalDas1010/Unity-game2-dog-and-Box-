using UnityEngine;

public class block : MonoBehaviour
{
// Update is called once per frame
    void Update()
    {
        if(transform.position.y < -6f)
        {
            
            Destroy(gameObject);
        }
        
    }
}
