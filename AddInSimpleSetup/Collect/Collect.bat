SET WIX=C:\Program Files (x86)\WiX Toolset v3.11\bin\heat
del *.wxs

"%WIX%" file ..\..\AddInSimple\bin\release\AddInSimple.dll -ag -template fragment -out AddInSimple.wxs


dir
