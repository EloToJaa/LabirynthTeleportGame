public class SpeedMultiplier : Pickup
{
    public float speedMultiplier = 2f;

    public override void Picked()
    {
        base.Picked();
        PlayerController.instance.SetSpeedMultiplier(speedMultiplier);
    }

    private void Update()
    {
        Rotation();
    }
}
