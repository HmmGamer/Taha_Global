///
/// this class is used to avoid hard coding
///

using UnityEngine;

public static class A
{
    public static class Tags
    {
        public static string player = "Player";
        public static string enemy = "enemy";
        public static string arrow = "Arrow";
    }
    public static class LayerMasks
    {
        public static LayerMask player = LayerMask.GetMask("player");
        public static LayerMask floor = LayerMask.GetMask("floor");
    }
    public static class Layers
    {
        public static int player = 8;
        public static int default1 = 0;
        public static int item = 7;
        public static int floor = 6;
    }
    public static class Anim
    {
        public static string playerJump = "jump";
        public static string playerSpeed = "Speed";
        public static string playerIsOnWall = "isOnWall";
        public static string playerIsGrounded = "isOnGround";
        public static string playerIsJumping = "isJumping";
        public static string enemyAttack = "attack";
    }
    public static class DataKey
    {
        private static string keyIdData = "ji385";
        public static string GetKeyIdData(int iId)
        {
            return keyIdData + iId;
        }
    }
}
