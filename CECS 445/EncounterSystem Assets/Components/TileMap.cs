using Components;
using UnityEngine;

public class Tilemap {
    private Grid<TilemapObject> grid;

    public Tilemap(int width, int height, float cellSize, Vector3 originPosition) {
        grid = new Grid<TilemapObject>(width, height, cellSize, originPosition, (Grid<TilemapObject> g, int x, int y) => new TilemapObject(g, x, y));
    }

    public void SetTilemapSprite(Vector3 worldPosition, TilemapObject.TilemapSprite tilemapSprite) {
        TilemapObject tilemapObject = grid.GetGridObject(worldPosition);
        tilemapObject?.SetTilemapSprite(tilemapSprite);
    }

    public class TilemapObject {

        public enum TilemapSprite {
            None,
            Ground
        }

        public enum BoardPiece {
            Empty,
            Unit
        }

        private Grid<TilemapObject> grid;
        private int x;
        private int y;
        private TilemapSprite tilemapSprite;
        private BoardPiece bpiece;

        public TilemapObject(Grid<TilemapObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetTilemapSprite(TilemapSprite tilemapSprite) {
            this.tilemapSprite = tilemapSprite;
            grid.TriggerGridObjectChanged(x, y);
        }

        public override string ToString() {
            return tilemapSprite.ToString() + "\n" + bpiece.ToString();
        }
    }
}