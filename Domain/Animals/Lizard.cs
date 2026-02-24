using Residence;

namespace Domain
{
    public class Lizard : Animal
    {
        public Lizard(string name, IResidence residence) : base(name, residence)
        {
            Eyes = 2;
            Legs = 4;
            Wings = 0;
        }
    }
}