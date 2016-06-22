
public class GameManagerEvents {

    public const string LOAD_GAMEPLAY_SCENE = "LOAD_GAMEPLAY_SCENE";
    public const string GAME_UPDATE = "GAME_UPDATE";

    public enum GAME_STATE
    {
        READY,
        PLAY,
        END,
        RESULT,
        FINISH
    }
}
