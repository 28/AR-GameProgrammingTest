using System;
using System.Collections.Generic;

namespace Task3
{
    public enum MapGridType
    {
        Water,
        Grass,
        Mountain,
        Desert,
        Arctic
    }

    public interface IGridDecorator
    {
        void PerformGridDecoratorAction();
    }

    public class ForestDecorator : IGridDecorator
    {
        public void PerformGridDecoratorAction()
        {
            Console.WriteLine("Growing wild fruit!");
        }
    }

    public class RiverDecorator : IGridDecorator
    {
        public void PerformGridDecoratorAction()
        {
            Console.WriteLine("Doing something with river!");
        }
    }

    public class LavaDecorator : IGridDecorator
    {
        public void PerformGridDecoratorAction()
        {
            Console.WriteLine("Doing something with lava!");
        }
    }

    public class BeachDecorator : IGridDecorator
    {
        public void PerformGridDecoratorAction()
        {
            // Do nothing...
        }
    }

    public class MonsterPatrolDecorator : IGridDecorator
    {
        public void PerformGridDecoratorAction()
        {
            Console.WriteLine("Spawning monster patrol!");
        }
    }

    public class MapGrid
    {
        public MapGridType GridType { get; }
        private readonly List<MapGrid> _adjacentGrids = new List<MapGrid>();
        private readonly List<IGridDecorator> _gridDecorators = new List<IGridDecorator>();
        private DateTime _lastDailyAction;
        public bool Explored { get; set; }
        public bool InVision { get; set; }

        public MapGrid(MapGridType mapGridType)
        {
            GridType = mapGridType;
        }

        public void AddAdjacentGrids(params MapGrid[] mapGrids)
        {
            _adjacentGrids.AddRange(mapGrids);
        }

        public void AddDecorators(params IGridDecorator[] decorators)
        {
            _gridDecorators.AddRange(decorators);
        }

        public void PerformGridActions()
        {
            if (_lastDailyAction.Date == DateTime.Today) return;
            _gridDecorators.ForEach(decorator => decorator.PerformGridDecoratorAction());
            _lastDailyAction = DateTime.Now;
        }

        public void Visible()
        {
            InVision = true;
        }

        public void NotVisible()
        {
            InVision = false;
        }

        public void Visit()
        {
            Explored = true;
            InVision = true;
            _adjacentGrids.ForEach(grid => grid.Visible());
        }

        public bool IsGridAdjacent(MapGrid grid)
        {
            return _adjacentGrids.Contains(grid);
        }
    }

    public class Map
    {
        public List<MapGrid> MapGrids { get; private set; }

        public Map(List<MapGrid> mapGrids)
        {
            MapGrids = mapGrids;
        }

        public void PerformGridActions()
        {
            MapGrids.ForEach(grid => grid.PerformGridActions());
        }

        public void MakeOtherGridsNotVisible(MapGrid grid)
        {
            MapGrids.ForEach(g =>
            {
                if (g != grid && !grid.IsGridAdjacent(g))
                    g.NotVisible();
            });
        }
    }

    public static class Maps
    {
        static void Main(string[] args)
        {
            var grid1 = new MapGrid(MapGridType.Grass);
            grid1.AddDecorators(new ForestDecorator(), new MonsterPatrolDecorator());
            var grid2 = new MapGrid(MapGridType.Mountain);
            grid2.AddDecorators(new RiverDecorator(), new MonsterPatrolDecorator());
            var grid3 = new MapGrid(MapGridType.Desert);
            var grid4 = new MapGrid(MapGridType.Arctic);
            grid4.AddDecorators(new BeachDecorator());
            grid3.AddAdjacentGrids(grid4);
            grid4.AddAdjacentGrids(grid3);

            var grids = new List<MapGrid> {grid1, grid2, grid3, grid4};
            var map = new Map(grids);

            foreach (var grid in grids)
            {
                map.PerformGridActions();
                grid.Visit();
                map.MakeOtherGridsNotVisible(grid);
                DisplayMap(map);
            }
        }

        private static void DisplayMap(Map map)
        {
            map.MapGrids.ForEach(grid =>
                Console.WriteLine(grid.GridType + ", explored: " + grid.Explored + ", visible: " + grid.InVision));
            Console.WriteLine();
        }
    }
}