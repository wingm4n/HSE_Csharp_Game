using Godot;

public static class GameState
{
	public enum Mode { Solo, Duo }
	public static Mode CurrentMode = Mode.Solo;
}
