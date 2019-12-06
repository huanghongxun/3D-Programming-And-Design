using System;
using System.Collections.Generic;

public class GameStates
{
    private Dictionary<GameState, GameState> states = new Dictionary<GameState, GameState>();

    /// <summary>
    /// 从胜利状态往回搜索，可以遍历出所有游戏状态的胜利状态转移
    /// </summary>
    /// <param name="currentState">当前状态</param>
    /// <returns>当前状态，如果传入的当前状态已经被计算过，则返回实际的当前状态对象</returns>
    public GameState Search(GameState currentState)
    {
        if (states.ContainsKey(currentState)) return states[currentState];
        states[currentState] = currentState;

        foreach (GameAction action in GameAction.GameActions)
        {
            if (action.toWest && currentState.boatState == BoatState.East ||
                action.toEast && currentState.boatState == BoatState.West)
            {
                GameState originalState = currentState.Transform(action);
                if (originalState == null || !originalState.valid || originalState.lose) continue;
                GameState nextState = Search(originalState);
                nextState.nextWinAction = action.Opposite;
            }
        }

        return currentState;
    }
    
    public GameStates()
    {
        Search(new GameState(BoatState.West, 3, 3, 0, 0));
    }

    public GameState GetInitialState()
    {
        return states[new GameState(BoatState.East, 0, 0, 3, 3)];
    }

    public GameState GetState(GameState state)
    {
        return states.ContainsKey(state) ? states[state] : null;
    }

    /// <summary>
    /// 游戏动作，表示一次船的运输过程
    /// 牧师和魔鬼的增减是相对于东岸的，也就是说，东岸牧师和魔鬼数均为 0 时游戏成功
    /// </summary>
    public sealed class GameAction
    {
        public static readonly List<GameAction> GameActions = new List<GameAction>();

        static GameAction()
        {
            for (var priests = 0; priests <= 2; priests++)
            for (var devils = 0; devils <= 2; devils++)
            {
                if (priests + devils < 1 || priests + devils > 2) continue;
                GameActions.Add(new GameAction(priests, devils));
                GameActions.Add(new GameAction(-priests, -devils));
            }
        }

        public int priestDifference { get; }
        public int devilDifference { get; }

        public bool toEast => priestDifference >= 0 && devilDifference >= 0;
        public bool toWest => priestDifference <= 0 && devilDifference <= 0;
        public bool valid => Math.Abs(priestDifference + devilDifference) == 2 && priestDifference * devilDifference > 0;

        public GameAction(int priestDifference, int devilDifference)
        {
            this.priestDifference = priestDifference;
            this.devilDifference = devilDifference;
        }

        public GameAction Opposite => new GameAction(-priestDifference, -devilDifference);
    }

    public class GameState
    {
        public int westPriests { get; }
        public int westDevils { get; }
        public int eastPriests { get; }
        public int eastDevils { get; }
        public BoatState boatState { get; }

        internal GameAction nextWinAction;

        public GameState(BoatState state, int westPriests, int westDevils, int eastPriests, int eastDevils)
        {
            this.boatState = state;
            this.westPriests = westPriests;
            this.westDevils = westDevils;
            this.eastPriests = eastPriests;
            this.eastDevils = eastDevils;
        }

        public bool lose =>
            westPriests < westDevils && westPriests > 0 ||
            eastPriests < eastDevils && eastPriests > 0;

        public bool valid =>
            westDevils >= 0 && westPriests >= 0 && eastDevils >= 0 && eastPriests >= 0 &&
            westDevils + westPriests + eastDevils + eastPriests == 6;

        public bool win => westDevils + westPriests == 6;

        public GameState Transform(GameAction action)
        {
            var nextState = new GameState(
                boatState == BoatState.East ? BoatState.West : BoatState.East,
                westPriests - action.priestDifference,
                westDevils - action.devilDifference,
                eastPriests + action.priestDifference,
                eastDevils + action.devilDifference);
            if (nextState.westPriests < 0 || nextState.westDevils < 0 ||
                nextState.eastPriests < 0 || nextState.eastDevils < 0)
                return null;
            else
                return nextState;
        }

        public bool Equals(GameState other)
        {
            return boatState == other.boatState && westPriests == other.westPriests && westDevils == other.westDevils && eastPriests == other.eastPriests && eastDevils == other.eastDevils;
        }

        public override bool Equals(object obj)
        {
            return obj is GameState other && Equals(other);
        }

        public override int GetHashCode()
        {
            var hashCode = (int)boatState;
            hashCode = (hashCode * 397) ^ westPriests;
            hashCode = (hashCode * 397) ^ westDevils;
            hashCode = (hashCode * 397) ^ eastPriests;
            hashCode = (hashCode * 397) ^ eastDevils;
            return hashCode;
        }
    }
}

