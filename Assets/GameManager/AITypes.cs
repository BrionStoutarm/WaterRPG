
public static class AITypes
{
    public enum Mode
    {
        WAIT,
        RANDOM,
        MAX_SPEED,
        GO_TO,
        FOLLOW,
        MODE_END
    }
    public enum Maneuver
    {
        NONE,
        LEFT,
        RIGHT,
        TACK_HALF_LEFT,
        TACK_LEFT,
        TACK_HALF_RIGHT,
        TACK_RIGHT,
        MANUEVER_END
    }

    public enum LandManeuver
    {
        NONE,
        LEFT,
        RIGHT,
        MANUEVER_END
    }
}
