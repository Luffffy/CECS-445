using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface Tileable
    {
        void SetLocation(float xLocation, float yLocation, float zLocation);
        float GetXLocation();
        float GetYLocation();
        float GetZLocation();
        void Highlight();
        void RemoveHighLight();
        void Initialize(GameBoard gameBoard, float xLocation, float yLocation, float zLocation);
        bool IsOccupiable();
        void IsAwaitingSelection(bool awaitingStatus);
    }
}
