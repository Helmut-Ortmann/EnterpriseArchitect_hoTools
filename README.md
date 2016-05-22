# EnterpriseArchitect_hoTools
Addin with Tools for SPARX Enterprise Architect

# Abstract #
Collection of useful tools:

- Booster for everyday work
  - Set diagram line style
  - Define your personal keys & buttons for searches and services
  - Quick search at your finger tip
    -- Tabw Windows and more
    -- Templates
    -- Macros
    -- See error
  - Conveyed Items
  - Favorites,..
  - Create powerful SQL queries (tab window, templates, macros and more)
  - Search & Replace, regular expression supported
  - Run Scripts with SQL Search results
- Configure
-- Buttons
-- Searches
- Administration of EA
  - Version Control
- Setting to adapt GUI and available features
- Intuitive GUI
- Online help
- ..

# Preconditions
- >= .NET 4
- >=  EA 9.0
- Visual Studio 2015
- Visual Studio Debug: Select "Enable native code debugging"

# SQL Query
- Tabbed Editor
- Macro replacement (ID, GUID, Branch, DiagramSelectObject, ConveyedItem,..)
- DB specific (#DB=ORACLE#,..)
- Comment your sql
- Easy find SQ
- Load / Save
- History / Last openedt
- Easy cooperation with your beloved editor (try e.g. atom,..)
- etc.

# SQL Query + Script (VB, JScript, JavaScript)
- Run SQL and do something by script with the results

# Development #
- C#
- ActiveX for Addin GUI
- BookMe for Online Help (powerful!)
- Configuration (*.xml)
- Installation via WIX
- Load MDG during starup
- Keys
- Integrated Geert Bellekens VBScript Library
- Useful Searches
- Installable for different customers (brands)

# Power features C/C++ for SPICE / FuSi #
Inside code are many powerful features to Reverse Engineer C/C++ Code for e.g. SPICE or FuSi (Functional Safety). With the existing code and a little knowhow it's easy to develop SPICE or FuSi compatible Architecture and Design.

Some features:
- Generate Activity Diagram from code snippets

# Installation #
- uninstall hoTools
- hoTools.msi  (Setup\bin\Release\hoTools.msi V2.0.1)
- hoToolsRemove.ps1 (deinstall with PowerShell)
- make sure only one instance is installed

# ToDo
- Drag SQL file on canvas
  (seems impossible with EA, no drag and drop for ActiveX supported)
- Undo (multiple)
