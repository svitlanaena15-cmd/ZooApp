using Domain;

namespace Residence
{
    public interface IResidence
    {
        string Name { get; }
        void AddAnimal(Animal animal);
        void RemoveAnimal(Animal animal);
    }
}