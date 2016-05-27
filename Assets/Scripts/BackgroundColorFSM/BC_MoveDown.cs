using UnityEngine;
using System.Collections;

public class BC_MoveDown : State {

    private BackgroundColorizer fsm;

    float decrement;

    public override void Initialize(MonoFSM callingfsm)
    {
        fsm = (BackgroundColorizer)callingfsm;
        decrement = 4f / 255f;
    }

    public override void OnUpdate()
    {
        switch (fsm.CurrentColor)
        {
            case BackgroundColorizer.Value.Red:
                fsm.MyImage.color = new Color(fsm.MyImage.color.r - decrement, fsm.MyImage.color.g, fsm.MyImage.color.b);
                break;
            case BackgroundColorizer.Value.Green:
                fsm.MyImage.color = new Color(fsm.MyImage.color.r, fsm.MyImage.color.g - decrement, fsm.MyImage.color.b);
                break;
            case BackgroundColorizer.Value.Blue:
                fsm.MyImage.color = new Color(fsm.MyImage.color.r, fsm.MyImage.color.g, fsm.MyImage.color.b - decrement);
                break;
        }
    }

    public override void CheckTransitions()
    {
        switch (fsm.CurrentColor)
        {
            case BackgroundColorizer.Value.Red:
                if (fsm.MyImage.color.r <= 0)
                {
                    fsm.MyImage.color = new Color(0, fsm.MyImage.color.g, fsm.MyImage.color.b);
                    fsm.CurrentColor = BackgroundColorizer.Value.Green;
                    fsm.AttemptTransition(BC_States.GoUp);
                }
                break;
            case BackgroundColorizer.Value.Green:
                if (fsm.MyImage.color.g <= 0)
                {
                    fsm.MyImage.color = new Color(fsm.MyImage.color.r, 0, fsm.MyImage.color.b);
                    fsm.CurrentColor = BackgroundColorizer.Value.Blue;
                    fsm.AttemptTransition(BC_States.GoUp);
                }
                break;
            case BackgroundColorizer.Value.Blue:
                if (fsm.MyImage.color.b <= 0)
                {
                    fsm.MyImage.color = new Color(fsm.MyImage.color.r, fsm.MyImage.color.g, 0);
                    fsm.CurrentColor = BackgroundColorizer.Value.Red;
                    fsm.AttemptTransition(BC_States.GoUp);
                }
                break;
        }
    }
}
