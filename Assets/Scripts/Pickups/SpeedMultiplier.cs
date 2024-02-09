public class SpeedMultiplier : Pickup
{
    public float speedMultiplier = 2f;

    public override void Picked()
    {
        base.Picked();
        PlayerController.instance.SetSpeedMultiplier(speedMultiplier);

        Invoke(nameof(CancelMultiplier), 5f);
    }

    public void CancelMultiplier()
    {
        PlayerController.instance.SetSpeedMultiplier(1f);
    }

    private void Update()
    {
        Rotation();
    }
}
