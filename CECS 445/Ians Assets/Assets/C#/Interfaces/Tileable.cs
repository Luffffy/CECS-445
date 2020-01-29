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
        void Highlight();
        void RemoveHighLight();
        void Initialize(Map gameBoard, float xLocation, float yLocation, float zLocation);
        bool IsOccupiable();
        void IsAwaitingSelection(bool awaitingStatus);
        float GetXLocation();
        float GetYLocation();
        float GetZLocation();
        float GetFutureXLocation();
        float GetFutureYLocation();
        float GetFutureZLocation();
    }
}
