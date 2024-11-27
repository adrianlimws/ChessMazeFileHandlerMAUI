using System.Xml;
using ChessMaze.FileHandler.Filer.Enums;
using ChessMaze.FileHandler.Filer.Interfaces;

namespace ChessMaze.FileHandler.Filer;

public partial class FileHandlerMain
{
    /// <summary>
    /// Loads game state from an XML file
    /// </summary>
    /// <param name="filePath">Path location to load the XML file</param>
    /// <returns>A loaded game state</returns>
    /// <exception cref="IOException">Thrown when an error is detected loading the game from XML format</exception>
    public IGame LoadGameFromXml(string filePath)
    {
        try
        {
            XmlDocument xmlDocx = new XmlDocument();
            xmlDocx.Load(filePath);
            return ConvertXmlToGame(xmlDocx);
        }
        catch (FileNotFoundException e)
        {
            throw new IOException("Failed to load game: XML file not found", e);
        }
        catch (XmlException e)
        {
            throw new IOException("Failed to load game: Invalid XML format", e);
        }
        catch (Exception e)
        {
            throw new IOException("Failed to load game from XML", e);
        }
    }

    /// <summary>
    /// Converts an XML document to a game object
    /// </summary>
    /// <param name="xmlDocx">XML document representing the game state</param>
    /// <returns>A reconstructed game object</returns>
    private IGame ConvertXmlToGame(XmlDocument xmlDocx)
    {
        XmlElement baseElement = xmlDocx.DocumentElement;
        int moveCount = int.Parse(baseElement.SelectSingleNode("MoveCount").InnerText);

        XmlNode levelNode = baseElement.SelectSingleNode("Level");
        ILevel level = ConvertLevelFromXml(levelNode);

        return new Game
        {
            CurrentLevel = level,
            MoveCount = moveCount
        };
    }

    /// <summary>
    /// Converts an XML node to a level object
    /// </summary>
    /// <param name="levelNode">XML node representing the level</param>
    /// <returns>A reconstructed level object</returns>
    private ILevel ConvertLevelFromXml(XmlNode levelNode)
    {
        XmlNode boardNode = levelNode.SelectSingleNode("Board");
        int rows = int.Parse(boardNode.Attributes["Rows"].Value);
        int columns = int.Parse(boardNode.Attributes["Columns"].Value);

        Board board = new Board(rows, columns);
        XmlNodeList rowNodes = boardNode.SelectNodes("Row");
        for (int row = 0; row < rows; row++)
        {
            XmlNodeList cellNodes = rowNodes[row].SelectNodes("Cell");
            for (int col = 0; col < columns; col++)
            {
                PieceType pieceType = (PieceType)Enum.Parse(typeof(PieceType), cellNodes[col].InnerText);
                board.Cells[row, col] = new Board.Piece(pieceType);
            }
        }

        IPosition startPos = ConvertPositionFromXml(levelNode.SelectSingleNode("StartPosition"));
        IPosition endPos = ConvertPositionFromXml(levelNode.SelectSingleNode("EndPosition"));
        IPosition playerPos = ConvertPositionFromXml(levelNode.SelectSingleNode("PlayerPosition"));
        bool isCompleted = bool.Parse(levelNode.SelectSingleNode("IsCompleted").InnerText);

        return new Level
        {
            Board = board,
            StartPosition = startPos,
            EndPosition = endPos,
            Player = new Player { CurrentPosition = playerPos },
            IsCompleted = isCompleted
        };
    }

    /// <summary>
    /// Converts an XML node to a position object
    /// </summary>
    /// <param name="positionNode">XML node representing the position</param>
    /// <returns>A reconstructed position object</returns>
    private IPosition ConvertPositionFromXml(XmlNode positionNode)
    {
        int row = int.Parse(positionNode.Attributes["Row"].Value);
        int column = int.Parse(positionNode.Attributes["Column"].Value);
        return new Position { Row = row, Column = column };
    }
}

// References
// https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmldocument.save?view=net-8.0
// https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlelement?view=net-8.0
// https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlexception?view=net-8.0
// https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlnode?view=net-8.0
// https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlnode.innertext?view=net-8.0#system-xml-xmlnode-innertext 
// https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlnode.selectsinglenode?view=net-8.0#system-xml-xmlnode-selectsinglenode(system-string) 