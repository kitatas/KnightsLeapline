namespace BoardTower.Game.Application
{
    public enum GameState
    {
        None = 0,
        Init = 1,
        SetUp = 2,
        Input = 3,
        Event = 4,
        Judge = 5,
        Clear = 6,
        Fail = 7,
        Finish = 8,
        Pick = 9,
        Send = 10,
    }

    public enum ChessmenType
    {
        None = 0,
        King = 1,
        Queen = 2,
        Rook = 3,
        Bishop = 4,
        Knight = 5,
    }

    public enum ChessmenMovementType
    {
        None = 0,
        Leaper = 1,
        Slider = 2,
    }

    public enum HighlightSquareType
    {
        None = 0,
        Movable = 1,
        Default = 2,
    }

    public enum SquareEventType
    {
        None = 0,
        Empty = 1,
        BeltRight = 2,
        BeltDown = 3,
        BeltLeft = 4,
        BeltUp = 5,
        Block = 6,
        Gem = 7,
        Ply = 8,
        Collapse = 9,
    }

    public enum RenderType
    {
        None = 0,
        Refresh = 1,
        RetainUp = 2,
        RetainDown = 3,
    }

    public enum RotateType
    {
        None = 0,
        Angle0 = 1,
        Angle90 = 2,
        Angle180 = 3,
        Angle270 = 4,
    }

    public enum FinishType
    {
        None = 0,
        Clear = 1,
        Fail = 2,
    }

    public enum GameModalType
    {
        None = 0,
        Menu = 1,
        Policy = 2,
        License = 3,
        Credit = 4,
        Sound = 5,
        DeleteConfirm = 6,
        DeleteComplete = 7,
        GiveUpConfirm = 8,
        GiveUpComplete = 9,
        Account = 10,
        Achievement = 11,
        Ranking = 12,
        Relic = 13,
        Guide = 14,
        HowTo = 15,
    }

    public enum RelicType
    {
        None = 0,
        Boots = 1,
        Codex = 2,
        Anchor = 3,
        Charm = 4,
        Chalice = 5,
        Baton = 6,
        Pact = 7,
        Seal = 8,
        Scales = 9,
        Banner = 10,
        Medal = 11,
    }

    public enum ScoreRateType
    {
        None = 0,
        RoundGem = 1,
        RoundClear = 2,
        GemCombo = 3,
        GemUnitRelic = 4,
        RoundClearRelic = 5,
    }

    public enum AchievementType
    {
        None = 0,
        Play = 1,
        Score = 2,
        Clear = 3,
    }

    public enum AchievementRankType
    {
        None = 0,
        Normal = 1,
        Bronze = 2,
        Silver = 3,
        Gold = 4,
        Platinum = 5,
    }

    public enum EventLogType
    {
        None = 0,
        Ride = 1,
        Combo = 2,
        Keep = 3,
        Gem = 4,
        Score = 5,
        Heart = 6,
    }
}