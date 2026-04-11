namespace Modules
{
    public class Battery
    {
        public float Charge { get; private set; }

        public void AddCharge(float charge)
        {
            Charge += charge;
        }
    }
}