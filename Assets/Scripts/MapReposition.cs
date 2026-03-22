using Unity.VisualScripting;
using UnityEngine;

public class MapReposition : MonoBehaviour
{
    private Player player; // gamemanager에서 받아오는 player 스크립트의 정보를 짧게 사용하기 위한 변수

    void Start()
    {
        player = GameManager.instance.player;
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;
        
        Vector3 playerPos = player.transform.position;
        Vector3 myPos     = transform.position;
        
        float   diffX     = Mathf.Abs(playerPos.x - myPos.x);
        float   diffY     = Mathf.Abs(playerPos.y - myPos.y);

        Vector3 playerDir = player.inputVec;
        float   dirX      = playerDir.x < 0 ? -1 : 1;
        float   dirY      = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }else if (diffY > dirY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                break;
        }
    }
}
