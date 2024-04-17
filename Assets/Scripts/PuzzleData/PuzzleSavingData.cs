
using PuzzleData;
using Utils;

namespace PuzzleData
{
    public class PuzzleSavingData
    {
        private int _id;
        private string[] _uncompletedPieces; //ever puzzle piece will have theit own index(id)

        PuzzleSavingData(PuzzleSO puzzleData, string[] uncompletedPieces)
        {
            _id = puzzleData.Id;
            _uncompletedPieces = uncompletedPieces;
        }
    }

}
