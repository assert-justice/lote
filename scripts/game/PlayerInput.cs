using Godot;
public enum InputMethod{
    Kb,
    Gamepad,
}
public class PlayerInput{
    float move;
    bool isJumping;
    bool isDropping;
    Vector2 aim;
    bool isFiring;
    bool isReloading;
    bool isPausing;
    InputMethod inputMethod = InputMethod.Gamepad;
    public void Poll(){
        // Handle controller input.
        move = Input.GetAxis("pad_move_left", "pad_move_right");
        aim = Input.GetVector("pad_aim_left", "pad_aim_right", "pad_aim_up", "pad_aim_down");
        if(aim.Length() > 0) aim = aim.Normalized();
        isFiring = Input.IsActionPressed("pad_fire");
        isJumping = Input.IsActionJustPressed("pad_jump");
        isDropping = Input.IsActionPressed("pad_drop");
        isReloading = Input.IsActionJustPressed("pad_reload");
        isPausing = Input.IsActionJustPressed("pad_pause");
        if(move != 0 || aim.Length() > 0 || isJumping || isDropping || isFiring || isReloading){
            inputMethod = InputMethod.Gamepad;
            return;
        }
        // Handle mouse/keyboard input.
        move = Input.GetAxis("kb_move_left", "kb_move_right");
        // aim = Input.GetVector("kb_aim_left", "kb_aim_right", "kb_aim_up", "kb_aim_down");
        // if(aim.Length() > 0) aim = aim.Normalized();
        isFiring = Input.IsActionPressed("kb_fire");
        isJumping = Input.IsActionJustPressed("kb_jump");
        isDropping = Input.IsActionPressed("kb_drop");
        isReloading = Input.IsActionJustPressed("kb_reload");
        isPausing = Input.IsActionJustPressed("kb_pause");
        if(move != 0 || isJumping || isDropping || isFiring || isReloading){
            inputMethod = InputMethod.Kb;
        }
    }
    public float GetMove(){return move;}
    public bool IsJumping(){return isJumping;}
    public bool IsDropping(){return isDropping;}
    public bool IsFiring(){return isFiring;}
    public bool IsReloading(){return isReloading;}
    public bool IsPausing(){return isPausing;}
    public Vector2 GetAim(){return aim;}
    public void SetAim(Vector2 v){aim = v;}
    public InputMethod GetInputMethod(){return inputMethod;}
}