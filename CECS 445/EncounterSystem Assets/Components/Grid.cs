﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Utilitys;

namespace Components {

    /*
     * This class was made by following the code monkey tutorial and it doesn't exactly follow the ecs architecture since it
     * contains some board component fields and board and movement system methods.
     */
    public class Grid<TGridObject> {

        public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

        public class OnGridObjectChangedEventArgs : EventArgs {
            public int x;
            public int y;
        }

        private BoardPiece _selectedPiece;
        public int width { get; }
        public int height { get; }
        public float cellSize { get; }
        private Vector3 originPosition;
        private TGridObject[,] gridArray;

        public BoardPiece selectedPiece {
            get {
                return _selectedPiece;
            }
            set {
                _selectedPiece = value;
                if (_selectedPiece != null) {
                    selectedPieceRange = FindValidMoves(selectedPiece);
                    ToggleTileZone(selectedPieceRange, true);
                } else {
                    ToggleTileZone(selectedPieceRange, false);
                    selectedPieceRange = default(List<Tile>);
                }
                ;
            }
        }

        public List<Tile> selectedPieceRange { get; set; }

        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject) {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridArray = new TGridObject[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++) {
                for (int y = 0; y < gridArray.GetLength(1); y++) {
                    gridArray[x, y] = createGridObject(this, x, y);
                }
            }

            bool showDebug = true;
            if (showDebug) {
                TextMesh[,] debugTextArray = new TextMesh[width, height];

                for (int x = 0; x < gridArray.GetLength(0); x++) {
                    for (int y = 0; y < gridArray.GetLength(1); y++) {
                        debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 20, Color.black, TextAnchor.MiddleCenter);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.black, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.black, 100f);
                    }
                }
                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.black, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.black, 100f);

                OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                    debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
                };
            }
        }

        // Used to find tiles that a board piece can reach with their movespeed. So far uses manhatten distance but must change in order to account for obstacles such as walls.
        public List<Tile> FindValidMoves(BoardPiece boardPiece) {
            List<Tile> range = new List<Tile>();
            Queue<Tile> queue = new Queue<Tile>();
            List<Tile> visitedList = new List<Tile>();
            Tile currentTile;
            Tile boardPieceTile = boardPiece.Tile;
            queue.Enqueue(boardPiece.Tile);
            while (queue.Count > 0) {
                currentTile = queue.Dequeue();
                Debug.Log(queue.Count);
                foreach (Tile neighbor in currentTile.Neighbors) {
                    if (!visitedList.Contains(neighbor)) {
                        visitedList.Add(neighbor);
                        Debug.Log(selectedPiece.MoveSpeed);
                        if (neighbor.Terrain != Terrain.Wall && UtilsClass.CalculateManhattanDistance(neighbor.xCoord, neighbor.yCoord, selectedPiece.Tile.xCoord, selectedPiece.Tile.yCoord) <= boardPiece.MoveSpeed) {
                            range.Add(neighbor);
                            neighbor.Highlight = true;
                            Debug.Log("Found: neighbor at " + (neighbor.xCoord - selectedPiece.Tile.xCoord) + " " + (neighbor.yCoord - selectedPiece.Tile.yCoord));
                            TriggerGridObjectChanged(neighbor.xCoord, neighbor.yCoord);
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }
            return range;
        }


        // Used to highlight a list of tiles (Tile Zone). As of now used to highlight/unhighlight the tiles in range of the selected board piece
        private void ToggleTileZone(List<Tile> tileZone, bool toggle) {
            foreach (Tile tile in tileZone) {
                tile.Highlight = toggle;
                TriggerGridObjectChanged(tile.xCoord, tile.yCoord);
            }
        }

        public Vector3 GetWorldPosition(int x, int y) {
            return new Vector3(x, y) * cellSize + originPosition;
        }

        private void GetXY(Vector3 worldPosition, out int x, out int y) {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        }

        public void SetGridObject(int x, int y, TGridObject value) {
            if (x >= 0 && y >= 0 && x < width && y < height) {
                gridArray[x, y] = value;
                OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
            }
        }

        public void TriggerGridObjectChanged(int x, int y) {
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }

        public void SetGridObject(Vector3 worldPosition, TGridObject value) {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetGridObject(x, y, value);
        }

        public TGridObject GetGridObject(int x, int y) {
            if (x >= 0 && y >= 0 && x < width && y < height) {
                return gridArray[x, y];
            } else {
                return default(TGridObject);
            }
        }

        public TGridObject GetGridObject(Vector3 worldPosition) {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetGridObject(x, y);
        }
    }
}