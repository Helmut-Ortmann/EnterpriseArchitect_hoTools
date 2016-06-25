# EnterpriseArchitect_hoTools
Addin with Tools for SPARX Enterprise Architect

# Abstract
Collection of useful tools (see also [WiKi](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/home)):

- [hoTools](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/hoTools) Assortment of tools
  - Toolbar for Searches and Services
  - Keys for Searches and Services 
  - Set diagram line style
  - Version Control + SVN
  - Port support
  - Favorites
  - ..
- [SQL](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/SQL) with tabbed Windows
    -- Select, Insert, Delete, Update
    -- Templates
    -- Macros (easy access to EA, a lot more than EA delivers)
    -- See error
    -- Conveyed Items
    -- *.sql files in file system (you may use favourite Editor)
- [Script](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/Script) which runs for [SQL](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/SQL) results 
  - all EA script languages (vbScript, JScript, JavaScript) 
- Configure
 - Buttons
 - Searches
 - GUI appearance
- Administration of EA
  - Version Control
- Intuitive GUI
- Online help
- ..

# Requirements
- Windows
- .NET 4.5 or greater
- EA 9.0 or greater
- Local administration rights for installation (register COM dll)


# SQL Query
- Tabbed Editor or use your own editor
- Macro replacement (ID, GUID, Branch, DiagramSelectObject, ConveyedItem,..)
- DB specific (#DB=ORACLE#,..)
- Comment your sql
- Easy find SQL error
- Load / Save to file
- History / Last opened
- Easy cooperation with your beloved editor (try e.g. atom,..)
- etc.

# SQL Query + Script (VB, JScript, JavaScript)
- Run SQL and do something by script with the results

# Development
- C#
- ActiveX for Addin GUI
- BookMe for Online Help (powerful!)
- Configuration (*.xml)
- Installation via WIX
- Load MDG during startup
- Handling Keys
- Integrated Geert Bellekens VBScript Library
- Useful Searches
- Installable for different customers (brands)
- Visual Studio 2015
- Visual Studio Debug: Select "Enable native code debugging"

# Power features C/C++ for SPICE / FuSi #
Inside code are many powerful features to Reverse Engineer C/C++ Code for e.g. SPICE or FuSi (Functional Safety). With the existing code and a little knowhow it's easy to develop SPICE or FuSi compatible Architecture and Design.

Some features:
- Generate Activity Diagram from code snippets

# Installation 
- uninstall hoTools
- hoTools.msi  (Setup\bin\Release\hoTools.msi V2.0.1)
- In EA: Extension, Addin Windows is selected
- In EA: Manage Addins, MDG: hoTools is selected
- hoToolsRemove.ps1 (deinstall with PowerShell)
- Make sure only one instance of hoTools is installed
- See also: [Installation](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/Installation)

# Scheduled (release 2.0.3)
- Improved help for Toolbar Buttons
- Use advanced [SQL](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/SQL) everywhere in hoTools
-- Toolbar Buttons
-- Quick Search 

# ToDo
- Drag SQL file on canvas
  (seems impossible with EA, no drag and drop for ActiveX supported)
- Undo (multiple)
