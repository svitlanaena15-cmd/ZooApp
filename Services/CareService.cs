using System.Collections.Generic;
using Domain;

namespace Services
{
    public class CareService
    {
        public void Clean(Animal animal)
        {
            animal.Clean();
        }

        public void CleanAll(IEnumerable<Animal> animals)
        {
            foreach (var animal in animals)
            {
                if (animal.IsAlive)
                    animal.Clean();
            }
        }
    }
}