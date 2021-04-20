using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace TowerDefense.Grid
{
    public class ShortestPath
    {


        public LinkedList<GridPos> Path;

        public List<List<GridPos>> ToPath;
        /// <summary>
        /// performs a breadth first search
        /// </summary>
        /// <param name="mapGrid"></param>
        /// <returns></returns>
        public LinkedList<GridPos> CalculateShortestPath(MapGrid mapGrid, int startX, int startY, int endX, int endY)
        {
            ToPath = new List<List<GridPos>>();
            var pathToReturn = new LinkedList<GridPos>();
            Queue<GridPos> gridPos = new Queue<GridPos>();
            Dictionary<string, bool> visitedDic = new Dictionary<string, bool>();
            visitedDic.Add(startX + "" + startY, true);
            gridPos.Enqueue(new GridPos(startX, startY,true,null));
            do
            {
                var currentPiece = gridPos.Dequeue();
                for(int x = 0; x < 4; x++)
                {
                    int tempX = currentPiece.XPos + 1;
                    int tempY = currentPiece.YPos;

                    if(x == 1)
                    {
                        tempX -= 2;
                    }
                    else if(x == 2)
                    {
                        tempX -= 1;
                        tempY += 1;
                    }
                    else if (x == 3)
                    {
                        tempX -= 1;
                        tempY -= 1;
                    }

                    //if you can't enqueue then go to the next
                    if (!CanEnqueue(tempX, tempY, mapGrid, visitedDic))
                        continue;

                    visitedDic.Add(tempX + "" + tempY, true);
                    var toBeQueued = new GridPos(tempX, tempY, true, currentPiece);
                    gridPos.Enqueue(toBeQueued);

                    //If this is true then we have reached the end and we can confidently say we have found the shortest path!
                    if(tempX == endX && tempY == endY)
                    {
                        var toAddToPath = new GridPos(toBeQueued);
                        do
                        {
                            pathToReturn.AddFirst(toAddToPath);
                            if (toAddToPath.Previous != null)
                                toAddToPath = new GridPos(toAddToPath.Previous);
                            else
                                toAddToPath = null;

                        } while (toAddToPath != null);
                        
                        return pathToReturn;
                    }
                }
                
               

            } while (gridPos.Count > 0);

            return null;
        }

        public (int index,List<GridPos> list) PathToShortest(int startX, int startY, MapGrid mapGrid,List<GridPos> path)
        {
            foreach(var paths in ToPath)
            {
                if(paths.Any(t=>t.XPos == startX && t.YPos == startY))
                {

                    return (paths.IndexOf(paths.First(t => t.XPos == startX && t.YPos == startY)), paths);
                }
            }
            LinkedList<GridPos> toReturn = new LinkedList<GridPos>();
            Queue<GridPos> gridPos = new Queue<GridPos>();
            Dictionary<string, bool> visitedDic = new Dictionary<string, bool>();
            visitedDic.Add(startX + "" + startY, true);
            gridPos.Enqueue(new GridPos(startX, startY, true, null));
            do
            {
                var currentPiece = gridPos.Dequeue();
                for (int x = 0; x < 4; x++)
                {
                    int tempX = currentPiece.XPos + 1;
                    int tempY = currentPiece.YPos;

                    if (x == 1)
                    {
                        tempX -= 2;
                    }
                    else if (x == 2)
                    {
                        tempX -= 1;
                        tempY += 1;
                    }
                    else if (x == 3)
                    {
                        tempX -= 1;
                        tempY -= 1;
                    }

                    //if you can't enqueue then go to the next
                    if (!CanEnqueue(tempX, tempY, mapGrid, visitedDic))
                        continue;

                    visitedDic.Add(tempX + "" + tempY, true);
                    var toBeQueued = new GridPos(tempX, tempY, true, currentPiece);
                    gridPos.Enqueue(toBeQueued);

                    //If this is true then we have reached the end and we can confidently say we have found the shortest path!
                    if (path.Any(t=>t.XPos == tempX && t.YPos == tempY))
                    {
                        var toAddToPath = new GridPos(toBeQueued);
                        bool done = false;
                        do
                        {
                            toReturn.AddFirst(toAddToPath);
                            if (toAddToPath.Previous != null)
                                toAddToPath = new GridPos(toAddToPath.Previous);
                            else
                            {
                                
                                var last = toReturn.Last();
                                var indexOfLast = path.FindIndex(t => t.XPos == last.XPos && t.YPos == last.YPos);
                                for(int i = indexOfLast + 1; i < path.Count; i++)
                                {
                                    toReturn.AddLast(path[i]);
                                }

                                done = true;
                            }

                        } while (done != true);

                        
                        var toReturnList = toReturn.ToList();
                        ToPath.Add(toReturnList);
                        return (0, toReturnList);
                    }
                }



            } while (gridPos.Count > 0);

            return (0,null);
        }

        public bool CanEnqueue(int x, int y, MapGrid map, Dictionary<string, bool> visitedDic)
        {
            if (!map.CoordinatesInMap(x, y))
                return false;
            if (map.IsPieceOccupied(x, y))
                return false;

            return !visitedDic.ContainsKey(x + "" + y);

        }


        public class GridPos
        {
            public bool Visited;
            public int XPos;
            public int YPos;
            public GridPos Previous;
            public GridPos(int xPos, int yPos,bool visited,GridPos previous)
            {
                this.XPos = xPos;
                this.YPos = yPos;
                this.Visited = visited;
                this.Previous = previous;
                
            }

            public GridPos(GridPos gridPos)
            {
                if (gridPos == null)
                    return;
                this.XPos = gridPos.XPos;
                this.YPos = gridPos.YPos;
                this.Visited = gridPos.Visited;
                this.Previous = gridPos.Previous;
            }
        }
        
    }
}
