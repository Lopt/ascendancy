using System;
using CocosSharp;
using client.Common.Models;
using System.Collections;
using System.Collections.Generic;
using Core.Models.Definitions;


namespace client.Common.Views
{
    public class MenuView
    {


        public MenuView(CCTileMapLayer menuLayer, CCTileMapCoordinates center, Definition[] types)
        {
            m_center = center;
            m_types = types;
            m_menuLayer = menuLayer;
    
        }

        public CCTileMapCoordinates[] GetSurroundedTiles()
        {
            var coordHelper = new CCTileMapCoordinates[6];

            coordHelper[0] = new CCTileMapCoordinates(m_center.Column + (m_center.Row) % 2,
                m_center.Row - 1);

            coordHelper[1] = new CCTileMapCoordinates(m_center.Column + (m_center.Row) % 2,
                m_center.Row + 1);

            coordHelper[2] = new CCTileMapCoordinates(m_center.Column,
                m_center.Row + 2);

            coordHelper[3] = new CCTileMapCoordinates(m_center.Column - (m_center.Row + 1) % 2,
                m_center.Row + 1);

            coordHelper[4] = new CCTileMapCoordinates(m_center.Column - (m_center.Row + 1) % 2,
                m_center.Row - 1);

            coordHelper[5] = new CCTileMapCoordinates(m_center.Column,
                m_center.Row - 2);

            return coordHelper;
        }


        /// <summary>
        /// Shows the menu at a given Location.
        /// </summary>
        /// <param name="location">Touch Location.</param>
        /// <param name="menutype">Menutype.</param>

        public void DrawMenu()
        {
            var surroundedCoords = GetSurroundedTiles();

            for (var index = 0; index < surroundedCoords.Length; ++index)
            {
                var coord = surroundedCoords[index];
                var gid = ViewDefinitions.Instance.DefinitionToTileGid(m_types[index], ViewDefinitions.Sort.Menu);
                m_menuLayer.SetTileGID(gid, coord);
            }
            /*
            {
            }
                case 0:

                    //m_touchGesture = TouchGesture.None;   !   
                    break;
                case 1: //UnitMenu
                    gidHelpercenter.Gid = ClientConstants.CROSS_GID;
                    gidHelper1.Gid = ClientConstants.MENUEBOWMAN_GID;
                    gidHelper2.Gid = ClientConstants.MENUEHERO_GID;
                    gidHelper3.Gid = ClientConstants.MENUEWARRIOR_GID;
                    gidHelper4.Gid = ClientConstants.MENUEMAGE_GID;
                    gidHelper5.Gid = ClientConstants.MENUESCOUT_GID;
                    gidHelper6.Gid = ClientConstants.MENUEUNKNOWN_GID;
                    MenuLayer.SetTileGID(gidHelper1, coordHelper1);
                    MenuLayer.SetTileGID(gidHelper2, coordHelper2);
                    MenuLayer.SetTileGID(gidHelper3, coordHelper3);
                    MenuLayer.SetTileGID(gidHelper4, coordHelper4);
                    MenuLayer.SetTileGID(gidHelper5, coordHelper5);
                    MenuLayer.SetTileGID(gidHelper6, coordHelper6);
                    break;
                case 2: //BuildingMenu
                    gidHelpercenter.Gid = ClientConstants.CROSS_GID;
                    gidHelper1.Gid = ClientConstants.MENUEEARTH_GID;
                    gidHelper2.Gid = ClientConstants.MENUEFIRE_GID;
                    gidHelper3.Gid = ClientConstants.MENUEGOLD_GID;
                    gidHelper4.Gid = ClientConstants.MENUEAIR_GID;
                    gidHelper5.Gid = ClientConstants.MENUEMANA_GID;
                    gidHelper6.Gid = ClientConstants.MENUEWATER_GID;
                    MenuLayer.SetTileGID(gidHelpercenter, location);
                    MenuLayer.SetTileGID(gidHelper1, coordHelper1);
                    MenuLayer.SetTileGID(gidHelper2, coordHelper2);
                    MenuLayer.SetTileGID(gidHelper3, coordHelper3);
                    MenuLayer.SetTileGID(gidHelper4, coordHelper4);
                    MenuLayer.SetTileGID(gidHelper5, coordHelper5);
                    MenuLayer.SetTileGID(gidHelper6, coordHelper6);
                    break;
                default:
                    gidHelpercenter.Gid = ClientConstants.CROSS_GID;
                    gidHelper1.Gid = ClientConstants.MENUEEARTH_GID;
                    gidHelper2.Gid = ClientConstants.MENUEFIRE_GID;
                    gidHelper3.Gid = ClientConstants.MENUEGOLD_GID;
                    gidHelper4.Gid = ClientConstants.MENUEAIR_GID;
                    gidHelper5.Gid = ClientConstants.MENUEMANA_GID;
                    gidHelper6.Gid = ClientConstants.MENUEWATER_GID;
                    MenuLayer.SetTileGID(gidHelpercenter, location);
                    MenuLayer.SetTileGID(gidHelper1, coordHelper1);
                    MenuLayer.SetTileGID(gidHelper2, coordHelper2);
                    MenuLayer.SetTileGID(gidHelper3, coordHelper3);
                    MenuLayer.SetTileGID(gidHelper4, coordHelper4);
                    MenuLayer.SetTileGID(gidHelper5, coordHelper5);
                    MenuLayer.SetTileGID(gidHelper6, coordHelper6);
                    break;

            }*/
            //UglyDraw();

        }

        public void CloseMenu()
        {
            var surroundedCoords = GetSurroundedTiles();
            foreach (var coord in surroundedCoords)
            {
                m_menuLayer.RemoveTile(coord);
            }
            //UglyDraw();
        }

        CCTileMapCoordinates m_center;
        Core.Models.Definitions.Definition[] m_types;
        CCTileMapLayer m_menuLayer;
    }
}

