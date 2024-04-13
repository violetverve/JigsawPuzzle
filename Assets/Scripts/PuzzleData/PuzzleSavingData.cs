
using PuzzleData;
using Utils;

namespace PuzzleData
{
    public class PuzzleSavingData
    {
        private int _id;
        private string[] _completedPieces; //ever puzzle piece will have theit own index(id)

        PuzzleSavingData(PuzzleSO puzzleData, string[] completedPieces)
        {
            _id = puzzleData.Id;
            _completedPieces = completedPieces;
        }
    }

}
