namespace VitoBox.Utils;

public class NullConsoleInput : VitoConsoleInput
{
    public NullConsoleInput() : base(null!) { }

    public override void Start()
    {
        // *перекати-поле катится по пустынным улицам*
    }
}
