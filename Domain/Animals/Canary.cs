using Residence;

namespace Domain
{
    public class Canary : Animal
    {
        public Canary(string name, IResidence residence) : base(name, residence)
        {
            Eyes = 2;
            Legs = 2;
            Wings = 2;
        }
    }
}