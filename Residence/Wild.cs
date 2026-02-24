using System.Collections.Generic;
using Domain;

namespace Residence
{
    public class Wild : IResidence
    {
        public string Name { get; private set; } = "Дика природа";
        private List<Animal> wildAnimals = new List<Animal>();

        public void AddAnimal(Animal animal)
        {
            if (!wildAnimals.Contains(animal))
            {
                wildAnimals.Add(animal);
                animal.Residence = this;
            }
        }

        public void RemoveAnimal(Animal animal)
        {
            if (wildAnimals.Contains(animal))
            {
                wildAnimals.Remove(animal);
            }
        }
    }
}