public interface IMonsterState
{
    void Enter(MonsterController ctx);
    void Tick(MonsterController ctx);
    void Exit(MonsterController ctx);
}