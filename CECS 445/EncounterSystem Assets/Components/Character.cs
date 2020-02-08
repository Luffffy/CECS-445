namespace Components {

    public class Character {
        public int name { get; }
        public int maxHealth { get; set; }
        public int health { get; set; }
        public int attack { get; set; }
        public int defense { get; set; }
        public int moveSpeed { get; set; }

        public Character(int name, int maxHealth, int health, int attack, int defense, int movespeed) {
            this.name = name;
            this.maxHealth = maxHealth;
            this.health = health;
            this.attack = attack;
            this.defense = defense;
            this.moveSpeed = movespeed;
        }
    }
}