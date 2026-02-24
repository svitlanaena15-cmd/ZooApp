using System.Collections.Generic;
using Domain;

namespace Residence
{
    public class PetShop : IResidence
    {
        public string Name { get; private set; }
        private List<Animal> animals = new List<Animal>();

        public PetShop(string name)
        {
            Name = $"Магазин: {name}";
        }

        public void AddAnimal(Animal animal)
        {
            if (!animals.Contains(animal))
            {
                animals.Add(animal);
                animal.Residence = this;
            }
        }

        public void RemoveAnimal(Animal animal)
        {
            if (animals.Contains(animal))
            {
                animals.Remove(animal);
            }
        }
    }
}