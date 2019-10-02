using System;
using System.Collections.Generic;
using System.Linq;
using GameWithTests;

namespace GameWithTests
{
    public enum AddressNumber { One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten };
    public enum AddressLetter { A, B, C, D, E, F, J, H, I, G }
    public enum CellState { InitialEmpty, InitialDeck, AffectedEmpty, AffectedDeck }
    public enum ShipName { FourDecker, ThreeDecker_1, ThreeDecker_2, TwoDecker_1, TwoDecker_2, TwoDecker_3, SingleDecker_1, SingleDecker_2, SingleDecker_3, SingleDecker_4 }
    public enum ShotResult { Past, Injured, Killed, Affected }

    public class Coordinate
    {
        public AddressNumber Number { get; private set; }
        public AddressLetter Letter { get; private set; }

        public Coordinate(AddressLetter letter, AddressNumber number)
        {
            Letter = letter;
            Number = number;
        }

        public override string ToString()
        {
            return Letter + ((int)Number).ToString();
        }

        public static bool operator ==(Coordinate myCoordinate, Coordinate otherCoordinate)
        {
            return (myCoordinate.Letter == otherCoordinate.Letter && myCoordinate.Number == otherCoordinate.Number);
        }
        public static bool operator !=(Coordinate myCoordinate, Coordinate otherCoordinate)
        {
            return (myCoordinate.Letter != otherCoordinate.Letter | myCoordinate.Number != otherCoordinate.Number);
        }

        public static Coordinate DefineCoordinateFromString(string letterNumber)
        {
            AddressLetter letter = (AddressLetter)Enum.Parse(typeof(AddressLetter), letterNumber[0].ToString());
            AddressNumber number;
            if (letterNumber.Length == 3)
            { number = (AddressNumber)int.Parse((letterNumber[1].ToString() + letterNumber[2].ToString()).ToString()); }
            else
            { number = (AddressNumber)int.Parse(letterNumber[1].ToString()); }
            Coordinate DefinedCoordinate = new Coordinate(letter, number - 1);
            return DefinedCoordinate;
        }
    }

    public class Cell
    {
        public Coordinate Address { get; private set; }
        public CellState State { get; set; }

        public Cell(Coordinate address)
        {
            Address = address;
            State = CellState.InitialEmpty;
        }

    }

    public abstract class Board
    {
        public List<Cell> Cells { get; private set; }

        public Board()
        {
            Cells = new List<Cell>(100);

            foreach (AddressLetter letter in Enum.GetValues(typeof(AddressLetter)))
            {
                foreach (AddressNumber number in Enum.GetValues(typeof(AddressNumber)))
                {
                    Cells.Add(new Cell(new Coordinate(letter, number)));

                }
            }
        }
    }
    public class ShipsBoard : Board
    {
        public ShipsBoard() : base()
        {

        }
    }
    public class ShotsBoard : Board
    {
        public ShotsBoard() : base()
        {

        }
    }

    public class Ship
    {
        public ShipName Name { get; private set; }              // название - напр. Однопалубный_1; Однопалубный_2;
        public Coordinate[] shipCoordinate { get; set; }        // адреса клеток, по которым можно найти корабль;
        public int DeckNumber { get; private set; }             //количество палуб у корабля всего;
        public int UnharmedDeckNumber { get; set; }             //количество палуб корабля, которые не задеты выстрелом;
        public bool IsShipSunk
        {
            get { return (UnharmedDeckNumber == 0); }
        }

        public Ship(ShipName name, int deckNumber)
        {
            Name = name;
            DeckNumber = deckNumber;
            UnharmedDeckNumber = deckNumber;
            Coordinate[] coordinate = new Coordinate[DeckNumber];
        }
    }
    public interface IPlayer
    {
        String Name { get; }
        ShipsBoard Board_1 { get; }
        ShotsBoard Board_2 { get; }
        bool DoesPlayerHaveShips { get; }

        void SetShips();
        Coordinate MakeShot();
        void SetShotsResult(Coordinate coordinateOfShot, ShotResult result);
        void DrawBoardsRow();
    }

    public class Player : IPlayer
    {
        private string _name;
        private List<Ship> _playerShips = new List<Ship>();
        private ShipsBoard _shipsBoard;
        private ShotsBoard _shotsBoard;

        public String Name
        {
            get { return _name; }
        }
        public ShipsBoard Board_1
        {
            get { return _shipsBoard; }
        }

        public ShotsBoard Board_2
        {
            get { return _shotsBoard; }
        }

        public bool DoesPlayerHaveShips
        {
            get
            {
                return _playerShips.Any(ship => !ship.IsShipSunk);
            }
        }

        public Player(string firstName)
        {
            _name = firstName;
            _shipsBoard = new ShipsBoard();
            _shotsBoard = new ShotsBoard();
            _playerShips = CreateShips();
        }

        public void SetShips()
        {
            List<Coordinate> shipsCoordinates = new List<Coordinate>();
            foreach (Ship certainShip in _playerShips)
            {
                int n = certainShip.DeckNumber;
                Console.WriteLine("{1}, enter {0} coordinates for your {0}-deck ship:", n, this.Name);
                string coordinates = Console.ReadLine().ToUpper();
                string[] coordinates1 = coordinates.Split(' ');

                List<Coordinate> certainShipCoordinates = new List<Coordinate>();
                foreach (string c in coordinates1)
                {
                    string[] partOfCoordinate = c.Split();
                    foreach (string part in partOfCoordinate)
                    {
                        var coordinateOfShip = Coordinate.DefineCoordinateFromString(c);
                        certainShipCoordinates.Add(coordinateOfShip);
                        certainShip.shipCoordinate = certainShipCoordinates.ToArray();
                        Board_1.Cells.Single(cell => cell.Address == coordinateOfShip).State = CellState.InitialDeck;
                    }
                }
                shipsCoordinates.AddRange(certainShipCoordinates);
                DrawBoardsRow();
            }
        }

        private List<Ship> CreateShips()
        {
            List<Ship> allPlayerShips = new List<Ship>();
            Ship fourdeck = new Ship(ShipName.FourDecker, 4);
            allPlayerShips.Add(fourdeck);

            Ship threedeck1 = new Ship(ShipName.ThreeDecker_1, 3);
            allPlayerShips.Add(threedeck1);

            //Ship threedeck2 = new Ship(ShipName.ThreeDecker_2, 3);
            //allPlayerShips.Add(threedeck2);

            //Ship twodeck1 = new Ship(ShipName.TwoDecker_1, 2);
            //allPlayerShips.Add(twodeck1);

            //Ship twodeck2 = new Ship(ShipName.TwoDecker_2, 2);
            //allPlayerShips.Add(twodeck2);

            //Ship twodeck3 = new Ship(ShipName.TwoDecker_3, 2);
            //allPlayerShips.Add(twodeck3);

            //Ship onedeck1 = new Ship(ShipName.SingleDecker_1, 1);
            //allPlayerShips.Add(onedeck1);

            //Ship onedeck2 = new Ship(ShipName.SingleDecker_2, 1);
            //allPlayerShips.Add(onedeck2);

            //Ship onedeck3 = new Ship(ShipName.SingleDecker_3, 1);
            //allPlayerShips.Add(onedeck3);

            //Ship onedeck4 = new Ship(ShipName.SingleDecker_4, 1);
            //allPlayerShips.Add(onedeck4);

            return allPlayerShips;
        }

        //public Coordinate MakeShot()
        //{
        //    Console.WriteLine("{0}, enter coordinate for your shot: ", this.Name);
        //    string coordinates = Console.ReadLine().ToUpper();
        //    string[] partOfCoordinate = coordinates.Split();
        //    var coordinateOfShot = Coordinate.DefineCoordinateFromString(coordinates);
        //    return coordinateOfShot;
        //}

        public string AskForCoordinateOfShot()    //refactoring: public Coordinate MakeShot()
        {
            Console.WriteLine("{0}, enter coordinate for your shot: ", this.Name);
            string coordinates = Console.ReadLine().ToUpper();
            //string[] partOfCoordinate = coordinates.Split();
            return coordinates;
        }
        public Coordinate MakeShot()                //refactoring: public Coordinate MakeShot()
        {
            var coordinateOfShot = Coordinate.DefineCoordinateFromString(AskForCoordinateOfShot());
            return coordinateOfShot;
        }


        public void SetShotsResult(Coordinate coordinateOfShot, ShotResult result)
        {
            if (result == ShotResult.Past)
            {
                Board_2.Cells.Single(cell => cell.Address == coordinateOfShot).State = CellState.AffectedEmpty;
            }
            else if (result == ShotResult.Injured)
            {
                Board_2.Cells.Single(cell => cell.Address == coordinateOfShot).State = CellState.AffectedDeck;

            }
            else
            {
                Board_2.Cells.Single(cell => cell.Address == coordinateOfShot).State = CellState.AffectedDeck;
            }
        }

        //public ShotResult DefineShotResultByCoordinate(Coordinate shotCoordinate)
        //{
        //    Cell cellResult = Board_1.Cells.Single(cell => cell.Address == shotCoordinate);

        //    if (cellResult.State == CellState.InitialEmpty || cellResult.State == CellState.AffectedEmpty)
        //    {
        //        cellResult.State = CellState.AffectedEmpty;
        //        Console.WriteLine(ShotResult.Past);
        //        return ShotResult.Past;
        //    }
        //    else if (cellResult.State == CellState.AffectedDeck)
        //    {
        //        Console.WriteLine(ShotResult.Injured);
        //        Console.WriteLine("You have already had the shot to this coordinate. But it's turn of your opponent.");
        //        return ShotResult.Affected;
        //    }
        //    else if (cellResult.State == CellState.InitialDeck)
        //    {
        //        Ship shipWithCellResultCoordinate = _playerShips.Single(ship => ship.shipCoordinate.Any(c => c == shotCoordinate));

        //        cellResult.State = CellState.AffectedDeck;
        //        shipWithCellResultCoordinate.UnharmedDeckNumber -= 1;
        //        if (shipWithCellResultCoordinate.IsShipSunk)
        //        {
        //            Console.WriteLine(ShotResult.Killed);
        //            return ShotResult.Killed;
        //        }
        //    }
        //    Console.WriteLine(ShotResult.Injured);
        //    return ShotResult.Injured;
        //}
        public ShotResult DefineShotResultByCoordinate(Coordinate shotCoordinate)  //refactoring: public ShotResult DefineShotResultByCoordinate(Coordinate shotCoordinate)
        {
            Cell cellResult = Board_1.Cells.Single(cell => cell.Address == shotCoordinate);

            if (cellResult.State == CellState.InitialEmpty || cellResult.State == CellState.AffectedEmpty)
            {
                cellResult.State = CellState.AffectedEmpty;
                // Console.WriteLine(ShotResult.Past);
                return ShotResult.Past;
            }
            else if (cellResult.State == CellState.AffectedDeck)
            {
                //Console.WriteLine(ShotResult.Injured);
                // Console.WriteLine("You have already had the shot to this coordinate. But it's turn of your opponent.");
                return ShotResult.Affected;
            }
            else if (cellResult.State == CellState.InitialDeck)
            {
                Ship shipWithCellResultCoordinate = _playerShips.Single(ship => ship.shipCoordinate.Any(c => c == shotCoordinate));

                cellResult.State = CellState.AffectedDeck;
                shipWithCellResultCoordinate.UnharmedDeckNumber -= 1;
                if (shipWithCellResultCoordinate.IsShipSunk)
                {
                    // Console.WriteLine(ShotResult.Killed);
                    return ShotResult.Killed;
                }
            }
            //Console.WriteLine(ShotResult.Injured);
            return ShotResult.Injured;
        }


        public void ShowResultOfShot(ShotResult result)  ////refactoring: public ShotResult DefineShotResultByCoordinate(Coordinate shotCoordinate)
        {
            if (result == ShotResult.Past)
            { Console.WriteLine(ShotResult.Past); }
            else if (result == ShotResult.Affected)
            {
                Console.WriteLine(ShotResult.Injured);
                Console.WriteLine("You have already had the shot to this coordinate. But it's turn of your opponent.");
            }
            else if (result == ShotResult.Killed)
            { Console.WriteLine(ShotResult.Killed); }
            else
                Console.WriteLine(ShotResult.Injured);
        }

        private List<string> CreateBoardRow(Board board)
        {
            List<string> rowList = new List<string>();
            rowList.Add("  1  2  3  4  5  6  7  8  9  10");
            foreach (AddressLetter letter in Enum.GetValues(typeof(AddressLetter)))
            {
                string row = letter.ToString();

                foreach (AddressNumber number in Enum.GetValues(typeof(AddressNumber)))
                {
                    Cell cell = board.Cells.Single(c => c.Address.Letter == letter && c.Address.Number == number);

                    if (cell.State == CellState.InitialEmpty)
                    {
                        row += "[ ]";
                    }
                    else if (cell.State == CellState.AffectedEmpty)
                    {
                        row += "[-]";
                    }
                    else if (cell.State == CellState.AffectedDeck)
                    {
                        row += "[X]";
                    }
                    else if (cell.State == CellState.InitialDeck)
                    {
                        row += "[D]";
                    }
                }
                rowList.Add(row);
            }
            return rowList;
        }
        public void DrawBoardsRow()
        {
            var rowShipsBoard = CreateBoardRow(Board_1);
            var rowShotsBoard = CreateBoardRow(Board_2);
            var shotsBoardAndShipsBoard = rowShipsBoard.Zip(rowShotsBoard, (ships, shots) => ships + "\t" + "\t" + shots);
            foreach (var item in shotsBoardAndShipsBoard)
                Console.WriteLine(item);
        }
    }

    public class Game
    {
        private Player _player1;
        private Player _player2;

        public Game()
        {
            _player1 = new Player("Yury");
            _player2 = new Player("Larysa");
        }
        private void DriveGameLoop()
        {
            while (_player1.DoesPlayerHaveShips && _player2.DoesPlayerHaveShips)
            {
                GameLoop(_player1, _player2);
                GameLoop(_player2, _player1);
            }
        }

        public void StartGame()
        {
            _player1.SetShips();
            _player2.SetShips();
            DriveGameLoop();
        }

        private void GameLoop(Player playerMakesShot, Player playerChecksShot)
        {
            ShotResult result;
            if (playerMakesShot.DoesPlayerHaveShips)
                do
                {
                    playerMakesShot.DrawBoardsRow();
                    Coordinate shot = playerMakesShot.MakeShot();

                    result = playerChecksShot.DefineShotResultByCoordinate(shot);
                    playerChecksShot.ShowResultOfShot(result); //refactoring: public ShotResult DefineShotResultByCoordinate(Coordinate shotCoordinate)


                    playerMakesShot.SetShotsResult(shot, result);
                }
                while ((result != ShotResult.Past && result != ShotResult.Affected) && playerChecksShot.DoesPlayerHaveShips);
        }
    }
}
public class Program
{
    public static void Main(string[] args)
    {
        GameWithTests.Game game = new Game();
        game.StartGame();
    }
}

