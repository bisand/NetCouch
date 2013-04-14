namespace Biseth.Net.Settee.Linq.TestModels
{
    public class Place
    {
        // Properties. 
        internal Place(string name,
                       string state,
                       PlaceType placeType)
        {
            Name = name;
            State = state;
            PlaceType = (PlaceType) placeType;
        }

        public string Name { get; private set; }
        public string State { get; private set; }
        public PlaceType PlaceType { get; private set; }

        // Constructor. 
    }
}