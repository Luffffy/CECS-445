using Components;
using System.Collections.Generic;
using UnityEngine;
using Utilitys;

/*
 * Old testing class before we decided on doing ECS
 */
public class Testing1 : MonoBehaviour {
    private Grid<Tile> grid;
    private int charName = 1;
    private List<Command> commands;

    private void Start() {
        //grid = new Tilemap(20, 10, 10f, Vector3.zero);
        grid = new Grid<Tile>(20, 10, 10f, Vector3.zero, (Grid<Tile> g, int x, int y) => new Tile(g, x, y));
    }

    private void Update() {
        Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
        /*if (Input.GetMouseButtonDown(0)) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            tilemap.SetTilemapSprite(mouseWorldPosition, Tilemap.TilemapObject.TilemapSprite.Ground);
        }*/

        if (Input.GetKeyDown(KeyCode.S)) {
            spawnUnit(mouseWorldPosition);
        }
        if (Input.GetMouseButtonDown(1)) {
            grid.selectedPiece = grid.GetGridObject(mouseWorldPosition).BoardPiece;
            Debug.Log(grid.selectedPiece);
        }
        if (grid.selectedPiece != null && Input.GetMouseButtonDown(0)) {
            Debug.Log(grid.selectedPiece);
            ((Unit)(grid.selectedPiece)).MoveTo(grid.GetGridObject(mouseWorldPosition));
            grid.selectedPiece = null;
            Debug.Log(grid.selectedPiece);
        }
    }

    private void spawnUnit(Vector3 position) {
        Tile tile = grid.GetGridObject(position);
        if (tile.BoardPiece != null) {
            return;
        }
        Character c = new Character(charName, 100, 100, 5, 5, 3);
        Unit unit = new Unit(c, tile);
        if (tile != null && tile.BoardPiece == null) {
            tile.BoardPiece = unit;
            grid.TriggerGridObjectChanged(tile.xCoord, tile.yCoord);
            charName++;
        }
    }
}