using System.Collections.Generic;
using Domain;

namespace Services
{
    public class FeedingService
    {
        public void Feed(Animal animal)
        {
            animal.Eat();
        }

        public void FeedAll(IEnumerable<Animal> animals)
        {
            foreach (var animal in animals)
            {
                if (animal.IsAlive)
                    animal.Eat();
            }
        }
    }
}