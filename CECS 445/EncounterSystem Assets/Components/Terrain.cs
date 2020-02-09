namespace Components {

    public class Terrain {
        public enum Property {
            None = 1,
            Puddle = 2,
            Wall,
            ClosedDoor,
            OpenDoor
        }

        public Property Type { get; set; }

        public int Cost { 
            get { 
                if (Type == Property.Puddle) { 
                    return 2; 
                } return 1; 
            } set {; } }

        public bool IsWalkable {
            get {
                return Type != Property.Wall && Type != Property.ClosedDoor;
            }
            set {
                IsWalkable = value;
            }
        }

        public Terrain(Property type) {
            this.Type = type;
        }
        public override string ToString() {
            return Type.ToString();
        }
    }

}