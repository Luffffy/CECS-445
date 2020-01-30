
namespace Interfaces
{
    public interface Tileable
    {
        void SetLocation(float xLocation, float yLocation, float zLocation);
        void Highlight();
        void RemoveHighLight();
        void Initialize(GameBoard gameBoard, float xLocation, float yLocation, float zLocation);
        bool IsOccupiable();
        float GetXLocation();
        float GetYLocation();
        float GetZLocation();
        float GetFutureXLocation();
        float GetFutureYLocation();
        float GetFutureZLocation();
    }
}
