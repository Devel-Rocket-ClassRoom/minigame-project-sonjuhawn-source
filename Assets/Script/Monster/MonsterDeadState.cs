public class MonsterDeadState : IMonsterState
{
    public void Enter(MonsterController ctx) 
    {
        ctx.Anim.SetTrigger(MonsterController.DieHash);
        ctx.enabled = false;
    }
    public void Tick(MonsterController ctx) { }
    public void Exit(MonsterController ctx) { }
}