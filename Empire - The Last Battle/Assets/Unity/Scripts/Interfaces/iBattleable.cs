﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface iBattleable 
{
    bool IsKO();

    int GetCurrentHP();

    int GetStrength();

    int GetSpeed();

    float GetHPPercentage();

    void ReduceHP(int HP);

    void Heal();

    int GetUniqueID();

	SoundEffect GetHitSound();

	SoundEffect GetAttackSound();

	SoundEffect GetDeathSound();
}