using System.Collections.Generic;
using Domain;

namespace Residence
{
    public class Owner : IResidence
    {
        public string Name { get; private set; }
        private List<Animal> pets = new List<Animal>();

        public Owner(string name)
        {
            Name = $"Власник: {name}";
        }

        public void AddAnimal(Animal animal)
        {
            if (!pets.Contains(animal))
            {
                pets.Add(animal);
                animal.Residence = this;
            }
        }

        public void RemoveAnimal(Animal animal)
        {
            if (pets.Contains(animal))
            {
                pets.Remove(animal);
            }
        }
    }
}