
public class LabButton : GazeButton {

    public int LabNum;

    public override void Click()
    {
        Gaze.labNum = LabNum;
    }
}
