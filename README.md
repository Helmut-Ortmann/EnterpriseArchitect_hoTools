# EnterpriseArchitect_hoTools
Addin with Tools for SPARX Enterprise Architect (EA)

# Abstract
Collection of useful tools (see also [WiKi](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/home)):

- [hoTools](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/hoTools) Assortment of tools
  - Toolbar for Searches and Services
  - Global Keys for Searches and Services (e.g.: F1+Ctrl executes your beloved Search)
  - Set diagram line style
  - Version Control + SVN
  - Port support
  - Favorites
  - ..
- [SQL](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/SQL) with tabbed Windows
    - Select, Insert, Delete, Update
    - Templates
    - Macros (easy access to EA items / Packages or complete Branches, a lot more than EA delivers)
    - Easy handling of SQL errors
    - Conveyed Items
    - *.sql files in file system (you may use favourite Editor)
- [Script](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/Script) which runs for [SQL](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/SQL) results 
  - All EA script languages (vbScript, JScript, JavaScript) 
  - Compatible to Geert Bellekens great VBScript Library
- [Find&Replace](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/FindAndReplace)
  - Find simple string or Regular Expression
  - Name, Description, Stereotype, Tagged Value
  - in Packages, Elements, Diagrams, Attributes, Operations
- [COM Server](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/ComServer) for [SQL searches](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/EaSearches)
- Configure
 - Buttons & Global Keys
 - Searches & Services
 - GUI appearance
- Administration of EA
  - Version Control
- Intuitive GUI
- [WiKi](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/home)


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
- Uninstall hoTools
- hoTools.msi  (Setup\bin\Release\hoTools.msi V2.0.1)
- In EA: Extension, Addin Windows is selected
- In EA: Manage Addins, MDG: hoTools is selected
- hoToolsRemove.ps1 (deinstall with PowerShell)
- Make sure only one instance of hoTools is installed
- See also: [Installation](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/Installation)

## Install folders
- c:\Users\&lt;user>\AppData\Local\Apps\hoTools\
  - The application installation folder
  - Sql.zip  Examples SQL files
- c:\Users\&lt;user>\AppData\Roaming\ho\hoTools\
  - Data folder
  - user.config Configuration 


# Scheduled (release 2.0.5)
- Lock/Unlock for Package
- Error fixed Script usage of hoTools as COM Object
- Run hoTools SQL from Script 

# Schedules (release 2.0.6)
- QuickSearch
  - With easy change of Search (EA, SQL)
  - Reset to default Search
  - Set Default Search

# Bugfixes
- Error fixed Script usage of hoTools as COM Object

# Not yet scheduled
- Drag SQL file on canvas
  (seems impossible with EA, no drag and drop for ActiveX supported)

# Feedback

I appreciate your feedback!!

