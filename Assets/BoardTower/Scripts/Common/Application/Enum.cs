namespace BoardTower.Common.Application
{
    public enum SceneName
    {
        None,
        Boot,
        Game,
    }

    public enum LoadType
    {
        None,
        Direct,
        Fade,
    }

    public enum Fade
    {
        None,
        In,
        Out,
        InOut,
    }

    public enum SoundType
    {
        None = 0,
        Unity = 1,
        CriWare = 2,
    }

    public enum BgmType
    {
        None = 0,
        Top = 1,
        Game = 2,
    }

    public enum SeType
    {
        None = 0,
        Decision = 1,
        Cancel = 2,
        Step = 3,
        PopLog = 4,
        GetItem = 5,
        Needle = 6,
    }

    public enum GameMode
    {
        None = 0,
        Online = 1,
        Offline = 2,
    }

    public enum UrlType
    {
        None = 0,
        Apps = 1,
        DeveloperApps = 2,
    }
}