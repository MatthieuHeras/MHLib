using System;
using UnityEngine;

namespace MHLib.Hexagon
{
    /// <summary>
    /// This is meant for pointed hexagons in offset coordinates.
    /// If this doesn't mean anything to you, please visit this excellent blog post: https://www.redblobgames.com/grids/hexagons/.
    /// Most of this Helper comes from it.
    /// In all this code, the tile size is equal to the distance between a point and its opposite one (ex: from bottom point to top point).
    /// </summary>
    public static class HexagonHelper
    {
        public const int DIRECTIONS_COUNT = 6;
        public const int AXES_COUNT = 3; // This represents the possible axes (horizontal, top right / bottom left, top left / bottom right).
        
        private const float ONE_THIRD = 1f / 3f;
        private const float TWO_THIRD = 2f / 3f;
        
        private static readonly float SQRT3 = Mathf.Sqrt(3f);
        private static readonly float SQRT3_DIVIDED_BY_3 = SQRT3 / 3f;
        private static readonly float SQRT3_DIVIDED_BY_2 = SQRT3 / 2f;
        
        // In offset coordinates, neighbours offset vary depending on the y axis.
        // That's why we need two sets of offsets, one for even Y, and one for odd Y.
        public static readonly Vector2Int[] EVEN_OFFSETS =
        {
            new(1, 0),      // Right
            new(0, 1),      // Top Right
            new(-1, 1),     // Top Left
            new(-1, 0),     // Left
            new(-1, -1),    // Bottom Left
            new(0, -1),     // Bottom Right
        };

        public static readonly Vector2Int[] ODD_OFFSETS =
        {
            new(1, 0),      // Right
            new(1, 1),      // Top Right
            new(0, 1),      // Top Left
            new(-1, 0),     // Left
            new(0, -1),     // Bottom Left
            new(1, -1),     // Bottom Right
        };

        public static float HorizontalSpacing(float tileSize) => tileSize * SQRT3_DIVIDED_BY_2;
        public static float HorizontalSpacingOffset(float tileSize) => HorizontalSpacing(tileSize) / 2f;
        public static float VerticalSpacing(float tileSize) => tileSize * 0.75f;
        
        #region HEXAGON_COORDINATES
        /// <summary> Convert an axial coordinates position to a cube coordinates position : https://www.redblobgames.com/grids/hexagons/#conversions </summary>
        private static Vector3 AxialToCube(Vector2 axialPos)
        {
            return new Vector3(axialPos.x, axialPos.y, -axialPos.x - axialPos.y);
        }
        
        /// <summary> Convert a cube coordinates position to an offset coordinates position : https://www.redblobgames.com/grids/hexagons/#conversions </summary>
        private static Vector2Int CubeToOffset(Vector3Int cubePos)
        {
            int x = cubePos.x + (cubePos.y - (cubePos.y & 1)) / 2;
            return new Vector2Int(x, cubePos.y);
        }

        /// <summary> Convert an offset coordinates position to an axial coordinates position : https://www.redblobgames.com/grids/hexagons/#conversions </summary>
        private static Vector2Int OffsetToAxial(Vector2Int offsetPos)
        {
            return new Vector2Int(offsetPos.x - (offsetPos.y - (offsetPos.y & 1)) / 2, offsetPos.y);
        }

        /// <summary> Rounds a float position (cube coordinates) to an hexagon position (cube coordinates) : https://www.redblobgames.com/grids/hexagons/#conversions </summary>
        private static Vector3Int CubeRound(Vector3 cubePosition)
        {
            Vector3Int roundedPos = new(Mathf.RoundToInt(cubePosition.x), Mathf.RoundToInt(cubePosition.y), Mathf.RoundToInt(cubePosition.z));
            Vector3 posDiff = new(Mathf.Abs(roundedPos.x - cubePosition.x), Mathf.Abs(roundedPos.y - cubePosition.y), Mathf.Abs(roundedPos.z - cubePosition.z));

            if (posDiff.x > posDiff.y && posDiff.x > posDiff.z)
                roundedPos.x = -roundedPos.y - roundedPos.z;
            else if (posDiff.y > posDiff.z)
                roundedPos.y = -roundedPos.x - roundedPos.z;
            else
                roundedPos.z = -roundedPos.x - roundedPos.y;

            return roundedPos;
        }
        
        /// <summary> Convert a world position to an hexagon position : https://www.redblobgames.com/grids/hexagons/#rounding </summary>
        public static Vector2Int WorldToTile(Vector3 pos, float tileSize)
        {
            // Convert mouse pos to axial coordinates position : https://www.redblobgames.com/grids/hexagons/#pixel-to-hex
            Vector2 axialPos = new((SQRT3_DIVIDED_BY_3 * pos.x - ONE_THIRD * pos.z) * 2f / tileSize, TWO_THIRD * pos.z * 2f / tileSize);
            // Convert axial coordinates position to cube coordinates position
            Vector3 cubePos = AxialToCube(axialPos);
            // Round to get hexagon position (cube coordinates)
            Vector3Int cubePosRounded = CubeRound(cubePos);
            // Convert to offset coordinates position
            Vector2Int offsetPos = CubeToOffset(cubePosRounded);
            
            return offsetPos;
        }

        /// <summary> Convert an hexagon position (offset coordinates) to a world position (x,z) : https://www.redblobgames.com/grids/hexagons/#hex-to-pixel </summary>
        public static Vector3 TileToWorld(Vector2Int offsetPos, float tileSize)
        {
            float xPos = (offsetPos.x + 0.5f * (offsetPos.y & 1)) * HorizontalSpacing(tileSize);
            float yPos = offsetPos.y * VerticalSpacing(tileSize);
            return new Vector3(xPos, 0f, yPos);
        }
        
        /// <summary> Convert an hexagon position (offset coordinates) to a world position in 2D (x,y) : https://www.redblobgames.com/grids/hexagons/#hex-to-pixel </summary>
        public static Vector2 TileToWorld2D(Vector2Int offsetPos, float tileSize)
        {
            float xPos = tileSize * SQRT3_DIVIDED_BY_2 * (offsetPos.x + 0.5f * (offsetPos.y & 1));
            float yPos = tileSize * 0.75f * offsetPos.y;
            return new Vector2(xPos, yPos);
        }
        
        /// <summary> Convert a screen position to an hexagon position (offset coordinates) </summary>
        public static Vector2Int? ScreenToTile(Vector3 screenPosition, Camera camera, Plane tilemapPlane, float tileSize)
        {
            Ray ray = camera.ScreenPointToRay(screenPosition);
            if (tilemapPlane.Raycast(ray, out float dist))
            {
                Vector3 position = ray.GetPoint(dist);
                return WorldToTile(position, tileSize);
            }

            return null;
        }
        
        /// <summary> Convert a viewport position to an hexagon position (offset coordinates) </summary>
        public static Vector2Int? ViewportToTile(Vector3 screenPosition, Camera camera, Plane tilemapPlane, float tileSize)
        {
            Ray ray = camera.ViewportPointToRay(screenPosition);
            if (tilemapPlane.Raycast(ray, out float dist))
            {
                Vector3 position = ray.GetPoint(dist);
                return WorldToTile(position, tileSize);
            }

            return null;
        }
        
        /// <summary> Convert an hexagon position (offset coordinates) to a viewport position </summary>
        public static Vector2 TileToViewport(Vector2Int tilePosition, Camera camera, float tileSize)
        {
            Vector3 worldPosition = TileToWorld(tilePosition, tileSize);
            return camera.WorldToViewportPoint(worldPosition);
        }

        /// <summary> Compute the distance between 2 hexagon positions (offset coordinates) </summary>
        public static int Distance(Vector2Int originHex, Vector2Int endHex)
        {
            Vector2Int axialOriginHex = OffsetToAxial(originHex);
            Vector2Int axialEndHex = OffsetToAxial(endHex);
            int xDiff = axialEndHex.x - axialOriginHex.x;
            int yDiff = axialEndHex.y - axialOriginHex.y;
            
            int distance = (Mathf.Abs(xDiff) + Mathf.Abs(xDiff + yDiff) + Mathf.Abs(yDiff)) / 2;
            return distance;
        }
        #endregion HEXAGON_COORDINATES

        #region NEIGHBOURS
        /// <summary> Offsets depend on the tile position in offset coordinates. This returns the right offsets depending on the tile's position. </summary>
        /// <param name="yPosition">The Y position of the tile.</param>
        /// <returns>The offsets, starting by right and rotating counterclockwise.</returns>
        public static Vector2Int[] GetTileOffsets(int yPosition)
        {
            return yPosition % 2 == 0 ? EVEN_OFFSETS : ODD_OFFSETS;
        }

        /// <summary> Offsets depend on the tile position in offset coordinates. This returns the right offsets depending on the tile's position. </summary>
        /// <param name="yPosition">The Y position of the tile.</param>
        /// <param name="directionIndex">The index of the direction for the offset.</param>
        public static Vector2Int GetTileOffset(int yPosition, int directionIndex)
        {
            return GetTileOffsets(yPosition)[directionIndex];
        }
        
        public static int GetOppositeDirectionIndex(int index)
        {
            return (index + AXES_COUNT) % DIRECTIONS_COUNT;
        }
        
        /// <summary>
        /// <para>Gives all the direction indices from one side. Imagine placing an axis and picking the 2 directions of one side.</para>
        /// <para>Example : axe index is 0, we "cut" horizontally and return top right and top left (1 and 2) if clockwise, bottom right and bottom left (6 and 5) if counterclockwise.</para>
        /// </summary>
        /// <param name="axeDirectionIndex">The direction of the imaginary axis.</param>
        /// <param name="clockwise">Whether we want the directions on the clockwise side or the other.</param>
        public static int[] GetSideIndices(int axeDirectionIndex, bool clockwise)
        {
            int[] sideIndices = new int[2];
            sideIndices[0] = GetNextDirectionIndex(axeDirectionIndex, clockwise);
            sideIndices[1] = GetNextDirectionIndex(sideIndices[0], clockwise);

            return sideIndices;
        }
        
        public static int GetNextDirectionIndex(int index, bool clockwise)
        {
            index += clockwise ? 1 : -1;
            if (index < 0)
                index += DIRECTIONS_COUNT;
            else
                index %= DIRECTIONS_COUNT;

            return index;
        }

        /// <summary> Check if at least one neighbour satisfies the given condition. </summary>
        /// <param name="tilePosition">The position of the tile.</param>
        /// <param name="condition">The condition to check on the neighbours.</param>
        /// <returns>True if at least one neighbour satisfies the given condition.</returns>
        public static bool AnyNeighbour(Vector2Int tilePosition, Func<Vector2Int, bool> condition)
        {
            Vector2Int[] offsets = GetTileOffsets(tilePosition.y);
            for (int i = 0, offsetsLength = offsets.Length; i < offsetsLength; i++)
            {
                Vector2Int offsetPosition = tilePosition + offsets[i];

                if (condition(offsetPosition))
                    return true;
            }

            return false;
        }
        
        /// <summary> Check if x neighbours satisfy the given condition. </summary>
        /// <param name="tilePosition">The position of the tile.</param>
        /// <param name="condition">The condition to check on the neighbours.</param>
        /// <param name="neighboursCount">The number of neighbours needed to satisfy the requirements.</param>
        /// <returns>True if at least x neighbours satisfy the given condition.</returns>
        public static bool DoXNeighbours(Vector2Int tilePosition, Func<Vector2Int, bool> condition, int neighboursCount)
        {
            int validNeighbours = 0;
            Vector2Int[] offsets = GetTileOffsets(tilePosition.y);
            
            for (int i = 0, offsetsLength = offsets.Length; i < offsetsLength; i++)
            {
                Vector2Int offsetPosition = tilePosition + offsets[i];
                
                if (condition(offsetPosition) && ++validNeighbours >= neighboursCount) // Here we check the neighbours count
                    return true;
            }
            
            return false;
        }
        
        public static void ApplyOnNeighbours(Vector2Int tilePosition, Action<Vector2Int> action)
        {
            Vector2Int[] offsets = GetTileOffsets(tilePosition.y);
            
            for (int i = 0, offsetsLength = offsets.Length; i < offsetsLength; i++)
                action(tilePosition + offsets[i]);
        }
        #endregion NEIGHBOURS
        
        // One row out of two is offset in X, compared to squares.
        public static float GetWorldPositionXOffset(int yPosition, float tileSize)
        {
            return yPosition % 2 == 0 ? 0 : HorizontalSpacingOffset(tileSize);
        }
    }
}