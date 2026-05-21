public class MonsterDeadState : IMonsterState
{
    public void Enter(MonsterController ctx) 
    {
        ctx.Anim.SetTrigger(MonsterController.DieHash);
        ctx.enabled = false;
        UnityEngine.Object.Destroy(ctx.gameObject, 3f);   // 3초 후 제거

    }
    public void Tick(MonsterController ctx) { }
    public void Exit(MonsterController ctx) { }
}