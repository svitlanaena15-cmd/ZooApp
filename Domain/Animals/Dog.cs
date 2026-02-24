using Residence;

namespace Domain
{
    public class Dog : Animal
    {
        public Dog(string name, IResidence residence) : base(name, residence)
        {
            Eyes = 2;
            Legs = 4;
            Wings = 0;
        }
    }
}