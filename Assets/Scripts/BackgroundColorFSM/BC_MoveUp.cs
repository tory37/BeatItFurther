using UnityEngine;
using System.Collections;

public class BC_MoveUp : State {

    private BackgroundColorizer fsm;

    float increment;

    public override void Initialize(MonoFSM callingfsm)
    {
        fsm = (BackgroundColorizer)callingfsm;

        increment = 4f / 255f;
    }

    public override void OnUpdate()
    {
        switch (fsm.CurrentColor)
        {
            case BackgroundColorizer.Value.Red:
                fsm.MyImage.color = new Color(fsm.MyImage.color.r + increment, fsm.MyImage.color.g, fsm.MyImage.color.b);
                break;
            case BackgroundColorizer.Value.Green:
                fsm.MyImage.color = new Color(fsm.MyImage.color.r, fsm.MyImage.color.g + increment, fsm.MyImage.color.b);
                break;
            case BackgroundColorizer.Value.Blue:
                fsm.MyImage.color = new Color(fsm.MyImage.color.r, fsm.MyImage.color.g, fsm.MyImage.color.b + increment);
                break;
        }
    }

    public override void CheckTransitions()
    {
        switch (fsm.CurrentColor)
        {
            case BackgroundColorizer.Value.Red:
                if (fsm.MyImage.color.r >= 1)
                {
                    fsm.MyImage.color = new Color(1, fsm.MyImage.color.g, fsm.MyImage.color.b);
                    fsm.CurrentColor = BackgroundColorizer.Value.Green;
                    fsm.AttemptTransition(BC_States.GoDown);
                }
                break;
            case BackgroundColorizer.Value.Green:
                if (fsm.MyImage.color.g >= 1)
                {
                    fsm.MyImage.color = new Color(fsm.MyImage.color.r, 1, fsm.MyImage.color.b);
                    fsm.CurrentColor = BackgroundColorizer.Value.Blue;
                    fsm.AttemptTransition(BC_States.GoDown);
                }
                break;
            case BackgroundColorizer.Value.Blue:
                if (fsm.MyImage.color.b >= 1)
                {
                    fsm.MyImage.color = new Color(fsm.MyImage.color.r, fsm.MyImage.color.g, 1);
                    fsm.CurrentColor = BackgroundColorizer.Value.Red;
                    fsm.AttemptTransition(BC_States.GoDown);
                }
                break;
        }
    }
	
}
