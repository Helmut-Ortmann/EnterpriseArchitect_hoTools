# EnterpriseArchitect_hoTools
Addin with Tools for SPARX Enterprise Architect (EA)

- Helmut.Ortmann@hoModeler.de
- hoModeler.de

# Requirements
- Windows
- .NET 4.5.2 or greater
- EA 10.0 or greater
- Local administration rights for installation (register COM dll)

# Known issues
- No

# News
- [Port, Move, Label, Rotate,..](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/Port)

# Abstract
Collection of useful tools (see also [Wiki](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/home)):

- [hoTools](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/hoTools) Assortment of tools
  - Toolbar for Searches, [SQL](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/SQL), [Services](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/Services) and [Scripts](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/Scripts)
    - 5 Searches (EA or from SQL File)
    - 5 Services (Predefined hoTools Services, your beloved Script (VBScript, JavaScript, JScript))
  - Global Keys for Searches, SQL, Services and Scripts 
    - e.g.: F1+Ctrl executes your beloved Search (EA, SQL-File)
    - e.g.: F1+Ctrl+Shft locks the selected Package
    - e.g.: F2+Ctrl+Shft runs your beloved Script (VBScript, JavaScript, JScript)
  - Set diagram Line Style
  - Version Control + SVN
  - [Quick Search](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/Quick Search) 
    - Auto complete Search, Find Search + List all Searches
  - Port support
  - Favorites
  - Export SQL query results to Excel (hoTools, SQL, Script)
  - Clipboard CSV to Excel
  - Diagram Style/Theme
  - ..
- [SQL](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/SQL) with tabbed Windows
    - Select, Insert, Delete, Update
	- Export SQL query results to Excel
    - Templates
    - Macros (easy access to EA items / Packages or complete Branches, a lot more than EA delivers)
    - Easy handling of SQL errors
    - Conveyed Items
    - *.sql files in file system (you may use favorite Editor)
    - [Find Search](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/Find Search)
- [Script](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/Script) which runs for [SQL](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/SQL) results 
  - All EA script languages (vbScript, JScript, JavaScript) 
  - Compatible to Geert Bellekens great VBScript Library
  - Script can be called from Search Results, Key, Toolbar
- [Find&Replace](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/FindAndReplace)
  - Find simple string or Regular Expression
  - Name, Description, Stereotype, Tagged Value
  - in Packages, Elements, Diagrams, Attributes, Operations
- [COM Server](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/ComServer) for [SQL searches](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/EaSearches)
- Configure
  - Buttons & Global Keys
  - Searches & Services & Scripts
  - GUI appearance
- Administration of EA
   - Version Control
- Intuitive GUI
- [Wiki](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/home)





# SQL Query
- Tabbed Editor or use your own editor
- Macro replacement (ID, GUID, Branch, DiagramSelectObject, ConveyedItem,..)
- DB specific (#DB=ORACLE#,..)
- Comment your SQL
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
- Visual Studio 2017
- Visual Studio Debug: Select "Enable native code debugging"

# Power features C/C++ for SPICE / FuSi #
Inside code are many powerful features to Reverse Engineer C/C++ Code for e.g. SPICE or FuSi (Functional Safety). With the existing code and a little knowhow it's easy to develop SPICE or FuSi compatible Architecture and Design.

Some features:
- Generate Activity Diagram from code snippets

# Installation 
- Uninstall hoTools
- hoTools.msi  (Setup\bin\Release\hoTools.msi)
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


# Releases

### Release 3.3

-  Service Move Copied Search elements to selected package

### Release 3.2.13
-  Fix Style Properties without nothing to change is now possible
-  Fix export SQL results to Excel
-  Fix Find & Replace (TaggedValue with <memo> and from MDG & more)

### Release 3.2.12

- Standard searches to search for updated to release 14. 
- Simplified GUI for generation Activities for Operation, workaround EA error for inherited operations

### Release 3.2.11

-  Fixed error nested query
-  linq2db 2.20 updated

### Release 3.2.10 Group (Un)lock

-  Group (Un)Lock for Package, Package Recursive and element



### Release 3.2.9 Add-In Search by LinqToSql instead of EA API (rearly fast)

- [NestedElements](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/ShowNestedElements)

### Release 3.2.8

- AddInSearch for [NestedElements](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/ShowNestedElements) like Requirements for selected Package/Element or GUID in SearchText
  - New Service Search/Show NestedElements like Requirements for selected Package/Element or GUID in SearchText
    -  New Service 'Search nested elements like requirements'
     - Show the nested requirements in Search windows with indentation
  - See also [ReqIF for EA](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoReverse/wiki/ReqIF)
- Show errors of Script engine
- fixing
- AddInSimple fixed


### Release 3.2.7
- New service "Copy ID of Package, Element, Diagram,.. to Clipboard"
- Minor bug fixing/improvements
- Improve speed of creating/updating Activities from operations.
- Exception adding note to diagram fixed
- Nuget packages updated

### Release 3.2.6 
- Settings were't accessable fixed
- No Error message if Settings.Json doesn't contain configuration chapter for Styles, Bulk changes and more
  - Menu entry shows the information of missing configuration

### Release 3.2.5
-  [Bulk change of Elements](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/BulkEaItemChange)
-  Find search: Coloring of matching characters optimised
-  Fix create Note linked to connector (Feature, Note)

### Release 3.2.4
-  Service Copy Type, Name, GUID, Review to Clipboard (for e.g. review)
- Fix Find Search
- Generating Activcity from Operation with combined create/update


### Release 3.2.3
-  Toolbar searches configurable to use <Search Term> from configuration or from GUI
   '<Search Term>' in configuration to use GUI <Search term>
-  Service Copy Type, Name, GUID, Review to Clipboard (for e.g. review)


### Release 3.2.2
-  Setup for per-user and per-user (Single Package authoring, per user without admin right)
-  NoAdmin install to show a simple per-user without admin rights and without dialog
-  Integrate SQLite
-  Integrate access to VS Code C/C++ Symbol Database
-  Minor fixes error handling

### Release 3.2.1

-  Update Glossary via *.csv
-  Update linq2db to 1.9

### Release 3.2

- Fix some minor errors 
-- locate type
-- update Activity from operation)
- Change Fuzzy Algorithm to find Search (EA+SQL)
- Repository informations (Help, Show Repository Properties)
- About with path of dll's and settings'

### Release 3.1.9
- Fixing error with Deployment
-- hoTools (3.1.8) has to be installed manually
### Release 3.1.8
- Output Repository information
- Port handling optimised
-- Default port position 1+2
-- Round robin ports around parend classifier
-- Rotate Port Label
-- Cofigurable with settings.json

## Release 3.1.7
-  WIX Setup names the *.msi file according to ProductVersion in Product.wxs 
-  MIT License
-  Advanced error checking in EA_OnPostNewConnector

## Release 3.1.6
- Error CheckIn/CheckOut fixed
- VC Set *.xml file optimised
- Sort Diagram Elements alphabetic (also packages in Diagram)

## Release 3.1.5
- Fix errors Port, Pin, Parameter, Label Handling
- Move all usages of source element to target element 
  Types, Diagram usage, generalization, ..
## Release 3.1.4
- Error fixing
- LINQPad output html supported
## Release 3.1.3
- LINQPad support error fixing
- LINQPad additional example queries (show LINPad connections)
- LINQPad find connection according to EA connection string

## Release 3.1.1
- AddInSimple (1.0.5):
-- Example EA context information for LINQPad
-- About with versions
- hoTools
-- LINQPad queries from hoTools
-- Send context information to LINQ
-- Error fixing
## Release 3.1.0
- About with product version and linq2db version
- Port Services added
- Fix copy Port
- Improved LINQ to SQL and LINQPad support
-- Run Query to html, csv, text
-- Output query results in EA Model Search
- AddInSimple: Example run LINQPad and output html, csv, text and output results to EA Search Window
## Release 3.0.9
- LINQ for SQL integrated (JET, SQLSVR, MySQL tested, other DBs implemented but not tested)
  Project: hoLinqToSql
- AddinSimple 1.0.3 With query example implemented with LINQ for SQL, tested with: JET, SqlServer, MySQL

## Release 3.0.8
- AddInSimple: Add-In Search 'AddInSearchSamplePackageContent' corrected
- hoTools: Reorganisation ReplaceWildCard
- Fix: Feature, Notes, Constraint for Diagram, Element, Package, Connector, Attribute, Operation
- optimise SQL to Excel (Check if SQL-file is available)

## Release 3.0.7
- Error fixed: hoTools installer doesn't create AppDataFolder'
- Default setting Linestyle. Activity corrected to 'no'
- Error fixed: hoTools configuration isAutoCounter = false

## Release 3.0.6
- Service Copy FQ (Package, Element, Diagram, Attribut, Operation, Stereotypes) to ClipBoard
- AddInSimple V1.0.1 added (AddInSimple to show Add-In and how to use Add-In for Shape Scripts )

## Release 3.0.5
- [Update Hyperlinks of Notes in Diagram, Package, Element, Attribute, Operation](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/UpdateHyperlinksInNotes)
- Service Copy GUID to Clipboard
- Item/Connector name to Clipboard
- Error configuration Toolbar services fixed

## Release 3.0.3 + 3.0.4
- Auto Numbering for Name/Alias for type/stereotype
  - Setting.json: Define format, object_types, stereotype, ..
  - Drag and Drop Diagram
  - New numbering according to createtedDate
  - Correct numbering according for formal incorrect Name/Alias
  - EA 13.0 Specification Manager don't calls EA_OnNewPostElememnt (known EA bug)
    Roundabout: Use Auto Number New of dialog in Menu Spec
- Fixing error with empty SQL <Search Term>

## Release 3.0.2
- Error fixing Find & Replace (Some buttons weren't visible')
- Code HyperScripting without/own get Model added (Example for own replacemenent of GetObject(,"EA.App"))


## Release 3.0.1
- Error fix Diagram selection handling (LineStyle and more)
- Error fix non recursive change for package
- Diagram Object + Link Style recursive
- Completeness Marker for Diagram objects (complete global, complete Diagram, incomplete Diagram)

## Release 3.0.0
- Bulk change Diagram Styles (Diagram, Diagram Objects, Diagram Links)

One Click and:
- Show hidden links
- Let them disappear
- Change Style of Diagram (recursive, conditional, ..)
- Change Style of Diagram Objects (Package, Elements,..., conditional)
- Change Style of Diagram Links (Package, Elements,..., conditional)
- Apply EA Layout Styles (Font, Colors, Linewidth, bold) for a bunch of Diagram Nodes or Links
- Amount of Buttons extended to 10

- Fix error context diagram (leads to an exception if current diagram and context diagram differ)
- Code refactoring

## Release 2.1.6
- Diagram Styles, configurable via 'Settings.json'
- Hybrid Scripting for *.exe (Using C#, Java,.. instead of VB Script & co), Example
- Hybrid Scripting for *.dll (Using C# dll,.. instead of VB Script & co), Example
- Service Added: Copy Name to Clipboard
- Service Added: Copy Name to Clipboard and search with "Quick Search" for item name
- Service Added: Diagram Style 1
- Service Added: Diagram Style 2
- Error fixing

- Remember: Services can be bound to Keys or Buttons 

## Release 2.1.5
- hoTools inventaries loaded MDGs (hoBasicMDG,..)
- Fixing error in Searches (CLASSID->CLASSGUID)
- Linestyle TH (Tree Horizontal) was implemented as Lateral Horizontal)
- Sorting Features (Attributes, Operations)
- Link features to a Note
- Preparation inventories C# Extensions to use as Scripts
- Error in Diagram.class fixed (last selected element is the first in the collection)
- Default line style StateMachine 'no' (need to delete configuration, %APPDATA%ho\hoTools\user.config, change per configuration)
- Reset settings to default settings
- Error fixing

## Release 2.1.3
- [External SQL Editor integration improved](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/sqlExternalEdit)
- Link to help improved
- [FindUsage](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/FindUsage) (Error Message if search not existed (catch try block))
- Find Usage (o.CLASSIFIER_ID=>o.CLASSIFIER_GUID, o1.CLASSIFIER_ID=>o1.CLASSIFIER_GUID corrected)
- Missing try catch in Show Specification
- Script: Catch error set SplitterDistance (I can't reproduce but it will do no harm)
- [Change Author optimised](https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/ChangeAuthor)
- Fixed: Modale windows stuck behind main window

## Release 2.1.2.1
- Wrong installation file 2.1.2 corrected
- Attribute up and down
- Set and view package folder optimised

## Release 2.1.2
- synchronisation Part Ports with its Type, usually a Class/Block
-- 'Show Ports' also synchronises ports between a block and its properties/parts (add/delete)
-- synchronised is the following information
--- Name
--- Stereotype
--- Notes
- Search with Context Menu (Right Click)
-- Edit current hoTools SQL Search with Editor
-- Open the folder of current hoTools SQL Search
-- Run Search and export results to Excel (no Excel required)
-- Export Clipboard with csv data to Excel (no Excel required)
- SQL Context Menu contains
-- Edit current hoTools SQL Search with Editor
-- Open the folder of current hoTools SQL Search
-  Fixed error in example Script hoDemoRunSQL (leads to error message during initialisation)
-- Delete Script 'hoDemoRunSQL' in EA ScriptWindow
-- Comment the last line by inserting a ' character at start of a line


## Release 2.1.1
- Error fixing hoModelView MDG
- Port,Parameter,Pin support increased (UML + SysML)
  (Structural Elements)
 - Button + Services show/hide Port,Parameter, Pin
 - Button + Services show/hide Port,Parameter, Pin Label
 - Button + Services show/hide Port


## Release 2.1
- Add Note, Constraint to Diagram, link to Node, Link, Diagram
- Example Model Views 'hoModelView' to view Diagrams in Model View
- SQL Searches of MDG, 'My Searches' are now inventoried.
- Searches were faulty (ea_id instead of ea_guid), fixed in hoToolsBasic.xml,..
- File error "d:\temp\sql\Branch.sql" fixed, cause unclear
- Jump between a composite diagram and owning diagram fixed (null value)
- Wiki updated


## Release 2.0.11
- SQL Query Results can be exported to Excel (no Excel required)
- Clipboard CSV content can be exported to Excel (no Excel required)
- User SQL Search definitions possible
- Reverse direction of edge (connector, dependency, association, information flow, SysML Item Flow)
- Unused references deleted (no functional implication)
- Implementation documentation added
- Search can't find Tagged Values in of Package


## Release 2.0.10
- Favourites (Show Favourites didn't work)
- SysML Locate Part for Sequence Call by name
- Add Load Scripts (if a Script is changed the user has to update the Script in hoTools before using it)
- Script Examples for JavaScript
- EAModel with Clipboard functions (for usage in Scripts)
- Default Line style all none (delete *.xml file required, or File, Settings,)
  It's possible that Activity Diagrams and Statechart have a default Line style
  You may want to change it by
  File, Settings, Default Line style

## Release 2.0.9

- Consolidate Tag of release (skip 2.0.8)    
- Fix: Conveyed Items for Information Flow and Connector
- Fix: Opened tabbed were duplicated by opening another repository
- optimisation: Only one Button Conveyed Items (former two, function decides from selected element/connector/flow)
- optimisation: Only one Button Notes (former two, function decides from selected element/diagram what to do)
- Switch off annoying Tooltip in SQL window
- Search description adapted to EA 13

## Release 2.0.7
    
- Global Keys with possible start Script (VBScript, JavaScript, JScript)
- Toolbar with possible start Script (VBScript, JavaScript, JScript)
- Fix: Automatic line style: Line-style of a length of 1 leads to Exception
- Error fixed Script usage of hoTools as COM Object


## Release 2.0.6
  - QuickSearch selection with autocomplete for available Searches
  - SQL Searches (multiple folders are possible, file name should be unique)
  - MDG
  - Own Searches
  - Standard Searches
  - Determine Search
    - Autocomplete (.NET standard feature)
    - Find the part of the string after Up, Down, Blank Key
      - Double Click in find starts Search




## Not yet scheduled 
- Search Management (for Quick Search)
  - Enable Category (Settings)
  - Favourites (configuration via JSON)
  - EA Standard Searches (configurable via JSON, my EA Standard Searches) 

# Feedback

I appreciate your feedback!!

Helmut.Ortmann@t-online.de

Helmut.Ortmann@hoModeler.de

