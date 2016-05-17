using Chroma.GameClasses.EnemySystem.Bosses;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chroma.GameClasses.EnemySystem
{

    /// <summary>
    /// Enumeration used to determine the state the boss is currently in.
    /// </summary>
    public enum BossState
    {
        Base = 0,       // Basic attack pattern of the Boss. Will be the more common state.
        Special = 1,    // Special attack pattern that can be used to launch specific attacks.
        Critical = 2    // When the boss is almost defeated it will enter a very difficult state.
    }

    /// <summary>
    /// Support class for the Boss class. Will contain a state machine that will act as the Boss' AI
    /// and will control it's movement and attack patterns.
    /// </summary>
    public abstract class AI
    {

        //Stores the current state of the Boss
        public BossState currentState;

        /// <summary>
        /// Default constructor for the AI abstract class. Will set currentState to Base state.
        /// </summary>
        public AI()
        {
            currentState = BossState.Base;
        }

        /// <summary>
        /// State machine for the Boss's AI. Will be run in the update loop and will transition when flags are set in each
        /// of the functions.
        /// </summary>
        public BossData state(BossData data, GameTimer timer)
        {
            switch (currentState)
            {
                case BossState.Base:
                    {
                        data = baseState(data, timer);
                        break;
                    }
                case BossState.Special:
                    {
                        data = specialState(data, timer);
                        break;
                    }
                case BossState.Critical:
                    {
                        data = criticalState(data, timer);
                        break;
                    }
            }

            return data;
        }

        /// <summary>
        /// Changes the state to that which is passed into this function.
        /// </summary>
        void changeState(BossState state)
        {
            currentState = state;
        }

        //Abstract functions that will be defined in each specific AI to specially define each of the Boss' unique patterns.
        abstract public BossData baseState(BossData data, GameTimer timer);
        abstract public BossData specialState(BossData data, GameTimer timer);
        abstract public BossData criticalState(BossData data, GameTimer timer);
    }
}
