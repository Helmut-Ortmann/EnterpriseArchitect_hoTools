SET WIX=C:\Program Files (x86)\WiX Toolset v3.11\bin\heat
del *.wxs

"%WIX%" file ..\..\hoToolsRoot\bin\release\hoToolsRoot.dll -ag -template fragment -out hoToolsRoot.wxs
"%WIX%" file ..\..\hoToolsGui\bin\release\hoToolsGui.dll -ag -template fragment -out hoToolsGui.wxs
"%WIX%" file ..\..\hoSqlGui\bin\release\hoSqlGui.dll -ag -template fragment -out hoSqlGui.wxs
"%WIX%" file ..\..\hoExtensionGui\bin\release\hoExtensionGui.dll -ag -template fragment -out hoExtensionGui.wxs
"%WIX%" file ..\..\hoFindAndReplaceGui\bin\release\hoFindAndReplaceGui.dll -ag -template fragment -out hoFindAndReplaceGui.wxs

dir
