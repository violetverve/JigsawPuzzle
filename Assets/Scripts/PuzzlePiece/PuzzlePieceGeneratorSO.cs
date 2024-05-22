using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using System.Linq;
using PuzzlePiece.Features;
using Utilities;

namespace PuzzlePiece
{
    [CreateAssetMenu(menuName = "PuzzlePiece/PuzzlePieceGenerator")]
    public class PuzzlePieceGeneratorSO : ScriptableObject
    {
        [SerializeField] private SplineContainer _splineContainer;
        [SerializeField] private int _pointsPerSpline = 40;
        [SerializeField] private Material _material;

        public Piece CreatePiece(PieceConfiguration pieceConfiguration, Vector2Int gridPosition, Vector2Int grid)
        {
            var points = GetPointsFromConfig(pieceConfiguration).Distinct();

            var gameObject = new GameObject($"PuzzlePiece {gridPosition.x}_{gridPosition.y}");

            var mesh = GenerateMesh(points, gridPosition, grid);

            gameObject.AddComponent<MeshFilter>().mesh = mesh;
            gameObject.AddComponent<MeshRenderer>().material = _material;

            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();

            gameObject.AddComponent<Draggable>();
            
            var piece = gameObject.AddComponent<Piece>();

            return piece;
        } 

        private IEnumerable<Vector2> GetPointsFromConfig(PieceConfiguration pieceConfiguration)
        {
            return new List<Vector2>[]
            {
                GetPointsFromFeature(pieceConfiguration.Left, FeaturePosition.Left),
                GetPointsFromFeature(pieceConfiguration.Top, FeaturePosition.Top),
                GetPointsFromFeature(pieceConfiguration.Right, FeaturePosition.Right),
                GetPointsFromFeature(pieceConfiguration.Bottom, FeaturePosition.Bottom)
            }.SelectMany(points => points);
        }


        private Vector2[] CalculateUVs(Vector3[] vertices, Vector2Int gridPosition, Vector2Int grid)
        {
            int totalColumns = grid.x;
            int totalRows = grid.y;

            float uvPieceWidth = 1f / totalColumns;
            float uvPieceHeight = 1f / totalRows;
            float uvStartX = gridPosition.x * uvPieceWidth;
            float uvStartY = gridPosition.y * uvPieceHeight;

            Vector2 vertexMin = new Vector2(-1, -1);
            Vector2 vertexMax = new Vector2(1, 1);
            Vector2 rangeSize = vertexMax - vertexMin;

            Vector2[] uv = new Vector2[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                float normalizedU = (vertices[i].x - vertexMin.x) / rangeSize.x;
                float normalizedV = (vertices[i].y - vertexMin.y) / rangeSize.y;

                uv[i] = new Vector2(
                    uvStartX + normalizedU * uvPieceWidth,
                    uvStartY + normalizedV * uvPieceHeight
                );
            }

            return uv;
        }
        private Mesh GenerateMesh(IEnumerable<Vector2> points, Vector2Int gridPosition, Vector2Int grid)
        {
            var vertices = points.Select(point => new Vector3(point.x, point.y, 0)).ToArray();
            var uv = CalculateUVs(vertices, gridPosition, grid);

            var mesh = new Mesh
            {
                vertices = vertices,
                triangles = new Triangulator(points.ToArray()).Triangulate(),
                uv = uv
            };

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }

        private List<Vector2> GetPointsFromFeature(FeatureType featureType, FeaturePosition featurePosition)
        {
            var points2D = InitializePointsByFeatureType(featureType);
            points2D = ApplyFeaturePositionTransformations(points2D, featurePosition);
            return points2D;
        }

        private List<Vector2> InitializePointsByFeatureType(FeatureType featureType)
        {
            if (featureType == FeatureType.Hole || featureType == FeatureType.Knob)
            {
                var points = GetSplinePoints2D(_splineContainer.Spline);
                return featureType == FeatureType.Hole
                    ? points.Select(p => new Vector2(-p.x, -p.y)).Reverse().ToList()
                    : points;
            }
            else
            {
                return new List<Vector2> { new Vector2(-1, 0), new Vector2(1, 0) };
            }
        }        

        private List<Vector2> ApplyFeaturePositionTransformations(List<Vector2> points, FeaturePosition featurePosition)
        {
            return points.Select(p => featurePosition switch
            {
                FeaturePosition.Top => new Vector2(p.x, p.y + 1),
                FeaturePosition.Bottom => new Vector2(-p.x, -p.y - 1),
                FeaturePosition.Left => new Vector2(-p.y - 1, p.x),
                FeaturePosition.Right => new Vector2(p.y + 1, -p.x),
                _ => p
            }).ToList();
        }

        private List<Vector2> GetSplinePoints2D(Spline spline)
        {
            var points = new List<Vector2>();

            for (int i = 0; i <= _pointsPerSpline; i++)
            {
                float t = i / (float)_pointsPerSpline;
                Vector3 position = spline.EvaluatePosition(t);
                points.Add(new Vector2(position.x, position.y));
            }

            return points;
        }

    }
}
