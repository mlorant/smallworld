﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using mapWrapper;

namespace SmallWorld
{
    /// <summary>
    /// Map creation and management. Store a grid of tile instance
    /// with a flyweight design pattern to optimize ressources.
    /// Units are located on the map via the tile index in the array.
    /// </summary>
    public class Map : IMap
    {

        /// <summary>
        /// Map grid. Composed of instances of ICase, which inform
        /// about the type of terrain (plain, sea, forest, ...)
        /// 
        /// Each tile is represented with (x, y) coordinates, which
        /// can be computed with the width of the grid (the grid is 
        /// necessarely a square)
        /// </summary>
        private ICase[] _grid;

        /// <summary>
        /// Instances list of square types (Flyweight pattern)
        /// </summary>
        private ICase[] _casesReferences;

        /// <summary>
        /// List of units present on each tiles
        /// </summary>
        private List<IUnit>[] _units;

        /// <summary>
        /// Width of the map
        /// </summary>
        private int _width;

        /// <summary>
        /// Size of the map
        /// </summary>
        public int Size
        {
            get { return _grid.Length; }
        }

        /// <summary>
        /// Width of the map
        /// </summary>
        public int Width
        {
            get { return _width; }
        }


        /// <summary>
        /// Init a new map with a new flyweigh pattern
        /// </summary>
        public Map()
        {
            _casesReferences = new ICase[5];
        }


        public void generateMap(int mapWidth)
        {
            this._width = mapWidth;

            int nbTiles = _width * _width;

            WrapperMapGenerator wrapper = new WrapperMapGenerator();
            List<int> cases = wrapper.generate_map(_width);

            _units = new List<IUnit>[nbTiles];
            _grid = new ICase[nbTiles];

            for (int i = 0; i < nbTiles; i++)
            {
                _grid[i] = this.getCaseTypeInstance(cases[i]);
            }     
        }


        private ICase getCaseTypeInstance(int caseIndex)
        {
            if (_casesReferences[caseIndex] == null)
            {
                _casesReferences[caseIndex] = generateCase(caseIndex);
            }

            return _casesReferences[caseIndex];
        }

        /// <summary>
        /// Create a new instance of case
        /// </summary>
        /// <param name="caseIndex"></param>
        /// <returns></returns>
        private ICase generateCase(int caseIndex) 
        {
            ICase obj = null;
            switch (caseIndex)
            {
                case 0:
                    obj = new Desert();
                    break;
                case 1:
                    obj = new Forest();
                    break;
                case 2:
                    obj = new Mountain();
                    break;
                case 3:
                    obj = new Plain();
                    break;
                case 4:
                    obj = new Sea();
                    break;
                default:
                    throw new UnknownTileException();
            }
            
            return obj;
        }

        /// <summary>
        /// Get two points at the north-west and south-east of the map
        /// which aren't and the water. This points will be used to 
        /// determine the initial position of player units
        /// </summary>
        /// <returns>An array of 2 points at opposites of the map</returns>
        public Point[] getStartPoints()
        {
            Point[] points = new Point[2];
            bool found = false;

            // Get first player
            Point p = new Point(0, 0);
            while (!found)
            {
                if (!(this.getCase(p) is Sea))
                {
                    points[0] = p;
                    found = true;
                }
                else
                {
                    if (p.X > p.Y)
                        p.Y++;
                    else
                        p.X++;
                }
            }

           
            // Get second player
            p = new Point(this.Width-1, this.Width-1);
            found = false;
            while (!found)
            {
                if (!(this.getCase(p) is Sea))
                {
                    points[1] = p;
                    found = true;
                }
                else
                {
                    if (p.X > p.Y)
                        p.Y--;
                    else
                        p.X--;
                }
            }

            return points;
        }

        private int getIndexFromPoint(Point pos)
        {
            int index = pos.X * _width + pos.Y;
            if (index > _grid.Length)
            {
                throw new IndexOutOfRangeException("Out of bounds: there's only " + _grid.Length + " tiles in the grid");
            }

            return index;
        }

        public ICase getCase(Point pos)
        {
            int index = this.getIndexFromPoint(pos);
            return _grid[index];
        }

        /// <summary>
        /// Get list of units of a given tile
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public List<IUnit> getUnits(Point pos)
        {
            int index = this.getIndexFromPoint(pos);
            if (_units[index] == null)
                _units[index] = new List<IUnit>();
                
            return _units[index];
            
        }


        ///////////////////////////////////
        /// <summary>
        /// Place units at the beginning
        /// </summary>
        /// <param name="playerUnits"></param>
        /// <param name="pos"></param>
        public void initUnits(List<IUnit> playerUnits, Point pos)
        {
            int index = getIndexFromPoint(pos);
            _units[index] = new List<IUnit>();
            foreach (IUnit unit in playerUnits)
            {
                _units[index].Add(unit);
            }
        }

        /// <summary>
        /// Returns the best defensive unit on the tile, which will
        /// be used to fight in the battle.
        /// </summary>
        /// <param name="tgt">Position (x, y) of the tile</param>
        /// <returns>The best defensive unit of the tile</returns>
        public IUnit getBestDefensiveUnit(Point tgt)
        {
            int index = this.getIndexFromPoint(tgt);

            IUnit bestUnit = null;
            foreach (IUnit unit in this._units[index])
            {
                if (bestUnit == null || bestUnit.Defense < unit.Defense)
                {
                    bestUnit = unit;
                }
            }

            return bestUnit;
        }

        
    }

    class UnknownTileException : Exception
    {
    }
}
