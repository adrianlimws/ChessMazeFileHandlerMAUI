using System.Xml;
using ChessMaze.FileHandler.Filer.Interfaces;

namespace ChessMaze.FileHandler.Filer
{
    public partial class FileHandlerMain
    {
        /// <summary>
        /// Saves current game state to XML file
        /// </summary>
        /// <param name="game">Game to be saved</param>
        /// <param name="filePath">Path location to save XML file</param>
        /// <exception cref="IOException">Thrown when an error is detected saving the game to XML format</exception>
        public void SaveGameAsXml(IGame game, string filePath)
        {
            try
            {
                XmlDocument xmlDocx = ConvertGameToXml(game);
                xmlDocx.Save(filePath);
            }
            catch (XmlException e)
            {
                throw new IOException("Failed to save game: Invalid XML format", e);
            }
            catch (UnauthorizedAccessException e)
            {
                throw new IOException("Failed to save game: Access denied", e);
            }
            catch (Exception e)
            {
                throw new IOException("Failed to save game into XML format", e);
            }
        }

        /// <summary>
        /// Converts game object to XML document
        /// </summary>
        /// <param name="game">Game to be converted</param>
        /// <returns>An XmlDocument representing the game state</returns>
        private XmlDocument ConvertGameToXml(IGame game)
        {
            XmlDocument xmlDocx = new XmlDocument();
            XmlElement baseElement = xmlDocx.CreateElement("Game");
            xmlDocx.AppendChild(baseElement);

            XmlElement moveCountElement = xmlDocx.CreateElement("MoveCount");
            moveCountElement.InnerText = game.GetMoveCount().ToString();
            baseElement.AppendChild(moveCountElement);

            XmlElement levelElement = xmlDocx.CreateElement("Level");
            AppendLevelToXml(xmlDocx, levelElement, game.CurrentLevel);
            baseElement.AppendChild(levelElement);

            return xmlDocx;
        }

        /// <summary>
        /// Appends level data to XML document
        /// </summary>
        /// <param name="xmlDocx">XML document being constructed</param>
        /// <param name="levelElement">XML element representing the level</param>
        /// <param name="level">Level data to be appended</param>
        private void AppendLevelToXml(XmlDocument xmlDocx, XmlElement levelElement, ILevel level)
        {
            XmlElement boardElement = xmlDocx.CreateElement("Board");
            boardElement.SetAttribute("Rows", level.Board.Rows.ToString());
            boardElement.SetAttribute("Columns", level.Board.Columns.ToString());

            for (int row = 0; row < level.Board.Rows; row++)
            {
                XmlElement rowElement = xmlDocx.CreateElement("Row");
                for (int col = 0; col < level.Board.Columns; col++)
                {
                    XmlElement cellElement = xmlDocx.CreateElement("Cell");
                    cellElement.InnerText = level.Board.Cells[row, col].Type.ToString();
                    rowElement.AppendChild(cellElement);
                }

                boardElement.AppendChild(rowElement);
            }

            levelElement.AppendChild(boardElement);

            AppendPosToXml(xmlDocx, levelElement, "StartPosition", level.StartPosition);
            AppendPosToXml(xmlDocx, levelElement, "EndPosition", level.EndPosition);
            AppendPosToXml(xmlDocx, levelElement, "PlayerPosition", level.Player.CurrentPosition);

            XmlElement isCompletedElement = xmlDocx.CreateElement("IsCompleted");
            isCompletedElement.InnerText = level.IsCompleted.ToString();
            levelElement.AppendChild(isCompletedElement);
        }

        /// <summary>
        /// Appends a position to XML document
        /// </summary>
        /// <param name="xmlDocx">XML document being constructed</param>
        /// <param name="mainElement">Main XML element</param>
        /// <param name="name">Name of the position element</param>
        /// <param name="position">Position data to be appended</param>
        private void AppendPosToXml(XmlDocument xmlDocx, XmlElement mainElement, string name, IPosition position)
        {
            XmlElement positionElement = xmlDocx.CreateElement(name);
            positionElement.SetAttribute("Row", position.Row.ToString());
            positionElement.SetAttribute("Column", position.Column.ToString());
            mainElement.AppendChild(positionElement);
        }
    }
}

// References
// https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmldocument.save?view=net-8.0
// https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlelement?view=net-8.0
// https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlexception?view=net-8.0
// https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlnode?view=net-8.0
// https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlnode.innertext?view=net-8.0#system-xml-xmlnode-innertext 
// https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlnode.selectsinglenode?view=net-8.0#system-xml-xmlnode-selectsinglenode(system-string) 

/*
 * example of what the xml should look like :D
    <?xml version="1.0" encoding="utf-8"?>
    <Game>
      <MoveCount>10</MoveCount>
      <Level>
        <Board Rows="3" Columns="3">
          <Row>
            <Cell>Rook</Cell>
            <Cell>Empty</Cell>
            <Cell>King</Cell>
          </Row>
          <Row>
            <Cell>Empty</Cell>
            <Cell>Pawn</Cell>
            <Cell>Empty</Cell>
          </Row>
          <Row>
            <Cell>Bishop</Cell>
            <Cell>Empty</Cell>
            <Cell>Knight</Cell>
          </Row>
        </Board>
        <StartPosition Row="0" Column="0" />
        <EndPosition Row="2" Column="2" />
        <PlayerPosition Row="1" Column="1" />
        <IsCompleted>False</IsCompleted>
      </Level>
    </Game>
 */