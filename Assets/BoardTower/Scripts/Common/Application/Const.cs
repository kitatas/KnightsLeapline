namespace BoardTower.Common.Application
{
    public sealed class ExceptionConfig
    {
        public const float FADE_DURATION = 0.5f;
        public const int MAX_RETRY_COUNT = 3;

        public const string REBOOT_MESSAGE = "Return to title.";
        public const string RETRY_MESSAGE = "Retry.";
        public const string QUIT_MESSAGE = "Exit this app.";

        public const string UNKNOWN_ERROR = "UNKNOWN_ERROR";
        public const string MAX_RETRY = "MAX_RETRY";
        public const string FAILED_TO_LOAD_SCENE = "FAILED_TO_LOAD_SCENE";
        public const string FAILED_TO_LOGIN = "FAILED_TO_LOGIN";
        public const string FAILED_TO_FETCH_MASTER = "FAILED_TO_FETCH_MASTER";
        public const string FAILED_TO_DESERIALIZE_MASTER = "FAILED_TO_DESERIALIZE_MASTER";
        public const string FAILED_TO_FETCH_PAYLOAD = "FAILED_TO_FETCH_PAYLOAD";
        public const string FAILED_TO_FETCH_RECORD = "FAILED_TO_FETCH_RECORD";
        public const string FAILED_TO_CREATE_UID = "FAILED_TO_CREATE_UID";
        public const string NOT_FOUND_BGM = "NOT_FOUND_BGM";
        public const string NOT_FOUND_LOAD = "NOT_FOUND_LOAD";
        public const string NOT_FOUND_LOCALE = "NOT_FOUND_LOCALE";
        public const string NOT_FOUND_SCENE = "NOT_FOUND_SCENE";
        public const string NOT_FOUND_SE = "NOT_FOUND_SE";
        public const string NOT_FOUND_SOUND_TYPE = "NOT_FOUND_SOUND_TYPE";
        public const string NOT_FOUND_STATE = "NOT_FOUND_STATE";
        public const string NOT_FOUND_APP_VERSION = "NOT_FOUND_APP_VERSION";
        public const string NOT_FOUND_WEBVIEW = "NOT_FOUND_WEBVIEW";
        public const string INVALID_ACHIEVEMENT = "INVALID_ACHIEVEMENT";
        public const string INVALID_ACHIEVEMENT_RANK = "INVALID_ACHIEVEMENT_RANK";
        public const string INVALID_BOOT_MODAL = "INVALID_BOOT_MODAL";
        public const string INVALID_CHESSMEN = "INVALID_CHESSMEN";
        public const string INVALID_CHESSMEN_MOVEMENT = "INVALID_CHESSMEN_MOVEMENT";
        public const string INVALID_COLLECTION = "INVALID_COLLECTION";
        public const string INVALID_DISPLAY_NAME = "INVALID_DISPLAY_NAME";
        public const string INVALID_DISPLAY_NAME_LENGTH = "INVALID_DISPLAY_NAME_LENGTH";
        public const string INVALID_DURATION = "INVALID_DURATION";
        public const string INVALID_FADE = "INVALID_FADE";
        public const string INVALID_FINISH = "INVALID_FINISH";
        public const string INVALID_GAME_MODAL = "INVALID_GAME_MODAL";
        public const string INVALID_GAME_MODE = "INVALID_GAME_MODE";
        public const string INVALID_HIGHLIGHT = "INVALID_HIGHLIGHT";
        public const string INVALID_MODAL = "INVALID_MODAL";
        public const string INVALID_PATTERN_LENGTH = "INVALID_PATTERN_LENGTH";
        public const string INVALID_RENDER = "INVALID_RENDER";
        public const string INVALID_RELIC = "INVALID_RELIC";
        public const string INVALID_ROTATE = "INVALID_ROTATE";
        public const string INVALID_ROUND = "INVALID_ROUND";
        public const string INVALID_SCORE_RATE = "INVALID_SCORE_RATE";
        public const string INVALID_SPLASH = "INVALID_SPLASH";
        public const string INVALID_SQUARE_EVENT = "INVALID_SQUARE_EVENT";
        public const string INVALID_SQUARE_INDEX = "INVALID_SQUARE_INDEX";
        public const string INVALID_URL = "INVALID_URL";
    }

    public sealed class LoadingConfig
    {
        public const float FADE_DURATION = 0.05f;
        public const float TEXT_ANIMATION_INTERVAL = 0.05f;
        public const float TEXT_ANIMATION_DURATION = 0.1f;
        public const float ICON_ANIMATION_DURATION = 2.0f;
    }

    public sealed class SceneConfig
    {
        public const float FADE_DURATION = 0.25f;
        public const float LOAD_PROGRESS_THRESHOLD = 0.9f;
        public const float STABILITY_FRAME = 3;
        public const float STABILITY_TIME = 0.5f;
    }

    public sealed class UiConfig
    {
        public const float DURATION = 0.25f;
    }

    public sealed class UrlConfig
    {
        // TODO: fix url
        public const string URL_BASE = "https://kitatas.github.io/games/numeri_rogue/";
        public const string URL_POLICY = URL_BASE + "policy";
        public const string URL_LICENSE = URL_BASE + "license";
        public const string URL_CREDIT = URL_BASE + "credit";

        // TODO: fix url
        public const string APP_URL = "https://play.google.com/store/apps/details?id=com.KitaLab.NumeriRogue";
        public const string DEVELOPER_APP_URL = "https://play.google.com/store/apps/developer?id=KitaLab";
    }

    public sealed class SoundConfig
    {
        public const float INIT_VOLUME = 0.7f;
    }

    public sealed class SaveConfig
    {
        public const string ES3_KEY = "";
    }

    public sealed class PlayFabConfig
    {
#if UNITY_EDITOR
        public const string TITLE_ID = "";
#else
        public const string TITLE_ID = "";
#endif

        public const string SCORE_RANKING_KEY = "";
        public const string SCORE_KEY = "";
        public const string ACHIEVEMENT_KEY = "";
        public const string PROGRESS_KEY = "";
        public const string APP_VERSION_KEY = "";

        public const int CREATE_UID_RETRY_COUNT = 10;
        public const int RANKING_RETRY_COUNT = 100;
    }

    public sealed class LocaleConfig
    {
        public const string TABLE_NAME = "LocaleTable";
        public const string ACHIEVEMENT_CONTENT_KEY = "Achievement_{0}";
    }

    public sealed class GameModeConfig
    {
        public const float FADE_DURATION = 0.25f;
    }
}