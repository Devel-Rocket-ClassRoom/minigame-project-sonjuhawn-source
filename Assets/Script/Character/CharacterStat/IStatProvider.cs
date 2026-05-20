using System;

public interface IStatProvider
{
    int Strength { get; }
    int Agility { get; }
    int Vitality { get; }
    int Stamina { get; }

    event Action OnStatChanged;
}