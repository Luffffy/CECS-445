﻿namespace Components {

    public class Unit : BoardPiece {
        public Character Character { get; }
        public Tile Tile { get; set; }

        public int MoveSpeed { get; set; }

        public Unit(Character character, Tile tile) {
            this.Character = character;
            this.MoveSpeed = character.moveSpeed;
            tile.BoardPiece = this;
            this.Tile = tile;
        }

        public void MoveTo(Tile targetLocation) {
            Tile.BoardPiece = null;
            targetLocation.BoardPiece = this;
            Grid<Tile> grid = Tile.grid;
            grid.TriggerGridObjectChanged(Tile.xCoord, Tile.yCoord);
            Tile = targetLocation;
            grid.TriggerGridObjectChanged(targetLocation.xCoord, targetLocation.yCoord);
        }

        // Method will be moved to combat system
        public void AttackUnit(Unit otherUnit) {
            otherUnit.TakeDamage(Character.attack);
        }

        // Method will be moved to combat system
        public void TakeDamage(int damageAmount) {
            Character.health -= damageAmount;
        }

        public override string ToString() {
            return Character.name.ToString();
        }
    }
}